using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(CharacterController))]
public class PlayerMovement : MonoBehaviour
{
    [Header("Referencias Principales")]
    public CharacterController controller;

    [Header("Velocidades de Movimiento")]
    public float walkSpeed = 4.0f;
    public float sprintSpeed = 8.5f;
    private float currentSpeed;

    [Header("Sistema de Estamina")]
    public float maxStamina = 100f;
    public float currentStamina;
    public float staminaDrainRate = 20f;
    public float staminaRegenRate = 15f;
    public float minStaminaToRun = 30f;

    public Slider staminaBar;
    private bool isExhausted = false;

    [Header("Físicas y Entorno")]
    public float gravity = -9.81f;
    public Transform groundCheck;
    public float groundDistance = 0.4f;
    public LayerMask groundMask;

    private Vector3 velocity;
    private bool isGrounded;

    [Header("Audio de Pasos Dinámico")]
    public AudioSource footstepAudioSource;
    public AudioClip[] grassFootstepSounds; // Sonidos para el bosque/pasto
    public AudioClip[] woodFootstepSounds;  // Sonidos para la cabaña/madera
    public float walkStepInterval = 0.5f;
    public float sprintStepInterval = 0.3f;
    private float stepTimer;

    void Start()
    {
        if (controller == null)
        {
            controller = GetComponent<CharacterController>();
        }

        currentStamina = maxStamina;
        if (staminaBar != null)
        {
            staminaBar.maxValue = maxStamina;
            staminaBar.value = maxStamina;
        }

        currentSpeed = walkSpeed;
    }

    void Update()
    {
        // 1. GRAVEDAD Y DETECCIÓN DE SUELO
        isGrounded = Physics.CheckSphere(groundCheck.position, groundDistance, groundMask);

        if (isGrounded && velocity.y < 0)
        {
            velocity.y = -2f;
        }

        // 2. INPUT DE MOVIMIENTO
        float x = Input.GetAxis("Horizontal");
        float z = Input.GetAxis("Vertical");

        Vector3 move = transform.right * x + transform.forward * z;
        bool isMoving = move.magnitude > 0.1f;

        // 3. LÓGICA DE ESTAMINA Y CORRER
        if (Input.GetKey(KeyCode.LeftShift) && isMoving && !isExhausted)
        {
            currentSpeed = sprintSpeed;
            currentStamina -= staminaDrainRate * Time.deltaTime;

            if (currentStamina <= 0)
            {
                currentStamina = 0;
                isExhausted = true;
            }
        }
        else
        {
            currentSpeed = walkSpeed;

            if (currentStamina < maxStamina)
            {
                currentStamina += staminaRegenRate * Time.deltaTime;

                if (isExhausted && currentStamina >= minStaminaToRun)
                {
                    isExhausted = false;
                }
            }
        }

        if (currentStamina > maxStamina) currentStamina = maxStamina;
        if (staminaBar != null) staminaBar.value = currentStamina;

        // 4. APLICAR MOVIMIENTO FÍSICO
        controller.Move(move * currentSpeed * Time.deltaTime);
        velocity.y += gravity * Time.deltaTime;
        controller.Move(velocity * Time.deltaTime);

        // 5. LÓGICA DE SONIDO DE PASOS
        if (isGrounded && isMoving)
        {
            stepTimer -= Time.deltaTime;

            if (stepTimer <= 0f)
            {
                PlayFootstepSound();
                stepTimer = (currentSpeed == sprintSpeed) ? sprintStepInterval : walkStepInterval;
            }
        }
        else
        {
            stepTimer = 0f;
        }
    }

    void PlayFootstepSound()
    {
        AudioClip[] currentSounds = grassFootstepSounds;
        bool maderaDetectada = false;

        // Usamos una esfera de detección en los pies, idéntica a la del isGrounded
        Collider[] hitColliders = Physics.OverlapSphere(groundCheck.position, groundDistance + 0.1f);

        // Revisamos todo lo que estamos pisando en este instante
        foreach (Collider col in hitColliders)
        {
            // Ignoramos el propio colisionador del jugador por seguridad
            if (col.gameObject == controller.gameObject) continue;

            Debug.Log("<color=yellow>TOCANDO CON LOS PIES:</color> " + col.gameObject.name + " | Tag: " + col.tag);

            // Si al menos una cosa que pisamos es madera, cambiamos el sonido
            if (col.CompareTag("Madera"))
            {
                maderaDetectada = true;
                break; // Ya encontramos la madera, no necesitamos seguir buscando
            }
        }

        // Si tocamos madera, cambiamos la lista de audios
        if (maderaDetectada)
        {
            currentSounds = woodFootstepSounds;
        }

        // Reproducimos el audio final
        if (currentSounds != null && currentSounds.Length > 0 && footstepAudioSource != null)
        {
            int randomIndex = Random.Range(0, currentSounds.Length);
            footstepAudioSource.volume = Random.Range(0.8f, 1f);
            footstepAudioSource.pitch = Random.Range(0.9f, 1.1f);
            footstepAudioSource.PlayOneShot(currentSounds[randomIndex]);
        }
    }

    // DIBUJAR LA ESFERA INVISIBLE EN EL EDITOR PARA PODER VERLA
    private void OnDrawGizmos()
    {
        if (groundCheck != null)
        {
            // Dibujamos la esfera de la gravedad (amarilla)
            Gizmos.color = Color.yellow;
            Gizmos.DrawWireSphere(groundCheck.position, groundDistance);

            // Dibujamos la esfera del sonido (roja, un poquito más grande)
            Gizmos.color = Color.red;
            Gizmos.DrawWireSphere(groundCheck.position, groundDistance + 0.1f);
        }
    }

}