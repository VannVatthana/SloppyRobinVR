using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;
using System.Collections;

public class ArrowSpawn : MonoBehaviour
{
    public GameObject arrow;
    public GameObject notch;

    private XRGrabInteractable bow;
    private bool arrowNotchPulled = false;
    private GameObject currentArrow = null;

    void Start()
    {
        bow = GetComponent<XRGrabInteractable>();
        PullInteraction.PullActionReleased += NotchEmpty;
    }

    private void OnDestroy()
    {
        PullInteraction.PullActionReleased -= NotchEmpty;
    }

    void Update()
    {
        if(bow.isSelected && arrowNotchPulled == false)
        {
            arrowNotchPulled = true;
            StartCoroutine("DelayedSpawn");
        }
        if (!bow.isSelected && currentArrow == null)
        {
            Destroy(currentArrow);
            NotchEmpty(0.5f);
        }
    }

    private void NotchEmpty(float value)
    {
        arrowNotchPulled = false;
        currentArrow = null;
    }

    IEnumerator DelayedSpawn()
    {
        yield return new WaitForSeconds(1f); // Delay 1s after shooting an arrow before respawn another 
        currentArrow = Instantiate(arrow, notch.transform);
    }
}
