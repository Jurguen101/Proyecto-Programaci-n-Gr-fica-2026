using UnityEngine;
using System.Collections.Generic;
using UnityEngine.UI; // Necesario para interactuar con el componente Image de la UI

public class RandomScreamer : MonoBehaviour
{
    [Header("Referencias de UI")]
    [Tooltip("Arrastra aquí el componente Image de tu Canvas (el objeto Imagen_Screamer)")]
    public Image targetUiImage;

    [Header("Colección de Sustos")]
    [Tooltip("Cambia el tamaño de la lista y arrastra aquí todos tus Sprites (Tachyon y otros)")]
    public List<Sprite> listaScreamers;

    [Header("Configuración de Tiempos")]
    public float minWaitTime = 60f;
    public float maxWaitTime = 180f;
    public float flashDuration = 0.15f;

    void Start()
    {
        // Validación de seguridad para que el juego no tire error si olvidaste rellenar la lista
        if (targetUiImage != null && listaScreamers.Count > 0)
        {
            // Nos aseguramos de que el objeto empiece apagado
            targetUiImage.gameObject.SetActive(false);
            StartCoroutine(ScreamerRoutine());
        }
        else
        {
            Debug.LogError("Falta asignar la Imagen de UI o la lista de Screamers está vacía.");
        }
    }

    System.Collections.IEnumerator ScreamerRoutine()
    {
        while (true)
        {
            // 1. Calcular el tiempo de espera aleatorio
            float waitTime = Random.Range(minWaitTime, maxWaitTime);
            yield return new WaitForSeconds(waitTime);

            // 2. Elegir un índice aleatorio de nuestra lista de imágenes
            int randomIndex = Random.Range(0, listaScreamers.Count);

            // 3. Cambiar el sprite de la interfaz por el elegido al azar
            targetUiImage.sprite = listaScreamers[randomIndex];

            // 4. ¡BOO! Mostrar la imagen en pantalla
            targetUiImage.gameObject.SetActive(true);

            // 5. Esperar el destello subliminal
            yield return new WaitForSeconds(flashDuration);

            // 6. Apagar la imagen y reiniciar el ciclo para el próximo susto
            targetUiImage.gameObject.SetActive(false);
        }
    }
}