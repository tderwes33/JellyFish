using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class display_category : MonoBehaviour
{
    Text txt;
    //force f;
    // Start is called before the first frame update
    void Start()
    {
        //f = GameObject.FindGameObjectWithTag("Letter1").GetComponent<force>();
        txt = gameObject.GetComponent<Text>();
        //txt.text = "Category :  "+f.getCat();
    }

    // Update is called once per frame
    public void doUpdate(string t)
    {
        txt.text = "Category :  " + t;
    }
}
