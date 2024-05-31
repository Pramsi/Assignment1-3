using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowObject : MonoBehaviour
{

    [SerializeField] private GameObject objectToFollow;

    private Transform otherObjectTransform;
    private Transform ownObjectTransform;

    private Vector3 offsetOfFollowObjectAndOwnObject;

    private float fixYOffset;
    private float distance;

    // Start is called before the first frame update
    void Start()
    {
        if (objectToFollow == null)
        {
            Debug.LogError("objectToFollow is not assigned in the FollowObject script.");
            return;
        }
        otherObjectTransform = objectToFollow.GetComponent<Transform>();
        ownObjectTransform = gameObject.GetComponent<Transform>();
        fixYOffset = ownObjectTransform.position.y - otherObjectTransform.position.y;
        distance = Vector3.Distance(ownObjectTransform.position, otherObjectTransform.position);
        SetOffset();
    }

    public void SetOffset()
    {
        offsetOfFollowObjectAndOwnObject = ownObjectTransform.position - otherObjectTransform.position;
        offsetOfFollowObjectAndOwnObject.Normalize();
        offsetOfFollowObjectAndOwnObject *= distance;
        offsetOfFollowObjectAndOwnObject.y = fixYOffset;
    }

    // Update is called once per frame
    void Update()
    {
        gameObject.transform.position = otherObjectTransform.position + offsetOfFollowObjectAndOwnObject;
    }
}
