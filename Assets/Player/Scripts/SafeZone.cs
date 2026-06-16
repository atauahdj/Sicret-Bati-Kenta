using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SafeZone : MonoBehaviour
{
    public bool InSafeZone = false;
/*private MoveDestination MD;
    private void Start()
    {
        MD = FindObjectOfType<MoveDestination>();
    }*/
    void OnTriggerStay(Collider other)
    {
        if (other.gameObject.name == "Player")
        {
            InSafeZone = true;
        }
    }
    void OnTriggerExit(Collider other)
    {
        if (other.gameObject.name == "Player")
        {
            InSafeZone = false;
        }
    }
}
