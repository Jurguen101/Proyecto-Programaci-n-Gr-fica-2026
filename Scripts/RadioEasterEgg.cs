using UnityEngine;

public class RadioEasterEgg : MonoBehaviour
{
    [Header("Referencias de Audio")]
    public AudioSource audioSource;

    [Header("Configuración")]
    public float distanciaInteraccion = 3.5f; // Qué tan cerca debe estar el player para encenderla
    public KeyCode teclaInteraccion = KeyCode.E; // Tecla para interactuar

    private Transform playerTransform;
    private bool estaEncendida = false;

    void Start()
    {
        // Buscar al jugador automáticamente por su Tag por seguridad
        GameObject player = GameObject.FindWithTag("Player");
        if (player != null)
        {
            playerTransform = player.transform;
        }

        // Si no se asignó el AudioSource manualmente, intentamos conseguirlo
        if (audioSource == null)
        {
            audioSource = GetComponent<AudioSource>();
        }
    }

    void Update()
    {
        if (playerTransform == null || audioSource == null) return;

        // Si el juego ya terminó (Muerte o Victoria), apagamos la radio automáticamente
        if (Time.timeScale == 0f && estaEncendida)
        {
            ApagarRadio();
            return;
        }

        // Calcular la distancia física entre la radio y el jugador
        float distancia = Vector3.Distance(transform.position, playerTransform.position);

        // Si está cerca y presiona la tecla de interacción
        if (distancia <= distanciaInteraccion && Input.GetKeyDown(teclaInteraccion))
        {
            ToggleRadio();
        }
    }

    void ToggleRadio()
    {
        estaEncendida = !estaEncendida;

        if (estaEncendida)
        {
            audioSource.Play();
            Debug.Log("¡Radio encendida! Easter Egg activado.");
        }
        else
        {
            audioSource.Stop();
            Debug.Log("Radio apagada.");
        }
    }

    void ApagarRadio()
    {
        estaEncendida = false;
        audioSource.Stop();
    }

    // Dibujar un círculo en el editor para que puedas ver el rango de interacción fácilmente
    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, distanciaInteraccion);
    }
}