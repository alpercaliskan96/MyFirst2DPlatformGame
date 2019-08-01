using System.Collections;
using System.Collections.Generic;
using UnityEngine;


//Bilgisayarda oynamak için FixedUpdate yarısını - Move ve ZeroVelocity tamamen kapat
//PlayerInput kısmındaki fire kısmını aç


[RequireComponent (typeof(Rigidbody2D))]
public class Player : MonoBehaviour
{
    public bool isFirstTime = false;

    Rigidbody2D body2D;
    public float knockBackForce;

    //colliders
    BoxCollider2D box2D;
    CircleCollider2D circle2D;

    public GameObject curCheckpoint;
    public GameObject curAmmo;

    //hız
    [Tooltip("Karakterin ne kadar hizli gidecegini belirler")]
    [Range(0,20)]
    public float playerSpeed;

    //Ziplama
    [Tooltip("Karakterin ne kadar yuksege ziplayacagini belirler")]
    [Range(0, 1500)]
    public float jumpPower;

    [Tooltip("Karakterin 2.kez ne yuksege ziplayacagini belirler")]
    [Range(0, 25)]
    public float doubleJumpPower;

    internal bool canDoubleJump;
    internal bool canDamage;

    //karakteri 180derece dondurme
    bool facingRight = true;

    [Tooltip("Karakterin yerde olup olmadigini kontrol eder")]
    public bool isGrounded;
    Transform groundCheck;
    const float groundCheckRadius = 0.2f;
    [Tooltip("Yerin ne oldugunu belirler")]
    public LayerMask groundLayer;

    //AnimatorControler animasyon kontrol etme yeri
    Animator playerAnimationControler;

    //oyuncu caniyla ilgili
    internal int maxPlayerHealth = 100;
    public int currentPlayerHealth;
    internal bool isHurt;
    //GiveDamage giveDamage;

    //oyuncu olecek
    internal bool isDead;
    public float deadForce;

    //kazanilan puan
    public int currentPoints;
    internal int point = 10;

    //gamemanager 
    GameManager gameManager;

    //checkpoint
    public GameObject startPosition;
    GameObject checkPoint;
    public GameObject deathCanvas;
    public GameObject restartCanvas;
    public GameObject mainMenu;
    public GameObject uNameText;

    //sesler eklendi
    AudioSource audioSource;
    public AudioClip au_Jump;
    public AudioClip au_Hurt;
    public AudioClip au_PickUpCoin;
    public AudioClip au_Dead;
    public AudioClip au_Shoot;

    //oyuncunun ates etmesi
    Transform firePoint;
    GameObject bullet;

    public int ammo;

    public float bulletSpeed;

    public GameObject checkPointHolder;

    void Start()
    {
        deathCanvas.SetActive(false);
        restartCanvas.SetActive(false);
        

        if(PlayerPrefs.GetString("userName") == "")
        {
            mainMenu.SetActive(true);
            Time.timeScale = 0;
        }
        else
        {
            mainMenu.SetActive(false);
            Time.timeScale = 1;
        }

        //oyuna baslangic noktasi belirlendi

        if (PlayerPrefs.GetString("checkpoint") == "yes") {
            ammo = PlayerPrefs.GetInt("curAmmo"); 
            foreach(Transform go in checkPointHolder.transform)
            {
                if(go.gameObject.GetComponent<CheckPointEffectManager>().index == PlayerPrefs.GetInt("curCheckpoint"))
                {
                    curCheckpoint = go.gameObject;
                }
            }

            transform.position = curCheckpoint.transform.position;
        }
        else { transform.position = startPosition.transform.position; }

        

        //rigidbody ayarları
        body2D = GetComponent<Rigidbody2D>();
        body2D.gravityScale = 5;
        body2D.freezeRotation = true;
        body2D.collisionDetectionMode = CollisionDetectionMode2D.Continuous;
        //groundchecki bulma islemi
        groundCheck = transform.Find("GroundCheck");

        //colliders'i cekme
        box2D = GetComponent<BoxCollider2D>();
        circle2D = GetComponent<CircleCollider2D>();

        //Animatoru aliriz
        playerAnimationControler = GetComponent<Animator>();

        //playerin canini max olarak baslatiriz
        //giveDamage = FindObjectOfType<GiveDamage>();
        currentPlayerHealth = maxPlayerHealth;
        gameManager = FindObjectOfType<GameManager>();

        //ses efektleri ve müzik eklendi
        audioSource = GetComponent<AudioSource>();

        //ates etmek
        firePoint = transform.Find("FirePoint");
        bullet = Resources.Load("Bullet") as GameObject;
        
    }

