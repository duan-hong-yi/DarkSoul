using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    public IUserInput userInput;
    public ActorController actorController;
    private GameObject playerHandle;
    private GameObject cameraHandle;
    [SerializeField]
    public float rotateSpeed;
    private float cameraEulerX;
    private GameObject model;
    void Awake()
    {
        cameraHandle = transform.parent.gameObject;
        playerHandle = cameraHandle.transform.parent.gameObject;
        
        cameraEulerX = 0;
        model = playerHandle.GetComponent<ActorController>().model;
    }
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        ///
        /// DHY version
        ///
        //cameraHandle.transform.Rotate(Vector3.up, move.JRight * rotateSpeed * Time.fixedDeltaTime);
        //cameraHandle.transform.Rotate(Vector3.right, move.JUp * rotateSpeed * Time.fixedDeltaTime);
        //if (move.targetDright != 0 || move.targetDup != 0)
        //{
        //    float rotationNumY = cameraHandle.transform.localRotation.eulerAngles.y;
        //    cameraHandle.transform.localRotation = Quaternion.Euler(Vector3.zero);
        //    playerHandle.transform.Rotate(Vector3.up, rotationNumY);

        //}
        Vector3 modelEulerAnglesBeforeRotate = model.transform.eulerAngles;
       
        playerHandle.transform.Rotate(Vector3.up, rotateSpeed * Time.fixedDeltaTime * userInput.JRight);
        model.transform.eulerAngles = modelEulerAnglesBeforeRotate;
        cameraEulerX -= userInput.JUp * rotateSpeed * Time.fixedDeltaTime;
        cameraEulerX = Mathf.Clamp(cameraEulerX, -40, 30);
        cameraHandle.transform.localEulerAngles = new Vector3(cameraEulerX,0,0);

        
    }
}
