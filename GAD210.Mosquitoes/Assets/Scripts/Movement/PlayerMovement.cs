using System.Collections;
using UnityEditor;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    [SerializeField] private Transform cam;
    [SerializeField] private float speed = 12;
    [SerializeField] private float boostSpeed = 25;
    [SerializeField] private float mouseSensitivity = 3f;
    [SerializeField] private float xRotation = 0;
    [SerializeField] private float yRotation;
    [SerializeField] private int dodgingDistance = 2;
    [SerializeField] private Vector3 moveDirection = Vector3.zero;
    [SerializeField] private float energy = 5f;
    [SerializeField] private float timeDuration = 3;
    [SerializeField] private bool isBoostActive = false;


    void Start()
    {

        if (cam == null)
        {

            cam = Camera.main.transform;

        }

           
        Cursor.lockState = CursorLockMode.Locked;
    }

    // Update is called once per frame
    void Update()
    {
        energy += Time.deltaTime;
        
        Movement();
        //RotateWithMouse();
        RotateToCamera();
        Dodging();

        if(Input.GetKeyDown(KeyCode.Space) && !isBoostActive && energy >= 8)
        {
            StartCoroutine(Boost());
        }
    }

    void Movement()
    {
        float vertical = Input.GetAxis("Vertical");

        float horizontal = Input.GetAxis("Horizontal");

        //Vector3 forward = cam.forward;

        //Vector3 right = cam.right;

        //forward.y = 0;

        //right.y = 0;

        Vector3 forward = cam.forward;

        Vector3 right = cam.right;

        forward.Normalize();

        right.Normalize();

        moveDirection = forward * vertical + right * horizontal;

        moveDirection.Normalize();

        transform.position += moveDirection * speed * Time.deltaTime;

        // transform.Translate(moveDirection * speed * Time.deltaTime, Space.World);
    }


    void RotateToCamera()
    {

        Vector3 lookDirection = cam.forward;

        lookDirection.y = 0;

        if (lookDirection != Vector3.zero)
        {

            Quaternion targetRotation = Quaternion.LookRotation(lookDirection);

            transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, Time.deltaTime * 5f);


        }




    }








    //void RotateWithMouse()
    //{
    //    float mouseX = Input.GetAxis("Mouse X") * mouseSensitivity;
    //    float mouseY = Input.GetAxis("Mouse Y") * mouseSensitivity;

    //    if(Input.GetKey(KeyCode.Mouse1))
    //    {
    //        cam.Rotate(-mouseY, mouseX, 0);
    //    }
    //    else if(Input.GetKeyUp(KeyCode.Mouse1))
    //    {
    //        Quaternion basic = new(0, 0, 0, 0);
    //        cam.rotation = basic;
    //    }
    //    else
    //    {
    //        transform.Rotate(Vector3.up * mouseX);

    //        yRotation += mouseX;
    //        xRotation -= mouseY;
    //        xRotation = Mathf.Clamp(xRotation, -90f, 90f);
    //        transform.localRotation = Quaternion.Euler(xRotation, yRotation, 0);
    //    }
    //}

    void Dodging()
    {
        //if(Input.GetKeyDown(KeyCode.A) || Input.GetKeyDown(KeyCode.D))
        //{
        //    float horizontal = Input.GetAxis("Horizontal");
        //    Vector3 moveDirection = cam.right * horizontal;
        //    moveDirection.Normalize();
        //    transform.Translate(moveDirection * dodgingDistance, Space.World);
        //}

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
