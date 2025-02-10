using UnityEngine;

public class PickupEat : MonoBehaviour
{

    public PlayerMovement pm;

    public Food foodScript;

    public GameObject player;

    public Transform holdPos;

    public bool canEat;

    private bool isHoldingObject;

    public float throwForce = 500f; 
    public float pickUpRange = 5f; 
    public GameObject heldObj; 
    private Rigidbody heldObjRb;
    private bool canDrop = true; 
    private int LayerNumber; 

    void Start()
    {
        LayerNumber = LayerMask.NameToLayer("HoldLayer"); 

        
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
            if (Input.GetKeyDown(KeyCode.Mouse1) && canDrop == true) 
            {
                StopClipping();
                ThrowObject();
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
        heldObj = null;
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
                pm.isEating = true;

                Debug.Log("Eating");

                if(foodScript.foodHP > 0)
                {
                    foodScript.foodHP -= Time.deltaTime;
                    Debug.Log("foodHP: " + foodScript.foodHP);
                }
                else if(foodScript.foodHP <= 0)
                {
                    
                    isHoldingObject = false;
                    pm.UpdateStamina();
                    canEat = false;
                    pm.maxStamina += foodScript.staminaIncrease;
                    pm.currentStamina = pm.maxStamina;
                    pm.SatietyTimer = foodScript.Sat;
                    pm.StamMaxIncrease();
                    pm.isEating = false;
                    Destroy(heldObj);
                    heldObj = null;
                } 
                
            }
            else 
            {
                pm.isEating = false;
            }
            
            
        }
    }

}
