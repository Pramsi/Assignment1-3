using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Playables;
using AK.Wwise;
using System;
using UnityEngine.UIElements;
using UnityEditor.SearchService;
using UnityEngine.SceneManagement;

internal enum MovementType
{
    TransformBased,
    PhysicsBased
}

public class Movement : MonoBehaviour
{
    [SerializeField]
    private float _velocity = 1;

    [SerializeField]
    private float _jumpPower = 1;

    [SerializeField]
    private ForceMode _selectedForceMode;

    [SerializeField]
    private MovementType movementType;
    
    private Vector3 movementDirection3d;
    private Rigidbody _rigidbody;

    private bool _isGrounded = true;
    private bool _isLanding = false;
    private bool _isMoving = false;

    private uint playgroundId;
    private uint dogHouseId;
    private uint loudHouseId;
    private uint backgroundId;
    private uint restaurantAmbienceId;
    private uint outdoorRestaurantAmbienceId;

    private int walkingId = -1;
    private int movementKeysPressed = 0;



    // Start is called before the first frame update
    void Start()
    {
        
        if (_rigidbody != null)
            return;

        _rigidbody = gameObject.GetComponent<Rigidbody>();

   if( SceneManager.GetActiveScene().name == "FirstScene") {
            AkSoundEngine.SetState("background", "backgroundNoise");
            backgroundId = AkSoundEngine.PostEvent("Play_background", gameObject);
            AkSoundEngine.SetSwitch("walking", "street", gameObject);
        }
        else if (SceneManager.GetActiveScene().name == "Indoor")
        {
            AkSoundEngine.PostEvent("Play_Bell", gameObject);
            restaurantAmbienceId = AkSoundEngine.PostEvent("Play_RestaurantAmbience", gameObject);
        }
           

    }

    // Update is called once per frame
    void Update()
    {

        PerformMovement();
        /* if (Input.GetKey(KeyCode.W))
             gameObject.transform.position += new Vector3(0, 0, -1f) * _velocity;*/
        if(movementDirection3d != Vector3.zero)
        {
            _isMoving = true;
        } else
        {
            _isMoving= false;   
        }
        
    }

    void PerformMovement()
    {
        if(movementType == MovementType.TransformBased)
        {
        gameObject.transform.position += movementDirection3d * _velocity;

        } else if(movementType == MovementType.PhysicsBased)
        {
        _rigidbody.AddForce(movementDirection3d, _selectedForceMode);
        }
        
        
    }

    void OnMovement(InputValue inputValue)
    {
        Vector2 movementDirection = inputValue.Get<Vector2>();
        movementDirection3d = new Vector3(movementDirection.x, 0, movementDirection.y);
Debug.Log(walkingId);  

        if (_isGrounded)
        {
            // Check if any of the movement keys are pressed down
            if (Input.GetKeyDown(KeyCode.W) || Input.GetKeyDown(KeyCode.A) || Input.GetKeyDown(KeyCode.S) || Input.GetKeyDown(KeyCode.D))
            {
                movementKeysPressed++;

                // Play walking sound only if it's not already playing
                if (walkingId == -1)
                {
                    walkingId = (int)AkSoundEngine.PostEvent("Play_walking", gameObject);
                     
                }
            }

            if (Input.GetKeyUp(KeyCode.W) || Input.GetKeyUp(KeyCode.A) || Input.GetKeyUp(KeyCode.S) || Input.GetKeyUp(KeyCode.D))
            {
                movementKeysPressed--;

                if (movementKeysPressed == 0)
                {
                    AkSoundEngine.StopPlayingID((uint)walkingId);
                    walkingId = -1; // Reset walkingId
                }
            }

        }
    }

    void OnJump(InputValue inputValue)
    {
        if(_isGrounded)
        {
            AkSoundEngine.PostEvent("Play_jump", gameObject);
            _rigidbody.AddForce(new Vector3(0, _jumpPower, 0), ForceMode.Impulse);
            _isGrounded = false;
            _isLanding = true;
            AkSoundEngine.StopPlayingID((uint)walkingId);
            walkingId = -1;
        }
        
    }


    void OnCollisionEnter(Collision other)
    {
        if (other.gameObject.tag == "Grounded")
        {
            _isGrounded = true;
            if (_isLanding)
            {
                AkSoundEngine.PostEvent("Play_landing", gameObject);
            }
            _isLanding = false;

        }
        
        if (other.gameObject.tag == "Enemy")
        {
            AkSoundEngine.PostEvent("Play_damage", gameObject);
        }

        if(other.gameObject.name.Contains("Lawn"))
        {
            AkSoundEngine.SetSwitch("walking", "grass", gameObject);

        }
        else if (other.gameObject.name.Contains("Ground"))
        {
            AkSoundEngine.SetSwitch("walking", "street", gameObject);

        }
        else if (other.gameObject.name.Contains("Wood"))
        {
            AkSoundEngine.SetSwitch("walking", "wood", gameObject);

        }



    }

    private void OnTriggerEnter(Collider other)
    { 
        if(other.gameObject.tag == "DogHouse")
        {
            dogHouseId = AkSoundEngine.PostEvent("Play_BarkingDog", gameObject);
        }

        if (other.gameObject.tag == "RestaurantAmbientTrigger")
        {
            
            outdoorRestaurantAmbienceId = AkSoundEngine.PostEvent("Play_RestaurantAmbienceOutdoor", gameObject);
        }

        if (other.gameObject.tag == "Coin")
        {
            AkSoundEngine.PostEvent("Play_collectCoin", gameObject);
        }

        if (other.gameObject.tag == "Playground")
        {
            playgroundId = AkSoundEngine.PostEvent("Play_playground", gameObject);
        }

        if (other.gameObject.tag == "House")
        {
            AkSoundEngine.PostEvent("Play_doorbell", gameObject);
        }

        if (other.gameObject.tag == "LoudHouse")
        {
            loudHouseId =AkSoundEngine.PostEvent("Play_bassMusic", gameObject);
        }
        
        if(other.gameObject.tag == "Jump")
        {
            _isLanding = true;
            AkSoundEngine.StopPlayingID((uint)walkingId);
            walkingId = -1;
            _isGrounded = false;
            _rigidbody.AddForce(new Vector3(0, _jumpPower*2, 0), ForceMode.Impulse);

            AkSoundEngine.PostEvent("Play_boing", gameObject);
        }

        if (other.gameObject.name.Contains("EnterShopTrigger"))
        {
            AkSoundEngine.StopPlayingID((uint)walkingId);
            AkSoundEngine.StopPlayingID(backgroundId);
            
        }

        if (other.gameObject.name.Contains("ExitShopTrigger"))
        {
            AkSoundEngine.StopPlayingID((uint)walkingId);
            AkSoundEngine.StopPlayingID(restaurantAmbienceId);

        }

        if (other.gameObject.tag == "OrderTrigger")
        {
            loudHouseId = AkSoundEngine.PostEvent("Play_takingOrder", gameObject);
        }


    }

    private void OnTriggerExit(Collider other)
    {
        if(other.gameObject.tag == "Playground")
        {
            AkSoundEngine.StopPlayingID(playgroundId, 2000);
        }

        if (other.gameObject.tag == "DogHouse")
        {
            AkSoundEngine.StopPlayingID(dogHouseId, 1000);
        }

        if (other.gameObject.tag == "LoudHouse")
        {
            AkSoundEngine.StopPlayingID(loudHouseId, 1000);
        }

        if (other.gameObject.tag == "RestaurantAmbientTrigger")
        {
            AkSoundEngine.StopPlayingID(outdoorRestaurantAmbienceId, 1000);
        }
    }
}
