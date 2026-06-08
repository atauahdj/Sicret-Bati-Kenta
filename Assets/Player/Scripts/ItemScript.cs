using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ItemScript : MonoBehaviour
{
    public Transform Hand;
    public Transform Parent;
    public Transform drop;
    public Camera playerCamera;
    public float pickupRange = 3f;

    private Transform Item;
    private bool Hold = false;
    private GameObject currentItem = null;

    void Update()
    {
        if (Hold)
        {
            Item.position = Hand.position;

            if (Input.GetKeyDown(KeyCode.G))
            {
                Item.SetParent(null);
                Item.GetComponent<Rigidbody>().isKinematic = false;
                Item.GetComponent<Collider>().enabled = true;
                Item.position = drop.position;
                Hold = false;
                Item = null;
            }
        }
        CheckForItem();
        if (Input.GetKeyDown(KeyCode.E) && !Hold && currentItem != null)
        {
            PickupItem(currentItem);
        }
    }

    void CheckForItem()
    {
        Ray ray = new Ray(playerCamera.transform.position, playerCamera.transform.forward);
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit, pickupRange))
        {
            if (hit.collider.CompareTag("Item"))
            {
                currentItem = hit.collider.gameObject;
                Debug.DrawRay(ray.origin, ray.direction * pickupRange, Color.green);
                return;
            }
        }

        currentItem = null;
        Debug.DrawRay(ray.origin, ray.direction * pickupRange, Color.red);
    }

    void PickupItem(GameObject item)
    {
        item.transform.SetParent(Parent);
        item.GetComponent<Rigidbody>().isKinematic = true;
        Item = item.transform;
        Hold = true;
        item.GetComponent<Collider>().enabled = false;
        Item.localRotation = Quaternion.Euler(0, 0, 0);
        currentItem = null;
    }
}