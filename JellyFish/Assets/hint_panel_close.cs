using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class hint_panel_close : MonoBehaviour
{
    public GameObject Panel;

    public void OpenPanel()
    {

        if (Panel != null)
        {

            Panel.SetActive(true);
        }
    }

    public void ClosePanel()
    {

        if (Panel != null)
        {
            Panel.SetActive(false);
        }
    }


    Text txt;

    // Start is called before the first frame update
    //void Start()
    //{
    //    try
    //    {
    //        txt = gameObject.GetComponent<Text>() as Text;
    //        if (txt == null)
    //            txt.text = "Hint : ABC";
    //    }
    //    catch (NullReferenceException ex)
    //    {
    //        Debug.Log("Error handled at hints_panel" + ex);
    //    }

    //}

    // Update is called once per frame
    void Update()
    {

    }
}
