﻿using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class hints_panel : MonoBehaviour
{

    public static Boolean paused = false;
    public GameObject Panel;

    public void OpenPanel()
    {

        if (Panel != null)
        {
            //Debug.Log(paused);
            paused = true;
            Panel.SetActive(true);
        }
    }

    public void ClosePanel()
    {

        if (Panel != null)
        {
            paused = false;
            Panel.SetActive(false);
        }
    }

    public Boolean getPaused()
    {
        return paused;
    }

    Text txt;

    // Start is called before the first frame update
    void Start()
    {
        
       
            //txt = GameObject.FindWithTag("Hint_Text").GetComponent<Text>() as Text;
        txt = gameObject.GetComponent<Text>() as Text;
        if (txt != null)
        {
            txt.text = "Hint : ABC";
        }
    }

    // Update is called once per frame
    void Update()
    {

    }
}
