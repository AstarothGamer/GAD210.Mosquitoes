using UnityEngine;

public class MosquitoAlign : MonoBehaviour
{
    [Header("Settings")]
    public KeyCode   alignKey      = KeyCode.F;
    public float     rayDistance   = 5f;
    public float     moveSpeed     = 3f;
    public float     rotationSpeed = 10f;
    public float     surfaceOffset = 0.1f;
    public LayerMask surfaceMask   = ~0;

    [Header("Landing")]
    private bool        isAligning     = false;
    private Transform   targetSurface  = null;
    private Vector3     targetLocalPos;
    private Quaternion  targetLocalRot;
    public bool isSitting = false;

    [SerializeField] private Abilities ability;

    void Update()
    {
        if (Input.GetKeyDown(alignKey) && !ability.isBiting)
        {
            if (isAligning)
            {
                isAligning = false;
                return;
            }

            if (targetSurface != null)
            {
                transform.SetParent(null, true);
                targetSurface = null;
                isSitting = false;
                transform.localScale = new Vector3(0.0025f, 0.0025f, 0.0025f);
                return;
            }

            StartAlignment();
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
            transform.SetParent(targetSurface, true);
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
        bool rotDone = Quaternion.Angle(transform.localRotation, targetLocalRot) < 0.1f;
        if (posDone && rotDone)
            isAligning = false;
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
