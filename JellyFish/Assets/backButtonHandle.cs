using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class backButtonHandle : MonoBehaviour
{
    string sceneName;
 
    void Update()
    {
        if(Input.GetKeyDown(KeyCode.Escape)) /* checks if back button is pressed */
        {
            sceneName = SceneManager.GetActiveScene().name;
            Debug.Log("scene name = " + sceneName);
            if (sceneName == "BubbleScene")
            {
                Debug.Log("Should enter StartScene");
                SceneManager.LoadScene(sceneName: "StartScene");
            }
            else if(sceneName == "StartScene")
            {
                Debug.Log("Should kill the app");
                Application.Quit();
            }
        }
        

    }
}
