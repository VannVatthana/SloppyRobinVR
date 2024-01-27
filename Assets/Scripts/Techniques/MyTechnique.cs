using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
// Your implemented technique inherits the InteractionTechnique class
public class MyTechnique : InteractionTechnique
{
    // You must implement your technique in this file
    // You need to assign the selected Object to the currentSelectedObject variable
    // Then it will be sent through a UnityEvent to another class for handling   
    public float speed = 40f;

    public GameObject arrow;
    public GameObject arrowPrefab;
    public GameObject notch;

    private Rigidbody _rigidbody;
    private bool _inAir = false;
    private Vector3 _lastPosition = Vector3.zero;
    
    private void Awake()
    {
        _rigidbody = arrow.GetComponent<Rigidbody>();
        PullInteraction.PullActionReleased += Release;

        Stop();
    }
    private void OnDestroy()
    {
        PullInteraction.PullActionReleased -= Release;
    }

    private void OnTriggerExit()
    {
        Release(2f);
    }
    private void FixedUpdate()
    {
        
        if (_inAir)
        {
            CheckCollision();
            _lastPosition = arrow.transform.position;
        }
        /*else
        {
            //GameObject notch = GameObject.Find("Notch");
            arrow.transform.parent = notch.transform;
            arrow.transform.localPosition = Vector3.zero;//notch.transform.position;
            arrow.transform.localRotation = Quaternion.Euler(Vector3.zero);//notch.transform.rotation;
        }*/
        // DO NOT REMOVE
        // If currentSelectedObject is not null, this will send it to the TaskManager for handling
        // Then it will set currentSelectedObject back to null
        base.CheckForSelection();
    }
    private void Release(float value)
    {
        PullInteraction.PullActionReleased -= Release;
        // Detatch from arrow
        if(arrow.transform.parent == notch.transform)
            arrow.transform.parent = null;
        _inAir = true;
        SetPhysics(true);
        GameObject arrow1 = arrow;
        Vector3 force = arrow.transform.forward * value * speed;
        _rigidbody.AddForce(force, ForceMode.Impulse);

        StartCoroutine(RotateWithVelocity());

        _lastPosition = arrow.transform.position;
        //}
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
                Destroy(arrow, 2);
                new WaitForSeconds(2f);
                arrow = Instantiate(arrowPrefab, notch.transform);//.transform.SetParent(notch.transform);
                //arrow.transform.localPosition = Vector3.zero;
                //arrow.transform.localRotation = Quaternion.Euler(Vector3.zero);
                //arrow.transform.position = notch.transform.position;
                //arrow.transform.rotation = notch.transform.rotation; //Quaternion.Euler(Vector3.zero);
            }
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


}
