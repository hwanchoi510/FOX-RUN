using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerControl : MonoBehaviour
{
    [SerializeField] private float MoveSpeed;
    [SerializeField] private float JumpPower;
    [SerializeField] private int ExtraJumpsNum;
    [SerializeField] private float ExtraJumpPower;
    [SerializeField] private LayerMask PlatformLayer;
    [SerializeField] private GameObject GameOverPanel;

    private Rigidbody2D rb;
    private BoxCollider2D coll;
    private Animator anim;
    private int collectPoint;
    private GameObject Collectibles;
    public static float scale;

    private Vector2 runSize;
    private Vector2 runOffset;
    private Vector2 crouchSize;
    private Vector2 crouchOffset;
    private enum State {run, crouch, jump, fall, death};
    private State state;
    private bool dead = false;
    private int ExtraJumps;

    // Start is called before the first frame update
    void Start()
    {
        Collectibles = GameObject.Find("Collectibles");
        rb = GetComponent<Rigidbody2D>();
        coll = GetComponent<BoxCollider2D>();
        anim = GetComponent<Animator>();
        scale = 1.0f;

        runSize = new Vector2(coll.size.x, coll.size.y);
        runOffset = new Vector2(coll.offset.x, coll.offset.y);
        crouchSize = new Vector2(coll.size.x, coll.size.y * 0.5f);
        crouchOffset = new Vector2(coll.offset.x, coll.offset.y - coll.size.y / 4.0f);

        collectPoint = 10000;
    }

    void Update()
    {
        if (!dead)
        {
            Animate();
            Movement();
        }
    }

    private bool IsGrounded()
    {
        RaycastHit2D rc2d = Physics2D.BoxCast(coll.bounds.center, coll.bounds.size, 0f, Vector2.down, 0.1f, PlatformLayer);
        return rc2d.collider != null && rb.velocity.y > -0.0001f && rb.velocity.y < 0.0001f;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.name == "End")
        {
            rb.transform.position = new Vector2(GameObject.Find("Start").transform.position.x, rb.transform.position.y);
            Time.timeScale += 0.5f;
            scale += 0.5f;
            collectPoint += 5000;
            for(int i = 0; i < Collectibles.transform.childCount; i++)
            {
                Collectibles.transform.GetChild(i).gameObject.SetActive(true);
            }
        } 
        
        if(collision.tag == "Obstacle")
        {
            MoveSpeed = 0;
            Time.timeScale = 1.0f;
            rb.velocity = new Vector2(0, 0);
            GameOverPanel.SetActive(true);
            GameObject.Find("Death").GetComponent<AudioSource>().Play();
            dead = true;
            anim.SetTrigger("Death");
        }

        if (collision.gameObject.tag == "Collectible")
        {
            Animator cherryAnimator = collision.gameObject.GetComponent<Animator>();
            GameObject.Find("Item").GetComponent<AudioSource>().Play();
            cherryAnimator.SetTrigger("Collected");
            GameUI.Score += collectPoint;
        }
    }

    private void GameOver()
    {
        PlayerPrefs.SetInt("Score", GameUI.Score);
        SceneManager.LoadScene("GameOver");
    }

    private void Animate()
    {
        if (rb.velocity.y > 0.0001f)
        {
            state = State.jump;
        }
        else if (rb.velocity.y < -0.0001f)
        {
            state = State.fall;
        }
        else if (IsGrounded() && state != State.crouch)
        {
            state = State.run;
        }
        anim.SetInteger("State", (int)state);
    }

    private void Movement()
    {
        rb.velocity = new Vector2(MoveSpeed, rb.velocity.y);

        if (IsGrounded())
        {
            ExtraJumps = ExtraJumpsNum;
        }

        if (Input.GetButtonDown("Jump") && !GameUI.isPaused)
        {
            if (IsGrounded())
            {
                GameObject.Find("Jump").GetComponent<AudioSource>().Play();
                rb.velocity = new Vector2(MoveSpeed, JumpPower);
            }
            else if (ExtraJumps > 0)
            {
                GameObject.Find("Jump").GetComponent<AudioSource>().Play();
                rb.velocity = new Vector2(MoveSpeed, ExtraJumpPower);
                ExtraJumps--;
            }
        }

        if (Input.GetKey(KeyCode.DownArrow) && IsGrounded() && !GameUI.isPaused)
        {
            coll.size = crouchSize;
            coll.offset = crouchOffset;
            state = State.crouch;
        } else
        {
            coll.size = runSize;
            coll.offset = runOffset;
            state = State.run;
        }

    }
}
