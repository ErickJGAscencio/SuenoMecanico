using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    public Transform target; // Referencia al objeto que la cámara seguirá
    public Vector3 offset = new Vector3(0f, 2f, -5f); // Offset de posición relativo al personaje
    public float smoothness = 5f; // Suavizado para la transición de la cámara

    void FixedUpdate()
    {
        if (!target)
            return;

        // Calcula la posición objetivo de la cámara con el offset
        Vector3 targetPosition = target.position + offset;

        // Suaviza la transición de la posición actual a la posición objetivo
        transform.position = Vector3.Lerp(transform.position, targetPosition, smoothness * Time.deltaTime);

        // La cámara siempre mira hacia el personaje
        transform.LookAt(target.position);
    }
}
