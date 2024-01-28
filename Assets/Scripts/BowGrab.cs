using UnityEngine;

public class BowGrab : MonoBehaviour
{
    public OVRInput.Controller controller;
    private float triggerValue;
    [SerializeField] private bool isInCollider;
    [SerializeField] private bool isSelected;

    private GameObject selectedObj;
    void FixedUpdate()
    {
        triggerValue = OVRInput.Get(OVRInput.Axis1D.PrimaryIndexTrigger, controller);
        
        if(isInCollider)
        { 
            if(!isSelected && triggerValue > 0.95f)
            { 
                isSelected = true;
                selectedObj.transform.parent = this.transform;
            }
            else if (isSelected && triggerValue < 0.95f)
            {
                isSelected = false;
                selectedObj.transform.parent = null; 
            }
        }
    }
    private void OnTriggerEnter(Collider other)
    { 
        
        if (other.gameObject.name == "Bow")
        { 
            isInCollider = true;
            selectedObj = other.gameObject;

        }
    }
    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.name == "Bow")
        {
            isInCollider = false;
            selectedObj = null;
        }
    }
}
