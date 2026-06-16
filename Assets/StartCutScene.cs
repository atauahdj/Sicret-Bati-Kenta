using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StartCutScene : MonoBehaviour
{
    public MoveDestination MoveDestination;
    public GameObject MoveDestinationObject;
    public GameObject CutScene;
    public GameObject Game;
    public FPC FPC;
    public float TimeCutScene = 5f;
    bool StatusCS = false;
    float tempSens;
    public Light DirectionalLight;
    public GameObject cam;
    public GameObject[] Remove;
    public GameObject[] Add;
    private void Start()
    {
        tempSens = FPC.mouseSensitivity;
    }
    void Update()
    {
        if (StatusCS)
        {
            if(TimeCutScene < 0)
            {
                FPC.mouseSensitivity = tempSens;
                FPC.gameObject.SetActive(true);
                cam.SetActive(false);
                gameObject.SetActive(false);
            }
            else
            {
                TimeCutScene -= Time.deltaTime;
                FPC.gameObject.SetActive(false);
                if(Remove != null)
                {
                    foreach (GameObject obj in Remove)
                    {
                        obj.SetActive(false);
                    }
                    foreach(GameObject obj in Add)
                    {
                        obj.SetActive(true);
                    }
                }
            }
        }
        Debug.Log("mouseSensevity: :" + FPC.mouseSensitivity);
    }
    private void OnTriggerEnter(Collider col)
    {
        if(col.gameObject.tag == "Player")
        {
            DirectionalLight.transform.rotation = Quaternion.Euler(1.125f, -30, 0);
            StatusCS = true;
            Game.SetActive(false);
            CutScene.SetActive(true);
            MoveDestinationObject.SetActive(false);
            FPC.mouseSensitivity = 0;
        }
    }
}
