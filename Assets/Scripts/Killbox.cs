using UnityEngine;

public class Killbox : MonoBehaviour
{
    public Transform respawnPoint; 
    public GameObject oggetto; 

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("CanPickUp"))
        {
            Rigidbody rigid = other.GetComponent<Rigidbody>();

            if (rigid != null)
            {
                rigid.linearVelocity = Vector3.zero; 
                rigid.angularVelocity = Vector3.zero; 
            }

            oggetto.transform.position = respawnPoint.position;
        }
    }
}
//odio sta merda