using UnityEngine;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }

    [Header("Configuración de Notas")]
    public GameObject notePrefab;
    public List<Texture2D> texturasDeNotas;
    public int totalNotesToSpawn = 8;
    private int notesCollected = 0;
    private List<NoteSpawnPoint> allSpawnPoints = new List<NoteSpawnPoint>();

    [Header("UI de Notas (Slenderman Style)")]
    public TMPro.TMP_Text noteAlertText;
    public float alertDuration = 3.0f;
    private Coroutine alertCoroutine;

    [Header("Enemigo")]
    public GameObject tachyonEntity;

    [Header("Audio General")]
    public AudioSource ambientAudioSource;

    [Header("Game Over (Estilo Slendytubbies)")]
    public GameObject panelScreamer;
    public CanvasGroup fondoNegroCanvasGroup;
    public GameObject menuGameOverUI;
    public MonoBehaviour playerMovement;
    public MonoBehaviour mouseLookScript;

    [Header("Secuencia Victoria (Falsa Paz)")]
    public CanvasGroup winFadeGroup;
    public CanvasGroup winTextWhite;
    public CanvasGroup winTextRed;
    public GameObject winScreamerFinal;
    public GameObject winMenuFinalUI;
    public AudioSource winMusicSource;

    void Awake()
    {
        if (Instance == null) Instance = this;
        else Destroy(gameObject);
    }

    void Start()
    {
        Time.timeScale = 1f;

        if (panelScreamer != null) panelScreamer.SetActive(false);
        if (menuGameOverUI != null) menuGameOverUI.SetActive(false);
        if (fondoNegroCanvasGroup != null) fondoNegroCanvasGroup.alpha = 0f;
        if (winScreamerFinal != null) winScreamerFinal.SetActive(false);
        if (winMenuFinalUI != null) winMenuFinalUI.SetActive(false);
        if (noteAlertText != null) noteAlertText.gameObject.SetActive(false);

        NoteSpawnPoint[] puntosEncontrados = FindObjectsOfType<NoteSpawnPoint>();
        allSpawnPoints = new List<NoteSpawnPoint>(puntosEncontrados);
        SpawnNotesRandomly();
    }

    void Update()
    {
        // Salida rápida con la tecla Escape
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            BotonSalir();
        }
    }

    void SpawnNotesRandomly()
    {
        if (allSpawnPoints.Count < totalNotesToSpawn || texturasDeNotas.Count < totalNotesToSpawn)
        {
            Debug.LogError("¡Error! Faltan Puntos de Spawn o Texturas asignadas en el GameManager.");
            return;
        }

        // Mezclar puntos de aparición
        for (int i = 0; i < allSpawnPoints.Count; i++)
        {
            NoteSpawnPoint temp = allSpawnPoints[i];
            int randomIndex = Random.Range(i, allSpawnPoints.Count);
            allSpawnPoints[i] = allSpawnPoints[randomIndex];
            allSpawnPoints[randomIndex] = temp;
        }

        // Mezclar texturas
        List<Texture2D> texturasMezcladas = new List<Texture2D>(texturasDeNotas);
        for (int i = 0; i < texturasMezcladas.Count; i++)
        {
            Texture2D temp = texturasMezcladas[i];
            int randomIndex = Random.Range(i, texturasMezcladas.Count);
            texturasMezcladas[i] = texturasMezcladas[randomIndex];
            texturasMezcladas[randomIndex] = temp;
        }

        // Instanciar notas
        for (int i = 0; i < totalNotesToSpawn; i++)
        {
            Transform t = allSpawnPoints[i].transform;
            GameObject notaGO = Instantiate(notePrefab, t.position, t.rotation, t);
            notaGO.transform.localRotation = Quaternion.Euler(90f, 180f, 0f);

            NoteObject scriptNota = notaGO.GetComponent<NoteObject>();
            if (scriptNota != null) scriptNota.SetNoteTexture(texturasMezcladas[i]);
        }
    }

    public void CollectNote()
    {
        notesCollected++;

        // Alerta UI
        if (noteAlertText != null)
        {
            if (alertCoroutine != null) StopCoroutine(alertCoroutine);
            alertCoroutine = StartCoroutine(ShowNoteAlertRoutine());
        }

        // Despertar enemigo
        if (notesCollected == 1 && tachyonEntity != null)
        {
            tachyonEntity.SetActive(true);
        }

        // Comprobar victoria
        if (notesCollected >= totalNotesToSpawn)
        {
            WinGame();
        }
    }

    private System.Collections.IEnumerator ShowNoteAlertRoutine()
    {
        noteAlertText.text = "Nota tomada\n" + notesCollected + " / " + totalNotesToSpawn + " notas tomadas";
        noteAlertText.gameObject.SetActive(true);
        yield return new WaitForSeconds(alertDuration);
        noteAlertText.gameObject.SetActive(false);
    }

    public int GetNotesCollected() => notesCollected;

    public void GameOver()
    {
        StartCoroutine(GameOverSequenceRoutine());
    }

    private System.Collections.IEnumerator GameOverSequenceRoutine()
    {
        if (playerMovement != null) playerMovement.enabled = false;
        if (mouseLookScript != null) mouseLookScript.enabled = false;

        if (ambientAudioSource != null && ambientAudioSource.isPlaying)
        {
            ambientAudioSource.Stop();
        }

        if (noteAlertText != null) noteAlertText.gameObject.SetActive(false);

        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;

        if (panelScreamer != null) panelScreamer.SetActive(true);

        yield return new WaitForSecondsRealtime(1.5f);

        float currentTime = 0f;
        while (currentTime < 1.0f)
        {
            currentTime += Time.unscaledDeltaTime;
            if (fondoNegroCanvasGroup != null) fondoNegroCanvasGroup.alpha = currentTime;
            yield return null;
        }

        if (panelScreamer != null) panelScreamer.SetActive(false);
        if (menuGameOverUI != null) menuGameOverUI.SetActive(true);

        Time.timeScale = 0f;
    }

    public void WinGame()
    {
        StartCoroutine(WinSequenceRoutine());
    }

    private System.Collections.IEnumerator WinSequenceRoutine()
    {
        if (playerMovement != null) playerMovement.enabled = false;
        if (mouseLookScript != null) mouseLookScript.enabled = false;
        if (tachyonEntity != null) tachyonEntity.SetActive(false);
        if (noteAlertText != null) noteAlertText.gameObject.SetActive(false);

        if (ambientAudioSource != null && ambientAudioSource.isPlaying)
        {
            ambientAudioSource.Stop();
        }

        float timer = 0;
        while (timer < 2f)
        {
            timer += Time.deltaTime;
            if (winFadeGroup != null) winFadeGroup.alpha = timer / 2f;
            yield return null;
        }

        yield return new WaitForSeconds(1f);
        timer = 0;
        while (timer < 1.5f)
        {
            timer += Time.deltaTime;
            if (winTextWhite != null) winTextWhite.alpha = timer / 1.5f;
            yield return null;
        }

        yield return new WaitForSeconds(2f);
        timer = 0;
        while (timer < 1.5f)
        {
            timer += Time.deltaTime;
            if (winTextRed != null) winTextRed.alpha = timer / 1.5f;
            yield return null;
        }

        yield return new WaitForSeconds(2.5f);
        if (winScreamerFinal != null) winScreamerFinal.SetActive(true);

        yield return new WaitForSeconds(0.8f);

        if (winScreamerFinal != null) winScreamerFinal.SetActive(false);
        if (winTextWhite != null) winTextWhite.alpha = 0f;
        if (winTextRed != null) winTextRed.alpha = 0f;

        yield return new WaitForSeconds(0.5f);
        if (winMenuFinalUI != null) winMenuFinalUI.SetActive(true);
        if (winMusicSource != null) winMusicSource.Play();

        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
        Time.timeScale = 0f;
    }

    public void BotonReintentar()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    public void BotonSalir()
    {
        Debug.Log("Saliendo del juego...");
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#else
            Application.Quit(); 
#endif
    }
}