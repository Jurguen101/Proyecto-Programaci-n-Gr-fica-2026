using UnityEngine;

public class NoteObject : MonoBehaviour
{
    // Esta función cambia la imagen de la hoja usando la propiedad de URP
    public void SetNoteTexture(Texture2D nuevaTextura)
    {
        Renderer render = GetComponent<Renderer>();
        if (render != null && render.material != null)
        {
            // _BaseMap es el nombre interno de la textura principal en los materiales de URP
            render.material.SetTexture("_BaseMap", nuevaTextura);
        }
    }

    public void PickUp()
    {
        if (GameManager.Instance != null)
        {
            GameManager.Instance.CollectNote();
        }

        // La nota desaparece del mapa al recogerla
        Destroy(gameObject);
    }
}