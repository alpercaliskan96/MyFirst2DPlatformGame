using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class EndGame : MonoBehaviour
{

    Player player;

    public Text point;

    public GameObject endGameManager;

    public GameObject endGameCanvas;

    void Start()
    {
        player = FindObjectOfType<Player>();
    }

    

    private void OnTriggerEnter2D(Collider2D other)
    {

        if (other.tag == "Player")
        {
            endGameCanvas.SetActive(true);


            point.text = player.currentPoints.ToString();

            Time.timeScale = 0;
        }
    }



}
