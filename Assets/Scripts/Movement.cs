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
    private float jumpingMaxHeight = .5f;

    [SerializeField]
    private float fallFactor = .9f;

   
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
        StartCoroutine(FallControlFlow());
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
        Vector3 movementDirection3d = new Vector3(
            Keyboard.current.dKey.ReadValue() - Keyboard.current.aKey.ReadValue(),
            0f,
            Keyboard.current.wKey.ReadValue() - Keyboard.current.sKey.ReadValue()
        ).normalized;


        if(movementDirection3d != Vector3.zero)
        {
            transform.forward = Camera.main.transform.forward;
            Quaternion cameraAlignedForwardRotation = transform.rotation;
            transform.forward = movementDirection3d;
            transform.rotation *= cameraAlignedForwardRotation;

        }

        _isMoving = movementDirection3d != Vector3.zero;

        float movementStrength = Vector3.Magnitude(movementDirection3d);
        gameObject.transform.Translate(new Vector3(0, 0, 1) * _velocity * Time.deltaTime * movementStrength);

        // Move the player based on the input direction and velocity
        //_rigidbody.MovePosition(transform.position + (movementDirection * -1) * _velocity * Time.deltaTime);

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

            StartCoroutine(JumpControlFlow());
            //_isGrounded = false;
            //_isLanding = true;
            AkSoundEngine.StopPlayingID((uint)walkingId);
            walkingId = -1;
        }
    }

    private IEnumerator JumpControlFlow()
    {
        _isLanding = true;
        float jumpHeight = transform.position.y + jumpingMaxHeight;
        _rigidbody.AddForce(Vector3.up * _jumpPower, ForceMode.Force);
        
        while(transform.position.y < jumpHeight)
        {
            yield return null;
        }

        _rigidbody.AddForce(new Vector3(0, _jumpPower* -1 * fallFactor, 0), ForceMode.Force);

    }

    private IEnumerator FallControlFlow()
    {
        RaycastHit hit;

        float prevY;
        float currentY = transform.position.y;
        // StopCoroutine(JumpControlFlow());
        while (true)
        {
            bool raycastSuccess = Physics.Raycast(transform.position, transform.up * -1, out hit);
            if(raycastSuccess && hit.collider.gameObject.CompareTag("Grounded") && hit.distance <= .50001f)
            {
                _isGrounded = true;
                StopCoroutine(JumpControlFlow());
                _isLanding = false;
            }
            else
            {
                _isGrounded = false;
            }
            yield return null;
        }
        

    }


    void OnCollisionEnter(Collision other)
    {

        if (other.gameObject.tag == "Grounded")
        {
            //_isGrounded = true;
            if (_isLanding)
            {
                AkSoundEngine.PostEvent("Play_landing", gameObject);
            }
            //_isLanding = false;

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
            StartCoroutine(FallControlFlow());
            //_rigidbody.AddForce(new Vector3(0, _jumpPower*.5f, 0), ForceMode.Impulse);

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
