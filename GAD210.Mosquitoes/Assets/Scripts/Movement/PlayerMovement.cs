using UnityEditor;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    [SerializeField] private Transform cam;
    [SerializeField] private float speed;
    [SerializeField] private float mouseSensitivity = 3f;
    [SerializeField] private float xRotation = 0;
    [SerializeField] private float yRotation;
    [SerializeField] private int dodgingDistance = 2;
    [SerializeField] private Vector3 moveDirection = Vector3.zero;
    [SerializeField] private float energy = 0f;
    [SerializeField] private float timeDuration;


    void Start()
    {
        cam = Camera.main.transform;
        Cursor.lockState = CursorLockMode.Locked;
    }

    // Update is called once per frame
    void Update()
    {
        energy += Time.deltaTime;
        timeDuration -= Time.deltaTime;
        
        Movement();
        RotateWithMouse();
        Dodging();

        if(Input.GetKeyDown(KeyCode.Space))
        {
            Biting();
        }
    }

    void Movement()
    {
        float vertical = Input.GetAxis("Vertical");
        
        if(Input.GetKeyDown(KeyCode.Mouse1))
        {
            moveDirection = cam.forward * vertical;
        }
        if(!Input.GetKey(KeyCode.Mouse1))
        {
            moveDirection = cam.forward * vertical;
        }

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
        else
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

    void Biting()
    {
        timeDuration = 3;

        if(energy >= 5 && timeDuration > 0)
        {
            energy = 0;
            transform.Translate(moveDirection * speed * 3 * Time.deltaTime, Space.World);
        }     
    }
}
