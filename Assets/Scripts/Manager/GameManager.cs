using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using TMPro;
public class GameManager : MonoBehaviour
{
    Player player;

    //UI
    public Slider healthBar;
    public Text points;
    public Text ammo;
    public TextMeshProUGUI userName;
    public GameObject userNameHolder;
    public GameObject endUserNameHolder;

    void Start()
    {
        userNameHolder.GetComponent<Text>().text = PlayerPrefs.GetString("userName");
        endUserNameHolder.GetComponent<Text>().text = PlayerPrefs.GetString("userName");
        player = FindObjectOfType<Player>();
        healthBar.maxValue = player.maxPlayerHealth;

    }

    void Update()
    {
        points.text = "Points : " + player.currentPoints.ToString();
        ammo.text = "Ammo : " + player.ammo.ToString();

        if (player.isDead)
        {
            Invoke("RestartGame", 1);
        }
        updateUI();
    }

    public void RestartGame()
    {
        player.RecoverPlayer();
    }


    void updateUI()
    {
        healthBar.value = player.currentPlayerHealth;
        if (player.currentPlayerHealth <= 0)
            healthBar.minValue = 0;
    }
}
