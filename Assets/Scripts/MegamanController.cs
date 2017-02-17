using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MegamanController : MonoBehaviour {

    [Header("Movement")]
    public float pixelToUnit = 40f;
    public float maxVelocity = 10f; // pixels/second
    public float jumpForce = 200;
    private float landRadius = 0.2f;

    [Header("Velocity")]
    public Vector3 moveSpeed = Vector3.zero; //(0,0,0)

    [Header("Components")]
    public Rigidbody2D rigidBodyVar;
    public SpriteRenderer spriteRenderer;
    public Animator animator;
    public LayerMask verifyLand;
    public Transform footMegaman;

    [Header("Animation")]
    public bool isRunning = false;
    public bool isFacingLeft = false;
    public bool isJumping = false;
    public bool isFalling = false;
    public bool isLand = false;

    // Use this for initialization
    void Start () {
        rigidBodyVar = GetComponent<Rigidbody2D>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        animator = GetComponent<Animator>();
    }
	
	// Update is called once per frame
	void Update () {
        HandleHorizontalMovement();
        HandleVerticalMovement();
        MoveCharacterController();
        UpdateAnimetionParameters();
    }

    void UpdateAnimetionParameters()
    {
        animator.SetBool("isRunning", isRunning);
        animator.SetBool("jump", isJumping);
        animator.SetBool("land", isLand);
    }

    void HandleHorizontalMovement()
    {
        moveSpeed.x = Input.GetAxis("Horizontal") * (maxVelocity * pixelToUnit);

        if(moveSpeed.x != 0)
        {
            isRunning = true;
        } else
        {
            isRunning = false;

        }
        if (Input.GetAxis("Horizontal") < 0 && !isFacingLeft)
        {
            //muda para esquerda
            isFacingLeft = true;
        } else if(Input.GetAxis("Horizontal") > 0 && isFacingLeft)
        {
            //muda para direita
            isFacingLeft = false;
        }

        spriteRenderer.flipX = isFacingLeft;
    }

    void HandleVerticalMovement()
    {
        //var absVelY = Mathf.Abs(rigidBodyVar.velocity.y);
        isLand = Physics2D.OverlapCircle(footMegaman.position, landRadius, verifyLand);
        //if(absVelY <= 0.05f)
        //    isLand = true;
        //else
        //   isLand = false;
        if (Input.GetButtonDown("Jump") && isLand)
        {
            isJumping = true;
            rigidBodyVar.AddForce(new Vector2 (rigidBodyVar.velocity.x , jumpForce));
        } else {
            isJumping = false;
        }
    }

    void MoveCharacterController()
    {
        rigidBodyVar.velocity = moveSpeed;
    }
}
