using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    public Transform target; // Referencia al objeto que la c�mara seguir�
    public Vector3 offset = new Vector3(0f, 2f, -5f); // Offset de posici�n relativo al personaje
    public float smoothness = 5f; // Suavizado para la transici�n de la c�mara

    void FixedUpdate()
    {
        if (!target)
            return;

        // Calcula la posici�n objetivo de la c�mara con el offset
        Vector3 targetPosition = target.position + offset;

        // Suaviza la transici�n de la posici�n actual a la posici�n objetivo
        transform.position = Vector3.Lerp(transform.position, targetPosition, smoothness * Time.deltaTime);

        // La c�mara siempre mira hacia el personaje
        transform.LookAt(target.position);
    }
}
