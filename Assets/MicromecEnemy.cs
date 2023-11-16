using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class MicromecEnemy : MonoBehaviour
{
    public float walkSpeed = 5f; // Velocidad de movimiento del enemigo
    public float runSpeed = 7f; // Velocidad de movimiento del enemigo al correr
    public float rotationSpeed = 360f; // Velocidad de rotación del enemigo
    public float fuerzaAtaque = 5f; // Fuerza del ataque
    public float tiempoEsperaEntreAtaques = 2f; // Tiempo de espera entre ataques
    public float radioDeteccionJugador = 10f; // Radio de detección del jugador
    public int cantidadEnGrupo = 3; // Cantidad de enemigos en el grupo

    public GameObject jugador;
    private NavMeshAgent navMeshAgent;
    private bool estaAtacando = false;

    void Start()
    {
       // jugador = GameObject.FindGameObjectWithTag("Player");
        navMeshAgent = GetComponent<NavMeshAgent>();

        // Configuración inicial del NavMeshAgent
        navMeshAgent.speed = walkSpeed;
        navMeshAgent.angularSpeed = rotationSpeed;

        // Llama a la función de ataque cada cierto tiempo
        StartCoroutine(AtaquePeriódico());
    }

    void Update()
    {
        // Comprueba si el jugador está dentro del radio de detección
        float distanciaAlJugador = Vector3.Distance(transform.position, jugador.transform.position);
        if (distanciaAlJugador <= radioDeteccionJugador)
        {
            // Establece la posición del jugador como destino del NavMeshAgent
            navMeshAgent.SetDestination(jugador.transform.position);

            // Cambia la velocidad del NavMeshAgent según la distancia al jugador
            if (distanciaAlJugador <= navMeshAgent.stoppingDistance)
            {
                // El enemigo está lo suficientemente cerca del jugador para atacar
                if (!estaAtacando)
                {
                    StartCoroutine(Atacar());
                }
            }
            else
            {
                // El enemigo sigue al jugador
                navMeshAgent.speed = runSpeed;
            }
        }
        else
        {
            // Reinicia la velocidad del NavMeshAgent si el jugador está fuera del radio de detección
            navMeshAgent.speed = walkSpeed;
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

        // Puedes agregar lógica adicional para el ataque aquí
        // Por ahora, solo aplicamos una fuerza al jugador
        Vector3 direccionAlJugador = (jugador.transform.position - transform.position).normalized;
        jugador.GetComponent<Rigidbody>().AddForce(direccionAlJugador * fuerzaAtaque, ForceMode.Impulse);

        // Espera hasta el próximo ataque
        yield return new WaitForSeconds(tiempoEsperaEntreAtaques);

        estaAtacando = false;
    }
}

