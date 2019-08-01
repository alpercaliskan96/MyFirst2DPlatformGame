using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GiveDamage : MonoBehaviour
{

    public int damage;
    Player player;

    void Start()
    {
        player = FindObjectOfType<Player>();
    }


    private void OnTriggerEnter2D(Collider2D other)
    {
        if(other.tag == "Player")
        {
            //oyuncuya zarar verir
            player.isHurt = true;
            player.currentPlayerHealth -= damage;
        }
    }

    private void OnTriggerStay2D(Collider2D other)
    {
        if (other.tag == "Player")
        {
            //oyuncuya zarar verir
            player.isHurt = false;
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.tag == "Player")
        {
            //oyuncuya zarar verir
            player.isHurt = false;
        }
    }




}
