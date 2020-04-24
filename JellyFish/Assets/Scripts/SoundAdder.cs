
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundAdder : MonoBehaviour
{
    public AudioClip saw;    // Add your Audi Clip Here;
                             // This Will Configure the  AudioSource Component; 
                             // MAke Sure You added AudioSouce to death Zone;
    void Start()
    {
        GetComponent<AudioSource>().playOnAwake = false;
        GetComponent<AudioSource>().clip = saw;
    }

    void OnCollisionEnter2D()  //Plays Sound Whenever collision detected
    {
      //  if(OnCollisionEnter.gameObject.tag == "Letter4")
        GetComponent<AudioSource>().Play();
    }

}
