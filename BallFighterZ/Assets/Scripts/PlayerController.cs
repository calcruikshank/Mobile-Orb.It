using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public Rigidbody2D rb;
    public float moveSpeed = 5f;
    private Vector2 movement;
    private Vector3 diveDir;
    private Vector3 lastMoveDir;
    public float diveSpeed;
    public float punchSpeed = 60f;
    public float returnSpeed = 50f;
    public Transform leftHand;
    public Transform rightHand;

    private float timer = 0.0f;

    public bool punchedLeft = false;
    public bool punchedRight = false;
    public bool returningLeft = false;
    public bool returningRight = false;
    private State state;
    private enum State
    {
        WithoutBall,
        WithBall,
        Knockback,
        Diving
    }


    private void Awake()
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

        switch (state)
        {
            case State.WithoutBall:
                HandleMovement();
                HandleThrowingHands();
                //HandleDivingWithoutBall
                break;
            case State.WithBall:
                HandleMovement();
                //HandleThrowingBall
                //HandleDivingWithBall
                break;
            case State.Knockback:
                HandleMovement();
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
                //FixedHandleThrowingHands();
                //FixedHandleDivingWithoutBall
                break;
            case State.WithBall:
                FixedHandleMovement();
                //HandleThrowingBall
                //FixedHandleDivingWithBall
                break;
            case State.Knockback:
                FixedHandleMovement();
                break;
            case State.Diving:
                FixedHandleDiving();
                break;
        }


    }


    public void HandleMovement()
    {
        faceMouse();
        movement.x = Input.GetAxisRaw("Horizontal");
        movement.y = Input.GetAxisRaw("Vertical");
        movement = movement.normalized;
        if (movement.x != 0 || movement.y != 0)
        {
            lastMoveDir = movement;
        }

        if (Input.GetKeyDown(KeyCode.Space))
        {
            diveDir = lastMoveDir;
            diveSpeed = 25f;
            state = State.Diving;
        }

    }
    public void FixedHandleMovement()
    {
        rb.velocity = movement * moveSpeed;
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


    public void HandleThrowingHands()
    {

        if (Input.GetButtonDown("Fire1") && punchedLeft == false)
        {
            punchedLeft = true;
            returningLeft = false;
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

        if (Input.GetButtonDown("Fire2") && punchedRight == false)
        {
            punchedRight = true;
            returningRight = false;
        }
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

        if (Input.GetButton("Fire2") || Input.GetButton("Fire1"))
        {
            timer += Time.deltaTime;

            if (timer > .2f)
            {
                moveSpeed = 3.5f;
            }
            Debug.Log(moveSpeed);
        }
        if (Input.GetButton("Fire2") == false && Input.GetButton("Fire1") == false)
        {
            moveSpeed = 7f;
            timer = 0;
        }


    }
    public void ThrowLeftHand()
    {
        leftHand.localPosition = Vector3.MoveTowards(leftHand.localPosition, new Vector2(1.4f, -.4f), punchSpeed * Time.deltaTime);
    }
    public void ReturnLeftHand()
    {
        leftHand.localPosition = Vector3.MoveTowards(leftHand.localPosition, new Vector2(0, 0), returnSpeed * Time.deltaTime);
    }

    public void ThrowRightHand()
    {
        rightHand.localPosition = Vector3.MoveTowards(rightHand.localPosition, new Vector2(1.4f, .4f), punchSpeed * Time.deltaTime);
    }
    public void ReturnRightHand()
    {
        rightHand.localPosition = Vector3.MoveTowards(rightHand.localPosition, new Vector2(0, 0), returnSpeed * Time.deltaTime);
    }


    void faceMouse()
    {
        Vector3 mousePosition = Input.mousePosition;
        mousePosition = Camera.main.ScreenToWorldPoint(mousePosition);

        Vector2 direction = new Vector2(mousePosition.x - transform.position.x, mousePosition.y - transform.position.y);
        transform.right = direction;
        //Debug.Log(mousePosition);
    }


}
