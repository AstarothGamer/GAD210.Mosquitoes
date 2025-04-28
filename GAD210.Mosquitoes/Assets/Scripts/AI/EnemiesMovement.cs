using UnityEngine;
using UnityEngine.AI;

public class EnemiesMovement : MonoBehaviour
{
    public Transform[] patrolPoints; 

    private NavMeshAgent agent;

    void Start()
    {
        agent = GetComponent<NavMeshAgent>();

        if (patrolPoints.Length > 0)
        {
            GoToRandomPoint();
        }
    }

    void Update()
    {
        if (!agent.pathPending && agent.remainingDistance < 0.5f)
        {
            GoToRandomPoint();
        }
    }

    void GoToRandomPoint()
    {
        if (patrolPoints.Length == 0)
            return;

        int randomIndex = Random.Range(0, patrolPoints.Length);
        agent.SetDestination(patrolPoints[randomIndex].position);
    }
}

