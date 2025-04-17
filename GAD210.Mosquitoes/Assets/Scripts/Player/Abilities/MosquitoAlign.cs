using UnityEngine;

public class MosquitoAlign : MonoBehaviour
{
    [Header("Настройки")]
    public KeyCode   alignKey      = KeyCode.F;
    public float     rayDistance   = 5f;
    public float     moveSpeed     = 3f;
    public float     rotationSpeed = 10f;
    public float     surfaceOffset = 0.1f;
    public LayerMask surfaceMask   = ~0;

    // Состояние посадки
    private bool        isAligning     = false;
    private Transform   targetSurface  = null;
    private Vector3     targetLocalPos;
    private Quaternion  targetLocalRot;
    public bool isSitting = false;

    void Update()
    {
        // Ловим нажатие F
        if (Input.GetKeyDown(alignKey))
        {
            // если сейчас выравниваемся — отменяем посадку
            if (isAligning)
            {
                isAligning = false;
                return;
            }

            // если мы уже «посадились» на поверхность — отцепляемся
            if (targetSurface != null)
            {
                transform.SetParent(null, true);
                targetSurface = null;
                isSitting = false;
                return;
            }

            // иначе — начинаем новую посадку
            StartAlignment();
        }

        // во время посадки двигаемся и поворачиваемся в локальных координатах
        if (isAligning)
            AlignInLocal();
    }

    private void StartAlignment()
    {
        // Raycast вперёд
        if (Physics.Raycast(transform.position, transform.forward, out RaycastHit hit,
                            rayDistance, surfaceMask))
        {
            // сразу делаем поверхностью родителя
            targetSurface = hit.collider.transform;
            transform.SetParent(targetSurface, true);
            isSitting = true;

            // мировая точка чуть над поверхностью
            Vector3 worldPos = hit.point + hit.normal * surfaceOffset;

            // рассчитываем будущую мировую ротацию
            Vector3 fwdProj = Vector3.ProjectOnPlane(transform.forward, hit.normal);
            if (fwdProj.sqrMagnitude < 0.001f)
                fwdProj = Vector3.Cross(hit.normal, Vector3.up);
            fwdProj.Normalize();
            Quaternion worldRot = Quaternion.LookRotation(fwdProj, hit.normal);

            // переводим их в локальные координаты нового родителя
            targetLocalPos = targetSurface.InverseTransformPoint(worldPos);
            targetLocalRot = Quaternion.Inverse(targetSurface.rotation) * worldRot;

            isAligning = true;
        }
        else
        {
            Debug.Log("MosquitoAlign: ничего не попалось вперёд");
        }
    }

    private void AlignInLocal()
    {
        // двигаемся к локальной цели
        transform.localPosition = Vector3.MoveTowards(
            transform.localPosition,
            targetLocalPos,
            moveSpeed * Time.deltaTime
        );

        // поворачиваемся к локальной ротации
        transform.localRotation = Quaternion.Slerp(
            transform.localRotation,
            targetLocalRot,
            Time.deltaTime * rotationSpeed
        );

        // проверяем, всё ли достигли
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
