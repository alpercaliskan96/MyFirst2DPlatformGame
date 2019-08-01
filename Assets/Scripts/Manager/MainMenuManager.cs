using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public class MainMenuManager : MonoBehaviour
{
    Player player;
    public GameObject userNameIF;
    public TextMeshProUGUI userName;
    public GameObject startPosition;
    GameObject checkPoint;
    public GameObject checkPointHolder;

    public void StartGame()
    {
        //build ediyorken scene içerisindeki game'i alır
        //game yerine 1 de diyebilridim SceneManager.LoadScene("Game");
        PlayerPrefs.SetString("checkpoint", "");
        SceneManager.LoadScene("Game");

    }

    public void StartGameWithUserName()
    {
        //build ediyorken scene içerisindeki game'i alır
        //game yerine 1 de diyebilridim SceneManager.LoadScene("Game");
            
        PlayerPrefs.SetString("userName", userNameIF.GetComponent<TextMeshProUGUI>().text.ToString());
        PlayerPrefs.SetString("checkpoint", "");
        SceneManager.LoadScene("Game");

    }

    public void BackToMainMenu()
    {
        PlayerPrefs.SetString("userName", "");
        PlayerPrefs.SetString("checkpoint", "");
        SceneManager.LoadScene("Game");        
    }


    //oyundan cıkıs yapmak icin
    public void QuitGame()
    {
        PlayerPrefs.SetString("userName", "");
        PlayerPrefs.SetString("checkpoint", "");
        Application.Quit();
    }
}
