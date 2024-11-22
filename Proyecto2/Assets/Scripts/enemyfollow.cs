using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class enemyfollow : MonoBehaviour
{
    public Transform player; // Referencia al jugador
    public float detectionRadius = 5.0f; // Radio de detecci�n general
    public float detectionRadius2 = 4.0f; // Radio de detecci�n para la lengua (Tongue)
    public float speed = 2.0f; // Velocidad de movimiento
    Animator anim;
    public FrogController frogController; // Referencia al controlador de la rana

    private Vector3 movement;
    private bool isChasing = false; // Estado para saber si est� persiguiendo al jugador
    private float lastTurnAngle = 0f; // �ngulo en el que el enemigo realiz� el �ltimo giro

    void Update()
    {
        // Calcula la distancia al jugador
        float distanceToPlayer = Vector3.Distance(transform.position, player.position);

        // Si el jugador est� dentro del radio de detecci�n
        if (distanceToPlayer < detectionRadius)
        {
            if (!isChasing)
            {
                frogController.Crawl(); // Llama al m�todo Crawl() del controlador de la rana
                isChasing = true; // Cambiar estado a perseguir
            }

            // Movimiento hacia el jugador
            Vector3 direction = (player.position - transform.position).normalized;

            // Determina el �ngulo entre la rana y el jugador para saber hacia d�nde girar
            float angle = Vector3.SignedAngle(transform.forward, direction, Vector3.up);

            // Establece un umbral para evitar cambios de direcci�n muy peque�os
            if (Mathf.Abs(angle) > 5f && Mathf.Abs(angle - lastTurnAngle) > 5f)
            {
                // Gira hacia la direcci�n del jugador
                if (angle > 0)
                {
                    frogController.TurnRightWithRotation(angle); // Gira a la derecha
                }
                else
                {
                    frogController.TurnLeftWithRotation(-angle); // Gira a la izquierda
                }

                // Guarda el �ngulo actual despu�s de girar
                lastTurnAngle = angle;
            }

            movement = new Vector3(direction.x, 0, direction.z); // Movimiento en el plano horizontal
        }
        else
        {
            if (isChasing)
            {
                frogController.Idle(); // Llama al m�todo Idle() cuando deja de perseguir
                isChasing = false; // Cambiar estado a inactivo
            }
            movement = Vector3.zero; // Detener el movimiento cuando el jugador est� fuera de alcance
        }

        // Control de la animaci�n de la lengua (Tongue) dentro del radio de detecci�n 2
        if (distanceToPlayer < detectionRadius2)
        {
            frogController.Tongue(); // Llama al m�todo Tongue() cuando el jugador est� dentro del radio de la lengua
        }

        // Mueve la rana directamente usando Translate
        transform.Translate(movement * speed * Time.deltaTime, Space.World);
    }

    // Dibuja los Gizmos en la vista de escena para mostrar los radios de detecci�n
    void OnDrawGizmosSelected()
    {
        // Dibuja el radio de detecci�n general en rojo
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, detectionRadius);

        // Dibuja el radio de detecci�n espec�fico para la lengua en azul
        Gizmos.color = Color.blue;
        Gizmos.DrawWireSphere(transform.position, detectionRadius2);
    }
}
