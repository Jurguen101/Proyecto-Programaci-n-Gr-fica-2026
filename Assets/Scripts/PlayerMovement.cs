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
    [Tooltip("Fuerza negativa que empuja al jugador hacia los escalones para evitar que flote.")]
    public float stickToGroundForce = -8f;
    public Transform groundCheck;
    public float groundDistance = 0.4f;
    public LayerMask groundMask;

    private Vector3 velocity;
    private bool isGrounded;

    [Header("Audio de Pasos Dinámico")]
    public AudioSource footstepAudioSource;
    public AudioClip[] grassFootstepSounds;
    public AudioClip[] woodFootstepSounds;
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
            velocity.y = stickToGroundForce;
        }

        // 2. INPUT DE MOVIMIENTO (Tu intención)
        float x = Input.GetAxis("Horizontal");
        float z = Input.GetAxis("Vertical");

        Vector3 move = transform.right * x + transform.forward * z;
        bool hasMovementInput = move.magnitude > 0.1f;

        // 3. DECIDIR LA VELOCIDAD ANTES DE MOVERNOS
        bool isTryingToRun = Input.GetKey(KeyCode.LeftShift) && hasMovementInput && !isExhausted;

        if (isTryingToRun)
        {
            currentSpeed = sprintSpeed;
        }
        else
        {
            currentSpeed = walkSpeed;
        }

        // --- EL TRUCO INFALIBLE ---
        // Guardamos exactamente dónde estamos parados AHORA (solo en X y Z)
        Vector3 previousPosition = new Vector3(transform.position.x, 0, transform.position.z);

        // 4. APLICAMOS EL MOVIMIENTO FÍSICO AL MOTOR
        controller.Move(move * currentSpeed * Time.deltaTime);
        velocity.y += gravity * Time.deltaTime;
        controller.Move(velocity * Time.deltaTime);

        // Medimos dónde quedamos DESPUÉS de intentar movernos
        Vector3 currentPosition = new Vector3(transform.position.x, 0, transform.position.z);

        // Si la distancia entre el antes y el después es mayor a casi cero, nos movimos de verdad
        float distanceMoved = Vector3.Distance(previousPosition, currentPosition);
        bool isActuallyMoving = distanceMoved > 0.001f;

        // 5. LÓGICA DE ESTAMINA (Basada en el movimiento real)
        if (isTryingToRun && isActuallyMoving)
        {
            // Solo bajamos la estamina si lograste avanzar físicamente
            currentStamina -= staminaDrainRate * Time.deltaTime;

            if (currentStamina <= 0)
            {
                currentStamina = 0;
                isExhausted = true;
            }
        }
        else
        {
            // Si no estás corriendo, o si un muro te detuvo, regeneras estamina
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

        // 6. LÓGICA DE SONIDO DE PASOS (Basada en el movimiento real)
        if (isGrounded && isActuallyMoving)
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

        Collider[] hitColliders = Physics.OverlapSphere(groundCheck.position, groundDistance + 0.1f);

        foreach (Collider col in hitColliders)
        {
            if (col.gameObject == controller.gameObject) continue;

            if (col.CompareTag("Madera"))
            {
                maderaDetectada = true;
                break;
            }
        }

        if (maderaDetectada)
        {
            currentSounds = woodFootstepSounds;
        }

        if (currentSounds != null && currentSounds.Length > 0 && footstepAudioSource != null)
        {
            int randomIndex = Random.Range(0, currentSounds.Length);
            footstepAudioSource.volume = Random.Range(0.8f, 1f);
            footstepAudioSource.pitch = Random.Range(0.9f, 1.1f);
            footstepAudioSource.PlayOneShot(currentSounds[randomIndex]);
        }
    }

    private void OnDrawGizmos()
    {
        if (groundCheck != null)
        {
            Gizmos.color = Color.yellow;
            Gizmos.DrawWireSphere(groundCheck.position, groundDistance);

            Gizmos.color = Color.red;
            Gizmos.DrawWireSphere(groundCheck.position, groundDistance + 0.1f);
        }
    }
}