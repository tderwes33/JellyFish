using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
public class EasyScript : MonoBehaviour
{
     
    public static int temp_level;

    void Update(){
        if (Application.platform == RuntimePlatform.Android)
        {

            // Check if Back was pressed this frame
            if (Input.GetKeyDown(KeyCode.Escape))
            {
                // Quit the application
                SceneManager.LoadScene(sceneName: "StartScene");
            }
        }
    }
    public void EasyClick()
    {
        SceneManager.LoadScene(sceneName: "BubbleScene");
        temp_level = 1; //1 indicates easy
    }
    public void MediumClick()
    {
        if (force.previousBestLevel >= 5)
        {
            SceneManager.LoadScene(sceneName: "BubbleScene");
            temp_level = 6; //6 onwards indicates medium
        }
        else
        {
            
            SceneManager.LoadScene(sceneName: "LockedLevelScene");
            //Display canvas saying complete Easy Level First
        }
    }
    public void HardClick()
    {
        if (force.previousBestLevel >= 12)
        {
            SceneManager.LoadScene(sceneName: "BubbleScene");
            temp_level = 13; //13 onwards indicates Hard
        }
        else
        {
          
            SceneManager.LoadScene(sceneName: "LockedLevelScene");
            //Display canvas saying complete Easy and Medium Levels first
        }
    }
    public void EasyBackClick()
    {
        SceneManager.LoadScene(sceneName: "LevelScene");
     
    }
    public void BackClick()
    {
        SceneManager.LoadScene(sceneName: "StartScene");
     
    }
}
