using UnityEngine;

public class HandMovement : MonoBehaviour
{
    private Vector3 targetPosition;
    private Transform target;
    public float speed = 1f;

    public void SetTarget(Transform targetTransform)
    {
        target = targetTransform;
    }
    public void SetTargetPosition(Vector3 position)
    {
        targetPosition = position;
    }

    private void Update()
    {
        if (targetPosition == null) return;

        Vector3 direction = (targetPosition - transform.position).normalized;
        transform.position += direction * speed * Time.deltaTime;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.transform == target)
        {
            Hit();
        }
        else
        {
            Mis();
        }
    }

    private void Hit()
    {
        Debug.Log("Hand hit collider of the player");
        target.GetComponent<MosquitoHealth>().GameOver();
        Destroy(gameObject);
    }

    private void Mis()
    {
        Debug.Log("Missed hit.");
        Destroy(gameObject);   
    }
}
