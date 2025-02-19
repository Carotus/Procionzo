using UnityEngine;

public class PickupEat : MonoBehaviour
{

    public PlayerMovement pm;

    public Food foodScript;

    public GameObject player;

    public Transform holdPos;

    public GameManager gm;

    public bool canEat;

    private bool isHoldingObject;
    

    public float throwForce = 500f; 
    public float pickUpRange = 5f; 
    public GameObject heldObj; 
    private Rigidbody heldObjRb;

    private float currentSpeed; 
    private bool canDrop = true; 
    private int LayerNumber; 


[Header("Audio")]
    public AudioSource audioSource;

    public bool isPlayingEat;
    public AudioClip eatingSound, pickupSound, dropSound, throwSound;

    void Start()
    {
        LayerNumber = LayerMask.NameToLayer("HoldLayer"); 
        gm = FindAnyObjectByType<GameManager>();
        currentSpeed = pm.speed;
        
    }
    void Update()
    {
        
        
        if (Input.GetKeyDown(KeyCode.E))
        {
            if (heldObj == null) 
            {
                RaycastHit hit;
                if (Physics.Raycast(transform.position, transform.TransformDirection(Vector3.forward), out hit, pickUpRange))
                {
                    if (hit.transform.gameObject.tag == "CanPickUp")
                    {
                        PickUpObject(hit.transform.gameObject);
                    }
                }
            }
            else
            {
                if(canDrop == true)
                {
                    StopClipping();
                    DropObject();
                }
            }
        }
        if (heldObj != null) //if player is holding object
        {
            MoveObject(); //keep object position at holdPos
            if (Input.GetKeyDown(KeyCode.Mouse1) && canDrop == true && foodScript.heavy == false) 
            {
                StopClipping();
                ThrowObject();
            }

            if(foodScript.heavy)
            {
                pm.speed = 0f;
            }

        }

        if(pm.isGrounded == true && pm.isClimbing == false && isHoldingObject)
        {
            canEat = true;
        }
        else 
        {
            canEat = false;
        }
        
        Eat();
        
    }



    void PickUpObject(GameObject pickUpObj)
    {
        if (pickUpObj.GetComponent<Rigidbody>()) 
        {
            heldObj = pickUpObj; //assign heldObj to the object that was hit by the raycast (no longer == null)
            heldObjRb = pickUpObj.GetComponent<Rigidbody>(); //assign Rigidbody
            foodScript = pickUpObj.GetComponent<Food>();
            heldObjRb.isKinematic = true;
            heldObjRb.transform.parent = holdPos.transform; //parent object to holdposition
            heldObj.layer = LayerNumber; //change the object layer to the holdLayer
            Physics.IgnoreCollision(heldObj.GetComponent<Collider>(), player.GetComponent<Collider>(), true);
            isHoldingObject = true;
        }
    }
    void DropObject()
    {
        Physics.IgnoreCollision(heldObj.GetComponent<Collider>(), player.GetComponent<Collider>(), false);
        heldObj.layer = 0; 
        heldObjRb.isKinematic = false;
        heldObj.transform.parent = null; 
        heldObj = null; 
        isHoldingObject = false;
        StopEatSound();
        pm.speed = currentSpeed;
    }




    void MoveObject()
    {
        heldObj.transform.position = holdPos.transform.position;
    }



    void ThrowObject()
    {

        Physics.IgnoreCollision(heldObj.GetComponent<Collider>(), player.GetComponent<Collider>(), false);
        heldObj.layer = 0;
        heldObjRb.isKinematic = false;
        heldObj.transform.parent = null;
        heldObjRb.AddForce(transform.forward * throwForce);
        StopEatSound();
        heldObj = null;
        pm.speed = currentSpeed;
    }


    
    void StopClipping() //function only called when dropping/throwing
    {
        var clipRange = Vector3.Distance(heldObj.transform.position, transform.position); 
        
        RaycastHit[] hits;
        hits = Physics.RaycastAll(transform.position, transform.TransformDirection(Vector3.forward), clipRange);
       
        if (hits.Length > 1)
        {

            heldObj.transform.position = transform.position + new Vector3(0f, -0.5f, 0f);
        }
    }

    void Eat()
    {
        if(canEat == true)
        {
            if(Input.GetButton("Eat"))
            {
                
                PlayEatSound();

                pm.isEating = true;
                

                Debug.Log("Eating");

                if(foodScript.foodHP > 0)
                {
                    foodScript.foodHP -= Time.deltaTime;
                    //Debug.Log("foodHP: " + foodScript.foodHP);
                }
                else if(foodScript.foodHP <= 0)
                {
                    
                    isHoldingObject = false;
                    pm.speed = currentSpeed;
                    pm.UpdateStamina();
                    canEat = false;
                    pm.SatietyTimer += foodScript.Sat;
                    pm.maxStamina += foodScript.staminaIncrease;
                    pm.currentStamina = pm.maxStamina;
                    gm.CurrentScore += 1;
                    pm.StamMaxIncrease();
                    pm.isEating = false;
                    StopEatSound();
                    Destroy(heldObj);
                    heldObj = null;
                    
                    
                } 
                
            }
            else 
            {
                StopEatSound();
                pm.isEating = false;
            }
            
            
        }
    }

    public void PlayEatSound()
    {
        if(isPlayingEat == false)
        {
            audioSource.clip = eatingSound;
            audioSource.loop = true;
            audioSource.Play();
            isPlayingEat = true;
        }
    }

    public void StopEatSound()
    {
        isPlayingEat = false;
        audioSource.Stop();
        audioSource.loop = false;
    }

}
