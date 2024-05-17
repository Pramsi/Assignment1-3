using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowObject : MonoBehaviour
{

    [SerializeField] private GameObject objectToFollow;

    private Transform otherObjectTransform;
    private Transform ownObjectTransform;

    private Vector3 offsetOfFollowObjectAndOwnObject;

    // Start is called before the first frame update
    void Start()
    {
        otherObjectTransform = objectToFollow.GetComponent<Transform>();
        ownObjectTransform = gameObject.GetComponent<Transform>();
        offsetOfFollowObjectAndOwnObject = ownObjectTransform.position - otherObjectTransform.position;
    }

    // Update is called once per frame
    void Update()
    {
        ownObjectTransform.position = otherObjectTransform.position + offsetOfFollowObjectAndOwnObject;
    }
}
