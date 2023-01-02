using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    // tells the game manager how many holes it should load in
    // the default value gives us three holes
    public static int holeKey = 0;

    public void PlayGame() {
        SceneManager.LoadScene(1);
    }

    // parameter is so that we can attach it to the dropdown and its value will change the key upon each change
    public void HoleNumberChange(int value)
    {
        holeKey = value;
    }
    public void QuitGame() {
        Debug.Log("User quit the game");
        Application.Quit();
    }
}
