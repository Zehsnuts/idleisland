using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerVision : MonoBehaviour
{
    public Camera playerCamera;

    public GameObject playerTarget;

    bool stopRayCasting;
    void FixedUpdate()
    {
        if (stopRayCasting)
            return;

        // Bit shift the index of the layer (8) to get a bit mask
        int layerMask = 1 << 8;
        //layerMask = ~layerMask; //Invert bitmask to collide with every layer but 8 

        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;
        if (Physics.Raycast(playerCamera.transform.position, playerCamera.transform.TransformDirection(Vector3.forward), out hit, 2f, layerMask))
        {
            Debug.Log(hit.transform.name);

            if (hit.transform.tag == "Collectables")
                playerTarget = hit.transform.gameObject;

            Debug.DrawRay(playerCamera.transform.position, playerCamera.transform.TransformDirection(Vector3.forward) * hit.distance, Color.red);
            //Debug.Log("Hit: " + hit.transform.name);
            Debug.DrawLine(ray.origin, hit.point);
        }
    }

    //Throw a Ray to check surfaces and use that info to ex: Deposit items on the ground. 
    public RaycastHit ThrowRay()
    {        
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;

        if (stopRayCasting)
        {
            hit = new RaycastHit();
            return hit;
        }

        if (Physics.Raycast(playerCamera.transform.position, playerCamera.transform.TransformDirection(Vector3.forward), out hit, 2f))
        {
            Debug.Log(hit.transform.name);
            return hit;
        }
        return hit;
    }

    public void StopRayCasting()
    {
        if (stopRayCasting)
            return;

        StartCoroutine(FlickerRayCasting());
    }

    IEnumerator FlickerRayCasting()
    {
        stopRayCasting = true;
        playerTarget = null;
        yield return new WaitForSeconds(0.3f);
        stopRayCasting = false;
    }
}
