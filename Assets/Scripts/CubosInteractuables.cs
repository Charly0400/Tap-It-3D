using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;


public class CubosInteractuables : MonoBehaviour
{
    public Direction blockDirection;
    public float moveDistance = 1f;
    private bool canMove = true;
    private MouseInputAction clickCentral;


    private void Awake()
    {
        clickCentral = new MouseInputAction();
        clickCentral.Enable();
    }
    private void Start()
    {
        clickCentral.Standard.CentralClick.performed += ClickCentral;
    }
    private void OnMouseDown()
    {
        // Verifica si el cubo puede moverse
        if (canMove)
        {
            StartCoroutine(MoveCube());
        }
    }

    IEnumerator MoveCube()
    {
        canMove = false;
        Collider collider = GetComponent<Collider>();
        collider.enabled = false;

        Vector3 currentPosition = transform.position;

        // Obtener la dirección hacia la que mira el cubo en su espacio local
        Vector3 moveDirection = transform.up;

        // Calcular la posición objetivo hacia la que mover el cubo
        Vector3 targetPosition = currentPosition + moveDirection * moveDistance;

        // Realizar un raycast en la dirección de movimiento para verificar si el espacio está vacío
        RaycastHit hit;
        if (!Physics.Raycast(targetPosition, -moveDirection, out hit, moveDistance))
        {
            // Si no hay ningún objeto en la posición objetivo, mover el cubo
            float duration = 1.0f;
            float elapsedTime = 0f;

            while (elapsedTime < duration)
            {
                elapsedTime += Time.deltaTime;
                float t = Mathf.Clamp01(elapsedTime / duration);
                t = t * t * (3f - 2f * t);
                transform.position = Vector3.Lerp(currentPosition, targetPosition, t);
                yield return null;
            }

            // Destruir el cubo solo si se pudo mover con éxito
            Destroy(gameObject);
        }
        else
        {
            // Revertir la desactivación del collider si el cubo no se movió
            canMove = true;
            collider.enabled = true;
        }
    }

    public void SetBlockDirection(Vector3 direction)
    {
        // Normaliza la dirección y la establece como la dirección de movimiento
        direction.Normalize();
        transform.up = direction;
    }

    public void ClickCentral(InputAction.CallbackContext context)
    {
        // Lanzar un rayo desde la posición del mouse
        Ray ray = Camera.main.ScreenPointToRay(Mouse.current.position.ReadValue());
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit))
        {
            // Verificar si el rayo colisiona con este cubo
            if (hit.collider.gameObject == gameObject)
            {
                Destroy(gameObject);
            }
        }
    }
}

