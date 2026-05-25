using UnityEngine;
using UnityEngine.AI;
using System.Collections;

[RequireComponent(typeof(NavMeshAgent))]
public class TachyonAI : MonoBehaviour
{
    [Header("Referencias")]
    public Transform player;
    private NavMeshAgent agent;

    [Header("Audio y Sustos")]
    public AudioSource audioSource;
    public AudioClip gritoProximidad;
    public float distanciaGrito = 12f; // Distancia a la que te grita al acercarse
    private bool yaGritoEnEstaCaza = false; // El seguro anti-spam de sonido

    [Header("Efecto Visual (Glitcheado)")]
    public float framesPerSecond = 15f;

    [Header("Estadísticas Base y Máximas")]
    public float minSpeed = 3.0f;
    public float maxSpeed = 12.0f;
    public float catchDistance = 2.5f;

    private bool isHiding = true;
    private float phaseTimer = 0f;
    private bool gameIsOver = false;
    private Renderer[] allRenderers;

    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        allRenderers = GetComponentsInChildren<Renderer>();
        agent.updatePosition = false;
        StartCoroutine(ChoppyMovementRoutine());
    }

    void Update()
    {
        if (GameManager.Instance == null || player == null || gameIsOver) return;

        int notes = GameManager.Instance.GetNotesCollected();
        if (notes == 0) return;

        // 1. CONDICIÓN DE MUERTE (Te alcanzó)
        if (Vector3.Distance(transform.position, player.position) <= catchDistance && !isHiding)
        {
            gameIsOver = true;

            // ¡EL ARREGLO! Cortar el sonido de persecución inmediatamente
            if (audioSource != null && audioSource.isPlaying)
            {
                audioSource.Stop();
            }

            GameManager.Instance.GameOver();
            return;
        }

        ManagePhases(notes);
    }

    void ManagePhases(int notes)
    {
        float aggression = (float)notes / GameManager.Instance.totalNotesToSpawn;
        float chaseDuration = Mathf.Lerp(5f, 25f, aggression);
        float hideDuration = Mathf.Lerp(15f, 3f, aggression);

        agent.speed = Mathf.Lerp(minSpeed, maxSpeed, aggression);
        phaseTimer += Time.deltaTime;

        if (isHiding)
        {
            if (phaseTimer >= hideDuration)
            {
                isHiding = false;
                phaseTimer = 0f;

                // Reseteamos el seguro de grito para que pueda volver a asustarte en este nuevo ataque
                yaGritoEnEstaCaza = false;

                float spawnDist = Mathf.Lerp(30f, 10f, aggression);
                TeleportNearPlayer(spawnDist);

                SetVisuals(true);
                agent.isStopped = false;
            }
        }
        else
        {
            agent.SetDestination(player.position);

            // ==========================================
            // LÓGICA DE GRITO POR PROXIMIDAD
            // ==========================================
            if (!yaGritoEnEstaCaza && Vector3.Distance(transform.position, player.position) <= distanciaGrito)
            {
                if (audioSource != null && gritoProximidad != null)
                {
                    audioSource.PlayOneShot(gritoProximidad);
                    yaGritoEnEstaCaza = true; // Bloqueamos para que no vuelva a gritar hasta que se oculte
                }
            }

            if (phaseTimer >= chaseDuration && notes < GameManager.Instance.totalNotesToSpawn)
            {
                isHiding = true;
                phaseTimer = 0f;
                SetVisuals(false);
                agent.isStopped = true;
            }
        }
    }

    IEnumerator ChoppyMovementRoutine()
    {
        while (true)
        {
            if (!gameIsOver && !isHiding)
            {
                transform.position = agent.nextPosition;
            }
            yield return new WaitForSeconds(1f / framesPerSecond);
        }
    }

    void TeleportNearPlayer(float distance)
    {
        Vector2 randomCircle = Random.insideUnitCircle.normalized * distance;
        Vector3 spawnDirection = -player.forward * distance;
        Vector3 targetPos = player.position + spawnDirection + new Vector3(randomCircle.x, 0, randomCircle.y);

        NavMeshHit hit;
        if (NavMesh.SamplePosition(targetPos, out hit, 10f, NavMesh.AllAreas))
        {
            agent.Warp(hit.position);
            agent.nextPosition = hit.position;
            transform.position = hit.position;
        }
    }

    void SetVisuals(bool state)
    {
        foreach (Renderer r in allRenderers)
        {
            r.enabled = state;
        }
    }
}