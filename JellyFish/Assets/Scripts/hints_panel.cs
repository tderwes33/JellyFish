using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class hints_panel : MonoBehaviour
{
    private force f;
    public GameObject Panel;
    public static int score;
    static Text txt;

    public int addscore(int i)
    {
        score += i;
        return score;
    }
    public void OpenPanel()
    {
        
        if (Panel != null)
        {
            f = GameObject.FindGameObjectWithTag("Letter1").GetComponent<force>();

            //Debug.Log(paused);
            f.setPaused(true);

            Panel.SetActive(true);
            txt = GameObject.FindWithTag("Hint_Text").GetComponent<Text>() as Text;
            //txt = gameObject.GetComponent<Text>();
            if (txt != null)
            {
                Debug.Log(f.getHint());

                txt.text = "Hint : " + f.getHint();
            }
        }
    }

    public void ClosePanel()
    {

        if (Panel != null)
        {
            f = GameObject.FindGameObjectWithTag("Letter1").GetComponent<force>();

            f.setPaused(false);
            Panel.SetActive(false);

        }
    }



    // Start is called before the first frame update
    void Start()
    {

       
    }
    
}
