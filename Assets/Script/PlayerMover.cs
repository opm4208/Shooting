using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Windows;

public class PlayerMover : MonoBehaviour
{
    [SerializeField] float walkSpeed;
    [SerializeField] float runSpeed;
    //[SerializeField] float moveSpeed;
    [SerializeField] float jumpSpeed;

    private CharacterController controller;
    private Animator animator;
    [SerializeField] private Vector3 moveDir;
    private float moveSpeed=3;
    private float ySpeed = 0;
    private bool isWalking;

    private void Awake()
    {
        controller = GetComponent<CharacterController>();
        animator = GetComponent<Animator>();
    }

    private void Update()
    {
        Move();
        Jump();
    }

    private void Move()
    {
        // controller.Move(moveDir*moveSpeed*Time.deltaTime); 월드 기준

        // 로컬기준 움직임
        if(moveDir.magnitude == 0)  // 안움직임
        {
            moveSpeed = Mathf.Lerp(moveSpeed, 0, 0.5f);
        }
        else if (isWalking)
        {
            moveSpeed = Mathf.Lerp(moveSpeed, walkSpeed, 0.5f);
        }
        else
        {
            moveSpeed = Mathf.Lerp(moveSpeed, runSpeed, 0.5f);
        }

        controller.Move(transform.forward*moveDir.z * moveSpeed * Time.deltaTime);
        controller.Move(transform.right*moveDir.x * moveSpeed * Time.deltaTime);

        //Mathf.Lerp(ySpeed, moveSpeed, Time.deltaTime);

        animator.SetFloat("xSpeed", moveDir.x, 0.1f, Time.deltaTime);
        animator.SetFloat("ySpeed", moveDir.z, 0.1f, Time.deltaTime);
        animator.SetFloat("MoveSpeed",moveSpeed,0.1f, Time.deltaTime);
    }

    private void OnMove(InputValue value)
    {
        Vector2 input = value.Get<Vector2>();
        moveDir = new Vector3(input.x, 0, input.y);
    }

    private void Jump()
    {
        ySpeed += Physics.gravity.y*Time.deltaTime;

        if (GroundCheck() && ySpeed < 0)
            ySpeed = -1;


        controller.Move(Vector3.up*ySpeed*Time.deltaTime);
    }

    private void OnJump(InputValue value)
    {
        if(GroundCheck())
            ySpeed = jumpSpeed;
    }

    private bool GroundCheck()
    {
        RaycastHit hit;
        return Physics.SphereCast(transform.position+ Vector3.up*1, 0.5f, Vector3.down, out hit, 0.6f);
    }

    private void OnWalk(InputValue value)
    {
        isWalking = value.isPressed;
    }
}
