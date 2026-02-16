using UnityEngine;
using UnityEngine.AI;

public class Target : MonoBehaviour
{
    private NavMeshAgent agent;
    private Animator animator;

    public Transform player;

    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        animator = GetComponentInChildren<Animator>();

        agent.stoppingDistance = 2f;
    }

    void Update()
    {
        if (player == null) return;

        float distance = Vector3.Distance(transform.position, player.position);

        if (distance > agent.stoppingDistance)
        {
            agent.SetDestination(player.position);
        }
        else
        {
            agent.ResetPath();
        }

        // Animation based on movement speed
        float speed = agent.velocity.magnitude;
        animator.SetFloat("State", speed);
    }
}