using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyFollow : MonoBehaviour
{
    public Transform player; // Referencia al jugador
    public float detectionRadius = 5.0f; // Radio de detección general
    public float detectionRadius2 = 4.0f; // Radio de detección para Tongue
    public float speed = 2.0f; // Velocidad de movimiento

    private Animator anim; // Controlador de animaciones
    private Vector3 movement; // Movimiento del enemigo

    void Start()
    {
        anim = GetComponent<Animator>(); // Inicializa el Animator
    }

    void Update()
    {
        // Calcula la distancia al jugador
        float distanceToPlayer = Vector3.Distance(transform.position, player.position);

        // Verifica si el jugador está en el área de ataque (detectionRadius2)
        if (distanceToPlayer < detectionRadius2)
        {
            // Activa la animación de Tongue y desactiva otras animaciones
            anim.SetBool("Tongue", true);
            anim.SetBool("Crawl", false);
        }
        else if (distanceToPlayer < detectionRadius)
        {
            // Activa la animación de Crawl cuando el jugador esté en el rango general
            anim.SetBool("Tongue", false);
            anim.SetBool("Crawl", true);
        }
        else
        {
            // Fuera de todos los rangos, se pone Idle
            anim.SetBool("Tongue", false);
            anim.SetBool("Crawl", false);
            anim.SetTrigger("Idle");
        }

        // Movimiento hacia el jugador si está en el rango general (detectionRadius)
        if (distanceToPlayer < detectionRadius && distanceToPlayer >= detectionRadius2)
        {
            Vector3 direction = (player.position - transform.position).normalized;
            movement = new Vector3(direction.x, 0, direction.z);
        }
        else
        {
            movement = Vector3.zero; // Detén el movimiento si está fuera de detectionRadius
        }

        // Gira hacia el jugador
        if (movement != Vector3.zero)
        {
            Quaternion toRotation = Quaternion.LookRotation(movement, Vector3.up);
            transform.rotation = Quaternion.RotateTowards(transform.rotation, toRotation, 200 * Time.deltaTime);
        }

        // Mueve al enemigo
        transform.Translate(movement * speed * Time.deltaTime, Space.World);
    }

    // Dibuja los Gizmos en la vista de escena
    void OnDrawGizmosSelected()
    {
        if (!Application.isPlaying || player == null)
        {
            // Dibujar siempre el radio de detección general en rojo
            Gizmos.color = Color.red;
            Gizmos.DrawWireSphere(transform.position, detectionRadius);

            // Dibujar siempre el radio de detección específico en azul
            Gizmos.color = Color.blue;
            Gizmos.DrawWireSphere(transform.position, detectionRadius2);

            return;
        }

        // Determinar la distancia al jugador
        float distanceToPlayer = Vector3.Distance(transform.position, player.position);

        // Mostrar Gizmo rojo si el jugador está en detectionRadius
        if (distanceToPlayer < detectionRadius)
        {
            Gizmos.color = Color.red;
            Gizmos.DrawWireSphere(transform.position, detectionRadius);
        }

        // Mostrar Gizmo azul si el jugador está en detectionRadius2
        if (distanceToPlayer < detectionRadius2)
        {
            Gizmos.color = Color.blue;
            Gizmos.DrawWireSphere(transform.position, detectionRadius2);
        }
    }
}

