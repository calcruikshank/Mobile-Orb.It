using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerInputController : MonoBehaviour
{
    public Rigidbody2D rb;
    Vector2 movement;
    Vector2 inputMovement;
    Vector2 lastMoveDir;
    Vector2 lookVector;
    Vector2 lastLookedPosition;
    Vector2 diveDir;
    float moveSpeed = 8f;
    float diveSpeed = 30f;
    public Transform rightHand;
    public Transform leftHand;
    public float punchSpeed = 80f;
    public float returnSpeed = 10f;

    public bool punchedRight = false;
    public bool punchedLeft = false;
    public bool returningRight = false;
    public bool returningLeft = false;


    private State state;
    private enum State
    {
        WithoutBall,
        WithBall,
        Knockback,
        Diving
    }

    void Awake()
    {
        state = State.WithoutBall;
    }
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        //Debug.Log(state);
        switch (state)
        {
            case State.WithoutBall:
                HandleMovement();
                HandleThrowHands();
                break;
            case State.Diving:
                HandleDiving();
                break;
        }
    }

    void FixedUpdate()
    {
        switch (state)
        {
            case State.WithoutBall:
                FixedHandleMovement();
                break;
            case State.Diving:
                FixedHandleDiving();
                break;
        }
    }


    public void HandleMovement()
    {
        faceJoystick();
        movement.x = inputMovement.x;
        movement.y = inputMovement.y;
        movement = movement;
        if (movement.x != 0 || movement.y != 0)
        {
            lastMoveDir = movement;
        }


    }
    public void FixedHandleMovement()
    {
        rb.velocity = movement * moveSpeed;
    }


    public void HandleThrowHands()
    {
        if (punchedRight == true && returningRight == false)
        {
            ThrowRightHand();
        }
        if (rightHand.localPosition.x >= 1.4f)
        {
            returningRight = true;
        }
        if (returningRight)
        {
            ReturnRightHand();
            punchedRight = false;
        }


        if (punchedLeft == true && returningLeft == false)
        {
            ThrowLeftHand();
        }

        if (leftHand.localPosition.x >= 1.4f)
        {
            returningLeft = true;
        }
        if (returningLeft)
        {
            ReturnLeftHand();
            punchedLeft = false;

        }
    }

    public void ThrowRightHand()
    {
        rightHand.localPosition = Vector3.MoveTowards(rightHand.localPosition, new Vector2(1.4f, .4f), punchSpeed * Time.deltaTime);
    }
    public void ReturnRightHand()
    {
        rightHand.localPosition = Vector3.MoveTowards(rightHand.localPosition, new Vector2(0, 0), returnSpeed * Time.deltaTime);
    }
    public void ThrowLeftHand()
    {
        leftHand.localPosition = Vector3.MoveTowards(leftHand.localPosition, new Vector2(1.4f, -.4f), punchSpeed * Time.deltaTime);
    }
    public void ReturnLeftHand()
    {
        leftHand.localPosition = Vector3.MoveTowards(leftHand.localPosition, new Vector2(0, 0), returnSpeed * Time.deltaTime);
    }

    public void HandleDiving()
    {
        float diveSpeedMultiplier = 5f;
        diveSpeed -= diveSpeed * diveSpeedMultiplier * Time.deltaTime;
        float diveSpeedMinimum = 1f;
        transform.right = lastMoveDir;
        if (diveSpeed < diveSpeedMinimum)
        {
            state = State.WithoutBall;
            //later check if you have ball
        }
    }

    public void FixedHandleDiving()
    {
        rb.velocity = diveDir * diveSpeed;

    }




    private void OnMove(InputValue value)
    {
        inputMovement = value.Get<Vector2>();
        //Debug.Log(inputMovement);
    }

    private void OnLook(InputValue value)
    {
        lookVector = value.Get<Vector2>();
    }

    private void OnPunchRight()
    {
        if (state != State.WithoutBall)
        {
            return;
        }
        punchedRight = true;
        returningRight = false;
    }
    private void OnDive()
    {
        if (state == State.Diving)
        {
            return;
        }
        diveSpeed = 30f;
        diveDir = lastMoveDir.normalized;
        state = State.Diving;
    }
    
    

    private void OnPunchLeft()
    {
        if (state != State.WithoutBall)
        {
            return;
        }
        punchedLeft = true;
        returningLeft = false;
    }

    public void faceJoystick()
    {
        Vector2 joystickPosition = lookVector.normalized;
        if (joystickPosition.x != 0 || joystickPosition.y != 0)
        {
            lastLookedPosition = joystickPosition;
        }

        //Vector2 direction = new Vector2(mousePosition.x - transform.position.x, mousePosition.y - transform.position.y);
        transform.right = lastLookedPosition;
    }


}
