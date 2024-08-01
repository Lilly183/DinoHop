using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Rigidbody2D))]
[RequireComponent(typeof(Animator))]
[RequireComponent(typeof(SpriteRenderer))]
[RequireComponent(typeof(Health))]
public class Player : MonoBehaviour
{
    //=======================
    // Component(s) Required:
    //=======================

    Rigidbody2D rb = null;
    Animator anim = null;
    SpriteRenderer sr = null;
    Health pHealth = null;

    //=======================================
    // Makes Reference(s) to Other Object(s):
    //=======================================

    Text coinText;
    Text healthText;

    //====================
    // Member Variable(s):
    //====================

    Vector3 respawnPoint;
    public float speed = 4.5f;
    public float jumpPower = 387.5f;

    float horizontalInput;
    bool jumpInput = false;

    //=================================
    // Persistent Game Element (Coins):
    //=================================

    private int coins;

    public int Coins
    {
        get { return coins; }
        
        // Anytime we set the value of coins, the HUD needs to be updated, too!
        set 
        { 
            coins = value;    
            UpdateHUD();
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
        sr = GetComponent<SpriteRenderer>();
        pHealth = GetComponent<Health>();

        healthText = GameObject.Find("HealthText").GetComponent<Text>();
        coinText = GameObject.Find("CoinText").GetComponent<Text>();

        /* 
        When the player spawns, use the Coins property to set the corresponding
        private member variable ('coins') with whatever value is fetched from a
        PlayerPrefs key entitled "Coins". If this key doesn't exist, a default
        value of 0 is returned. This will also take care of updating the HUD
        (which includes health).
        */

        Coins = PlayerPrefs.GetInt("Coins", 0);
        
        respawnPoint = transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetAxis("Cancel") == 1)
        {
            ChangeLevel.GoToMainMenu();
        }

        horizontalInput = Input.GetAxis("Horizontal");
        jumpInput = Input.GetAxis("Jump") > 0;
    }

    private void FixedUpdate()
    {
        // If the player (somehow) falls, reset their position to respawnPoint.

        if (transform.position.y < -30)
        {
            rb.velocity = Vector2.zero;
            transform.position = respawnPoint;
        }

        //================
        // Movement Logic:
        //================

        rb.velocity = new Vector2(horizontalInput * speed, rb.velocity.y);

        /*
        ===========
        Jump Logic:
        ===========
        
        If there's jump input, get the position of the player's feet (Child 0), and 
        return all of the colliders within a certain radius of this position. Iterate 
        through the colliders array (ignoring the parent object) and add upwards force 
        to the rigid body if another collider was found.
        */

        if (jumpInput)
        {
            Vector3 feetPosition = transform.GetChild(0).position;

            Collider2D[] colliders = Physics2D.OverlapCircleAll(feetPosition, 0.125f);

            for (int i = 0; i < colliders.Length; ++i)
            {
                if (colliders[i].gameObject == gameObject)
                {
                    continue;
                }

                rb.velocity = new Vector2(rb.velocity.x, 0);
                rb.AddForce(Vector2.up * jumpPower);
                break;
            }
        }

        /*
        ===========================
        Animation Logic (Idle/Run):
        ===========================

        Set the boolean for the Animator component so that the appropriate animation 
        plays based on whether or not the player is moving above a certain threshold.
        */

        if (rb.velocity.magnitude > 0.05f)
        {
            anim.SetBool("isMoving", true);
        }

        else
        {
            anim.SetBool("isMoving", false);
        }

        /*
        ==================
        Flip Sprite Logic:
        ==================

        If the rigid body's velocity on the x-axis is negative, the player is moving 
        to the left; set the value of flipX to true for the SpriteRenderer component 
        so that the sprite flips (and vice versa if the rigid body's velocity is 
        positive).
        */

        if (rb.velocity.x < 0)
        {
            sr.flipX = true;
        }

        else if (rb.velocity.x > 0)
        {
            sr.flipX = false;
        }
    }

    /*
    =========================
    When the Player Gets Hit:
    =========================

    Whenever there's a collision, check the tag of whatever the player has collided 
    with. If it's tagged with "Projectiles", adjust the player's health by the 
    projectile's damage and update the HUD. If this ends up killing the player, access 
    the transitionScreen GameObject through transitionManagerInstance and call the Transition() 
    method, passing the condition 'Lose'.
    */

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Projectiles"))
        {
            bool hasKilled = pHealth.ChangeHealth(-(collision.gameObject.GetComponent<Projectile>().damage));
            
            UpdateHUD();

            if (hasKilled)
            {
                TransitionScreenController.transitionManagerInstance.transitionScreen.Transition(TransitionScreen.GameState.Lose);
            }
        }
    }

    /*
    Regardless of whether the player collects a coin or equips jump boots, the first 
    set of actions is the same. Thus, we can declare the following method to take 
    care of playing a sound effect, hiding the pickup from view, and then destroying
    the object shortly thereafter.
    */

    private void PickupObject(Collider2D collision)
    {
        AudioSource pickupSFX = collision.gameObject.GetComponent<AudioSource>();
        SpriteRenderer sr = collision.gameObject.GetComponent<SpriteRenderer>();
        LifeTime lt = collision.gameObject.GetComponent<LifeTime>();
        
        if (pickupSFX && sr && lt)
        {
            pickupSFX.Play();
            sr.color = Color.clear;
            lt.StartTimer();
        }
    }

    /*
    ==================================================
    When the Player Collects a Coin or the Jump Boots:
    ==================================================
    */

    private void OnTriggerEnter2D(Collider2D collision)
    {
        switch (collision.gameObject.tag)
        {
            case "Coins": 
            {
                PickupObject(collision);
                ++Coins;
                break;
            }

            case "JumpBoots":
            {
                PickupObject(collision);

                ParticleSystem ps = transform.GetChild(1).GetComponent<ParticleSystem>();

                if (ps)
                {
                    ps.Play();
                }

                jumpPower = 610f;
                break;
            }

            default:
                break;
        }
    }

    //============
    // Update HUD:
    //============

    public void UpdateHUD()
    {
        coinText.text = "Coins: " + coins;

        float healthPercentage = pHealth.CalcHealthPercentage();
        string healthPercentageColor;

        if (healthPercentage >= 75)
        {
            healthPercentageColor = "<color=#06FF01>";
        }

        else if (healthPercentage >= 25)
        {
            healthPercentageColor = "<color=#FFFF00>";
        }

        else
        {
            healthPercentageColor = "<color=#FF0000>";
        }

        healthText.text = "Health: " + healthPercentageColor + Mathf.Max(healthPercentage, 0.0f).ToString("F0") + "%</color>";
    }
}
