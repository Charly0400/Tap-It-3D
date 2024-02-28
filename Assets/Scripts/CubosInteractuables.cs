using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;


public class CubosInteractuables : MonoBehaviour
{
    public float moveDistance = 1f;
    private bool canMove = true;

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

        // Obtener la direcci�n hacia la que mira el cubo en su espacio local
        Vector3 moveDirection = transform.up;

        // Calcular la posici�n objetivo hacia la que mover el cubo
        Vector3 targetPosition = currentPosition + moveDirection * moveDistance;

        // Realizar un raycast en la direcci�n de movimiento para verificar si el espacio est� vac�o
        RaycastHit hit;
        if (!Physics.Raycast(targetPosition, -moveDirection, out hit, moveDistance))
        {
            // Si no hay ning�n objeto en la posici�n objetivo, mover el cubo
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

            // Destruir el cubo solo si se pudo mover con �xito
            Destroy(gameObject);
        }
        else
        {
            // Revertir la desactivaci�n del collider si el cubo no se movi�
            canMove = true;
            collider.enabled = true;
        }
    }
}

