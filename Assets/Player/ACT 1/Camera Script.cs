using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;

public class CameraScript : MonoBehaviour
{
    private Camera cam;
    public bool Record;
    private FPC FPC;
    private float currentTime;
    public TMP_Text JustText;
    public GameObject[] Remove;
    private Zvonok zvonok;
    private float chaseCT;
    void Start()
    {
        JustText.gameObject.SetActive(true);
        cam = gameObject.GetComponent<Camera>();
        FPC = FindObjectOfType<FPC>();
        Record = true;
        zvonok = FindObjectOfType<Zvonok>();
        zvonok.enabled = false;
        JustText.text = "Засними происходящее на ПКМ";
    }
    void Update()
    {
        Ray ray = new Ray(cam.transform.position, cam.transform.forward);
        RaycastHit obj;
        if (Input.GetMouseButton(1) && Record)
        {
            FPC.mouseSensitivity = 0.5f;
            Record = true;
            cam.fieldOfView = 10f;
            if(Physics.Raycast(ray, out obj, 100f))
            {
                if (obj.collider.CompareTag("CutScene"))
                {
                    currentTime += Time.deltaTime;
                    Debug.Log(currentTime);
                    if (currentTime >= 6f)
                    {
                        JustText.text = "Проследи куда они ушли";
                        FPC.walkSpeed = 5f;
                        FPC.sprintSpeed = 5f;
                        FPC.crouchSpeed = 3f;
                        Debug.Log("Record End");
                        Record = false;
                        if(Remove != null)
                        {
                            foreach(GameObject rem in Remove)
                            {
                                rem.SetActive(false);
                            }
                        }
                    }
                }
            }
        }
        else
        {
            FPC.mouseSensitivity = 2f;
            cam.fieldOfView = 70f;
        }
    }
}
