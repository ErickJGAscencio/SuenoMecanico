using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlatformMovement : MonoBehaviour
{
    public Transform puntoA;
    public Transform puntoB;
    public float velocidad = 2f;

    public Vector3 posicionInicial;
    public bool haciaPuntoB = true;

    void Start()
    {
        posicionInicial = transform.position;
    }

    void FixedUpdate()
    {
        // Calcula el nuevo destino basado en la dirección actual
        Vector3 destino = haciaPuntoB ? puntoB.position : puntoA.position;

        // Mueve la plataforma hacia el destino usando Lerp
        transform.position = Vector3.Lerp(transform.position, destino, velocidad * Time.fixedDeltaTime);

        // Si la plataforma llega al destino, cambia la dirección
        if (Vector3.Distance(transform.position, destino) < 0.01f)
        {
            haciaPuntoB = !haciaPuntoB;
        }
    }


    private void OnTriggerEnter(Collider other)
    {
        other.transform.parent = transform;
    }
    private void OnTriggerExit(Collider other)
    {
        other.transform.parent = null;
    }
}

