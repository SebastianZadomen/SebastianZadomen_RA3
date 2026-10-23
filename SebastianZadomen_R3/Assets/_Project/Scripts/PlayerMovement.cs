using UnityEngine;
using UnityEngine.InputSystem;
using static UnityEngine.Rendering.DebugUI;

public class PlayerMovement : MonoBehaviour, InputSystem_Actions.IPlayerMoveActions
{
    private InputSystem_Actions inputSystem;
	private Rigidbody2D rb;
	private Vector2 moveVector = Vector2.zero;
    [SerializeField] private float moveSpeed;
	public float rayLength = 0.6f;
	public LayerMask groundLayer;
	private Animator AnimationPj;
	private SpriteRenderer sp;

	// Start is called once before the first execution of Update after the MonoBehaviour is created

	private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
		inputSystem = new InputSystem_Actions();
		inputSystem.PlayerMove.SetCallbacks(this);
		AnimationPj = GetComponent<Animator>();
		sp = GetComponent<SpriteRenderer>();
	}
	void Start()
    {
        rb.gravityScale = 10;

    }

    // Update is called once per frame
    void Update()
    {
		
		rb.linearVelocity = moveSpeed * moveVector;
		//sp.flipX = (rb.linearVelocity.x > transform.position.x ? false : true);

		if (rb.linearVelocity == new Vector2(0,0))
		{
			AnimationPj.SetBool("Move", false);

		}

		if (!IsGrounded())
		{
			AnimationPj.SetBool("Jump", true);
		}
		else
		{
			AnimationPj.SetBool("Jump", false);
		}
	}
	public void OnJump(InputAction.CallbackContext context)
	{
		
		if (IsGrounded())
		{
			
			if (rb.gravityScale == -20)
			{
				rb.gravityScale = 20;
				sp.flipY = false;
				
			}
			else
			{
				rb.gravityScale = -20;
				sp.flipY = true;
				
			}
			
		}
		
     
	}

    public void OnMovement(InputAction.CallbackContext context)
    {
		
		moveVector = context.ReadValue<Vector2>();
		AnimationPj.SetBool("Move", true);

		if (moveVector == new Vector2(1,0))
		{
			sp.flipX = false;
		}
		if (moveVector == new Vector2(-1, 0))
		{
			sp.flipX = true;
		}
	}

	bool IsGrounded()
	{
		Debug.DrawRay(transform.position, Vector2.down * rayLength, Color.red);
		RaycastHit2D hit = Physics2D.Raycast(transform.position, (sp.flipY == false? Vector2.down : Vector2.up), rayLength, groundLayer);
		return hit.collider != null;
	}
	private void OnEnable()
	{
		inputSystem.Enable();
	}
	private void OnDisable()
	{
		inputSystem.Disable();
	}
}
