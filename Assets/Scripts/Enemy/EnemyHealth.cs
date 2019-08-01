using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyHealth : MonoBehaviour
{

    public int maxEnemyHealth;
    public float currentEnemyHealth;
    internal bool gotDamage;
    public float damage;
    //mermi ile verilen hasar
    public float projectTileDamage = 25;
    Transform deathParticle;
    SpriteRenderer graph;
    CircleCollider2D circle2D;
    Rigidbody2D body2D;


    Player player;
    AudioSource audioSource;
    public AudioClip ac_Dead;


    void Start()
    {
        currentEnemyHealth = maxEnemyHealth;
        player = FindObjectOfType<Player>();
        graph = GetComponent<SpriteRenderer>();
        circle2D = GetComponent<CircleCollider2D>();
        body2D = GetComponent<Rigidbody2D>();
        deathParticle = transform.Find("DeathParticle");
        deathParticle.gameObject.SetActive(false);
        audioSource = GetComponent<AudioSource>();
      
    }

    // Update is called once per frame
    void Update()
    {
        if (currentEnemyHealth <= 0)
        {
            graph.enabled = false;
            circle2D.enabled = false;
            deathParticle.gameObject.SetActive(true);
            body2D.constraints = RigidbodyConstraints2D.FreezePositionX;
            //bug fix - animasyon eklendi - havada kalması çözüldü
            gameObject.GetComponent<BoxCollider2D>().enabled = false;
            Destroy(gameObject,1);

        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {

        if (other.tag == "PlayerItem" && player.canDamage)
        {
            currentEnemyHealth -= damage;
            audioSource.PlayOneShot(ac_Dead);
            player.ammo += 5;
            player.currentPoints += 30;
            player.currentPlayerHealth += 10;

        }

        //mermiyse düşmanı öldürebilsin
        if (other.tag == "PlayerProjectTile")
        {
            currentEnemyHealth -= projectTileDamage;

            if(currentEnemyHealth <= 0)
            {
                Destroy(other.gameObject);
                player.ammo += 4;
                player.currentPoints += 20;
                player.currentPlayerHealth += 5;
                audioSource.PlayOneShot(ac_Dead);
                
            }
            
        }
    }
}
