using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Asdf : MonoBehaviour
{
    public int health = 100;
    public float recoilForce = 5f;
    public float recoilDuration = 0.2f;

    private Rigidbody rb;
    private bool isRecoiling = false;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    void Update()
    {
        if (health <= 0)
        {
            // El enemigo ha sido derrotado
            Destroy(gameObject);
        }
    }

    // Método para aplicar retroceso cuando el enemigo es golpeado
    public void TakeDamage()
    {
        if (!isRecoiling)
        {
            // Aplicar retroceso
            StartCoroutine(Recoil());

            // Reducir la salud del enemigo
            health -= 25;
        }
    }

    IEnumerator Recoil()
    {
        isRecoiling = true;

        // Calcular la dirección opuesta al jugador
        Vector3 recoilDirection = -transform.forward;

        // Aplicar la fuerza de retroceso
        rb.AddForce(recoilDirection * recoilForce, ForceMode.Impulse);

        // Esperar la duración del retroceso
        yield return new WaitForSeconds(recoilDuration);

        // Restablecer el estado de retroceso
        rb.velocity = Vector3.zero;
        isRecoiling = false;
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.tag.Equals("Armor"))
        {
            TakeDamage();
        }
    }
}