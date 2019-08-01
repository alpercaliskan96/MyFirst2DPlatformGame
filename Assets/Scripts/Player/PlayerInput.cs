using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInput : MonoBehaviour
{

    Player player;

    void Start()
    {
        player = GetComponent<Player>();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetButtonDown("Jump") && player.isGrounded)
        {
            player.Jump();
        }
        else if (Input.GetButtonDown("Jump") && !player.isGrounded && player.canDoubleJump)
        {
            player.DoubleJump();
        }

        if (Input.GetButtonDown("Fire1"))
        {
            player.playerShoot();

        }


    }
}
