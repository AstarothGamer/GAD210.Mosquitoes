using UnityEngine;

public class EnemiyAttack : MonoBehaviour
{
    public Abilities playersAbility;
    public GameObject handPrefab;
    public float timer;
    public int random;

    void Start()
    {
        random = Random.Range(1, 5);   
    }
    void Update()
    {
        Attack();
    }

    public void Attack()
    {
        if(!playersAbility.isBiting)
        {
            timer = 0f;
            return;
        }

        timer += Time.deltaTime;


        if(timer >= random)
        {
            random = Random.Range(1, 5);
            timer = 0;

            Vector3 playerPositionBite = playersAbility.transform.position;
            Vector3 spawnPosition = playersAbility.transform.position + playersAbility.transform.up * 2;
            
            GameObject hand = Instantiate(handPrefab, spawnPosition, Quaternion.identity);
            HandMovement handMovement = hand.GetComponent<HandMovement>();
            if(handMovement != null)
            {
                handMovement.SetTarget(playersAbility.transform);
                handMovement.SetTargetPosition(playerPositionBite);
            }
        }
    }
}
