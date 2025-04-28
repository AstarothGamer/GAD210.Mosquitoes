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
    public float rayLength = 0.2f;
    public LayerMask  surfaceMask = ~0;

    public Transform currentTarget=null;
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
    }

    public void SetTarget(Transform target)
    {
        if (target == null)
        {
            currentNPC = null;
            interactionPanel?.SetActive(false);
            return;
        }

        currentTarget = target;
        var npc = target.GetComponentInParent<EnemiesMovement>();
        if (npc != null)
        {
            currentNPC = npc;
            interactionPanel?.SetActive(true);
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

    IEnumerator ClearMessages(TMP_Text text)
    {
        yield return new WaitForSeconds(2f);
        text.text = "";
    }
}
