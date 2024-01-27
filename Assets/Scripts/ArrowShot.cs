using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
public class ArrowShot : MonoBehaviour
{
    //public MyTechnique myTechnique;
    public float speed = 40f;
    public GameObject arrow;
    
    //public GameObject allArrowShot;
    private Rigidbody _rigidbody;
    private bool _inAir = false;
    private Vector3 _lastPosition = Vector3.zero;
    private GameObject selectedObj;
    
    private void Awake()
    {
        _rigidbody = GetComponent<Rigidbody>();
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
    // Release to shoot the arrow
    private void Release(float value)
    {
        PullInteraction.PullActionReleased -= Release;
        //gameObject.transform.parent = null;   // Detatch from arrow
        arrow.transform.parent = null;
        _inAir = true;
        SetPhysics(true);

        Vector3 force = arrow.transform.forward * value * speed;
        _rigidbody.AddForce(force, ForceMode.Impulse);

        StartCoroutine(RotateWithVelocity());

        _lastPosition = arrow.transform.position;


    }
    private IEnumerator RotateWithVelocity()
    {
        yield return new WaitForFixedUpdate();
        while(_inAir)
        {
            Quaternion newRotation = Quaternion.LookRotation(_rigidbody.velocity, transform.up);
            transform.rotation = newRotation;
            yield return null;
        }
    }
    private void FixedUpdate()
    {
        /*if (_inAir)
        {
            CheckCollision();
            _lastPosition = arrow.transform.position;
        }*/
        //base.CheckForSelection();
    }
    /*private void CheckCollision()
    {
        if (Physics.Linecast(_lastPosition,arrow.transform.position, out RaycastHit hitInfo))
        {
            if(hitInfo.transform.gameObject.layer != 6)
            { 
                _rigidbody.interpolation = RigidbodyInterpolation.None;
                //HaveSelectedObj.Invoke(hitInfo.collider.gameObject);
                //transform.parent = hitInfo.transform;
                selectedObj = hitInfo.collider.gameObject;
                //currentSelectedObject = hitInfo.collider.gameObject;
                Stop();
            }
        }
    }*/
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
    public GameObject GetObjectHitByArrow() {
        return selectedObj;
    }
}
