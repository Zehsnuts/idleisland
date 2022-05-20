using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerHands : MonoBehaviour
{
    public bool handsFull;

    public Transform heldItem;
    bool throwing;
    float throwSpeed;
    float throwSpeedMax;
    float throwUpSpeed;
    bool chargingThrow;
    private PlayerVision _playerVision;

    private void OnEnable()
    {
        ClearHands();
        chargingThrow = false;
        throwSpeed = 2;
        throwSpeedMax = 8;
        throwUpSpeed = 1;

        _playerVision = FindObjectOfType<PlayerVision>();
    }
    public bool GrabObject(Transform item)
    {
        if (handsFull)
        {
            Debug.Log("Hands Full!");
            return false;
        }

        handsFull = true;

        heldItem = item;
        Rigidbody heldItemRigidbody = heldItem.transform.GetComponent<Rigidbody>();


        heldItemRigidbody.freezeRotation = true;
        heldItemRigidbody.velocity = Vector3.zero;
        heldItemRigidbody.angularVelocity = Vector3.zero;
        

        heldItem.transform.position = transform.position;
        heldItem.transform.rotation = transform.rotation;
        heldItem.transform.parent = transform;

        heldItemRigidbody.freezeRotation = false;        

        return true;
    }

    private void FixedUpdate()
    {
        if(chargingThrow && throwSpeed< throwSpeedMax)
        {
            throwSpeed += 0.2f;
            //Debug.Log("throwSpeed = " + throwSpeed);
        }
    }

    public bool ThrowObject(bool addForce = false)
    {
        if (!handsFull)
            return false;

        if (addForce && throwSpeed < throwSpeedMax)
        {
            chargingThrow = true;
            return false;
        }

        heldItem.transform.parent = null;

        heldItem.GetComponent<Interactables>().ThrowMe();

        heldItem.GetComponent<Rigidbody>().AddRelativeForce(Vector3.forward * throwSpeed, ForceMode.Impulse);
        heldItem.GetComponent<Rigidbody>().AddRelativeForce((Vector3.up * throwSpeed)/ (throwUpSpeed*10), ForceMode.Impulse);


        ClearHands();
        throwSpeed = 2;
        chargingThrow = false;
        return true;
    }

    public void DepositObject(Vector3 point, Transform obj)
    {
        Vector3 newPoint = new Vector3(point.x, point.y + 0.2f, point.z);

        if (!handsFull && obj != heldItem)
            return;

        heldItem.transform.parent = null;
        heldItem.transform.position = newPoint;
        _playerVision.StopRayCasting();
        ClearHands();        
    }

    private void ClearHands()
    {
        heldItem = null;
        handsFull = false;
    }
}
