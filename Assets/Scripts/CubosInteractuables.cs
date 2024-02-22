using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;


public class CubosInteractuables : MonoBehaviour
{
    private MouseInputAction inputMouse;

    private void Awake()
    {
        inputMouse = new MouseInputAction();
        inputMouse.Enable();
    }

    private void Start()
    {
        inputMouse.Standard.LeftClick.performed += clickIzq;   
    }

    
    void clickIzq (InputAction.CallbackContext context)
    {
        Destroy(gameObject);
    }
}
