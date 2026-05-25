using UnityEngine;

public class MouseLook : MonoBehaviour
{
    [Header("Configuración")]
    public float mouseSensitivity = 150f;
    public Transform playerBody; // Aquí arrastraremos el objeto "Player" en el Inspector

    private float xRotation = 0f;

    void Start()
    {
        // Oculta y bloquea el cursor en el centro de la pantalla
        Cursor.lockState = CursorLockMode.Locked;
    }

    void Update()
    {
        // Capturar el movimiento del mouse
        float mouseX = Input.GetAxis("Mouse X") * mouseSensitivity * Time.deltaTime;
        float mouseY = Input.GetAxis("Mouse Y") * mouseSensitivity * Time.deltaTime;

        // Calcular la rotación vertical y limitarla a 90 grados para no romper el cuello
        xRotation -= mouseY;
        xRotation = Mathf.Clamp(xRotation, -90f, 90f);

        // Aplicar la rotación a la cámara (arriba y abajo)
        transform.localRotation = Quaternion.Euler(xRotation, 0f, 0f);

        // Rotar todo el cuerpo del jugador (izquierda y derecha)
        playerBody.Rotate(Vector3.up * mouseX);
    }
}