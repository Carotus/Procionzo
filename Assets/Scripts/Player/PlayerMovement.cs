using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class PlayerMovement : MonoBehaviour
{

[Header("Stamina & climb")]
    public float currentStamina;
    public bool isClimbing;

    public float climbingStaminaReduction = 1f;

    public bool isEating;

    public float SatietyTimer;

    public float staminaDepMult;

    public float satDepMult;
    public float maxStamina;
    public float StaminaRegenSpeed;
    public LayerMask Wall;
    public Transform orientation;
    public float climbSpeed = 2f;
    public float detectionLength;
    public float sphereCastRadius;
    public float maxWallLookAngle;
    private float currentWallLookAngle;
    private RaycastHit frontWallHit;
    public bool wallFront;

    [Header("HUD")]
    public staminaBar staminabar;
    public GameObject canvas;
    public TextMeshProUGUI staminaText;

    public bool satChanged;
    public SatBar satBar;

    private GameManager gm;
    


    [Header("Basic movement")]
    public float speed = 12f;
    public CharacterController controller;
    Vector3 velocity;
    public float gravity = -10f;
    public Transform groundCheck;
    public float groundDistance = 0.4f;
    public LayerMask groundMask;
    public bool isGrounded;

    [Header("Audio")]

    public AudioSource audioSource;

    public bool isClimbingSoundPlaying;

    public bool isMovingSoundPlaying;
    public AudioClip moveSound;
    public AudioClip climbSound;

    void Start()
    {   
        gm = GameObject.Find("GameManager").GetComponent<GameManager>();
        currentStamina = maxStamina;
        staminabar.SetMaxStamina((float)maxStamina);
    }



    private void WallCheck()
    {
        wallFront = Physics.SphereCast(groundCheck.position, sphereCastRadius, orientation.forward, out frontWallHit, detectionLength, Wall);
        currentWallLookAngle = Vector3.Angle(orientation.forward, -frontWallHit.normal);
    }



    private void StartClimbing()
    {
        isClimbing = true;

    }



    private void ClimbingMovement()
    {
        currentStamina -= climbingStaminaReduction * Time.deltaTime;
        staminabar.SetStamina((float)currentStamina);
        //Debug.Log("stamina: " + currentStamina);

       if(Input.GetKey(KeyCode.W))
        {
        controller.Move(transform.up * climbSpeed * Time.deltaTime);
        PlaySoundClimb();
        //riduzione stamina
        }
        else 
        {
            StopSoundClimb();
        }
    }



    private void StopClimbing()
    {
        isClimbing = false;
        isClimbingSoundPlaying = false;
    }



    void Update()
    {

        if (SatietyTimer > 0) //|| isEating)
        {
            if(satChanged == false)
            {
                Debug.Log("Sat changed");
                satBar.SetMaxSat((float)SatietyTimer);
                satChanged = true;
            }
            else
            {
            satBar.SetCurrentSat((float)SatietyTimer);

            if(SatietyTimer >= 0)
            {
            SatietyTimer -= satDepMult * Time.deltaTime;
            }
            }
        }
        else if(SatietyTimer <= 0)
        {
            satChanged = false;
            currentStamina -= staminaDepMult * Time.deltaTime;
            staminabar.SetStamina((float)currentStamina);
        }
        
        Dead();
        
        UpdateStamina();

        WallCheck();

        if(wallFront && Input.GetKey(KeyCode.LeftShift) && currentWallLookAngle < maxWallLookAngle  && currentStamina > 0)
        {
            StartClimbing();
            
        }
        else
        {
            StopClimbing();
            //StopSoundClimb();
        }     
    }

    void FixedUpdate()
    {

        if(isClimbing == false) 
        {

            //controllo pavimento
            isGrounded = Physics.CheckSphere(groundCheck.position, groundDistance, groundMask);

            if(isGrounded && velocity.y < 0)
            {
            velocity.y = -2f;
            }

            //controllo movimento
            float x = Input.GetAxis("Horizontal");
            float z = Input.GetAxis("Vertical");
            Vector3 move = transform.right * x + transform.forward * z;
            controller.Move(move * speed * Time.deltaTime);
            
            if(isClimbing == false && x != 0 || z != 0)
            {
                PlaySoundMove();
            }
            else
            {
                StopSoundMove();
            }


            //controllo gravità
            velocity.y += gravity * Time.deltaTime;
            controller.Move(velocity * Time.deltaTime);

            //if(currentStamina < maxStamina && isGrounded)
            //{
                //currentStamina += Time.deltaTime * StaminaRegenSpeed;
                //staminabar.SetStamina((float)currentStamina);
            //}

        }
        else if(isClimbing == true)
        {
            ClimbingMovement();
            Debug.Log("Climbing");
        }
        }



    public void UpdateStamina()
    {
        int HUDStamina = Mathf.FloorToInt(currentStamina);
        staminaText.text = HUDStamina.ToString();
    }

    public void StamMaxIncrease()
    {
        staminabar.SetMaxStamina((float)maxStamina);
    }

    void Dead()
    {
        if (currentStamina <= 0)
        {
           gm.LoseScreen();
        }
    }


    void PlaySoundMove()
    {
        if(isMovingSoundPlaying == false)
        {
            audioSource.clip = moveSound;
            audioSource.loop = true;
            audioSource.Play();
            isMovingSoundPlaying = true;
        }
    }

    void StopSoundMove()
    {
        isMovingSoundPlaying = false;
        audioSource.loop = false;
        audioSource.Stop();
    }

    void PlaySoundClimb()
    {
        if(isClimbingSoundPlaying == false)
        {
            audioSource.clip = climbSound;
            audioSource.loop = true;
            audioSource.Play();
            isClimbingSoundPlaying = true;
        }
    }

    void StopSoundClimb()
    {
        isClimbingSoundPlaying = false;
        audioSource.loop = false;
        audioSource.Stop();
    }
}



