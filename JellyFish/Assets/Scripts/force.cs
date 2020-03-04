using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using UnityEngine;
using UnityEngine.UI;
using System.Text.RegularExpressions;


public class force : MonoBehaviour
{


    public int new_hint = 0;
    public int temp_flag = 0;

    public static int health = 3;

    public static string actual_word = "";
    public static string hint_1 = "";
    public static string hint_2 = "";
    public static string hint_3 = "";
    public static string category = "";
    Color trans = new Color(1f, 1f, 1f, 0f);
    Color opaque = new Color(1f, 1f, 1f, 1f);

    int direction = -1;

    public static char[] allCharacters = { 'A', 'B', 'C', 'D', 'E', 'F', 'G', 'H', 'I', 'J', 'K', 'L', 'M', 'N', 'O', 'P', 'Q', 'R', 'S', 'T', 'U', 'V', 'W', 'X', 'Y', 'Z' };

    public GameObject h1, h2, h3, gameover, restart_Button;
    static GameObject bullet;
    // the three hearts

    //public hints_panel hp;


    // Update is called once per frame
    public static string word1=actual_word.ToUpper();
    public static string word_formed = new string(word1.Distinct().ToArray());
    public static string word_formed1 = word1;

    hints_panel hp;
    //public static string word_formed = word1;
    public int word_length = word_formed.Length;
    public int word_length1 = word_formed1.Length;

    public static char[] char_arr = word_formed.ToCharArray();
    public Text wordCreated;
    static public int i = 0;
    public static List<char> CorrectandIncorrect = new List<char>();
    static char[] remaining;
    public static List<char> Incorrect = new List<char>();
    public static List<char> Correct = new List<char>();
    public static bool paused = false;
    public static bool reset = false;

    public bool getPaused()
    {
        return reset || paused;
    }

    public void setPaused(bool bo)
    {
        paused = bo;
    }
    public void setReset(bool bo)
    {
        reset = bo;
    }
    public string getHint()
    {
        string hint = "";
        //if(h1.GetComponentInChildren<SpriteRenderer>().color == trans)
        {
            hint = hint + "\n" + hint_1;
        }
        if (h1.GetComponentInChildren<SpriteRenderer>().color == trans)
        {
            hint = hint + "\n" + hint_2;
        }
        if (h2.GetComponentInChildren<SpriteRenderer>().color == trans)
        {
            hint = hint + "\n" + hint_3;
        }
        Debug.Log(hint);
        return hint;
    }

    public string getCat()
    {
        Debug.Log(category);
        return category;
    }

    class CSVReader
    {
        static string SPLIT_RE = @",(?=(?:[^""]*""[^""]*"")*(?![^""]*""))";
        static string LINE_SPLIT_RE = @"\r\n|\n\r|\n|\r";
        static char[] TRIM_CHARS = { '\"' };

        public static List<Dictionary<string, object>> Read(string file)
        {
            var list = new List<Dictionary<string, object>>();
            TextAsset data = Resources.Load(file) as TextAsset;

            var lines = Regex.Split(data.text, LINE_SPLIT_RE);

            if (lines.Length <= 1) return list;

            var header = Regex.Split(lines[0], SPLIT_RE);
            for (var i = 1; i < lines.Length; i++)
            {

                var values = Regex.Split(lines[i], SPLIT_RE);
                if (values.Length == 0 || values[0] == "") continue;

                var entry = new Dictionary<string, object>();
                for (var j = 0; j < header.Length && j < values.Length; j++)
                {
                    string value = values[j];
                    value = value.TrimStart(TRIM_CHARS).TrimEnd(TRIM_CHARS).Replace("\\", "");
                    object finalvalue = value;
                    int n;
                    float f;
                    if (int.TryParse(value, out n))
                    {
                        finalvalue = n;
                    }
                    else if (float.TryParse(value, out f))
                    {
                        finalvalue = f;
                    }
                    entry[header[j]] = finalvalue;
                }
                list.Add(entry);
            }
            return list;
        }
    }




    //static public char[] char_arr1 = "brunda".ToCharArray();

