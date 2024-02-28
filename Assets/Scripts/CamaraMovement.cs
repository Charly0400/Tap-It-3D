using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class CamaraMovement : MonoBehaviour
{
    [SerializeField]
    private float forceMovement = 1f;

    public Transform cameraLook;
    private InputCamera inputCamera;
    [SerializeField]
    private Rigidbody camaraRb;
    //LookAt
    //perimetro circulo
    // Start is called before the first frame update
    private void Awake()
    {
        inputCamera = new InputCamera();
        inputCamera.Enable();
    }
    void Start()
    {
        inputCamera.Camera.CameraMove.performed += CameraMovement;
        
    }

    // Update is called once per frame
    void Update()
    {
        transform.LookAt(cameraLook);
    }


    public void CameraMovement(InputAction.CallbackContext context)
    {
        print("WSASD");
        Vector3 direction = new Vector3(0,inputCamera.Camera.CameraMove.ReadValue<float>(), 0);
        transform.RotateAround(cameraLook.position, direction, forceMovement*Time.deltaTime);
        //transform.Translate(direction * forceMovement);
        //transform.Rotate(direction * forceMovement);

        //string tecla = context.control.name;
        //switch (tecla)
        //{
        //    case "a":
        //        transform.Translate(Vector3.left * forceMovement);
        //        break;
        //    case "d":
        //        transform.Translate(Vector3.right * forceMovement);
        //        break;
        //}


        //Debug.Log(tecla);
    }

    

}
