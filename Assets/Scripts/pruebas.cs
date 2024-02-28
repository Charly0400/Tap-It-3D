using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class pruebas : MonoBehaviour
{
    public float moveDistance = 2f;
    public bool canMove = true;

    void Update()
    {
        if (Mouse.current.leftButton.wasPressedThisFrame)
        {
            // Realizar un raycast desde la posición del ratón
            Ray ray = Camera.main.ScreenPointToRay(Mouse.current.position.ReadValue());
            RaycastHit hit;

            // Realizar el raycast y verificar si golpea un objeto con la capa "Cubos"
            if (Physics.Raycast(ray, out hit, Mathf.Infinity, LayerMask.GetMask("Obstacle")))
            {
                CubosInteractuables cuboInteractuable = hit.collider.GetComponent<CubosInteractuables>();

                // Si el raycast golpea un cubo interactuable y puede moverse, iniciar la corrutina para mover el cubo
                /*if (cuboInteractuable != null && cuboInteractuable.canMove)
                {
                    StartCoroutine(cuboInteractuable.MoveCube());
                }*/
            }
        }
    }

    IEnumerator MoveCube()
    {
        canMove = false;
        Collider collider = GetComponent<Collider>();
        collider.enabled = false;

        Vector3 currentPosition = transform.position;
        Vector3 targetPosition = currentPosition + Vector3.up * moveDistance;
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

        Destroy(gameObject);
    }
}
