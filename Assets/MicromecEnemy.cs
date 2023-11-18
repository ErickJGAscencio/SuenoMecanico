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
    public float rotationSpeed = 360f; // Velocidad de rotación del enemigo
    public float fuerzaAtaque = 5f; // Fuerza del ataque
    public float tiempoEsperaEntreAtaques = 2f; // Tiempo de espera entre ataques
    public float radioDeteccionJugador = 10f; // Radio de detección del player
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

        StartCoroutine(AtaquePeriódico());
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
            // Comprueba si el player está dentro del radio de detección
            float distanciaAlJugador = Vector3.Distance(transform.position, player.transform.position);

            if (distanciaAlJugador <= radioDeteccionJugador)
            {
                navMeshAgent.SetDestination(player.transform.position);// Establece la posición del player como destino del NavMeshAgent

                if (distanciaAlJugador <= navMeshAgent.stoppingDistance)// Cambia la velocidad del NavMeshAgent según la distancia al player
                {                    
                    if (!estaAtacando)// El enemigo está lo suficientemente cerca del player para atacar
                        StartCoroutine(Atacar());
                }
                else
                {
                    navMeshAgent.speed = runSpeed;// El enemigo sigue al player
                }
            }
            else
            {
                navMeshAgent.speed = walkSpeed;// Reinicia la velocidad del NavMeshAgent si el player está fuera del radio de detección
            }
        }
    }

    IEnumerator AtaquePeriódico()
    {
        while (true)
        {
            yield return new WaitForSeconds(tiempoEsperaEntreAtaques);
            // Puedes agregar más lógica aquí si es necesario
        }
    }

    IEnumerator Atacar()
    {
        estaAtacando = true;
        player.GetComponent<ThirdPersonMovement>().enabled = false;
        Vector3 direccionAlJugador = (player.transform.position - transform.position).normalized;
        player.GetComponent<Rigidbody>().AddForce(direccionAlJugador * fuerzaAtaque, ForceMode.Impulse);

        // Espera hasta el próximo ataque
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
    // Dibuja una esfera para representar el radio de detección del enemigo
    Gizmos.color = Color.red;
    Gizmos.DrawWireSphere(transform.position, radioDeteccionJugador);
}
}