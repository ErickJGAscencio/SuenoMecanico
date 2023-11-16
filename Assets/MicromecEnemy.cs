using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class MicromecEnemy : MonoBehaviour
{
    public float walkSpeed = 5f; // Velocidad de movimiento del enemigo
    public float runSpeed = 7f; // Velocidad de movimiento del enemigo al correr
    public float rotationSpeed = 360f; // Velocidad de rotaci�n del enemigo
    public float fuerzaAtaque = 5f; // Fuerza del ataque
    public float tiempoEsperaEntreAtaques = 2f; // Tiempo de espera entre ataques
    public float radioDeteccionJugador = 10f; // Radio de detecci�n del jugador
    public int cantidadEnGrupo = 3; // Cantidad de enemigos en el grupo

    public GameObject jugador;
    private NavMeshAgent navMeshAgent;
    private bool estaAtacando = false;

    void Start()
    {
       // jugador = GameObject.FindGameObjectWithTag("Player");
        navMeshAgent = GetComponent<NavMeshAgent>();

        // Configuraci�n inicial del NavMeshAgent
        navMeshAgent.speed = walkSpeed;
        navMeshAgent.angularSpeed = rotationSpeed;

        // Llama a la funci�n de ataque cada cierto tiempo
        StartCoroutine(AtaquePeri�dico());
    }

    void Update()
    {
        // Comprueba si el jugador est� dentro del radio de detecci�n
        float distanciaAlJugador = Vector3.Distance(transform.position, jugador.transform.position);
        if (distanciaAlJugador <= radioDeteccionJugador)
        {
            // Establece la posici�n del jugador como destino del NavMeshAgent
            navMeshAgent.SetDestination(jugador.transform.position);

            // Cambia la velocidad del NavMeshAgent seg�n la distancia al jugador
            if (distanciaAlJugador <= navMeshAgent.stoppingDistance)
            {
                // El enemigo est� lo suficientemente cerca del jugador para atacar
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
            // Reinicia la velocidad del NavMeshAgent si el jugador est� fuera del radio de detecci�n
            navMeshAgent.speed = walkSpeed;
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

        // Puedes agregar l�gica adicional para el ataque aqu�
        // Por ahora, solo aplicamos una fuerza al jugador
        Vector3 direccionAlJugador = (jugador.transform.position - transform.position).normalized;
        jugador.GetComponent<Rigidbody>().AddForce(direccionAlJugador * fuerzaAtaque, ForceMode.Impulse);

        // Espera hasta el pr�ximo ataque
        yield return new WaitForSeconds(tiempoEsperaEntreAtaques);

        estaAtacando = false;
    }
}

