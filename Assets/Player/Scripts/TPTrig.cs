using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TPTrig : MonoBehaviour
{
    public Vector3 TpCord;
    private void OnTriggerEnter(Collider other)
    {
        other.gameObject.transform.position = TpCord;
    }
}
