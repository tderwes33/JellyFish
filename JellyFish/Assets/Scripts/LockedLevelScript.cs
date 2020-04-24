using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using UnityEngine;

public class LockedLevelScript : MonoBehaviour
{
    void Update(){
        if (Application.platform == RuntimePlatform.Android)
        {

            // Check if Back was pressed this frame
            if (Input.GetKeyDown(KeyCode.Escape))
            {
                // Quit the application
                SceneManager.LoadScene(sceneName: "LevelScene");
            }
        }
    }
    public void click()
    { 
        SceneManager.LoadScene(sceneName: "LevelScene");
    }
   
}
