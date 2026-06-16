using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Zapiski : MonoBehaviour
{
    public GameObject DeskZapiski;
    private Uliki Uliki;
    private MoveDestination MD;
    private void Start()
    {
        MD = FindAnyObjectByType<MoveDestination>();
        Uliki = FindAnyObjectByType<Uliki>();
        DeskZapiski.SetActive(false);
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            MD.ZvonokUlik = true;
            Uliki.Ulik++;
            DeskZapiski.SetActive(true);
            Destroy(gameObject);
        }
    }
}
