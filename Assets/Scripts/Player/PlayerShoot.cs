using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerShoot : MonoBehaviour
{
    /// <summary>
    /// mermiyle alakalı şeyler burda bulunuyor
    /// ses efektleri vs player.cs içinde
    /// </summary>

    Rigidbody2D bulletBody;
    public float bulletSpeed;


    void Start()
    {
        bulletBody = GetComponent<Rigidbody2D>();

        bulletBody.AddForce(new Vector2(bulletSpeed , 0), ForceMode2D.Impulse);

        Invoke("SelfDestroy", 1.5f);
    }

    void SelfDestroy()
    {
        Destroy(gameObject);
    }

    void OnCollisionEnter2D(Collision2D other)
    {
        if (other.gameObject.tag == "Ground" || other.gameObject.tag == "Enemy")
        {
            Destroy(gameObject);
        }
        
    }
    
      
    
}
