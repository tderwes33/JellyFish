using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;
using UnityEngine.UI;

public class force : MonoBehaviour
{
    int direction = -1;
    // Update is called once per frame

    void Update()
    {
        transform.Translate(Time.deltaTime * 2 * direction, 0, 0);
    }
    private void OnCollisionEnter2D(Collision2D collision)
    {

		if (collision.gameObject.tag == "bullet")
		{
            //GameObject g = Instantiate(collision.gameObject, new Vector3(0,0), transform.rotation);
            collision.gameObject.SetActive(false);
            gameObject.SetActive(false);
            Invoke("ResetBall", 1.0f);
            Text t = gameObject.GetComponentInChildren<Text>();
            Debug.Log(t.text);
            t.text = "B";
        }
        else if(collision.gameObject.tag == "SideWall")
		{
			direction *= -1;
		}
        
    }
    public void ResetBall()
    {
        gameObject.SetActive(true);
        direction *= -1;
    }
}
