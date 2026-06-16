using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class Zvonok : MonoBehaviour
{
    private ItemScript ItemScript;
    public AudioSource AudioSource;
    public AudioClip DialogClip;
    private MoveDestination MoveDestination;
    public float playDuration = 5f;
    private float currentTime;
    public GameObject Dictophone;
    private bool isPlaying = false;
    private bool ZvonokRecord = false;
    private Uliki Uliki;
    public GameObject UlikiPrefab;
    public TMP_Text text;
    public GameObject TrigToNext;

    private void Start()
    {
        Uliki = FindObjectOfType<Uliki>();
        MoveDestination = FindObjectOfType<MoveDestination>();
        currentTime = playDuration;
    }
    private void Update()
    {
        if(MoveDestination != null)
        {
            if (Vector3.Distance(transform.position, MoveDestination.transform.position) <= 6f && MoveDestination.ZvonokUlik == true)
            {
                text.text = "Нажмите E чтобы записывать";
                if (Input.GetKeyDown(KeyCode.E) && !ZvonokRecord)
                {
                    ZvonokRecord = true;
                    AudioSource.clip = DialogClip;
                    AudioSource.Play();
                }
            }
        }
        else
        {
            text.text = "";
        }
        if (ZvonokRecord)
        {
            text.text = "";
            Dictophone.SetActive(true);
            currentTime -= Time.deltaTime;
            if(currentTime <= 0)
            {
                text.text = "Иди домой";
                MoveDestination.ZvonokUlik = false;
                AudioSource.clip = null;
                Uliki.Ulik++;
                Dictophone.SetActive(false);
                UlikiPrefab.SetActive(true);
                TrigToNext.SetActive(true);
                gameObject.GetComponent<Zvonok>().enabled = false;
                ZvonokRecord = false;   
            }
        }
    }
}