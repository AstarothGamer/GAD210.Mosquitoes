using TMPro;
using UnityEngine;

public class MosquitoAlign : MonoBehaviour
{
    [Header("Settings")]
    public KeyCode alignKey = KeyCode.F;
    public float rayDistance = 5f;
    public float moveSpeed = 10f;
    public float rotationSpeed = 20f;
    public float surfaceOffset = 0.1f;
    public LayerMask surfaceMask = ~0;

    [Header("Landing")]
    [SerializeField] private bool isAligning = false;
    public Transform targetSurface = null;
    private Vector3 targetLocalPos;
    private Quaternion targetLocalRot;
    public bool isSitting = false;
    [SerializeField] private TMP_Text interactionPanel;

    [SerializeField] private Abilities ability;
    [SerializeField] Transform objectToChild;

    void Update()
    {

        if (Input.GetKeyDown(alignKey) && !ability.isBiting)
        {
            if (isAligning)
            {
                isAligning = false;
                return;
            }

            if (isSitting)
            {
                transform.SetParent(null, true);
                targetSurface = null;
                ability.SetTarget(targetSurface);
                isSitting = false;
                transform.localScale = new Vector3(0.1f, 0.1f, 0.1f);
                return;
            }
            StartAlignment();
            ability.SetTarget(targetSurface);
        }

        if (isAligning)
            AlignInLocal();
    }

    private void StartAlignment()
    {
        if (Physics.Raycast(transform.position, transform.forward, out RaycastHit hit,
                            rayDistance, surfaceMask))
        {
            targetSurface = hit.collider.transform;

            Vector3 worldScale = transform.lossyScale;
            var setter= targetSurface.GetComponent<MosquitePositionSetter>().obj;
            if(setter)targetSurface=setter;

            transform.SetParent(targetSurface, true);

            Vector3 parentScale = targetSurface.lossyScale;
            transform.localScale = new Vector3(
                worldScale.x / parentScale.x,
                worldScale.y / parentScale.y,
                worldScale.z * parentScale.z
            );
            
            isSitting = true;

            Vector3 worldPos = hit.point + hit.normal * surfaceOffset;

            Vector3 fwdProj = Vector3.ProjectOnPlane(transform.forward, hit.normal);
            if (fwdProj.sqrMagnitude < 0.001f)
                fwdProj = Vector3.Cross(hit.normal, Vector3.up);
            fwdProj.Normalize();
            Quaternion worldRot = Quaternion.LookRotation(fwdProj, hit.normal);

            targetLocalPos = targetSurface.InverseTransformPoint(worldPos);
            targetLocalRot = Quaternion.Inverse(targetSurface.rotation) * worldRot;

            isAligning = true;
        }
        else
        {
            Debug.Log("MosquitoAlign: nothing found");
        }
    }

    private void AlignInLocal()
    {
        transform.localPosition = Vector3.MoveTowards(
            transform.localPosition,
            targetLocalPos,
            moveSpeed * Time.deltaTime
        );

        transform.localRotation = Quaternion.Slerp(
            transform.localRotation,
            targetLocalRot,
            Time.deltaTime * rotationSpeed
        );

        bool posDone = Vector3.Distance(transform.localPosition, targetLocalPos) < 0.01f;
        bool rotDone = Quaternion.Angle(transform.localRotation, targetLocalRot) < 0.01f;
        Debug.Log(posDone + "    rot:" + rotDone);
        if (posDone && rotDone)
        {
            isAligning = false;
        }
    }

    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawLine(transform.position, transform.position + transform.forward * rayDistance);
        if (isAligning && targetSurface != null)
        {
            Gizmos.color = Color.green;
            Gizmos.DrawSphere(targetSurface.TransformPoint(targetLocalPos), 0.05f);
        }
    }
}
