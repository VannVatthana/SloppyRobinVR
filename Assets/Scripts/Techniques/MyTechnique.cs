using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Your implemented technique inherits the InteractionTechnique class
public class MyTechnique : InteractionTechnique
{
    // You must implement your technique in this file
    // You need to assign the selected Object to the currentSelectedObject variable
    // Then it will be sent through a UnityEvent to another class for handling   
    public float speed = 40f;

    public GameObject arrow;
    public GameObject notch;

    private Rigidbody _rigidbody;
    private bool _inAir = false;
    private Vector3 _lastPosition = Vector3.zero;
    
    public OVRInput.Controller rightController;
    public Transform cameraRig;

    private void Awake()
    {
        _rigidbody = arrow.GetComponent<Rigidbody>();
        PullString.PullActionReleased += Release;

        Stop();
    }
    private void FixedUpdate()
    {
        MoveAround();

        if (_inAir)
        {
            CheckCollision();
            _lastPosition = arrow.transform.position;
        }
        else
        {
            arrow.transform.SetParent(notch.transform);
            arrow.transform.SetLocalPositionAndRotation(Vector3.zero, Quaternion.Euler(Vector3.zero)) ;
        }
        // DO NOT REMOVE
        // If currentSelectedObject is not null, this will send it to the TaskManager for handling
        // Then it will set currentSelectedObject back to null
        base.CheckForSelection();
    }
    private void Release(float value)
    {
        //PullString.PullActionReleased -= Release;   // Makes arrow unable to release at the second time
        // Detatch from arrow
        if(arrow.transform.parent == notch.transform)
            arrow.transform.parent = null;
        _inAir = true;
        SetPhysics(true);
        GameObject arrow1 = arrow;
        Vector3 force = arrow1.transform.forward * value * speed;
        _rigidbody.AddForce(force, ForceMode.Impulse);

        StartCoroutine(RotateWithVelocity());

        _lastPosition = arrow.transform.position;
    }
    private IEnumerator RotateWithVelocity()
    {
        yield return new WaitForFixedUpdate();
        while (_inAir)
        {
            Quaternion newRotation = Quaternion.LookRotation(_rigidbody.velocity, arrow.transform.up);
            arrow.transform.rotation = newRotation;
            yield return null;
        }
    }
    private void CheckCollision()
    {
        if (Physics.Linecast(_lastPosition,arrow.transform.position,out RaycastHit hitInfo))
        {
            if (hitInfo.transform.gameObject.layer != 6)
            {
                _rigidbody.interpolation = RigidbodyInterpolation.None;
                currentSelectedObject = hitInfo.collider.gameObject;
                Stop();
            }
        }
        else
        {
            StartCoroutine(DeactivatePhysics(5f));
        }
    }
    private void Stop()
    {
        _inAir = false;
        SetPhysics(false);
    }

    private void SetPhysics(bool usePhysics)
    {
        _rigidbody.useGravity = usePhysics;
        _rigidbody.isKinematic = !usePhysics;
    }
    IEnumerator DeactivatePhysics(float delaySeconds)
    {
        yield return new WaitForSeconds(delaySeconds);
        Stop();
    }

    void MoveAround()  // Move around 
    {
        if (OVRInput.Get(OVRInput.Button.Two))  // Press B to move forward
            cameraRig.position = cameraRig.position + new Vector3(-3f,0,0);
        if (OVRInput.Get(OVRInput.Button.One))  // Press A to move backward
            cameraRig.position = cameraRig.position + new Vector3(3f, 0, 0);

    }

    
    
}
