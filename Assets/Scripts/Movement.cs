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
   
    private Vector3 movementDirection3d;
    private Rigidbody _rigidbody;

    private bool _isGrounded = true;
    private bool _isLanding = false;
    private bool _isMoving = false;
    private bool _isWalkingSoundPlaying = false;


    private uint playgroundId;
    private uint dogHouseId;
    private uint loudHouseId;
    private uint backgroundId;
    private uint restaurantAmbienceId;
    private uint outdoorRestaurantAmbienceId;

    private int walkingId = -1;


    // Start is called before the first frame update
    void Start()
    {        
        if (_rigidbody != null)
            return;

        _rigidbody = gameObject.GetComponent<Rigidbody>();
    }


    // Update is called once per frame
    void Update()
    {
        PerformMovement();
        _isMoving = CheckIfMoving();
    }


    private bool CheckIfMoving()
    {
        if (movementDirection3d.magnitude > 0)
        {
            return _isMoving = true;
        }
        else
        {
            return _isMoving = false;
        }
    }

    private void PlayWalkingSounds()
    {
        if (!_isWalkingSoundPlaying)
        {
            walkingId = (int)AkSoundEngine.PostEvent("Play_walking", gameObject);
            _isWalkingSoundPlaying = true;
        }
    }

    private void StopWalkingSounds()
    {
        AkSoundEngine.StopPlayingID((uint)walkingId);
        walkingId = -1; // Reset walkingId
        _isWalkingSoundPlaying = false;
    }

    private void PerformMovement()
    {
        // Get movement input from Unity's new input system
        Vector3 movementDirection = new Vector3(
            Keyboard.current.dKey.ReadValue() - Keyboard.current.aKey.ReadValue(),
            0f,
            Keyboard.current.wKey.ReadValue() - Keyboard.current.sKey.ReadValue()
        ).normalized;

        _isMoving = movementDirection != Vector3.zero;

        // Move the player based on the input direction and velocity
        _rigidbody.MovePosition(transform.position + (movementDirection * -1) * _velocity * Time.deltaTime);

        if (_isMoving && !_isLanding)
        {
            // Call a method to play walking sounds
            PlayWalkingSounds();
        }
        else
        {
            // Stop walking sounds when the player is not moving
            StopWalkingSounds();
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
        Debug.Log(other.gameObject.name);


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
            dogHouseId = AkSoundEngine.PostEvent("Play_dogBark", gameObject);
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
            _rigidbody.AddForce(new Vector3(0, _jumpPower*1.5f, 0), ForceMode.Impulse);

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
            AkSoundEngine.PostEvent("Play_takingOrder", gameObject);
        }


    }

    private void OnTriggerExit(Collider other)
    {
        if(other.gameObject.tag == "Playground")
        {
            AkSoundEngine.StopPlayingID(playgroundId, 1000);
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
