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

    public static char[] allCharacters = { 'A', 'B', 'C', 'D', 'E', 'F', 'G', 'H', 'I', 'J', 'K', 'L', 'M', 'N', 'O', 'P', 'Q', 'R', 'S', 'T', 'U', 'V', 'W', 'X', 'Y', 'Z' };

    public GameObject h1, h2, h3, gameover;
    // the three hearts

    //public hints_panel hp;

    // Update is called once per frame
    public static string word1="helps".ToUpper();
    public static string word_formed = new string(word1.Distinct().ToArray());

    hints_panel hp;
    //public static string word_formed = word1;
    public int word_length = word_formed.Length;
    public static char[] char_arr = word_formed.ToCharArray();
    public Text wordCreated;
    static public int i = 0;
    public static List<char> CorrectandIncorrect = new List<char>();
    static char[] remaining;


    //static public char[] char_arr1 = "brunda".ToCharArray();

    void Update()
    {
        //hp = gameObject.AddComponent<hints_panel>();
        //Debug.Log(hp.paused);
        if (!hp.getPaused())
        {
            transform.Translate(Time.deltaTime * 2 * direction, 0, 0);
        }
        
    }
    void Start()
    {
        //hp.paused = false;
        h1 = GameObject.FindGameObjectWithTag("heart1");
        h2 = GameObject.FindGameObjectWithTag("heart2");
        h3 = GameObject.FindGameObjectWithTag("heart3");
        // game object reference put using unity ui only

        if (remaining == null)
        {
       
            remaining = new string(allCharacters.Except(char_arr).ToArray()).ToCharArray();

            for (int j = 0; j < word_length + 3; j++)
            {
                if (j >= word_length)
                {
                    var random_index = Random.Range(0, remaining.Length);
                    CorrectandIncorrect.Add(remaining[random_index]);
                }
                else
                {
                    CorrectandIncorrect.Add(char_arr[j]);
                }
            }
            Debug.Log(new string(remaining.ToArray()));
        }
        Text t = gameObject.GetComponentInChildren<Text>();
        Debug.Log(t.text);
        //if (i < char_arr.Length)
        Debug.Log(CorrectandIncorrect.Count);
        //if (i < CorrectandIncorrect.Count)
        {
            //var random_index = Random.Range(0, char_arr.Length);
            var random_index = Random.Range(0, CorrectandIncorrect.Count);
            t.text = CorrectandIncorrect[random_index].ToString();
           // i++;

        }
        //else if (i == char_arr.Length)
       // else if (i == CorrectandIncorrect.Count)
        {
         //   i = 0;
        }

        hp = GameObject.FindGameObjectWithTag("hint_button").GetComponent<hints_panel>();


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


           

            if (word_formed.Contains(t.text[0]) && CorrectandIncorrect.Contains(t.text[0])) //Condition when it is a correct character  
            {
                var foundIndexes = new List<int>();
                

                for (int j = word_formed.IndexOf(t.text[0]); j > -1; j = word_formed.IndexOf(t.text[0], j+ 1))
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
                Debug.Log("BEfore collision" + new string(CorrectandIncorrect.ToArray()));
                CorrectandIncorrect.Remove(t.text[0]);
                Debug.Log("After collision" + new string(CorrectandIncorrect.ToArray()));
                int random_index;
                do { random_index = Random.Range(0, remaining.Length);
                }
                while (CorrectandIncorrect.Contains(remaining[random_index]));
                CorrectandIncorrect.Add(remaining[random_index]);

                wordCreated.text =new string(x);
            }
           
          
	else {
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
	     if (i < CorrectandIncorrect.Count) {
                var random_index = Random.Range(0, CorrectandIncorrect.Count);
                
                t.text = CorrectandIncorrect[random_index].ToString();
                //t.text = "B";
                i++;

            }
            else if (i == CorrectandIncorrect.Count)
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
