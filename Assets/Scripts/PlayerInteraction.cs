using UnityEngine;

public class PlayerInteraction : MonoBehaviour
{
    [Header("Configuración de Interacción")]
    public Camera playerCamera;
    public float interactionDistance = 4.0f;
    public KeyCode interactKey = KeyCode.E;

    void Start()
    {
        if (playerCamera == null)
        {
            playerCamera = Camera.main;
        }
    }

    void Update()
    {
        if (Time.timeScale == 0f) return;

        RaycastHit hit;
        if (Physics.Raycast(playerCamera.transform.position, playerCamera.transform.forward, out hit, interactionDistance))
        {
            if (Input.GetKeyDown(interactKey))
            {
                NoteObject nota = hit.collider.GetComponent<NoteObject>();

                if (nota != null)
                {
                    // ¡AQUÍ ESTÁ LA MAGIA!
                    // En lugar de destruir el objeto aquí, llamamos a la función 
                    // que ya tiene todo el código de audio y puntuación configurado.
                    nota.PickUp();
                }
            }
        }
    }
}