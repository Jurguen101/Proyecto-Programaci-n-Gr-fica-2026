using UnityEngine;

public class NoteObject : MonoBehaviour
{
    [Header("Audio")]
    [Tooltip("Arrastra aquí el clip de sonido para cuando se recoge la nota")]
    public AudioClip sonidoRecoger;

    public void SetNoteTexture(Texture2D nuevaTextura)
    {
        Renderer render = GetComponent<Renderer>();
        if (render != null && render.material != null)
        {
            render.material.SetTexture("_BaseMap", nuevaTextura);
        }
    }

    public void PickUp()
    {
        // 1. LA SOLUCIÓN DEFINITIVA: PlayOneShot directo a la cabeza del jugador
        if (sonidoRecoger != null)
        {
            // Buscamos la cámara principal del jugador
            if (Camera.main != null)
            {
                // Revisamos si la cámara ya tiene un reproductor de audio, si no, se lo ponemos
                AudioSource sourceCamara = Camera.main.GetComponent<AudioSource>();
                if (sourceCamara == null)
                {
                    sourceCamara = Camera.main.gameObject.AddComponent<AudioSource>();
                }

                // Aseguramos que el volumen esté al 100% y en 2D
                sourceCamara.spatialBlend = 0f;
                sourceCamara.volume = 1f;

                // PlayOneShot dispara el sonido y se olvida. No se corta, no requiere cálculos de tiempo.
                sourceCamara.PlayOneShot(sonidoRecoger);

                Debug.Log("[AUDIO EXITOSO] Se envió el sonido a la cámara.");
            }
            else
            {
                Debug.LogError("No hay ninguna cámara con la etiqueta 'MainCamera' en la escena.");
            }
        }
        else
        {
            Debug.LogWarning("Falta asignar el clip de sonido en la nota.");
        }

        // 2. Sumar la nota al contador global
        if (GameManager.Instance != null)
        {
            GameManager.Instance.CollectNote();
        }

        // 3. La nota desaparece del mapa al recogerla
        Destroy(gameObject);
    }
}