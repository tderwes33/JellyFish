using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Start_Music : MonoBehaviour
{
    // Start is called before the first frame update
    static bool AudioBegin = false;
    GameObject otherSound;

    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    

    void Awake()
    {
        otherSound = GameObject.FindGameObjectWithTag("Game Music");

        if (otherSound == this.gameObject)
        {
            if (!AudioBegin)
            {
                DontDestroyOnLoad(this.gameObject);
                AudioBegin = true;
            }
        }
        else
        {
            Debug.Log("Else");
            Destroy(this.gameObject);
        }



    }
}