    void Update()
    {
        UpdateAnimations();
        if (isHurt)
        {
            ReduceHealth();
        }

        isDead = currentPlayerHealth <= 0;   


        if(transform.position.y <= -15)
        {
            isDead = true;
        }

        KillPlayer();

        //can maxCani gecmesin diye, maxCana esitledim
        if (currentPlayerHealth >maxPlayerHealth)
            currentPlayerHealth = maxPlayerHealth;

    }

    //Frameratedan bagimsiz olarak çalışır.Fizikle ilgili şeyler burda
    void FixedUpdate()
    {
        isGrounded = Physics2D.OverlapCircle(groundCheck.position, groundCheckRadius, groundLayer);

        //float v = Input.GetAxis("Vertical");


        float h = Input.GetAxis("Horizontal");
        body2D.velocity = new Vector2(h * playerSpeed , body2D.velocity.y);

       

        Flip(h);
        

        if (isGrounded)
            canDamage = false;

    }
    /*
    public void Move(bool right)
    {
        if (right)
        {
            body2D.velocity = new Vector2(playerSpeed, body2D.velocity.y);
            Flip(1);
        }
        else
        {
            body2D.velocity = new Vector3(-playerSpeed, body2D.velocity.y);
            Flip(-1);
        }
    }

    public void ZeroVelocity()
    {
        body2D.velocity = Vector2.zero;
    }
    */

    public void Jump()
    {
        if (isGrounded)
        {
            body2D.AddForce(new Vector2(0, jumpPower));
            audioSource.PlayOneShot(au_Jump);
            audioSource.pitch = Random.Range(0.8f, 1.1f);
            canDoubleJump = true;
        }
        else DoubleJump();
       
    }
    //karakteri dondurmefonk
    void Flip(float h)
    {
        if(h > 0 && !facingRight || h < 0 && facingRight)
        {
            facingRight = !facingRight;

            Vector2 theScale = transform.localScale;

            theScale.x *= -1;

            transform.localScale = theScale;
        }
    }
    //2kez ziplama
    public void DoubleJump()
    {
        if(!isGrounded && canDoubleJump)
        {
            //2.kez zipliyorken ekstra ani guc uygular..(Y yonunde)
            body2D.AddForce(new Vector2(0, doubleJumpPower), ForceMode2D.Impulse);
            canDamage = true;
            canDoubleJump = false;
        }

        
    }
    //animatoryenileme
    void UpdateAnimations()
    {
        //ters yonde gidiyorken animasyonu oynatması icin yazildi, -15 < 0.1 debug
        playerAnimationControler.SetFloat("VelocityX", Mathf.Abs(body2D.velocity.x));
        playerAnimationControler.SetBool("isGrounded", isGrounded);
        playerAnimationControler.SetFloat("VelocityY", body2D.velocity.y);
        playerAnimationControler.SetBool("isDead", isDead);
        //cani azaliyorkenki animasyon
        if (isHurt && !isDead)
            playerAnimationControler.SetTrigger("isHurt"); 

    }
    //can azaltmaFonk
    void ReduceHealth()
    {
        if (isHurt)
        {
            //hasar yerse, cani damage kadar azalir
            //currentPlayerHealth -= giveDamage.damage;
            isHurt = false;

            //eger havadaysa karakter, sola veya saga dogru guc uygular
            if (facingRight && !isGrounded)
                body2D.AddForce(new Vector2(-knockBackForce, knockBackForce / 1.5f), ForceMode2D.Force);
            else if(!facingRight && !isGrounded)
                body2D.AddForce(new Vector2(knockBackForce, knockBackForce / 1.5f), ForceMode2D.Force);

            if(facingRight && isGrounded)
                body2D.AddForce(new Vector2(-knockBackForce, 0), ForceMode2D.Force);
            else if(!facingRight && isGrounded)
                body2D.AddForce(new Vector2(knockBackForce, 0), ForceMode2D.Force);

        }

        if (!isDead)
        {
            audioSource.PlayOneShot(au_Hurt);
            audioSource.pitch = Random.Range(0.8f, 1.1f); 
        }
    }