    void Update()
    {
        //hp = gameObject.AddComponent<hints_panel>();
        //Debug.Log(hp.getPaused());
        if (gameObject.tag == "restart_Button")
            return;
        if (!getPaused())
        {
            Time.timeScale = 1;
            transform.Translate(Time.deltaTime * 1 * direction, 0, 0);
        } else
        {
            RayCastShooter bull = GameObject.FindGameObjectWithTag("ball").GetComponent<RayCastShooter>();
            //bull.gameObject.SetActive(false);
            bull.bullet.gameObject.SetActive(false);
            bull.dots.Clear();
            bull.bulletProgress = 0.0f;
            //bull.dotsPool.Clear();
            bull.DrawPaths();
            Time.timeScale = 0;
        }
        
        
    }

    public void Reset()
    {
        if(gameover!=null)
        gameover.SetActive(false);
        //hp = GameObject.FindGameObjectWithTag("hint_button").GetComponent<hints_panel>();
        if (hp != null)
        {
            
        }
        setReset(false);
        health = 3;
        
        GameObject.FindGameObjectWithTag("hint_button").GetComponent<Image>().color = Color.white;
        gameObject.SetActive(false);
        if (bullet != null)
            bullet.SetActive(true);
        Incorrect = new List<char>();
        Correct = new List<char>();
        CorrectandIncorrect = new List<char>();
        //hp.paused = false;
        h1 = GameObject.FindGameObjectWithTag("heart1");
        h1.GetComponentInChildren<SpriteRenderer>().color = opaque;
        h2 = GameObject.FindGameObjectWithTag("heart2");
        h2.GetComponentInChildren<SpriteRenderer>().color = opaque;
        h3 = GameObject.FindGameObjectWithTag("heart3");
        h3.GetComponentInChildren<SpriteRenderer>().color = opaque;
        //h1.SetActive(true);
        //h2.SetActive(true);
        //h3.SetActive(true);

        List<Dictionary<string, object>> data = CSVReader.Read("data");
        var random_index_1 = Random.Range(0, data.Count);
        Debug.Log("3");

        actual_word = data[random_index_1]["word"].ToString();
        hint_1 = data[random_index_1]["Hint 1"].ToString();
        hint_2 = data[random_index_1]["Hint 2"].ToString();
        hint_3 = data[random_index_1]["Hint 3"].ToString();
        category = data[random_index_1]["Category"].ToString();
        Debug.Log("4");
        Text cat = GameObject.FindGameObjectWithTag("category").GetComponent<Text>();
        cat.text = "Category : " + (category);
        word1 = actual_word.ToUpper();
        word_formed = new string(word1.Distinct().ToArray());
        word_formed1 = word1;
        word_length = word_formed.Length;
        word_length1 = word_formed1.Length;
        char_arr = word_formed.ToCharArray();
        remaining = new string(allCharacters.Except(char_arr).ToArray()).ToCharArray();
        Debug.Log("5");
        //Comment an entire block
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
        //Till here
        for (int j = 0; j < word_length; j++)
        {
            Correct.Add(char_arr[j]);
        }
        for (int j = 0; j < remaining.Length; j++)
        {
            Incorrect.Add(remaining[j]);
        }

        Debug.Log(new string(remaining.ToArray()));

        
            // i++;
            Debug.Log(Incorrect);
            Debug.Log(Correct);
            string s = "";
            int totalLength = 0;
            while (totalLength != word_length1)
            {
                Debug.Log("Step 1");
                s += "_ ";
                totalLength += 1;
            }
            Debug.Log("Step 2");
            wordCreated.text = s;
        //else if (i == char_arr.Length)
        // else if (i == CorrectandIncorrect.Count)

        updateLetters();
      

    }

