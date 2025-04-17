using System.Collections;
using UnityEditor;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    [SerializeField] private Transform cam;
    [SerializeField] private float speed = 4;
    [SerializeField] private float boostSpeed = 8;
    [SerializeField] private float mouseSensitivity = 3f;
    [SerializeField] private float xRotation = 0;
    [SerializeField] private float yRotation;
    [SerializeField] private int dodgingDistance = 2;
    [SerializeField] private Vector3 moveDirection = Vector3.zero;
    [SerializeField] private float energy = 5f;
    [SerializeField] private float timeDuration = 3;
    [SerializeField] private bool isBoostActive = false;
    [SerializeField] private Abilities abilities;
    [SerializeField] private MosquitoAlign aligner;


    void Start()
    {
        cam = Camera.main.transform;
        Cursor.lockState = CursorLockMode.Locked;
    }

    // Update is called once per frame
    void Update()
    {
        energy += Time.deltaTime;


        RotateWithMouse();


        if(abilities.isBiting)
            return;
        
        if (aligner != null && aligner.isSitting)
            return;
        
        Movement();
        Dodging();

        if(Input.GetKeyDown(KeyCode.LeftShift) && !isBoostActive && energy >= 8)
        {
            StartCoroutine(Boost());
        }
    }

    void Movement()
    {
        float vertical = Input.GetAxis("Vertical");

        moveDirection = transform.forward * vertical;

        moveDirection.Normalize();

        transform.Translate(moveDirection * speed * Time.deltaTime, Space.World);
    }

    void RotateWithMouse()
    {
        float mouseX = Input.GetAxis("Mouse X") * mouseSensitivity;
        float mouseY = Input.GetAxis("Mouse Y") * mouseSensitivity;

        if(Input.GetKey(KeyCode.Mouse1))
        {
            cam.Rotate(-mouseY, mouseX, 0);
        }
        else if(Input.GetKeyUp(KeyCode.Mouse1))
        {
            Quaternion basic = new(0, 0, 0, 0);
            cam.rotation = basic;
        }
        else if(!aligner.isSitting)
        {
            transform.Rotate(Vector3.up * mouseX);

            yRotation += mouseX;
            xRotation -= mouseY;
            xRotation = Mathf.Clamp(xRotation, -90f, 90f);
            transform.localRotation = Quaternion.Euler(xRotation, yRotation, 0);
        }
    }

    void Dodging()
    {
        if(Input.GetKeyDown(KeyCode.A) || Input.GetKeyDown(KeyCode.D))
        {
            float horizontal = Input.GetAxis("Horizontal");
            Vector3 moveDirection = cam.right * horizontal;
            moveDirection.Normalize();
            transform.Translate(moveDirection * dodgingDistance, Space.World);
        }
    }

    IEnumerator Boost()
    {
        energy = 0;
        isBoostActive = true;
        float originalSpeed = speed;
        speed = boostSpeed;

        transform.Translate(moveDirection * speed * Time.deltaTime, Space.World);
        yield return new WaitForSeconds(timeDuration);

        speed = originalSpeed;
        isBoostActive = false;
    }
}
