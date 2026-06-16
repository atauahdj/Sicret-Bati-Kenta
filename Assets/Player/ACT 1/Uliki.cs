using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Uliki : MonoBehaviour
{
    public int Ulik = 0;
    public TMP_Text ScoreUlik;
    private void Start()
    {
        ScoreUlik.text = "Собрано улик: " + "0/3";
    }
    void Update()
    {
        ScoreUlik.text = "Собрано улик: " + Ulik.ToString() + "/3";
        if (Ulik >= 3)
        {
            Debug.Log("CutScene");
        }
    }
}
