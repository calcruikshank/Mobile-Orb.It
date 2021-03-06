﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class MoonShotController : MonoBehaviour
{

    public Rigidbody2D moonRB;
    
    public float appliedForce = 0f;
    public bool ableToShoot = true;
    public float chargeTimer = 0f;
    public Vector2 direction;
    public float startingVelocity;
    public GameObject blackHolePrefab;
    public GameObject newBlackHole;

    public GameObject explosionPrefab;
    public GameObject newExplosion;

    public float topSpeed = 80f;
    public bool moonCollided = false;

    public bool addedMoonToVictoryInt = false;

    public MoonJuice moonJuice;

    private bool _MouseDown;
    private bool _MouseUp;

    void Awake()
    {
        moonCollided = false;
        moonRB.AddForce(-transform.up * startingVelocity * 3.3f, ForceMode2D.Impulse);
        
    }


    void Start()
    {
        moonJuice = FindObjectOfType<MoonJuice>();
    }
    // Update is called once per frame
    void Update()
    {
        
        //Debug.Log(moonRB.velocity.magnitude);
        if (Input.GetButtonDown("Fire1") && moonJuice.currentJuice >= 0)
        {

            Vector3 mousePosition = GetMousePosition();
            newBlackHole = Instantiate(blackHolePrefab, new Vector3(mousePosition.x, mousePosition.y, 0f), Quaternion.identity);
            
            if (!_MouseDown)
            {
                AudioManager._Main.PlayBlackholeStart();
                _MouseDown = true;
            }

            _MouseUp = false;
        }
        

        if (Input.GetButtonUp("Fire1") || moonJuice.currentJuice < 0)
        {
            //GetDirection();
            //MoonShot();
            Destroy(newBlackHole);

            if (!_MouseUp)
            {
                AudioManager._Main.PlayBlackholeRelease();
                _MouseUp = true;
            }
            _MouseDown = false;

        }
        if (Input.GetButton("Fire1") && newBlackHole != null)
        {
            
            if (moonJuice.currentJuice >= 0f)
            {
                SetBlackHoleTransform();
                
            }


        }

        //AudioManager._Main._InVictoryOrbit = addedMoonToVictoryInt;

    }
    public void FixedUpdate()
    {
        if (Input.GetButton("Fire1") && newBlackHole != null)
        {
            moonJuice.currentJuice -= 1f;
            if (moonJuice.currentJuice >= 0f)
            {
                SetBlackHoleTransform();
                GetDirection();
                MoonShot();
            }
            

        }

    }

    public void MoonShot()
    {
        appliedForce = 1.65f;
        moonRB.AddForce(direction * appliedForce, ForceMode2D.Impulse);
        
        
    }

    public void GetDirection()
    {
        Vector3 mousePosition = GetMousePosition();

        direction = new Vector2(mousePosition.x - transform.position.x, mousePosition.y - transform.position.y).normalized;
        //transform.right = direction;
    }

    public Vector3 GetMousePosition()
    {
        Vector3 mousePosition = Input.mousePosition;
        mousePosition = Camera.main.ScreenToWorldPoint(mousePosition);
        
        return mousePosition;
    }

    public void SetBlackHoleTransform()
    {
        Vector3 blackHolePosition = GetMousePosition();
        blackHolePosition = new Vector3(blackHolePosition.x, blackHolePosition.y, 0f);
        newBlackHole.transform.position = blackHolePosition;
    }

    public void MoonCollision(Transform otherCollision)
    {
        if (moonCollided == false)
        {
            newExplosion = Instantiate(explosionPrefab, new Vector3(transform.position.x, transform.position.y, 0f), Quaternion.identity);
            newExplosion.transform.up = newExplosion.transform.position - otherCollision.position;
            gameObject.SetActive(false);
            moonCollided = true;
        }
        

        if (newBlackHole != null)
        {
            Destroy(newBlackHole);
        }
       
        
    }

}
