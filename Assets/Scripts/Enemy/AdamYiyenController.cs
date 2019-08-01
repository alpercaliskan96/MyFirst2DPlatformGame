using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AdamYiyenController : MonoBehaviour
{
    Rigidbody2D enemyBody2D;
    EnemyHealth enemyHealth;
    Animator adamYiyenAnimator;

    public float enemySpeed;
    //duvar bulma 
    [Tooltip("Karakterin duvar olup olmadigini kontrol eder")]
    bool isGrounded;
    Transform groundCheck;
    const float groundCheckRadius = 0.2f;
    [Tooltip("duvarin ne oldugunu belirler")]
    public LayerMask groundLayer;

    public bool moveRight;

    //dusman ucurumdan dusmesin diye yazildi
    bool onEdge;
    Transform edgeCheck;

    void Start()
    {
        enemyBody2D = GetComponent<Rigidbody2D>();
        groundCheck = transform.Find("GroundCheck");
        edgeCheck = transform.Find("EdgeCheck");
        adamYiyenAnimator = GetComponent<Animator>();
        enemyHealth = GetComponent<EnemyHealth>();
    }

    // Update is called once per frame
    void Update()
    {

    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
 
        if (collision.gameObject.tag == "wall")
        {
            enemySpeed = -enemySpeed;
            if (enemyBody2D.transform.localRotation.y == 0)
            {
                enemyBody2D.transform.localRotation = Quaternion.Euler(enemyBody2D.transform.localRotation.x, 180, enemyBody2D.transform.localRotation.z);
            }
            else
            {
                enemyBody2D.transform.localRotation = Quaternion.Euler(enemyBody2D.transform.localRotation.x, 0, enemyBody2D.transform.localRotation.z);
            }

        }
    }
    private void FixedUpdate()
    {
        //duvara carpiyor mu diye kontrol et
        //isGrounded = Physics2D.OverlapCircle(groundCheck.position, groundCheckRadius, groundLayer);

        if (Physics2D.OverlapCircle(edgeCheck.position, groundCheckRadius, groundLayer) == null)
        {
            onEdge = false;
            if (enemySpeed > 0)
            {
                enemyBody2D.transform.localRotation = Quaternion.Euler(enemyBody2D.transform.localRotation.x, 0, enemyBody2D.transform.localRotation.z);
            }
            else
            {
                enemyBody2D.transform.localRotation = Quaternion.Euler(enemyBody2D.transform.localRotation.x, 180, enemyBody2D.transform.localRotation.z);
            }
        }
        else
        {
            onEdge = true;
        }
        

        if (!onEdge)
        {
            moveRight = !moveRight;
            enemySpeed = -enemySpeed;
            //transform.rota = (moveRight) ? new Vector2(-1, 1) : new Vector2(1, 1);
        }
        enemyBody2D.velocity = new Vector2(-enemySpeed, 0);


    }
}
