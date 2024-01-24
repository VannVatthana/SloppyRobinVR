using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PullBowNotch : MonoBehaviour
{
    public OVRInput.Controller controller;
    private float triggerValue;
    [SerializeField] private bool isInCollider;
    [SerializeField] private bool isSelected;

    public GameObject notch;
    private Vector3 initalNotchPos;
    private GameObject selectedObj;

    private void Start()
    {
        initalNotchPos = notch.transform.position;
    }
    void Update()
    {
        triggerValue = OVRInput.Get(OVRInput.Axis1D.PrimaryIndexTrigger, controller);

        if (isInCollider)
        {
            if (!isSelected && triggerValue > 0.95f)
            {
                isSelected = true;
                notch.transform.position = OVRInput.GetLocalControllerPosition(controller);
            }
            else if (isSelected && triggerValue < 0.95f)
            {
                isSelected = false;
                notch.transform.position = initalNotchPos;
            }
        }
    }

    private void OnTriggerEnter(Collider other)
    {

        if (other.gameObject.name == "Notch")
        {
            isInCollider = true;
            selectedObj = other.gameObject;
        }
    }
    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.name == "Notch")
        {
            isInCollider = false;
            selectedObj = null;
        }
    }
}