    void updateLetters()
    {
        Text t = gameObject.GetComponentInChildren<Text>();
        Debug.Log(t.text);
        //if (i < char_arr.Length)
        Debug.Log(CorrectandIncorrect.Count);
        //if (i < CorrectandIncorrect.Count)
        {
            var tmp_correct = Correct;
            var tmp_incorrect = Incorrect;

            t = GameObject.FindGameObjectWithTag("Letter1").GetComponentInChildren<Text>();
            {
                var random_index = Random.Range(0, tmp_correct.Count);

                t.text = tmp_correct[random_index].ToString();
                tmp_correct.Remove(t.text[0]);
            }
            t = GameObject.FindGameObjectWithTag("Letter2").GetComponentInChildren<Text>();
            {
                var random_index = Random.Range(0, tmp_correct.Count);

                t.text = tmp_correct[random_index].ToString();
                tmp_correct.Remove(t.text[0]);
            }
            t = GameObject.FindGameObjectWithTag("Letter3").GetComponentInChildren<Text>();
            {
                var random_index = Random.Range(0, tmp_incorrect.Count);

                t.text = tmp_incorrect[random_index].ToString();
                tmp_incorrect.Remove(t.text[0]);
            }
            t = GameObject.FindGameObjectWithTag("Letter4").GetComponentInChildren<Text>();
            {
                var random_index = Random.Range(0, tmp_incorrect.Count);

                t.text = tmp_incorrect[random_index].ToString();
                tmp_incorrect.Remove(t.text[0]);
            }
            t = GameObject.FindGameObjectWithTag("Letter5").GetComponentInChildren<Text>();
            {
                var random_index = Random.Range(0, tmp_incorrect.Count);

                t.text = tmp_incorrect[random_index].ToString();
                tmp_incorrect.Remove(t.text[0]);
            }

        }
    }

void Start()
    {
        if (gameObject.tag == "restart_Button")
            return;
        Random.seed = System.DateTime.Now.Millisecond;
        GameObject.FindGameObjectWithTag("hint_button").GetComponent<Image>().color = Color.white;
        Debug.Log("1");
        //hp.paused = false;
        h1 = GameObject.FindGameObjectWithTag("heart1");
        h2 = GameObject.FindGameObjectWithTag("heart2");
        h3 = GameObject.FindGameObjectWithTag("heart3");
        // game object reference put using unity ui only
        Debug.Log("2");
        if (hp == null)
            hp = GameObject.FindGameObjectWithTag("hint_button").GetComponent<hints_panel>();

    if (remaining == null)
    {
        Debug.Log("21");
            

            List<Dictionary<string, object>> data = CSVReader.Read("data");
            var random_index_1 = Random.Range(0, data.Count);
            Debug.Log("3");

            actual_word = data[random_index_1]["word"].ToString();
            hint_1 = data[random_index_1]["Hint 1"].ToString();
            hint_2 = data[random_index_1]["Hint 2"].ToString();
            hint_3 = data[random_index_1]["Hint 3"].ToString();
            category = data[random_index_1]["Category"].ToString();
            Debug.Log("4");
            Text cat = GameObject.FindGameObjectWithTag("category").GetComponent<Text>();
            cat.text="Category : "+(category);
            word1 = actual_word.ToUpper();
            word_formed = new string(word1.Distinct().ToArray());
            word_formed1 = word1;
            word_length = word_formed.Length;
            word_length1 = word_formed1.Length;
            char_arr = word_formed.ToCharArray();
            remaining = new string(allCharacters.Except(char_arr).ToArray()).ToCharArray();
            Debug.Log("5");
            //Comment an entire block
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
            //Till here
            for (int j = 0; j < word_length; j++)
            {
                Correct.Add(char_arr[j]);
            }
            for (int j = 0; j < remaining.Length; j++)
            {
                Incorrect.Add(remaining[j]);
            }

            Debug.Log(new string(remaining.ToArray()));

            
                string s = "";
                int totalLength = 0;
                while (totalLength != word_length1)
                {
                    Debug.Log("Step 1");
                    s += "_ ";
                    totalLength += 1;
                }
                Debug.Log("Step 2");
                wordCreated.text = s;
                // i++;
                Debug.Log(Incorrect);
                Debug.Log(Correct);

                //else if (i == char_arr.Length)
                // else if (i == CorrectandIncorrect.Count)


            }
        
    Text t = gameObject.GetComponentInChildren<Text>();
    Debug.Log(t.text);
            //if (i < char_arr.Length)
            Debug.Log(CorrectandIncorrect.Count);
        //if (i < CorrectandIncorrect.Count)
        {
            var tmp_correct = Correct;
            var tmp_incorrect = Incorrect;

            if (gameObject.tag == "Letter1")
            {
                var random_index = Random.Range(0, tmp_correct.Count);

                t.text = tmp_correct[random_index].ToString();
                tmp_correct.Remove(t.text[0]);
            }
            else if (gameObject.tag == "Letter2")
            {
                var random_index = Random.Range(0, tmp_correct.Count);

                t.text = tmp_correct[random_index].ToString();
                tmp_correct.Remove(t.text[0]);
            }
            else if (gameObject.tag == "Letter3")
            {
                var random_index = Random.Range(0, tmp_incorrect.Count);

                t.text = tmp_incorrect[random_index].ToString();
                tmp_incorrect.Remove(t.text[0]);
            }
            else if (gameObject.tag == "Letter4")
            {
                var random_index = Random.Range(0, tmp_incorrect.Count);

                t.text = tmp_incorrect[random_index].ToString();
                tmp_incorrect.Remove(t.text[0]);
            }
            else if (gameObject.tag == "Letter5")
            {
                var random_index = Random.Range(0, tmp_incorrect.Count);

                t.text = tmp_incorrect[random_index].ToString();
                tmp_incorrect.Remove(t.text[0]);
            }
            hp = GameObject.FindGameObjectWithTag("hint_button").GetComponent<hints_panel>();

        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        Debug.Log(word_formed);
        if (collision.gameObject.tag == "bullet" && gameObject.tag == "Letter5")
        {
            collision.gameObject.SetActive(false);
            gameObject.SetActive(false);
            Invoke("ResetBall", 0.2f);
            Debug.Log("hit heart");
            if (health == 3)
            {
                // do nothing
                Debug.Log("health=" + health);
            }
            else if (health <= 2)
            {
                // increase health
                health++;
                if (health == 3)
                {
                    Debug.Log("health=" + health);
                    //h1.SetActive(true);
                    h1.GetComponentInChildren<SpriteRenderer>().color = opaque;
                }
                else if (health == 2)
                {
                    Debug.Log("health=" + health);
                    //h2.SetActive(true);
                    h2.GetComponentInChildren<SpriteRenderer>().color = opaque;
                }
            }
        }

        else if(collision.gameObject.tag == "bullet")
        {

            //GameObject g = Instantiate(collision.gameObject, new Vector3(0,0), transform.rotation);
            collision.gameObject.SetActive(false);
            gameObject.SetActive(false);
            Invoke("ResetBall", 0.2f);
            Text t = gameObject.GetComponentInChildren<Text>();
            Debug.Log(Incorrect);
            Debug.Log(Correct);

            if (word_formed1.Contains(t.text[0])) //Condition when it is a correct character  
            {
                var foundIndexes = new List<int>();


                for (int j = word_formed1.IndexOf(t.text[0]); j > -1; j = word_formed1.IndexOf(t.text[0], j + 1))
                {
                    // for loop end when i=-1 ('a' not found)
                    foundIndexes.Add(j);
                }
                char[] x = wordCreated.text.ToCharArray();
                Debug.Log(x.ToString());
                for (int j = 0; j < foundIndexes.Count; j++)
                {
                    x[foundIndexes[j] * 2] = t.text[0];

                }

                Debug.Log("Before collision" + new string(CorrectandIncorrect.ToArray()));
                CorrectandIncorrect.Remove(t.text[0]);
                Debug.Log("After collision" + new string(CorrectandIncorrect.ToArray()));
                int random_index;
                do
                {
                    random_index = Random.Range(0, remaining.Length);
                }
                while (CorrectandIncorrect.Contains(remaining[random_index]));
                CorrectandIncorrect.Add(remaining[random_index]);

                wordCreated.text = new string(x);
                if (wordCreated.text.Replace(" ", "").Equals(word_formed1))
                {
                    Text t11 = gameover.GetComponentInChildren<Text>();
                    t11.text = "Yay!";
                    gameover.SetActive(true);
                    setReset(true);
                    //hp.gameObject.SetActive(false);
                    gameObject.SetActive(true);
                    collision.gameObject.SetActive(false);
                    //TODO
                    bullet = collision.gameObject;
                    restart_Button.SetActive(true);
                }
            }


            else
            {
                /** decrement health **/
                health--;
                //new_hint = 1;
                if (health == 2)
                {
                    h1.GetComponentInChildren<SpriteRenderer>().color = trans;
                    //h1.SetActive(false);
                    GameObject.FindGameObjectWithTag("hint_button").GetComponent<Image>().color = Color.red;
                }

                else if (health == 1)
                {
                    h2.GetComponentInChildren<SpriteRenderer>().color = trans;
                    //h2.SetActive(false);
                    GameObject.FindGameObjectWithTag("hint_button").GetComponent<Image>().color = Color.green;

                }
                else if (health == 0)
                {
                    Text t11 = gameover.GetComponentInChildren<Text>();
                    t11.text = "Game Over!";
                    gameover.SetActive(true);
                    setReset(true);
                    //hp.gameObject.SetActive(false);
                    gameObject.SetActive(true);
                    collision.gameObject.SetActive(false);
                    h3.GetComponentInChildren<SpriteRenderer>().color = trans;
                    //h3.SetActive(false);
                    //TODO
                    restart_Button.SetActive(true);
                }
            }



            if (gameObject.tag == "Letter1" || gameObject.tag == "Letter2")
            {
                Correct.Remove(t.text[0]);
            }
            else
            {
                Incorrect.Remove(t.text[0]);
            }

            var tmp_correct = Correct;
            var tmp_incorrect = Incorrect;

            if (tmp_correct.Count == 0 && gameObject.tag == "Letter1")
            {
                var random_index = Random.Range(0, tmp_incorrect.Count);

                t.text = tmp_incorrect[random_index].ToString();
                tmp_incorrect.Remove(t.text[0]);
            }
            else if (gameObject.tag == "Letter1")
            {
                var random_index = Random.Range(0, tmp_correct.Count);

                t.text = tmp_correct[random_index].ToString();
                tmp_correct.Remove(t.text[0]);
            }
            else if (tmp_correct.Count == 0 && gameObject.tag == "Letter2")
            {
                var random_index = Random.Range(0, tmp_incorrect.Count);

                t.text = tmp_incorrect[random_index].ToString();
                tmp_incorrect.Remove(t.text[0]);
            }
            else if (gameObject.tag == "Letter2")
            {
                var random_index = Random.Range(0, tmp_correct.Count);

                t.text = tmp_correct[random_index].ToString();
                tmp_correct.Remove(t.text[0]);
            }
            else if (gameObject.tag == "Letter3")
            {
                var random_index = Random.Range(0, tmp_incorrect.Count);

                t.text = tmp_incorrect[random_index].ToString();
                tmp_incorrect.Remove(t.text[0]);
            }
            else if (gameObject.tag == "Letter4")
            {
                var random_index = Random.Range(0, tmp_incorrect.Count);

                t.text = tmp_incorrect[random_index].ToString();
                tmp_incorrect.Remove(t.text[0]);
            }
            else if (gameObject.tag == "Letter5")
            {
                //var random_index = Random.Range(0, tmp_incorrect.Count);

                //t.text = tmp_incorrect[random_index].ToString();
                //tmp_incorrect.Remove(t.text[0]);
            }

            if (i < CorrectandIncorrect.Count)
            {
                var random_index = Random.Range(0, CorrectandIncorrect.Count);

                //t.text = CorrectandIncorrect[random_index].ToString();
                //t.text = "B";
                i++;

            }
            else if (i == CorrectandIncorrect.Count)
            {

                i = 0;
            }



        }
        else if (collision.gameObject.tag == "SideWall")
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
