using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    private CharacterController cc;
    public Animator controller;
    public Transform cam;
    public float speed = 6f;
    public float turnSmoothTime = 0.3f;
    public Transform groundCheck;
    public float groundDistance = 0.4f;
    public LayerMask floorMask;

    private float turnSmoothVelocity;
    private bool isGrounded;
    private Vector3 velocity;
    
    // Start is called before the first frame update
    void Start()
    {
        cc = GetComponent<CharacterController>();
    }

    // Update is called once per frame
    void Update()
    { //Movimiento
        float horizontal = Input.GetAxisRaw("Horizontal");
        float vertical = Input.GetAxisRaw("Vertical");
        Vector3 direction = new Vector3(horizontal, 0, vertical).normalized;

        if (Input.GetButtonDown("Fire1"))
            controller.SetBool("sAttack1", true);

        controller.SetFloat("speed", direction.magnitude);
        if (direction.magnitude >= 0.01f)
        {
            if (direction.magnitude >= 0.6f)
            {
                speed = 7f;
            }
            else
            {
                speed = 2f;
            }
            
            float targetAngle = Mathf.Atan2(direction.x, direction.z) * Mathf.Rad2Deg + cam.eulerAngles.y;
            float angle = Mathf.SmoothDampAngle(transform.eulerAngles.y, targetAngle, ref turnSmoothVelocity,
                turnSmoothTime);
            transform.rotation = Quaternion.Euler(0, angle, 0);

            Vector3 moveDirection = Quaternion.Euler(0f, targetAngle, 0f) * Vector3.forward;
            cc.Move(moveDirection.normalized * speed * Time.deltaTime);
        }

        isGrounded = Physics.CheckSphere(groundCheck.position, groundDistance, floorMask);

        velocity.y += -9.81f * Time.deltaTime;
        if (isGrounded && velocity.y < 0)
        {
            velocity.y = -1.86f;
        }
        cc.Move(velocity * Time.deltaTime);
    }
}
