using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;


public class CubosInteractuables : MonoBehaviour
{
    public float moveDistance = 2f;
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
        Vector3 moveDirection = transform.up;
        Vector3 targetPosition = currentPosition + moveDirection * moveDistance;
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
