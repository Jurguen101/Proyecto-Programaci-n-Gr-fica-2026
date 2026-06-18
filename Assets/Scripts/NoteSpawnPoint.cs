using UnityEngine;

public class NoteSpawnPoint : MonoBehaviour
{


    // Dibuja un ícono visual en el editor de Unity para saber dónde pusiste los puntos
    void OnDrawGizmos()
    {
        Gizmos.color = Color.white;
        Gizmos.DrawCube(transform.position, new Vector3(0.1f, 0.4f, 0.3f));
    }
}