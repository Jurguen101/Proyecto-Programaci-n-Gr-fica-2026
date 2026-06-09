using UnityEngine;

public class FlickerLight : MonoBehaviour
{
    private Light fireLight;

    [Header("Configuración del Parpadeo")]
    public float minIntensity = 0.5f;
    public float maxIntensity = 2.0f;
    public float flickerSpeed = 10f; // Velocidad del parpadeo

    void Start()
    {
        fireLight = GetComponent<Light>();
    }

    void Update()
    {
        // Usamos Perlin Noise para que el parpadeo sea suave y orgánico, no brusco
        float noise = Mathf.PerlinNoise(Time.time * flickerSpeed, 0f);
        fireLight.intensity = Mathf.Lerp(minIntensity, maxIntensity, noise);
    }
}