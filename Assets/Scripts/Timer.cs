using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;
using System.Linq;
using TMPro;

public class Timer : MonoBehaviour
{
    public Text UItexto;
    private int segundos = 0;
    private int minutos = 0;

    public string playername ="";

    [Header("Texts")]
    [SerializeField] public GameObject inputField;
    [SerializeField] public GameObject textDisplayS;
    [SerializeField] public GameObject textDisplayM;

    private void Awake()
    {
        InvokeRepeating("Cronometro", 0f, 1f);
    }

    public void StoreName()
    {
        playername = inputField.GetComponent<TMP_Text>().text;
        textDisplayS.GetComponent<TMP_Text>().text = playername;
        textDisplayM.GetComponent<TMP_Text>().text = playername;


    }

    void Cronometro()
    {
        segundos++;
        if (segundos == 60)
        {
            minutos++;
            segundos = 0;
        }

        UItexto.text = minutos + " M  " + segundos + " S";
    }

}
