using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
public class EasyScript : MonoBehaviour
{
    
    public static int temp_level;

    public void EasyClick()
    {
        SceneManager.LoadScene(sceneName: "BubbleScene");
        temp_level = 1; //1 indicates easy
    }
    public void MediumClick()
    {
        SceneManager.LoadScene(sceneName: "BubbleScene");
        temp_level = 6; //6 onwards indicates medium
    }
    public void HardClick()
    {
        SceneManager.LoadScene(sceneName: "BubbleScene");
        temp_level = 13; //13 onwards indicates Hard
    }
}