    //oyuncuyu oldurme fonk
    void KillPlayer()
    {
        if (isDead)
        {
            isHurt = false;
            body2D.AddForce(new Vector2(0, deadForce), ForceMode2D.Impulse);
            body2D.drag += Time.deltaTime * 50;
            deadForce -= Time.deltaTime * 20;
            body2D.constraints = RigidbodyConstraints2D.FreezePositionX;
            box2D.enabled = false;
            circle2D.enabled = false;
            
            deathCanvas.SetActive(false);
            restartCanvas.SetActive(true);
            RecoverPlayer();
            Time.timeScale = 0;
        }

    }

    public void RecoverPlayer()
    {
        if (checkPoint != null)
        {
            PlayerPrefs.SetString("checkpoint", "yes");
            PlayerPrefs.SetInt("curCheckpoint", curCheckpoint.GetComponent<CheckPointEffectManager>().index);
            PlayerPrefs.Save();
        }
        else
        {
            transform.position = startPosition.transform.position;
        }

        body2D.gravityScale = 5;
        //body2D.freezeRotation = true;
        body2D.collisionDetectionMode = CollisionDetectionMode2D.Continuous;
        
        currentPlayerHealth = maxPlayerHealth;
        box2D.enabled = true;
        circle2D.enabled = true;
        body2D.constraints = RigidbodyConstraints2D.None;
        body2D.freezeRotation = true;
        body2D.simulated = true;
        body2D.drag = 0;
        currentPoints = 0;


    }

    public void playerShoot()
    {
        if (ammo > 0)
        {

        GameObject b = Instantiate(bullet) as GameObject;
        b.transform.position = firePoint.transform.position;
        b.transform.rotation = firePoint.transform.rotation;
        audioSource.PlayOneShot(au_Shoot);
        audioSource.pitch = Random.Range(0.8f, 1.1f);

            if (transform.localScale.x < 0)
            {
                b.GetComponent<PlayerShoot>().bulletSpeed *= -1;
                b.GetComponent<SpriteRenderer>().flipX = true;
            }

            else
            {
                b.GetComponent<PlayerShoot>().bulletSpeed *= 1;
                b.GetComponent<SpriteRenderer>().flipX = false;

            }
            ammo--;
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if(other.tag == "Coin")
        {
            Transform coinEffect;
            currentPoints += point;
            other.gameObject.GetComponent<SpriteRenderer>().enabled = false;
            other.gameObject.GetComponent<CircleCollider2D>().enabled = false;
            coinEffect = other.gameObject.transform.GetChild(0);
            coinEffect.gameObject.SetActive(true);
            Destroy(other.gameObject, 1f);
            audioSource.PlayOneShot(au_PickUpCoin);
            audioSource.pitch = Random.Range(0.8f, 1.1f);

        }
        
        if(other.tag == "CheckPoint")
        {
            checkPoint = other.gameObject;
        }

        if(other.tag == "Enemy" && isDead)
        {
             
            audioSource.PlayOneShot(au_Dead);
        }

    }


}
