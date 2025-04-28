using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class MosquitoMoveNew : MonoBehaviour
{
    [Header("Settings")]
    [SerializeField] private float maxSpeed = 5;
    [SerializeField] private float minSpeed = 1;
    [SerializeField] private float currentSpeed = 1;
    [SerializeField] private float speedStep = 0.1f;
    [SerializeField] private float boostSpeed = 10;
    [SerializeField] private float yRotation;
    [SerializeField] private int dodgingDistance = 2;
    [SerializeField] private float energy = 5f;
    [SerializeField] private float timeDuration = 3;
    [SerializeField] private bool boostCoolDown = true;
    [Header("Other")]
    [SerializeField] private bool isBoostActive = false;
    [SerializeField] private Vector3 moveDirection = Vector3.zero;
    [SerializeField] private Abilities abilities;
    [SerializeField] private MosquitoAlign aligner;
    [SerializeField] private Transform cam;
    public GameObject boostIcon;

    private Rigidbody rb;



    void Start()
    {
        StartCoroutine(CoolDown());

        if (cam == null)
        {

            cam = Camera.main.transform;

        }

        rb = GetComponent<Rigidbody>();
        Cursor.lockState = CursorLockMode.Locked;
    }

    // Update is called once per frame
    void Update()
    {
        float scroll = Input.GetAxis("Mouse ScrollWheel");
        if(scroll > 0f)
            currentSpeed = Mathf.Min(currentSpeed + speedStep, maxSpeed);
        else if(scroll < 0f)
            currentSpeed = Mathf.Max(currentSpeed - speedStep, minSpeed);

        if(scroll != 0f)
            Debug.Log("Speed: " + currentSpeed);


        if (abilities != null && abilities.isBiting)
        {
            return;
        }

        if (aligner != null && aligner.isSitting)
        {
            return;
        }

        RotateToCamera();


        Movement();

        Dodging();

        if (Input.GetKeyDown(KeyCode.LeftShift) && !isBoostActive && !boostCoolDown)
        {
            StartCoroutine(Boost());
        }


    }

    void Movement()
    {
        float vertical = Input.GetAxis("Vertical");
        float horizontal = Input.GetAxis("Horizontal");


        Vector3 forward = cam.forward;
        Vector3 right = cam.right;

        forward.Normalize();
        right.Normalize();


        moveDirection = forward * vertical + right * horizontal;
        moveDirection.Normalize();

        Vector3 movement = moveDirection * currentSpeed * Time.deltaTime;
        rb.MovePosition(rb.position + movement);
    }


    void RotateToCamera()
    {
        Quaternion targetRotation = Quaternion.LookRotation(cam.forward);

        transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, Time.deltaTime * 5f);
    }

    void Dodging()
    {
        if (Input.GetKeyDown(KeyCode.A) || Input.GetKeyDown(KeyCode.D))
        {

            float horizontal = Input.GetAxis("Horizontal");


            Vector3 dodgeDirection = cam.right * horizontal;


            dodgeDirection.y = 0;

            dodgeDirection.Normalize();

            transform.position += dodgeDirection * dodgingDistance;
        }
    }

    IEnumerator Boost()
    {
        boostCoolDown = true;
        isBoostActive = true;
        boostIcon.SetActive(false);

        float originalSpeed = currentSpeed;
        currentSpeed = boostSpeed;

        transform.Translate(moveDirection * currentSpeed * Time.deltaTime, Space.World);
        yield return new WaitForSeconds(timeDuration);

        currentSpeed = originalSpeed;
        isBoostActive = false;
        StartCoroutine(CoolDown());
    }

    IEnumerator CoolDown()
    {
        yield return new WaitForSeconds(energy);
        boostCoolDown = false;
        boostIcon.SetActive(true);
    }
}