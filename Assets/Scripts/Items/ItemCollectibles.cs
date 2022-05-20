using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Extends monobehavior? Maybe Object
public class ItemCollectibles : MonoBehaviour, Interactables
{

    private double _id;
    bool collected;
    Collider ownCollider;
    Rigidbody ownRigidbody;

    PlayerHands playersHands;

    private void OnEnable()
    {
        collected = false;
        ownCollider = transform.GetComponent<Collider>();
        ownRigidbody = transform.GetComponent<Rigidbody>();        
    }

    private void Awake()
    {
        playersHands = FindObjectOfType<PlayerHands>();
    }

    public ItemCollectibles(double id) => _id = CheckID(id);

    private double CheckID(double id)
    {
        throw new NotImplementedException();
    }

    public bool InteractMe()
    {
        //If GrabOject is true, there will be an item in player's hands. If (GrabObject == false) or (collected == true) either hands are full or
        //the object is already collected and Raycast is picking it again.
        if (!playersHands.GrabObject(transform) || collected)   
            return DepositMe(); 
        

        Debug.Log("collected me.");
        collected = true;
        DisableObjectInteraction();
        return collected;
    }

    //Used when player throw or deposit object.
    public void EnableObjectInteraction()
    {
        ownCollider.enabled = true;
        ownRigidbody.useGravity = true;

        collected = false;
    }
    //Used when player grab object.
    void DisableObjectInteraction()
    {
        ownCollider.enabled = false;
        ownRigidbody.useGravity = false;
    }

    public bool ThrowMe()
    {  
        if (playersHands.heldItem != transform)
            return false;
        
        EnableObjectInteraction();
        return true;

    }
    public bool DepositMe()
    {
        RaycastHit hit;
        hit = FindObjectOfType<PlayerVision>().ThrowRay();
        if (hit.transform == null)
            return false;
        if (hit.transform.tag != "Surface")
            return false;

        playersHands.DepositObject(hit.point, transform);

        EnableObjectInteraction();

        return true;
    }

}
