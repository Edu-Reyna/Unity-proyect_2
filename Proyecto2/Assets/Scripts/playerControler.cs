using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    // Velocidad de movimiento en el plano horizontal (X-Z)
    public float speed;

    // Fuerza del salto inicial
    public float jumpForce;

    // Distancia para detectar si el personaje est� en el suelo
    public float groundDist;

    // Duraci�n m�xima de vuelo permitida en segundos
    public float flyDuration = 2f;

    // Velocidad a la que el personaje se eleva y se mueve durante el vuelo
    public float flySpeed = 2f;

    // Referencia al componente Animator para controlar animaciones
    public Animator animator;

    // Contadores para ataques en combo
    public int combo;
    public bool atacando;

    // Capa que define el terreno para detectar colisiones
    public LayerMask terrainLayer;

    // Referencia al Rigidbody del personaje (f�sica)
    public Rigidbody rb;

    // Referencia al SpriteRenderer para voltear el sprite horizontalmente
    public SpriteRenderer sr;

    // Estado del personaje
    private bool isGrounded;        // Indica si el personaje est� tocando el suelo
    private bool isFlying;          // Indica si el personaje est� actualmente volando
    private float remainingFlyTime; // Tiempo restante permitido para volar

    void Start()
    {
        // Obtener el Rigidbody del objeto
        rb = gameObject.GetComponent<Rigidbody>();

        // Inicializar el tiempo restante de vuelo con el m�ximo permitido
        remainingFlyTime = flyDuration;
    }

    void Update()
    {
        // Comprobar si el personaje est� en el suelo mediante un Raycast
        RaycastHit hit;
        Vector3 castPos = transform.position + Vector3.up * 0.5f; // Origen del Raycast elevado
        isGrounded = Physics.Raycast(castPos, Vector3.down, out hit, groundDist, terrainLayer);

        // Debug para visualizar el Raycast en el editor
        Debug.DrawRay(castPos, Vector3.down * groundDist, Color.red);

        // Si el personaje est� en el suelo, recargar el tiempo de vuelo
        if (isGrounded)
        {
            remainingFlyTime = flyDuration; // Restablecer el tiempo permitido de vuelo
        }

        // Leer entrada de movimiento en los ejes X y Z
        float x = Input.GetAxis("Horizontal"); // Movimiento horizontal
        float z = Input.GetAxis("Vertical");   // Movimiento en profundidad
        Vector3 moveDir = new Vector3(x, 0, z).normalized * speed; // Direccion y velocidad

        // Si no est� volando, aplicar movimiento en el plano horizontal
        if (!isFlying)
        {
            rb.velocity = new Vector3(moveDir.x, rb.velocity.y, moveDir.z);
        }

        // Actualizar animaciones de movimiento seg�n la velocidad
        float currentSpeed = new Vector3(rb.velocity.x, 0, rb.velocity.z).magnitude;
        animator.SetFloat("movement", currentSpeed);

        // Controlar la orientaci�n del sprite seg�n la direcci�n de movimiento
        if (x < 0)
        {
            sr.flipX = true; // Mirar a la izquierda
        }
        else if (x > 0)
        {
            sr.flipX = false; // Mirar a la derecha
        }

        // Manejo del salto y vuelo
        if (Input.GetKeyDown(KeyCode.Space) && isGrounded)
        {
            animator.SetTrigger("jump");
            // Saltar si est� en el suelo
            rb.velocity = new Vector3(rb.velocity.x, jumpForce, rb.velocity.z);

        }

        // Manejar el vuelo mientras la barra espaciadora est� presionada
        if (Input.GetKey(KeyCode.Space) && !isGrounded && remainingFlyTime > 0)
        {
            isFlying = true; // Activar estado de vuelo
            remainingFlyTime -= Time.deltaTime; // Reducir tiempo de vuelo disponible

            // Actualizar velocidad para moverse y elevarse durante el vuelo
            rb.velocity = new Vector3(moveDir.x * flySpeed, flySpeed, moveDir.z * flySpeed);
        }
        else
        {
            // Si la barra espaciadora no est� presionada o el tiempo de vuelo se agot�, terminar vuelo
            isFlying = false;
        }

        // Manejo de combos de ataque
        Combos();
    }

    void Combos()
    {
        // Iniciar un combo al presionar la tecla "C" si no est� atacando
        if (Input.GetKeyDown(KeyCode.C) && !atacando)
        {
            atacando = true;
            animator.SetTrigger("" + combo);
        }
    }

    void Star_Combo()
    {
        // Incrementar el combo si es posible
        atacando = false;
        if (combo < 3)
        {
            combo++;
        }
    }

    void Finish_ani()
    {
        // Terminar animaci�n de ataque y reiniciar el combo
        atacando = false;
        combo = 0;
    }
}
