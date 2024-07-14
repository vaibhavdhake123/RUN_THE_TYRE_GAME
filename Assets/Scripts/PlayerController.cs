using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    private CharacterController controller;

    private Vector3 direction;

    public float forwardSpeed;

    public float maxSpeed;

    private int desiredLane = 1;

    public float laneDistance = 4;

    public float jumpForce;

    public float Gravity = -20;

    public bool isGrounded;

    public LayerMask groundLayer;

    public Transform groundCheck;

    public Animator animator;

    private bool isSliding = false;

    void Start()
    {
        controller = GetComponent<CharacterController>();
        
    }
    
    void Update()
    {
        if (!Playermanger.isGameStarted)
            return;


        if (forwardSpeed < maxSpeed)
            forwardSpeed += 0.1f * Time.deltaTime;


         animator.SetBool("isGameStarted", true);

         direction.z = forwardSpeed;

         direction.y += Gravity * Time.deltaTime;

           if (controller.isGrounded)
           {
               if (SwipeManger.swipeUp)
               {
                   Jump();
               }
           }
      
          isGrounded = Physics.CheckSphere(groundCheck.position, 0.15f, groundLayer);
          animator.SetBool("isGrounded", isGrounded);
        

        if(SwipeManger.swipeDown && !isSliding) 
        {
            StartCoroutine(Slide());
        }

        if (SwipeManger.swipeRight)
        {
            desiredLane++;
            if(desiredLane == 3) 
              desiredLane = 2;
        }
        if (SwipeManger.swipeLeft)
        {
            desiredLane--;
            if (desiredLane == -1)
                desiredLane = 0;
        }

        Vector3 targetPostion = transform.position.z * transform.forward + transform.position.y * transform.up;

        if(desiredLane == 0)
        {
            targetPostion += Vector3.left * laneDistance;
        }else if(desiredLane == 2)
        {
            targetPostion += Vector3.right * laneDistance;
        }

        if (transform.position == targetPostion)
            return;
        Vector3 diff = targetPostion - transform.position;
        Vector3 moveDir = diff.normalized * 25 * Time.deltaTime;
        if(moveDir.sqrMagnitude < diff.sqrMagnitude)
            controller.Move(moveDir);
        else
            controller.Move(diff);

    }
    private void FixedUpdate()
    {
        if(!Playermanger.isGameStarted)
            return;
        controller.Move(direction * Time.fixedDeltaTime);
    }

    private void Jump()
    {
        direction.y = jumpForce;
    }

    private void OnControllerColliderHit(ControllerColliderHit hit)
    {
        if(hit.transform.tag == "Obstacle")
        {
            Playermanger.gameOver = true;
            FindObjectOfType<AudioManger>().PlaySound("GameOver");
        }
    }

    private IEnumerator Slide()
    {
        isSliding = true;
        animator.SetBool("isSliding", true);
        controller.center = new Vector3(0, -0.5f, 0);
        controller.height = 1;

        yield return new WaitForSeconds(1.3f);
        controller.center = new Vector3(0,0,0);
        controller.height = 2;

        animator.SetBool("isSliding", false);

        isSliding = false;
    }
}

