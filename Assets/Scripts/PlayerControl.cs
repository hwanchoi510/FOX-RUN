using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerControl : MonoBehaviour
{
    [SerializeField] private float MoveSpeed;
    [SerializeField] private float JumpPower;
    [SerializeField] private int ExtraJumpsNum;
    [SerializeField] private float ExtraJumpPower;
    [SerializeField] private LayerMask PlatformLayer;

    private Rigidbody2D rb;
    private BoxCollider2D coll;
    private Animator anim;

    private Vector2 runSize;
    private Vector2 runOffset;
    private Vector2 crouchSize;
    private Vector2 crouchOffset;
    private enum State {run, crouch, jump, fall };
    private State state;
    private int ExtraJumps;

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        coll = GetComponent<BoxCollider2D>();
        anim = GetComponent<Animator>();

        runSize = new Vector2(coll.size.x, coll.size.y);
        runOffset = new Vector2(coll.offset.x, coll.offset.y);
        crouchSize = new Vector2(coll.size.x, coll.size.y * 0.5f);
        crouchOffset = new Vector2(coll.offset.x, coll.offset.y - coll.size.y / 4);
    }

    void Update()
    {
        Animate();
        Movement();   
    }

    private bool IsGrounded()
    {
        RaycastHit2D rc2d = Physics2D.BoxCast(coll.bounds.center, coll.bounds.size, 0f, Vector2.down, 0.1f, PlatformLayer);
        return rc2d.collider != null;
    }

    private void Animate()
    {
        if (rb.velocity.y > 0)
        {
            state = State.jump;
        }
        else if (rb.velocity.y < 0)
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

        if (Input.GetButtonDown("Jump"))
        {
            if (IsGrounded())
            {
                rb.velocity = new Vector2(MoveSpeed, JumpPower);
            }
            else if (ExtraJumps > 0)
            {
                rb.velocity = new Vector2(MoveSpeed, ExtraJumpPower);
                ExtraJumps--;
            }
        }

        if (Input.GetKey(KeyCode.DownArrow) && IsGrounded())
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

        Debug.Log(state);
    }
}
