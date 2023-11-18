using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.AI;

public class MicromecEnemy : MonoBehaviour
{
    public int hp = 40;
    public float walkSpeed = 5f; // Velocidad de movimiento del enemigo
    public float runSpeed = 7f; // Velocidad de movimiento del enemigo al correr
    public float rotationSpeed = 360f; // Velocidad de rotaci�n del enemigo
    public float fuerzaAtaque = 5f; // Fuerza del ataque
    public float tiempoEsperaEntreAtaques = 2f; // Tiempo de espera entre ataques
    public float radioDeteccionJugador = 10f; // Radio de detecci�n del player
    public int cantidadEnGrupo = 3; // Cantidad de enemigos en el grupo

    public GameObject player;
    private NavMeshAgent navMeshAgent;
    public bool estaAtacando = false;
    public float timeToWait = 1f;
    public Rigidbody rb;
    public bool isAttacked;
    public Vector3 velocidadRebote;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        player = GameObject.FindGameObjectWithTag("Player");
        navMeshAgent = GetComponent<NavMeshAgent>();

        navMeshAgent.speed = walkSpeed;
        navMeshAgent.angularSpeed = rotationSpeed;

        StartCoroutine(AtaquePeri�dico());
    }

    void FixedUpdate()
    {
        if (hp <= 0)
        {
            StopAllCoroutines();
            Destroy(this.gameObject);
        }

        if (!isAttacked)
        {
            // Comprueba si el player est� dentro del radio de detecci�n
            float distanciaAlJugador = Vector3.Distance(transform.position, player.transform.position);

            if (distanciaAlJugador <= radioDeteccionJugador)
            {
                navMeshAgent.SetDestination(player.transform.position);// Establece la posici�n del player como destino del NavMeshAgent

                if (distanciaAlJugador <= navMeshAgent.stoppingDistance)// Cambia la velocidad del NavMeshAgent seg�n la distancia al player
                {                    
                    if (!estaAtacando)// El enemigo est� lo suficientemente cerca del player para atacar
                        StartCoroutine(Atacar());
                }
                else
                {
                    navMeshAgent.speed = runSpeed;// El enemigo sigue al player
                }
            }
            else
            {
                navMeshAgent.speed = walkSpeed;// Reinicia la velocidad del NavMeshAgent si el player est� fuera del radio de detecci�n
            }
        }
    }

    IEnumerator AtaquePeri�dico()
    {
        while (true)
        {
            yield return new WaitForSeconds(tiempoEsperaEntreAtaques);
            // Puedes agregar m�s l�gica aqu� si es necesario
        }
    }

    IEnumerator Atacar()
    {
        estaAtacando = true;
        player.GetComponent<ThirdPersonMovement>().enabled = false;
        Vector3 direccionAlJugador = (player.transform.position - transform.position).normalized;
        player.GetComponent<Rigidbody>().AddForce(direccionAlJugador * fuerzaAtaque, ForceMode.Impulse);

        // Espera hasta el pr�ximo ataque
        yield return new WaitForSeconds(tiempoEsperaEntreAtaques);

        estaAtacando = false;
        player.GetComponent<ThirdPersonMovement>().enabled = true;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag.Equals("Armor"))
        {
            hp -= 25;
            print("pega");
            isAttacked = true;
            StartCoroutine(IsAttaked(other));          
        }
    }
    IEnumerator IsAttaked(Collider other)
    {
        Vector3 retroceso = (transform.position - player.transform.position).normalized;
        rb.AddForce(retroceso * fuerzaAtaque, ForceMode.Impulse);

        yield return new WaitForSeconds(timeToWait);
        rb.velocity = Vector3.zero;
        isAttacked = false;
    }
    private void OnDrawGizmos()
{
    // Dibuja una esfera para representar el radio de detecci�n del enemigo
    Gizmos.color = Color.red;
    Gizmos.DrawWireSphere(transform.position, radioDeteccionJugador);
}
}