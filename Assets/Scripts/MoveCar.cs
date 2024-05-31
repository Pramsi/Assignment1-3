using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class MoveCar : MonoBehaviour
{
    [SerializeField] private float originalVelocity = .5f;

    private float currentVelocity;
    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(CheckForVelocity());
    }

    // Update is called once per frame
    void Update()
    {
        transform.Translate(Vector3.forward * currentVelocity);
        //use transform.Translate to move Car into its forward direction by currentVelocity
    }

    IEnumerator CheckForVelocity()
    {
        RaycastHit hit;
        //DONT FORGET TO EXECUTE the Coroutine "CheckForVelocity" once

        while (true)
        {
        bool raycastSuccess = Physics.Raycast(transform.position, transform.forward, out hit);

            if(raycastSuccess && hit.collider.gameObject.CompareTag("CarRayHit"))
            {
                currentVelocity = originalVelocity * 0.5f;
            } else
            {
                currentVelocity = originalVelocity;
            }
            //Raycast?
            //if player is hit currentVelocity is half of original velocity.
            //if not, currentVelocity is original velocity
            yield return null;
        }
    }

    private void OnCollisionEnter(Collision other) /*or make OnTriggerEnter(Collider other) - depending on how you configure your colliders.. I went with physical collisions here, since my car should act as a physical object.. e.g. for moving the player if it hits it.*/
    {
        if (other.gameObject.CompareTag("TurnCar"))
        {
            transform.Rotate(Vector3.up, 180f);
            //use transform.Rotate to rotate car by 180 degrees around its y Axis
        }
    }
   
}
