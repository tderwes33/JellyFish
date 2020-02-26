using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using UnityEngine;
using UnityEngine.UI;

public class force : MonoBehaviour
{
    public static int health = 3;

    int direction = -1;

    public GameObject h1, h2, h3, gameover;
    // the three hearts


    // Update is called once per frame
    public static string word1="helps".ToUpper();
    //static string word_formed = word1.Distinct().ToArray().ToString();
    public static string word_formed = word1;
    public int word_length = word_formed.Length;
    public static char[] char_arr = word_formed.ToCharArray();
    public Text wordCreated;
    static public int i = 0;
    
    //static public char[] char_arr1 = "brunda".ToCharArray();

    void Update()
    {
        transform.Translate(Time.deltaTime * 2 * direction, 0, 0);
    }
    void Start()
    {

        
        h1 = GameObject.FindGameObjectWithTag("heart1");
        h2 = GameObject.FindGameObjectWithTag("heart2");
        h3 = GameObject.FindGameObjectWithTag("heart3");
        // game object reference put using unity ui only 


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
            if (word_formed.Contains(t.text[0])) //Condition when it is a correct character  
            {
                var foundIndexes = new List<int>();


                for (int j = word_formed.IndexOf('a'); j > -1; j = word_formed.IndexOf('a', j+ 1))
                {
                    // for loop end when i=-1 ('a' not found)
                    foundIndexes.Add(j);
                }
                char[] x = wordCreated.text.ToCharArray();
                Debug.Log(x.ToString());
                for (int j = 0; j < foundIndexes.Count; j++)
                {
                    x[foundIndexes[j] * 2]= t.text[0];

                }
                
                
                wordCreated.text =new string(x);
            }
           
            if (i < char_arr.Length) {
                var random_index = Random.Range(0, char_arr.Length-1);
                
                t.text = char_arr[random_index].ToString();
                //t.text = "B";
                i++;

            }
            else if (i == char_arr.Length)
            {
                
                i = 0;
            }

            /** decrement health **/
            health--;

            if (health == 2)
            {
                h1.SetActive(false);
            }

            else if(health==1){
                h2.SetActive(false);
            }
            else if (health == 0)
            {
                h3.SetActive(false);
                gameover.SetActive(true);
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