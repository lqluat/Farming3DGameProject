using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MovementStateManager : MonoBehaviour
{
    public float moveSpeed = 0f;
    public float jumpHeight = 2f;
    [HideInInspector] public Vector3 direction;
    float hzIn, vtIn;
    CharacterController controller;
    Animator animator;
    PlayerInteraction playerInteraction;

    [SerializeField] Transform checkGround;
    [SerializeField] LayerMask groundMask;
    Vector3 Position;

    [SerializeField] float gravity = -9.81f;
    Vector3 velocity;

    //Move Variables
    public bool CheckMove(KeyCode check)
    {
        if(Input.GetKey(check)) return true;
        return false;
    }
    bool forwards, left, right, backwards, leftForwards, rightForwards, leftBackwards, rightBackwards;

    // Start is called before the first frame update
    void Start()
    {
        controller = GetComponent<CharacterController>();    
        animator = GetComponent<Animator>();
        playerInteraction = GetComponentInChildren<PlayerInteraction>();
        Position = transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        //Set Move variables
        forwards = (CheckMove(KeyCode.W) || CheckMove(KeyCode.UpArrow));
        left = (CheckMove(KeyCode.A) || CheckMove(KeyCode.LeftArrow));
        right = (CheckMove(KeyCode.D) || CheckMove(KeyCode.RightArrow));
        backwards = (CheckMove(KeyCode.S) || CheckMove(KeyCode.DownArrow));
        leftForwards = ((CheckMove(KeyCode.W) && CheckMove(KeyCode.A)) || (CheckMove(KeyCode.UpArrow) && CheckMove(KeyCode.LeftArrow)));
        rightForwards = ((CheckMove(KeyCode.W) && CheckMove(KeyCode.D)) || (CheckMove(KeyCode.UpArrow) && CheckMove(KeyCode.RightArrow)));
        leftBackwards = ((CheckMove(KeyCode.S) && CheckMove(KeyCode.A)) || (CheckMove(KeyCode.DownArrow) && CheckMove(KeyCode.LeftArrow)));
        rightBackwards = ((CheckMove(KeyCode.S) && CheckMove(KeyCode.D)) || (CheckMove(KeyCode.DownArrow) && CheckMove(KeyCode.RightArrow)));
        //Move
        try
        {
            GetDirectionAndMove();
        } catch (Exception ex)
        {
            transform.position = Position;
            Debug.LogException(ex);
        }
        
        //Interact
        Interact();


        //Debugging purposes only
        //Skip the time when the right square bracket is pressed
        if (Input.GetKey(KeyCode.RightBracket))
        {
            TimeManager.Instance.Tick();
        }
    }

    public void Interact()
    {
        //Tool interaction
        if (Input.GetButtonDown("Fire1"))
        {
            //Interact
            playerInteraction.Interact();
        }

        //Item interaction
        if (Input.GetButtonDown("Fire2"))
        {
            playerInteraction.ItemInteract();
        }
    }
    void GetDirectionAndMove()
    {
        //Invisible Mouse
        //Cursor.visible = false;

        hzIn = Input.GetAxis("Horizontal");
        vtIn = Input.GetAxis("Vertical");

        direction = transform.forward * vtIn + transform.right * hzIn;
        Vector3 dir = direction * moveSpeed * Time.deltaTime;
        controller.Move(dir);
        //Movement
        if (forwards) animator.SetBool("Move Forwards", true);
        else animator.SetBool("Move Forwards", false);
        if (right) animator.SetBool("Move Right", true);
        else animator.SetBool("Move Right", false);
        if (rightForwards) animator.SetBool("Move Forwards Right", true);
        else animator.SetBool("Move Forwards Right", false);
        if (left) animator.SetBool("Move Left", true);
        else animator.SetBool("Move Left", false);
        if (leftForwards) animator.SetBool("Move Forwards Left", true);
        else animator.SetBool("Move Forwards Left", false);
        if (backwards) animator.SetBool("Move Backwards", true);
        else animator.SetBool("Move Backwards", false);
        if (rightBackwards) animator.SetBool("Move Backwards Right", true);
        else animator.SetBool("Move Backwards Right", false);
        if (leftBackwards) animator.SetBool("Move Backwards Left", true);
        else animator.SetBool("Move Backwards Left", false);

        //Run
        if (Input.GetKey(KeyCode.LeftShift) && !CheckMove(KeyCode.S))
        {
            if (moveSpeed <= 5f) moveSpeed += 2 * Mathf.Abs(Physics.gravity.y) * Time.deltaTime;
            else moveSpeed = 5f;
            animator.SetBool("Running", true);
        }
        else
        {
            if (moveSpeed <= 2f) moveSpeed += Mathf.Abs(Physics.gravity.y) * Time.deltaTime;
            else moveSpeed = 2f;
            animator.SetBool("Running", false);
        }
        if (!Input.anyKey)
        { 
            moveSpeed = 0f;
        }
        //Walk
        animator.SetFloat("Speed", dir.magnitude);
        Gravity(CheckGrounded());
    }
    bool CheckGrounded()
    {
        return (Physics.CheckSphere(checkGround.position, 0.5f, groundMask));
    }
    void Gravity(bool isGrounded)
    {
        velocity.y += gravity * Time.deltaTime;
        if (velocity.y < Mathf.Sqrt(gravity * -2f * jumpHeight) - 2) animator.SetBool("Jumping", false);
        if (velocity.y < -100)
        {
            throw new Exception("You fell off this map! You Died!!!!!!!");
        }
        if (isGrounded)
        {
            if (velocity.y < 0f)
            {
                velocity.y = -2f;
            }
            if (Input.GetKey(KeyCode.Space))
            {
                velocity.y = Mathf.Sqrt(gravity * -2f * jumpHeight);
                animator.SetBool("Jumping", true);
            }
        }
        
        controller.Move(velocity*Time.deltaTime);
        Debug.Log($"{velocity.y} - {CheckGrounded()} - ");
    }
}
