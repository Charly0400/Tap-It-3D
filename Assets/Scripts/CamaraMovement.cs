using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class CamaraMovement : MonoBehaviour
{
    [SerializeField]
    private float forceMovement = 1f;

    public Transform cameraLook;
    private MouseInputAction inputCamera;
    [SerializeField]

    //perimetro circulo
    private void Awake()
    {
        inputCamera = new MouseInputAction();
        inputCamera.Enable();
    }
    void Start()
    {
        inputCamera.Camera.Movement.performed += CameraMovement;
        
    }

    // Update is called once per frame
    void Update()
    {
        transform.LookAt(cameraLook);


        float horizontalInput = inputCamera.Camera.Movement.ReadValue<Vector2>().x;
        float verticalInput = inputCamera.Camera.Movement.ReadValue<Vector2>().y;

        // Calcular la rotación en los ejes X e Y basada en la entrada
        float rotationX = verticalInput * forceMovement * Time.deltaTime;
        float rotationY = horizontalInput * forceMovement * Time.deltaTime;

        // Rotar la cámara alrededor del punto de mira en los ejes X e Y
        transform.RotateAround(cameraLook.position, Vector3.up, rotationY);
        transform.RotateAround(cameraLook.position, transform.right, -rotationX);
    }


    public void CameraMovement(InputAction.CallbackContext context)
    {

        // Obtener la dirección horizontal y vertical de la entrada

        /* print("WSASD");
         Vector3 direction = new Vector3(0,inputCamera.Camera.CameraMove.ReadValue<float>(), 0);
         transform.RotateAround(cameraLook.position, direction, forceMovement*Time.deltaTime);
         transform.Translate(direction * forceMovement);
         transform.Rotate(direction * forceMovement);

         string tecla = context.control.name;
         switch (tecla)
         {
             case "a":
                 transform.Translate(Vector3.left * forceMovement);
                 break;
             case "d":
                 transform.Translate(Vector3.right * forceMovement);
                 break;
         }


         Debug.Log(tecla); */
    }   

    

}
