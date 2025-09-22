using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.SceneManagement;

public class AgentScript : MonoBehaviour
{
    NavMeshAgent agent;

    [Header("Patrulla")]
    public Transform[] waypoints;
    private int currentWaypoint = 0;

    [Header("Animación")]
    [SerializeField] Animator anim;
    [SerializeField] float velocity;

    [Header("Visión")]
    public RayCastVisionn vision;

    private enum NPCState { Patrol, Chase }
    private NPCState state = NPCState.Patrol;
    private float lostPlayerTimer = 0f;

    private void Awake()
    {
        agent = GetComponent<NavMeshAgent>();
    }

    void Start()
    {
        if (waypoints.Length > 0)
            agent.destination = waypoints[0].position;
    }

    void Update()
    {
        bool canSee = vision != null && vision.CanSeePlayer();

        // Cambios de estado
        if (state == NPCState.Patrol && canSee)
        {
            state = NPCState.Chase;
            anim.SetBool("IsChasing", true);
        }
        else if (state == NPCState.Chase && !canSee)
        {
            lostPlayerTimer += Time.deltaTime;
            if (lostPlayerTimer >= 2f)
            {
                state = NPCState.Patrol;
                lostPlayerTimer = 0f;
                anim.SetBool("IsChasing", false);
                GoToNextWaypoint();
            }
        }
        else if (state == NPCState.Chase && canSee)
        {
            lostPlayerTimer = 0f;
        }

        // Actualizar animación de velocidad
        velocity = agent.velocity.magnitude;
        anim.SetFloat("Speed", velocity);

        // Ejecutar comportamiento según estado
        if (state == NPCState.Patrol)
            Patrol();
        else if (state == NPCState.Chase)
            Chase();
    }

    void Patrol()
    {
        if (waypoints.Length == 0) return;

        if (!agent.pathPending && agent.remainingDistance < 0.5f)
            GoToNextWaypoint();
    }

    void GoToNextWaypoint()
    {
        if (waypoints.Length == 0) return;

        agent.destination = waypoints[currentWaypoint].position;
        currentWaypoint = (currentWaypoint + 1) % waypoints.Length;
    }

    void Chase()
    {
        if (vision != null && vision.player != null)
            agent.destination = vision.player.position;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
            SceneManager.LoadScene("GameOverScene");
    }
}
