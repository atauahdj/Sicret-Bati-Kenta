using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrigAnim : MonoBehaviour
{
    public GameObject player;
    public bool Left;
    public Animator anim;
    public GameObject cam;
    public float currentTime;
    public bool GoCutScene;
    void Update()
    {
        if (GoCutScene)
        {
                    currentTime += Time.deltaTime;
        if(currentTime >= 5f)
        {
            Destroy(cam);
        }
        }

    }
    void OnTriggerStay(Collider other)
    {
        if(other.gameObject.tag == "Player")
        {
            player.SetActive(false);
            cam.SetActive(true);
            if (Left)
            {
                currentTime += Time.deltaTime;
                anim.SetBool("RightAnim", false);
                anim.SetBool("LeftAnim", true);
            }
            else
            {
                currentTime += Time.deltaTime;
                anim.SetBool("LeftAnim", false);
                anim.SetBool("RightAnim", true);
                if(currentTime >= 5f)
                {
                    Destroy(cam);
                }
            }
        }
    }
}
