using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public float speed = 12f;
    public CharacterController controller;

    //variabili per la gravità
    Vector3 velocity;
    public float gravity = -10f;

    public Transform groundCheck;

    public AudioSource footSteps;

    public float groundDistance = 0.4f;
    public LayerMask groundMask;
    bool isGrounded;
    //

    void Start()
    {
    }

    void Update()
    {
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

        if (x > 0 || z > 0)
        {
            Debug.Log("bip");
            footSteps.enabled = true;
        }
        else
        {
            Debug.Log("no bip");
            footSteps.enabled = false;
        }
        //controllo gravità
        velocity.y += gravity * Time.deltaTime;
        controller.Move(velocity * Time.deltaTime);
        
    }

}
