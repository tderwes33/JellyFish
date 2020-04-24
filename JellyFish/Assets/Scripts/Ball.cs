using UnityEngine;
using System.Collections;


public class Ball : MonoBehaviour {
    //public AudioSource source;
    public enum BALL_TYPE {
		NONE = -1,
		TYPE_1,
		TYPE_2,
		TYPE_3,
		TYPE_4,
		TYPE_5
	}

    
/*
    void Start()
    {
        source = GetComponent<AudioSource>();
    }

    private void OnCollisionEnter(Collision collision)
    {

        if (collision.gameObject.tag == "Letter4") {
            source.Play();
            Debug.Log("Hello");
        }
            
    }
    */

}
