using UnityEngine;
using UnityEngine.UI;




public class HungerAl : MonoBehaviour
{

    [Header("Hunger Settings")]
    public int maxHunger = 100;
    public int currentHunger = 100;
    public float hungerdecreasingTime = 1f;
    public int hungerdecreasing = 1;
    public LayerMask NPCLayer;

    [Header("UI")]
    [SerializeField] private Scrollbar hungerBar; 
    private float timer = 0f;

    void Start()
    {
        if (hungerBar != null) 
        {

            hungerBar.size = (float)currentHunger / maxHunger;

        }
    }

    void Update()
    {
        timer += Time.deltaTime;

        if (timer >= hungerdecreasingTime)
        {
            timer = 0f;

            currentHunger = Mathf.Max(currentHunger - hungerdecreasing, 0);


            if (hungerBar != null)
            {

                hungerBar.size = (float)currentHunger / maxHunger;

            }
        }
    }

    void OnCollisionEnter(Collision collision)
    {
        if (((1 << collision.gameObject.layer) & NPCLayer) != 0)
        {
            currentHunger = maxHunger;

            if (hungerBar != null)
            { 

                hungerBar.size = (float)currentHunger / maxHunger; 



            }


        }


    }


}