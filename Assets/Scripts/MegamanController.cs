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

        // aula 3
        if (RaycastAgainstLayer("Ground", footMegaman))
        {
            isLand = false;
        }
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
            rigidBodyVar.AddForce(new Vector2 (0 , jumpForce));
        } else {
            isJumping = false;
        }
    }

    void MoveCharacterController()
    {
        rigidBodyVar.velocity = moveSpeed;
    }

    RaycastHit2D RaycastAgainstLayer(string layerName, Transform endPoint)
    {
        //layer - 00000000000000000000000000001001
        int layer = LayerMask.NameToLayer(layerName); // camada 1, 2, 3...
        int layerMask = 1 << layer; // camada 2 -> 100, camada 4 -> 10000
        // camadas 2,4 | (1 << 2) + (1 << 4) | 100 + 10000 = 10100

        Vector2 originPosition = new Vector2(footMegaman.position.x, footMegaman.position.y);
        float rayLength = Mathf.Abs(endPoint.localPosition.y);
        Vector2 direction = endPoint.localPosition.normalized;

        RaycastHit2D hit2d = Physics2D.Raycast(originPosition, direction, rayLength, layerMask);

        #if UNITY_EDITOR
            Color color;
            if(hit2d != null && hit2d.collider != null)
            {
                color = Color.green; // acerta o chão
            } else
            {
                color = Color.red; // ñ acerta o chão
            }

            Debug.DrawLine(originPosition, // inicio
                originPosition + direction * rayLength, //fim
                color, 0f, false);
        #endif
        return hit2d;
    }
}
