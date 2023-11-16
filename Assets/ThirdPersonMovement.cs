using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ThirdPersonMovement : MonoBehaviour
{
    public float walk_speed = 5f; // Velocidad de movimiento del personaje
    public float run_speed = 7f; // Velocidad de movimiento del personaje
    public float rotationSpeed = 360f; // Velocidad de rotación del personaje
    public float fuerzaSalto = 10f;
    public float longitudRayo = 0.1f;

    private Rigidbody rb;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    void Update()
    {
        // Obtener las entradas del jugador
        float horizontalMovement = Input.GetAxis("Horizontal");
        float verticalMovement = Input.GetAxis("Vertical");

        // Calcular la dirección del movimiento
        Vector3 movement = new Vector3(horizontalMovement, 0f, verticalMovement).normalized;

        if (movement.magnitude >= 0.1f)
        {
            // Calcular el ángulo objetivo de rotación
            float targetAngle = Mathf.Atan2(movement.x, movement.z) * Mathf.Rad2Deg;

            // Establecer la rotación directamente
            transform.rotation = Quaternion.Euler(0f, targetAngle, 0f);

            // Seleccionar la velocidad de movimiento según la tecla Shift
            Vector3 movementVelocity = Input.GetKey(KeyCode.LeftShift)
                ? new Vector3(horizontalMovement, 0f, verticalMovement).normalized * run_speed
                : new Vector3(horizontalMovement, 0f, verticalMovement).normalized * walk_speed;//sino

            // Mover al personaje en la dirección del input
            rb.MovePosition(rb.position + movementVelocity * Time.deltaTime);
        }

        // Saltar
        if (Input.GetButtonDown("Jump") && EstaEnElSuelo())
        {
            rb.AddForce(Vector3.up * fuerzaSalto, ForceMode.Impulse);
        }
    }

    bool EstaEnElSuelo()
    {
        RaycastHit hit;

        // Lanzar un rayo hacia abajo desde el centro del personaje
        if (Physics.Raycast(transform.position, Vector3.down, out hit, longitudRayo))
        {
            // Comprobar si el objeto golpeado es parte del suelo
            if (hit.collider.CompareTag("Suelo"))
            {
                return true; // El personaje está en el suelo
            }
        }

        return false;
    }

}