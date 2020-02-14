using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class hints_panel : MonoBehaviour
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
    void Start()
    {
        txt = gameObject.GetComponent<Text>();
        txt.text = "Hint : ";
    }

    // Update is called once per frame
    void Update()
    {

    }
}
