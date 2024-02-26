using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;


public class CubosInteractuables : MonoBehaviour
{
    public Animator animator; // Referencia al componente Animator
    public LayerMask obstacleLayer; // Capa de obstáculos para detectar colisiones

    private bool canMove = true; // Indica si el cubo puede moverse
/*
    private MouseInputAction inputMouse;

    private void Awake()
    {
        inputMouse = new MouseInputAction();
        inputMouse.Enable();
    }

    private void Start()
        inputMouse.Standard.LeftClick.performed += clickIzq;

    void clickIzq(InputAction.CallbackContext context) { }
  */
    void Update()
    {
        // Verificamos si se ha presionado el botón izquierdo del ratón
        if (Mouse.current.leftButton.wasPressedThisFrame)
        {
            if (canMove)
            {
                MoveCube();
            }
        }
    }

    void MoveCube()
    {
        // Ejecutar animación de desaparición
        animator.Play("New Animation");

        // Deshabilitar movimiento y colisiones
        canMove = false;
        GetComponent<Collider>().enabled = false;

        // Obtener el estado actual de la animación
        AnimatorStateInfo stateInfo = animator.GetCurrentAnimatorStateInfo(0);

        // Obtener la duración de la animación
        float animationDuration = stateInfo.length;

        // Destruir el objeto después de la duración de la animación
        Destroy(gameObject, animationDuration);
    }
}
