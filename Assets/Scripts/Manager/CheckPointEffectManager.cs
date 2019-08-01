using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CheckPointEffectManager : MonoBehaviour
{
    Player player;
    public GameObject Idle;
    public GameObject PlayerIn;
    public GameObject PlayerSelf;
    public int index;

    public bool playerGotIn = false;

    private void OnTriggerEnter2D(Collider2D other)
    {

        if (other.tag == "Player" && !playerGotIn)
        {
            Idle.SetActive(false);  
            PlayerIn.SetActive(true);
            playerGotIn = true;
            PlayerSelf.GetComponent<Player>().curCheckpoint = gameObject;
            PlayerPrefs.SetInt("curAmmo", other.GetComponent<Player>().ammo);

        }

    }

}
