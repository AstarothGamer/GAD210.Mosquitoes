using System.Collections;
using TMPro;
using UnityEngine;

public class Abilities : MonoBehaviour
{
    [SerializeField] private Stats playerStats;
    [SerializeField] private TMP_Text biteNotificationText;    
    public GameObject interactionPanel;
    public bool isBiting = false;
    private EnemiesMovement currentNPC = null;
    private Coroutine bitingCoroutine = null;

    public float expGainInterval = 1.0f; 
    public int expPerInterval = 10;     

    void Start()
    {
        if (interactionPanel != null)
        {
            interactionPanel.SetActive(false);
        }
    }

    void Update()
    {
        if (currentNPC != null && Input.GetKeyDown(KeyCode.E))
        {
            if (!isBiting)
            {
                StartBiting();
            }
            else
            {
                StopBiting();
            }
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        Debug.Log("NPC can be bitten");
        EnemiesMovement npc = other.GetComponent<EnemiesMovement>();
        if (npc != null)
        {
            currentNPC = npc;
            if (interactionPanel != null)
            {
                interactionPanel.SetActive(true);
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        Debug.Log("NPC lost");
        EnemiesMovement npc = other.GetComponent<EnemiesMovement>();
        if (npc != null && npc == currentNPC)
        {
            currentNPC = null;
            if (interactionPanel != null)
                interactionPanel.SetActive(false);
        }
    }

    void StartBiting()
    {
        if (currentNPC == null)
            return;

        isBiting = true;

        transform.SetParent(currentNPC.transform);

        if (biteNotificationText != null)
        {
            biteNotificationText.text = "Biting started";
        }
        Debug.Log("Bite started.");

        bitingCoroutine = StartCoroutine(BiteCoroutine());
    }

    void StopBiting()
    {
        if (!isBiting)
            return;

        isBiting = false;
        transform.SetParent(null);

        if (bitingCoroutine != null)
        {
            StopCoroutine(bitingCoroutine);
            bitingCoroutine = null;
        }
        Debug.Log("Bite ended.");

        if (biteNotificationText != null)
        {
            biteNotificationText.text = "Biting ended.";
        }

    }

    IEnumerator BiteCoroutine()
    {
        while (isBiting)
        {
            yield return new WaitForSeconds(expGainInterval);
            playerStats.AddExperience(expPerInterval);
        }
    }
}
