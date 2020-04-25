using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using UnityEngine;
using UnityEngine.UI;
using System.Text.RegularExpressions;
using UnityEngine.Analytics;
using System.Globalization;
using UnityEngine.SceneManagement;
using System.IO;
using System;
using System.Text;
using Random = UnityEngine.Random;

public class force : MonoBehaviour
{
    private static force instance;
    static public float levelTimer=0;
    public bool updateTimer=true;
    public static int attempts_for_current_level = 1;
    public int new_hint = 0;
    public int temp_flag = 0;
    public static int no_of_hearts_collected = 0;
    public static int health = 3;
    public static int level = 1;
    public static string actual_word = "";
    public static string hint_1 = "";
    public static string hint_2 = "";
    public static string hint_3 = "";
    public static string category = "";
    public static int bubble_flag = 0;
    public static char bubble_letter = ' ';
    public static HashSet<string> guessed_wordset = new HashSet<string>();
    //public int temp_level=1;
    /* for saving and storing */

        public static int previousBestLevel=0;
        public static int previousEasyBestLevel = 1;
        public static int previousMediumBestLevel = 6;
        public static int previousHardBestLevel = 13;
        public static int previousLevel=0;
        public static int previousHealth=0;

    /*                         */

    Color trans = new Color(1f, 1f, 1f, 0f);
    Color opaque = new Color(1f, 1f, 1f, 1f);

    int direction = -1;

    public static char[] allCharacters = { 'A', 'B', 'C', 'D', 'E', 'F', 'G', 'H', 'I', 'J', 'K', 'L', 'M', 'N', 'O', 'P', 'Q', 'R', 'S', 'T', 'U', 'V', 'W', 'X', 'Y', 'Z' };

    public GameObject h1, h2, h3, gameover, restart_Button, reset_canvas, exit_button;
    static GameObject bullet;
    // the three hearts

    //public hints_panel hp;


    // Update is called once per frame
    public static string word1=actual_word.ToUpper();
    public static string word_formed = new string(word1.Distinct().ToArray());
    public static string word_formed1 = word1;
    int bestlevel = 0;
    hints_panel hp;
    //public static string word_formed = word1;
    public int word_length = word_formed.Length;
    public int word_length1 = word_formed1.Length;
    public static int hint_shown = 1;

    public static char[] char_arr = word_formed.ToCharArray();
    public Text wordCreated;
    static public int i = 0;
    public static List<char> CorrectandIncorrect = new List<char>();
    static char[] remaining;
    public static List<char> Incorrect = new List<char>();
    public static List<char> Correct = new List<char>();
    public static bool paused = false;
    public static bool reset = false;

    public static List<int[]> letter_sort = new List<int[]>();
    //public AudioClip bubblepop;

    double timerStart = 0;
    bool extra_showing = false;

