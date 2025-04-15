using UnityEngine;

public class EnemiesMovement : MonoBehaviour
{
    public Transform[] waypoints;

    public float speed = 2f;
    public float rotationSpeed = 5f;
    public float reachThreshold = 0.1f;

    private int currentWaypointIndex = 0;

    void Update()
    {
        if (waypoints == null || waypoints.Length == 0)
            return;

        Transform targetWaypoint = waypoints[currentWaypointIndex];
        Vector3 direction = targetWaypoint.position - transform.position;
        direction.y = 0;

        if (direction.magnitude < reachThreshold)
        {
            currentWaypointIndex = (currentWaypointIndex + 1) % waypoints.Length;
            return;
        }

        Vector3 moveVector = direction.normalized * speed * Time.deltaTime;
        transform.position += moveVector;

        if (moveVector != Vector3.zero)
        {
            Quaternion toRotation = Quaternion.LookRotation(direction);
            transform.rotation = Quaternion.Slerp(transform.rotation, toRotation, rotationSpeed * Time.deltaTime);
        }
    }
}

