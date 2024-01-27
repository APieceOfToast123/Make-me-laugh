using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Trainsiton : MonoBehaviour
{
    public void StartNewGame(){
        SceneManager.LoadScene("GamePage");
    }

    public void Quit()
    {
        if (Application.isEditor) {
            UnityEditor.EditorApplication.isPlaying = false; 
        } 
        // Application.Quit();
    }

    public void BackToMainPage(){
        SceneManager.LoadScene("MainPage");
    }
    
    public void GoToEndPage(){
        SceneManager.LoadScene("EndPage");
    }
}
