using UnityEngine;

public class PlayerInteraction : MonoBehaviour
{
    [Header("Configuración de Interacción")]
    public Camera playerCamera;
    public float interactionDistance = 4.0f; // A qué distancia máxima puedes agarrar la nota
    public KeyCode interactKey = KeyCode.E;

    void Start()
    {
        // Si se te olvida asignar la cámara, Unity la buscará automáticamente
        if (playerCamera == null)
        {
            playerCamera = Camera.main;
        }
    }

    void Update()
    {
        // Si el juego está pausado (Game Over o Victoria), no hacer nada
        if (Time.timeScale == 0f) return;

        // Lanzar un rayo invisible desde el centro de la cámara hacia el frente
        RaycastHit hit;
        if (Physics.Raycast(playerCamera.transform.position, playerCamera.transform.forward, out hit, interactionDistance))
        {
            // Si el jugador presiona "E" mientras mira algo
            if (Input.GetKeyDown(interactKey))
            {
                // Verificar si el objeto que miramos tiene tu script 'NoteObject'
                NoteObject nota = hit.collider.GetComponent<NoteObject>();

                if (nota != null)
                {
                    // 1. Le decimos al GameManager que sume la nota
                    GameManager.Instance.CollectNote();

                    // 2. Destruimos el papel del mundo 3D
                    Destroy(hit.collider.gameObject);
                }
            }
        }
    }
}