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
        
        force f = new force();
        f.loadData();
       
        // if (force.previousEasyBestLevel>=1  && force.previousEasyBestLevel <= 5)
        //{
            temp_level = force.previousEasyBestLevel;
        //}
        //else { 
        //    temp_level = 1; //1 indicates easy
        //}
        SceneManager.LoadScene(sceneName: "BubbleScene");
    }
    public void MediumClick()
    {
        force f = new force();
        f.loadData();
        
       if (force.previousBestLevel > 5)
        {
            
            
        //        if (force.previousBestLevel >= 6 && force.previousBestLevel <= 12)
          //      {
                    temp_level = force.previousMediumBestLevel;
           //     }
             //   else
             //   {
              //      temp_level = 6; //6 onwards indicates medium
               // }
            SceneManager.LoadScene(sceneName: "BubbleScene");
        }
        else
        {
            
            SceneManager.LoadScene(sceneName: "LockedLevelScene");
            //Display canvas saying complete Easy Level First
        }
    }
    public void HardClick()
    {
        force f = new force();
        f.loadData();
            if (force.previousBestLevel > 12)
            {
               
               // if (force.previousBestLevel >= 13 && force.previousBestLevel <= 21)
                //{
                    temp_level = force.previousHardBestLevel;
                //}
                //else
                //{
                 //   temp_level = 13; //13 onwards indicates Hard
                //}
            SceneManager.LoadScene(sceneName: "BubbleScene");
        }
        else
        {
          
            SceneManager.LoadScene(sceneName: "LockedLevelScene");
            //Display canvas saying complete Easy and Medium Levels first
        }
    }
    public void EasyBackClick()
    {
        force f = new force();
        f.saveData();
        SceneManager.LoadScene(sceneName: "LevelScene");
     
    }
    public void BackClick()
    {
        force f = new force();
        f.saveData();
        SceneManager.LoadScene(sceneName: "StartScene");
     
    }
}
