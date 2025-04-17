using System.Collections;
using TMPro;
using UnityEngine;

public class Abilities : MonoBehaviour
{
    [Header("Other")]
    [SerializeField] private Stats playerStats;
    [SerializeField] private TMP_Text biteNotificationText;    
    [SerializeField] private GameObject interactionPanel;
    [SerializeField] private MosquitoAlign aligner;
    public bool isBiting = false;
    private EnemiesMovement currentNPC = null;
    private Coroutine bitingCoroutine = null;
    [Header("Settings")]
    public float expGainInterval = 1.0f; 
    public int expPerInterval = 10;     
    public float      rayLength   = 0.2f;
    public LayerMask  surfaceMask = ~0;

    void Start()
    {
        if (interactionPanel != null)
        {
            interactionPanel.SetActive(false);
        }
    }

    void Update()
    {
        Vector3 origin = transform.position;
        Vector3 dir    = -transform.up;

        Debug.DrawRay(origin, dir * rayLength, Color.cyan, 1f);

        if(isBiting && Input.GetKeyDown(KeyCode.E))
        {
            StopBiting();
        }
        else if (currentNPC != null && Input.GetKeyDown(KeyCode.E))
        {
            if (!isBiting)
            {
                StartBiting();
            }
        }
        
        if(aligner.isSitting)
        {
            CheckLegRay();
        }


    }

    void StartBiting()
    {
        if (currentNPC == null)
            return;

        isBiting = true;

        if (biteNotificationText != null)
        {
            biteNotificationText.text = "Biting started";
        }
        StopCoroutine(ClearMessages(biteNotificationText));
        Debug.Log("Bite started.");

        bitingCoroutine = StartCoroutine(BiteCoroutine());
    }

    void StopBiting()
    {
        if (!isBiting)
            return;

        isBiting = false;

        if (bitingCoroutine != null)
        {
            StopCoroutine(bitingCoroutine);
            bitingCoroutine = null;
        }
        Debug.Log("Bite ended.");

        if (biteNotificationText != null)
        {
            biteNotificationText.text = "Biting ended.";
            StartCoroutine(ClearMessages(biteNotificationText));
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

    private void CheckLegRay()
    {
        Vector3 origin = transform.position;
        Vector3 dir    = -transform.up;

        Debug.DrawRay(origin, dir * rayLength, Color.cyan, 1f);

        if (Physics.Raycast(origin, dir, out RaycastHit hit, rayLength, surfaceMask))
        {
            EnemiesMovement npc = hit.collider.GetComponentInParent<EnemiesMovement>();
            Debug.Log($"Hit {hit.collider.name} at {hit.point}, normal {hit.normal}");
            if(npc != null)
            {
                currentNPC = npc;
                if(interactionPanel != null)
                {
                    interactionPanel.SetActive(true);
                }
            }
        }
        else
        {
            currentNPC = null;
            if(interactionPanel != null)
            {
                interactionPanel.SetActive(false);
            }
            Debug.Log("Nothing found");
        }

    }

    IEnumerator ClearMessages(TMP_Text text)
    {
        yield return new WaitForSeconds(2f);
        text.text = "";
    }
}
