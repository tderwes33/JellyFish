using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class force : MonoBehaviour
{
    int direction = -1;

    // Update is called once per frame
    void Update()
    {
        transform.Translate(Time.deltaTime * 2 * direction, 0,0);
    }
    private void OnCollisionEnter2D(Collision2D collision)
    {
        

		if (collision.gameObject.tag == "bullet")
		{
			//Destroy(collision.gameObject);
			collision.gameObject.transform.position = new Vector2(0,0);
			//collision.gameObject.transform.position.y = 0;
			Destroy(gameObject);
		}
		else
		{
			direction *= -1;
		}
        
    }
}