    public int increment_letter_selection()
    {
        return Random.Range(0, letter_sort.Count);
    
    }
    
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
        //string hint = "Hints";
        //if(h1.GetComponentInChildren<SpriteRenderer>().color == trans)
        {
            hint = hint + "1. " + hint_1;
        }
        if (hint_shown == 3 || hint_shown == 2 || h1.GetComponentInChildren<SpriteRenderer>().color == trans)
        {
            hint = hint + "\n\n2. " + hint_2;
            if(hint_shown == 1)
                hint_shown = 2;
        }
        if (hint_shown == 3 || h2.GetComponentInChildren<SpriteRenderer>().color == trans)
        {
            hint = hint + "\n\n3. " + hint_3;
            hint_shown = 3;
        }
        Debug.Log(hint);
        return hint;
    }

    public string getCat()
    {   category= string.Join(" ", category.Split(' ').Select(i => i.Substring(0, 1).ToUpper() + i.Substring(1).ToLower()).ToArray()); ;
        Debug.Log(category);
        //return new CultureInfo("en").TextInfo.ToTitleCase(category.ToLower());
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


    public void back_button_press(){
        hp = null;
        remaining = null;
        setReset(false);
        //saveData();
        SceneManager.LoadScene(sceneName: "LevelScene");
    }

    void Update()
    {
        if (updateTimer)
            levelTimer += Time.deltaTime;
        if(extra_showing && (levelTimer - timerStart > 5)){
            extra_showing = false;
            GameObject.FindGameObjectWithTag("extralife").GetComponent<CanvasGroup>().alpha = 0f;
            GameObject.FindGameObjectWithTag("extrahint").GetComponent<CanvasGroup>().alpha = 0f;
        }
        //hp = gameObject.AddComponent<hints_panel>();s
        //Debug.Log(hp.getPaused());
        if (gameObject.tag == "restart_Button" || gameObject.tag == "exit_button")
            return;
        if (!getPaused())
        {
            Time.timeScale = 1;
            //Debug.Log(gameObject.transform.position);
            if (level > 20){
                if(gameObject.tag == "Letter1")
                    transform.Translate(Time.deltaTime * 0.35f * 20 * direction, 0, 0);
                if(gameObject.tag == "Letter2")
                    transform.Translate(Time.deltaTime * 0.25f * 20 * direction, 0, 0);
                if(gameObject.tag == "Letter3")
                    transform.Translate(Time.deltaTime * 0.3f * 20 * direction, 0, 0);
                if(gameObject.tag == "Letter4")
                    transform.Translate(Time.deltaTime * 0.2f * 20 * direction, 0, 0);
                if(gameObject.tag == "Letter5")
                    transform.Translate(Time.deltaTime * 0.4f * 20 * direction, 0, 0);
            }
            else{
                if(gameObject.tag == "Letter1")
                transform.Translate(Time.deltaTime * 0.35f * level * direction, 0, 0);
                if(gameObject.tag == "Letter2")
                transform.Translate(Time.deltaTime * 0.25f * level * direction, 0, 0);
                if(gameObject.tag == "Letter3")
                transform.Translate(Time.deltaTime * 0.3f * level * direction, 0, 0);
                if(gameObject.tag == "Letter4")
                transform.Translate(Time.deltaTime * 0.2f * level * direction, 0, 0);
                if(gameObject.tag == "Letter5")
                transform.Translate(Time.deltaTime * 0.4f * level * direction, 0, 0);
            }
        } else
        {
            
            Time.timeScale = 0;
            RayCastShooter bull = GameObject.FindGameObjectWithTag("ball").GetComponent<RayCastShooter>();
            //bull.gameObject.SetActive(false);
            bull.bullet.gameObject.SetActive(false);
            foreach (var d in bull.dotsPool)
                d.SetActive(false);
            bull.dots.Clear();
            bull.bulletProgress = 0.0f;

        }
        //Trail
        if (Application.platform == RuntimePlatform.Android)
        {

            // Check if Back was pressed this frame
            if (Input.GetKeyDown(KeyCode.Escape))
            {

                // Quit the application
                        hp = null;
                        remaining = null;
                        setReset(false);
                        
        SceneManager.LoadScene(sceneName: "LevelScene");
            }
        }
        //Till here

    }

    public void Reset()
    {
        updateTimer = true;
        GameObject.FindGameObjectWithTag("confetti").GetComponent<ParticleSystem>().Stop();
        Debug.Log("Stopping confetti");
        AnalyticsEvent.Custom("Level", new Dictionary<string, object>
    {
        { "level", level },
        { "word", actual_word }
    });

    Firebase.Analytics.FirebaseAnalytics.LogEvent("level", "number", level);

        if (gameover!=null)
            gameover.SetActive(false);
        reset_canvas.SetActive(false);
        //hp = GameObject.FindGameObjectWithTag("hint_button").GetComponent<hints_panel>();
        hint_shown = 1;
        setReset(false);
        health = 3;
        GameObject.FindGameObjectWithTag("level").GetComponent<Text>().text = "Level : "+level;
        GameObject.FindGameObjectWithTag("hint_button").GetComponent<Image>().color = Color.white;
        gameObject.SetActive(false);
        //if (bullet != null)
         //   bullet.SetActive(true);
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
        string level_status = "";
        
       
        if (level >= 1 && level <= 5)
        {
            level_status = "Easy";
        }
        else if(level >= 6 && level <= 12)
        {
            
            level_status = "Medium";

        }
        else
        {
            
            level_status = "Hard";
            if(level>21)
            {
                level = 1;
                level_status= "Easy";
            }
        }

        List<Dictionary<string, object>> data = CSVReader.Read(level_status);
        var random_index_1 = Random.Range(0, data.Count);
        Debug.Log("3");

        actual_word = data[random_index_1]["word"].ToString();

        while(guessed_wordset.Contains(actual_word.ToUpper()))
        {
            random_index_1 = Random.Range(0, data.Count);
            actual_word = data[random_index_1]["word"].ToString();

        }
        guessed_wordset.Add(actual_word.ToUpper());
        hint_1 = data[random_index_1]["Hint 1"].ToString();
        hint_2 = data[random_index_1]["Hint 2"].ToString();
        hint_3 = data[random_index_1]["Hint 3"].ToString();
        category = data[random_index_1]["Category"].ToString();
        category = string.Join(" ", category.Split(' ').Select(i => i.Substring(0, 1).ToUpper() + i.Substring(1).ToLower()).ToArray()); ;

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
        //Random index selected to be filled 
        var random_letter_selected = Random.Range(0, word_length);
        var random_letter = char_arr[random_letter_selected];
        //Till here
        for (int j = 0; j < word_length; j++)
        {
            if (char_arr[j] != random_letter)
            {
                Correct.Add(char_arr[j]);
            }
        }
        for (int j = 0; j < remaining.Length; j++)
        {
            Incorrect.Add(remaining[j]);
        }

        Debug.Log(new string(remaining.ToArray()));


        // i++;
        Debug.Log(new string(Incorrect.ToArray()));
        Debug.Log(new string(Correct.ToArray()));
        string s = "";
            int totalLength = 0;
            while (totalLength != word_length1)
            {
                Debug.Log("Step 1");
                if (word1[totalLength] != random_letter)
                {
                    s += "_ ";
                }
                else
                {
                    s += random_letter + " ";
                }
            totalLength += 1;
            }
            Debug.Log("Step 2");
            wordCreated.text = s;
        //else if (i == char_arr.Length)
        // else if (i == CorrectandIncorrect.Count)

        updateLetters();
        if (hp == null)
            hp = GameObject.FindGameObjectWithTag("hint_button").GetComponent<hints_panel>();

        hp.OpenPanel();
    }

    void updateLetters()
    {

       
        Text t;
        Debug.Log("Updating letters");
        //if (i < char_arr.Length)
        //if (i < CorrectandIncorrect.Count)
        int selection = increment_letter_selection();
        {
            List<char> tmp_correct = new List<char>(Correct);
            List<char> tmp_incorrect = new List<char>(Incorrect);

            Debug.Log(new string(tmp_correct.ToArray()));
            Debug.Log(new string(tmp_incorrect.ToArray()));

            GameObject g1 = GameObject.FindGameObjectWithTag("Letter1");
            GameObject g2 = GameObject.FindGameObjectWithTag("Letter2");
            GameObject g3 = GameObject.FindGameObjectWithTag("Letter3");
            GameObject g4 = GameObject.FindGameObjectWithTag("Letter4");
            GameObject g5 = GameObject.FindGameObjectWithTag("Letter5");
            Vector3 pos = g1.transform.position;
            pos.y = 3;
            g1.transform.position = pos;
            pos = g2.transform.position;
            pos.y = 1;
            g2.transform.position = pos;
            pos = g3.transform.position;
            pos.y = 2;
            g3.transform.position = pos;
            pos = g4.transform.position;
            pos.y = 0;
            g4.transform.position = pos;
            pos = g5.transform.position;
            pos.y = 4;
            g5.transform.position = pos;

            t = GameObject.FindGameObjectWithTag("Letter"+ letter_sort[selection][0]).GetComponentInChildren<Text>();
           
            if (tmp_correct.Count == 0)
            {
                var random_index = Random.Range(0, tmp_incorrect.Count);

                t.text = tmp_incorrect[random_index].ToString();
                tmp_incorrect.Remove(t.text[0]);
            }
            else
            {
                var random_index = Random.Range(0, tmp_correct.Count);

                t.text = tmp_correct[random_index].ToString();
                tmp_correct.Remove(t.text[0]);
            }

           

            t = GameObject.FindGameObjectWithTag("Letter" + letter_sort[selection][1]).GetComponentInChildren<Text>();
            if (tmp_correct.Count == 0)
            {
                var random_index = Random.Range(0, tmp_incorrect.Count);

                t.text = tmp_incorrect[random_index].ToString();
                tmp_incorrect.Remove(t.text[0]);
            } else
            {
                var random_index = Random.Range(0, tmp_correct.Count);

                t.text = tmp_correct[random_index].ToString();
                tmp_correct.Remove(t.text[0]);
            }
            
            t = GameObject.FindGameObjectWithTag("Letter" + letter_sort[selection][2]).GetComponentInChildren<Text>();
            {
                var random_index = Random.Range(0, tmp_incorrect.Count);

                t.text = tmp_incorrect[random_index].ToString();
                tmp_incorrect.Remove(t.text[0]);
            }
            
            t = GameObject.FindGameObjectWithTag("Letter" + letter_sort[selection][3]).GetComponentInChildren<Text>();
            {
                var random_index = Random.Range(0, tmp_incorrect.Count);

                t.text = tmp_incorrect[random_index].ToString();
                tmp_incorrect.Remove(t.text[0]);
            }
          
            /*
            t = GameObject.FindGameObjectWithTag("Letter5").GetComponentInChildren<Text>();
            {
                var random_index = Random.Range(0, tmp_incorrect.Count);

                t.text = tmp_incorrect[random_index].ToString();
                tmp_incorrect.Remove(t.text[0]);
            }
            */

        }
    }
   

    void Start()
    {
        // GetComponent<AudioSource>().playOnAwake = false;
        // GetComponent<AudioSource>().clip = bubblepop;
        
        AnalyticsEvent.Custom("Level", new Dictionary<string, object>
        {
            { "level", level },
            { "word", actual_word }
        });

       

        Analytics.CustomEvent("Level", new Dictionary<string, object>
          {
            { "level", level },
            { "word", actual_word }
          });
       Firebase.Analytics.FirebaseAnalytics.LogEvent("word", "string", actual_word);

        if (gameObject.tag == "restart_Button" || gameObject.tag == "exit_button")
            return;
        Random.seed = System.DateTime.Now.Millisecond;
        if(letter_sort.Count == 0)
        {
            letter_sort.Add(new int[] { 1, 2, 3, 4 });
            letter_sort.Add(new int[] { 2, 1, 3, 4 });
            letter_sort.Add(new int[] { 3, 1, 2, 4 });
            letter_sort.Add(new int[] { 1, 3, 2, 4 });
            letter_sort.Add(new int[] { 2, 3, 1, 4 });
            letter_sort.Add(new int[] { 3, 2, 1, 4 });
            letter_sort.Add(new int[] { 3, 2, 4, 1 });
            letter_sort.Add(new int[] { 2, 3, 4, 1 });
            letter_sort.Add(new int[] { 4, 3, 2, 1 });
            letter_sort.Add(new int[] { 3, 4, 2, 1 });
            letter_sort.Add(new int[] { 2, 4, 3, 1 });
            letter_sort.Add(new int[] { 4, 2, 3, 1 });
            letter_sort.Add(new int[] { 4, 1, 3, 2 });
            letter_sort.Add(new int[] { 1, 4, 3, 2 });
            letter_sort.Add(new int[] { 3, 4, 1, 2 });
            letter_sort.Add(new int[] { 4, 3, 1, 2 });
            letter_sort.Add(new int[] { 1, 3, 4, 2 });
            letter_sort.Add(new int[] { 3, 1, 4, 2 });
            letter_sort.Add(new int[] { 2, 1, 4, 3 });
            letter_sort.Add(new int[] { 1, 2, 4, 3 });
            letter_sort.Add(new int[] { 4, 2, 1, 3 });
            letter_sort.Add(new int[] { 2, 4, 1, 3 });
            letter_sort.Add(new int[] { 1, 4, 2, 3 });
            letter_sort.Add(new int[] { 4, 1, 2, 3 });
        }
        GameObject.FindGameObjectWithTag("level").GetComponent<Text>().text = "Level : " + level;
        GameObject.FindGameObjectWithTag("hint_button").GetComponent<Image>().color = Color.white;
        Debug.Log("1");
        //hp.paused = false;
        h1 = GameObject.FindGameObjectWithTag("heart1");
        h2 = GameObject.FindGameObjectWithTag("heart2");
        h3 = GameObject.FindGameObjectWithTag("heart3");
        // game object reference put using unity ui only
        //Debug.Log("2");
        if (hp == null)
            hp = GameObject.FindGameObjectWithTag("hint_button").GetComponent<hints_panel>();
         //gameObject.SetActive(true);
        
         
        if (remaining == null)
    {
            level = EasyScript.temp_level;
            levelTimer = 0.0f;
            Debug.Log("21");
            Incorrect = new List<char>();
            Correct = new List<char>();
            CorrectandIncorrect = new List<char>();
            guessed_wordset = new HashSet<string>();
            //List<Dictionary<string, object>> data = CSVReader.Read("data");
            string level_status = "";
            if (level >= 1 && level <= 5)
            {
                level_status = "Easy";
            }
            else if (level >= 6 && level <= 12)
            {
                level_status = "Medium";

            }
            else
            {
                level_status = "Hard";
                if (level > 21)
                {
                    level = 1;
                    level_status = "Easy";
                }
            }

            List<Dictionary<string, object>> data = CSVReader.Read(level_status);
            var random_index_1 = Random.Range(0, data.Count);
            //Debug.Log("3");

            actual_word = data[random_index_1]["word"].ToString();
            while (guessed_wordset.Contains(actual_word.ToUpper()))
            {
                random_index_1 = Random.Range(0, data.Count);
                actual_word = data[random_index_1]["word"].ToString();

            }
            guessed_wordset.Add(actual_word.ToUpper());
            Debug.Log(guessed_wordset);
            hint_1 = data[random_index_1]["Hint 1"].ToString();
            hint_2 = data[random_index_1]["Hint 2"].ToString();
            hint_3 = data[random_index_1]["Hint 3"].ToString();
            category = data[random_index_1]["Category"].ToString();
            category = string.Join(" ", category.Split(' ').Select(i => i.Substring(0, 1).ToUpper() + i.Substring(1).ToLower()).ToArray()); ;

            //Debug.Log("4");
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
            //Random index selected to be filled 
            var random_letter_selected = Random.Range(0, word_length);
            var random_letter = char_arr[random_letter_selected];
            //Till here
            for (int j = 0; j < word_length; j++)
            {   if (word1[j] != random_letter)
                {
                    Correct.Add(char_arr[j]);
                }
                
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
                    if (word1[totalLength] != random_letter)
                    {
                        s += "_ ";
                    }
                    else
                    {
                        s += random_letter + " ";
                    }
                    totalLength += 1;
                }
                Debug.Log("Step 2");
                wordCreated.text = s;
                // i++;
                Debug.Log(new string(Incorrect.ToArray()));
                Debug.Log(new string(Correct.ToArray()));

                //else if (i == char_arr.Length)
                // else if (i == CorrectandIncorrect.Count)

            updateLetters();
            if(hp==null)
                 hp = GameObject.FindGameObjectWithTag("hint_button").GetComponent<hints_panel>();
                    hp.OpenPanelInit("1. " + hint_1);


    }
        

    //Text t = gameObject.GetComponentInChildren<Text>();
    //Debug.Log(t.text);
            //if (i < char_arr.Length)
            Debug.Log(CorrectandIncorrect.Count);
        //if (i < CorrectandIncorrect.Count)
        {
            if(hp==null)
                hp = GameObject.FindGameObjectWithTag("hint_button").GetComponent<hints_panel>();

        }
        /* load previous save data */
        if (gameObject.tag == "Letter1") // because I want only one object to do file IO
        {
            loadData();
            //level = previousBestLevel +1;
            /* loads from previous best */
            //if(level >=21)
            {
            //    previousBestLevel = 1;
            //    level = 1;
            }
        }
        /* end */
    }

   
    private void OnCollisionEnter2D(Collision2D collision)
    {
        Debug.Log(word_formed);
        //if (collision.gameObject.tag == "Letter" ){
        //  GetComponent<AudioSource>().Play();
       // }


        if (collision.gameObject.tag == "bullet" && gameObject.tag == "Letter5")
        {
            GameObject.FindGameObjectWithTag("conf").GetComponent<ParticleSystem>().Play();
            collision.gameObject.SetActive(false);
            gameObject.SetActive(false);
            Invoke("ResetBall", 0.2f);
            Debug.Log("hit heart");
            //Hearts collected by user
            no_of_hearts_collected += 1;
            
            //yield return StartCoroutine(FadeEffect.FadeCanvas(GameObject.FindGameObjectWithTag("extralife").GetComponent<CanvasGroup>(), 0f, 1f, 1f));
            //StartCoroutine(FadeEffect.FadeCanvas(GameObject.FindGameObjectWithTag("extralife").GetComponent<CanvasGroup>(), 1f, 0f, 1f));
            if (health == 3)
            {
                GameObject.FindGameObjectWithTag("extrahint").GetComponent<CanvasGroup>().alpha = 1f;
                
                Text t1 = GameObject.FindGameObjectWithTag("Letter1").GetComponentInChildren<Text>();
                Text t2 = GameObject.FindGameObjectWithTag("Letter2").GetComponentInChildren<Text>();
                Text t3 = GameObject.FindGameObjectWithTag("Letter3").GetComponentInChildren<Text>();
                Text t4 = GameObject.FindGameObjectWithTag("Letter4").GetComponentInChildren<Text>();
                // To highight the bubble
                Color newCol;
                if (ColorUtility.TryParseHtmlString("#F1FF5D", out newCol))
                {
                    if (word1.Contains(t1.text[0]))
                    {
                        GameObject.FindGameObjectWithTag("Letter1").GetComponent<SpriteRenderer>().color = newCol;

                    }
                    else if (word1.Contains(t2.text[0]))
                    {
                        GameObject.FindGameObjectWithTag("Letter2").GetComponent<SpriteRenderer>().color = newCol;

                    }
                    else if (word1.Contains(t3.text[0]))
                    {
                        GameObject.FindGameObjectWithTag("Letter3").GetComponent<SpriteRenderer>().color = newCol;

                    }
                    else if (word1.Contains(t4.text[0]))
                    {
                        GameObject.FindGameObjectWithTag("Letter4").GetComponent<SpriteRenderer>().color = newCol;

                    }
                }

                Debug.Log("health=" + health);
            }
            else if (health <= 2)
            {
                GameObject.FindGameObjectWithTag("extralife").GetComponent<CanvasGroup>().alpha = 1f;
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
            timerStart = levelTimer;
            extra_showing = true;
        }
        else if(collision.gameObject.tag == "bullet")
        {
            //AA93DD
            //GameObject g = Instantiate(collision.gameObject, new Vector3(0,0), transform.rotation);
            collision.gameObject.SetActive(false);
            gameObject.GetComponentInChildren<SpriteRenderer>().color = Color.white;
            GameObject.FindGameObjectWithTag("Letter1").GetComponent<SpriteRenderer>().color = Color.white;
            GameObject.FindGameObjectWithTag("Letter2").GetComponent<SpriteRenderer>().color = Color.white;
            GameObject.FindGameObjectWithTag("Letter3").GetComponent<SpriteRenderer>().color = Color.white;
            GameObject.FindGameObjectWithTag("Letter4").GetComponent<SpriteRenderer>().color = Color.white;

            gameObject.SetActive(false);
            Invoke("ResetBall", 0.2f);
            Text t = gameObject.GetComponentInChildren<Text>();
            Debug.Log(new string(Incorrect.ToArray()));
            Debug.Log(new string(Correct.ToArray()));

            if (word_formed1.Contains(t.text[0])) //Condition when it is a correct character  
            {
                var foundIndexes = new List<int>();
                bubble_flag = 0;

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
                    t11.text = "Congrats!";
                    Text t12 = restart_Button.GetComponentInChildren<Text>();
                    t12.text = "Next";
                    //\n Completed Level "+ level+" ! \n Word: "+ word_formed1+"\n";

                    level += 1;
                    /* save game state */
                    saveData();
                    /* finished saving */
                    gameover.SetActive(true);
                    setReset(true);
                    GameObject.FindGameObjectWithTag("confetti").GetComponent<ParticleSystem>().Play();
                    Debug.Log("Level completed " + level);
                    Analytics.CustomEvent("Level_Health_Attempts", new Dictionary<string, object>
                      {
                        { "level "+level, health},
                        { "Attempts for "+(level), attempts_for_current_level}
                      });
                    Debug.Log("level " + level + health);
                    Debug.Log("Attempts for " + (level) + attempts_for_current_level);
                    //if(level > previousBestLevel)
                    //{
                    //    previousBestLevel = level;
                    //}
                    if (level == 6 )
                    {
                        
                        // //Display all the Easy levels are over
                        // hp = null;
                        // remaining = null;
                         //setReset(false);
                        // //Display a canvas with showing all easy levels completed
                        // SceneManager.LoadScene(sceneName: "LevelScene");

                    }
                    else if (level == 13)
                    {
                        //Display all the Easy levels are over
                        // hp = null;
                        // remaining = null;
                        // setReset(false);
                        // //Display a canvas with showing all medium levels completed

                        // SceneManager.LoadScene(sceneName: "LevelScene");

                    }
                    /* if level is greater than 20 go back to 1 */
                    else if (level >= 21)
                    {
                        // level = 1;
                    }
                    else
                        restart_Button.SetActive(true);
                    /*
                     * for analytics use
                     */

                    //Analytics.CustomEvent("Attempts", new Dictionary<string, object>
                    //  {
                    //    { "Attempts for "+(level-1), attempts_for_current_level}

                    //  });
                    attempts_for_current_level = 1; // reset the attempts count to 1
                    

                    /* analytics ends */
                    //hp.gameObject.SetActive(false);
                    //collision.gameObject.SetActive(false);
                    gameObject.SetActive(true);
                    //TODO
                    bullet = collision.gameObject;
                    
                    reset_canvas.SetActive(true);
                    exit_button.SetActive(true);
                    return;
                } else 
                    GameObject.FindGameObjectWithTag("conf").GetComponent<ParticleSystem>().Play();
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
                    Debug.Log(word1);

                    /* save data */
                    saveData();

                    t11.text = "Game Over!\nCorrect Word: "+actual_word.ToUpper();
                    Text t12 = restart_Button.GetComponentInChildren<Text>();
                    t12.text = "Restart";
                    attempts_for_current_level += 1; //increment number of attempts
                    gameover.SetActive(true);
                    setReset(true);
                    //hp.gameObject.SetActive(false);
                    collision.gameObject.SetActive(false);
                    gameObject.SetActive(true);
                    h3.GetComponentInChildren<SpriteRenderer>().color = trans;
                    //h3.SetActive(false);
                    //TODO
                    restart_Button.SetActive(true);
                    exit_button.SetActive(true);
                    reset_canvas.SetActive(true);
                    Analytics.CustomEvent("gameOver", new Dictionary<string, object>
                        {
                            { "no_of_hearts_collected_by_user:", no_of_hearts_collected }
                        });
                    Debug.Log("no_of_hearts_collected_by_user:" + no_of_hearts_collected);
                    Debug.Log("time played:" + levelTimer);
                    AnalyticsEvent.Custom("gameOver", new Dictionary<string, object>
                    {
                        { "no_of_hearts_collected_by_user:", no_of_hearts_collected }

                    });
                    Debug.Log("no_of_hearts_collected_by_user:" + no_of_hearts_collected);
                    updateTimer = false;
                    return;
                }
       

               
            }



            if (Correct.Contains(t.text[0]))
            {
                Correct.Remove(t.text[0]);
            }
            else
            {
                Incorrect.Remove(t.text[0]);
            }
            gameObject.SetActive(true);
            updateLetters();



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
   
    /* for saving and loading player data */
    public void saveData()
    {
        if (level > previousBestLevel)
        {
            previousBestLevel = level;
        }
        if (level>=1 && level<=6)
        {
            previousEasyBestLevel = level;
            if(level == 6)
                previousEasyBestLevel = 1;
        }
        else if (level> 6 && level <= 13)
        {
            previousMediumBestLevel = level;
            if (level == 13)
                previousMediumBestLevel = 6;
        }
        else if (level > 13 && level <= 22)
        {
            previousHardBestLevel = level;
            if (level == 22)
                previousHardBestLevel = 13;
        }
        SaveSystem.SaveForce(this);
        Debug.Log("Data Saved\n");
    }
    public void loadData()
    {
        PlayerData data = SaveSystem.LoadForce();
        if (data == null)
        {
            return;
        }
        previousLevel = data.level;
        previousHealth = data.health;
        previousBestLevel = data.best;
        previousEasyBestLevel = data.easybest;
        previousMediumBestLevel = data.mediumbest;
        previousHardBestLevel = data.hardbest;
        Debug.Log("Data Loaded\n");
        Debug.Log("Previous Health: " + previousHealth);
        Debug.Log("Previous Level: " + previousLevel);
        Debug.Log("Previous Best: " + previousBestLevel);
        //saveData();
    }
    /* end saving and loading functions */
}
