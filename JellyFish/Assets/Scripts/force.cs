using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using UnityEngine;
using UnityEngine.UI;

public class force : MonoBehaviour
{
    int direction = -1;

    // Update is called once per frame
    static string word1="brunda".ToUpper();
    //static string word_formed = word1.Distinct().ToArray().ToString();
    static string word_formed = word1;
    public char[] char_arr = word_formed.ToCharArray();
    public int i = 0;

    void Update()
    {
        transform.Translate(Time.deltaTime * 2 * direction, 0, 0);
    }
    void Start()
    {
        Text t = gameObject.GetComponentInChildren<Text>();
        Debug.Log(t.text);
        if (i < char_arr.Length)
        {
            var random_index = Random.Range(0, char_arr.Length - 1);

            t.text = char_arr[random_index].ToString();

            i++;

        }
        else if (i == char_arr.Length)
        {
            i = 0;
        }
    }
    private void OnCollisionEnter2D(Collision2D collision)
    {
        Debug.Log(word_formed);
		if (collision.gameObject.tag == "bullet")
		{
            //GameObject g = Instantiate(collision.gameObject, new Vector3(0,0), transform.rotation);
            collision.gameObject.SetActive(false);
            gameObject.SetActive(false);
            Invoke("ResetBall", 0.2f);
            Text t = gameObject.GetComponentInChildren<Text>();
            Debug.Log(t.text);
            if (i < char_arr.Length) {
                var random_index = Random.Range(0, char_arr.Length-1);
                
                t.text = char_arr[i].ToString();

                i++;

            }
            else if (i == char_arr.Length)
            {
                i = 0;
            }
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