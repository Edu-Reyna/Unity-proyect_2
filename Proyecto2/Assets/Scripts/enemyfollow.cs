using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class enemyfollow : MonoBehaviour
{
    public Transform player; // Referencia al jugador
    public float detectionRadius = 5.0f; // Radio de detección general
    public float detectionRadius2 = 4.0f; // Radio de detección para la lengua (Tongue)
    public float speed = 2.0f; // Velocidad de movimiento
    Animator anim;
    public FrogController frogController; // Referencia al controlador de la rana

    private Vector3 movement;
    private bool isChasing = false; // Estado para saber si está persiguiendo al jugador
    private float lastTurnAngle = 0f; // Ángulo en el que el enemigo realizó el último giro

    void Update()
    {
        // Calcula la distancia al jugador
        float distanceToPlayer = Vector3.Distance(transform.position, player.position);

        // Si el jugador está dentro del radio de detección
        if (distanceToPlayer < detectionRadius)
        {
            if (!isChasing)
            {
                frogController.Crawl(); // Llama al método Crawl() del controlador de la rana
                isChasing = true; // Cambiar estado a perseguir
            }

            // Movimiento hacia el jugador
            Vector3 direction = (player.position - transform.position).normalized;

            // Determina el ángulo entre la rana y el jugador para saber hacia dónde girar
            float angle = Vector3.SignedAngle(transform.forward, direction, Vector3.up);

            // Establece un umbral para evitar cambios de dirección muy pequeños
            if (Mathf.Abs(angle) > 5f && Mathf.Abs(angle - lastTurnAngle) > 5f)
            {
                // Gira hacia la dirección del jugador
                if (angle > 0)
                {
                    frogController.TurnRightWithRotation(angle); // Gira a la derecha
                }
                else
                {
                    frogController.TurnLeftWithRotation(-angle); // Gira a la izquierda
                }

                // Guarda el ángulo actual después de girar
                lastTurnAngle = angle;
            }

            movement = new Vector3(direction.x, 0, direction.z); // Movimiento en el plano horizontal
        }
        else
        {
            if (isChasing)
            {
                frogController.Idle(); // Llama al método Idle() cuando deja de perseguir
                isChasing = false; // Cambiar estado a inactivo
            }
            movement = Vector3.zero; // Detener el movimiento cuando el jugador esté fuera de alcance
        }

        // Control de la animación de la lengua (Tongue) dentro del radio de detección 2
        if (distanceToPlayer < detectionRadius2)
        {
            frogController.Tongue(); // Llama al método Tongue() cuando el jugador está dentro del radio de la lengua
        }

        // Mueve la rana directamente usando Translate
        transform.Translate(movement * speed * Time.deltaTime, Space.World);
    }

    // Dibuja los Gizmos en la vista de escena para mostrar los radios de detección
    void OnDrawGizmosSelected()
    {
        // Dibuja el radio de detección general en rojo
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, detectionRadius);

        // Dibuja el radio de detección específico para la lengua en azul
        Gizmos.color = Color.blue;
        Gizmos.DrawWireSphere(transform.position, detectionRadius2);
    }
}
