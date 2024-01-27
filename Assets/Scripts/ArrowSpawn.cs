using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;
using System.Collections;

public class ArrowSpawn : MonoBehaviour
{
    public GameObject arrow;
    public GameObject notch;

    private GameObject currentArrow = null;

    void Start()
    {
        PullInteraction.PullActionReleased += NotchEmpty;
    }

    private void OnDestroy()
    {
        PullInteraction.PullActionReleased -= NotchEmpty;
    }

    void Update()
    { 
        if(currentArrow == null)
        {
            //arrowNotchPulled = true;
            StartCoroutine("DelayedSpawn");
        }
    }

    private void NotchEmpty(float value)
    {
        //arrowNotchPulled = false;
        currentArrow = null;
    }

    IEnumerator DelayedSpawn()
    {
        yield return new WaitForSeconds(1f); // Delay 1s after shooting an arrow before respawn another 
        currentArrow = Instantiate(arrow, notch.transform);
    }
}
