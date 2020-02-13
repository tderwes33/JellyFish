using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveBubbles : MonoBehaviour
{
    public GameObject Bubble;
    public float LAUNCH_SPEED = 10f;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        Rigidbody2D rb = Bubble.GetComponent<Rigidbody2D>();
        if (rb != null)
            rb.velocity = transform.right * LAUNCH_SPEED;
    }
}
