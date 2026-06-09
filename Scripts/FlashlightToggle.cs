using UnityEngine;

public class FlashlightToggle : MonoBehaviour
{
    private Light linterna;
    private bool estaEncendida = true;

    void Start()
    {
        linterna = GetComponent<Light>();

        if (linterna == null)
        {
            Debug.LogError("No se encontró el componente Light. Asegúrate de poner este script en el Spotlight.");
        }
    }

    void Update()
    {
        // Presionar la tecla F para encender/apagar
        if (Input.GetKeyDown(KeyCode.F))
        {
            estaEncendida = !estaEncendida;
            linterna.enabled = estaEncendida;

            // Opcional: Aquí podrías añadir un AudioSource.Play() para que suene un "click"
        }
    }
}