using Unity.Mathematics;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;
using static UnityEngine.Rendering.DebugUI;

public class PlayerMovement : MonoBehaviour, InputSystem_Actions.IPlayerMoveActions
{
    private InputSystem_Actions inputSystem;
	private Rigidbody2D rb;
	private Vector2 moveVector = Vector2.zero;
    [SerializeField] private float moveSpeed;
	[SerializeField] private GameObject effectDeadprefab;
    [SerializeField] private GameObject effectSpawnprefab;
    private GameObject instantiateEffectSpawn;


    public float rayLength = 0.6f;
	public LayerMask groundLayer;
	private Animator AnimationPj;
	private SpriteRenderer sp;
	private float forceDash;
	private float timeDead;
	private GameObject instantiateEffectDead;

    private bool animationDeadStarted = false;
    private bool IsDead = false;
	private bool animationSpawnStarted = false;

	private float spawnTimer = 0f;
	private float spawnDuration = 0.4f;

	private bool dashEjecution = false;
	private float dashTimer = 0f;
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
        rb.gravityScale = 30;
		forceDash = 1.1f;
		ChangeColor();
		if (effectSpawnprefab != null)
		{
			instantiateEffectSpawn = Instantiate(effectSpawnprefab, transform.position + Vector3.down, Quaternion.identity);
			Destroy(instantiateEffectSpawn, spawnDuration);
		}
	}

	private void ChangeColor()
	{
		
		Color colorState = sp.color;
		if (colorState.a == 1)
		{
			colorState.a = 0.0f;
			sp.color = colorState;
		}
		else
		{
			colorState.a = 1.0f;
			sp.color = colorState;
		}
	}
// Update is called once per frame
	void Update()
    {
		if (!animationSpawnStarted)
		{
			spawnTimer += Time.deltaTime;

			if (spawnTimer >= spawnDuration)
			{
				animationSpawnStarted = true;
				ChangeColor() ;
			}
			return; 
		}

		if (IsDead)
        {
            rb.linearVelocity = Vector2.zero;
            timeDead += Time.deltaTime;

            if (timeDead >= 0.3f && !animationDeadStarted)
            {
                animationDeadStarted = true;
                AnimationPj.SetBool("IsDead", true);
            }

            if (timeDead >= 0.8f)
            {
                gameObject.SetActive(false);
				Destroy(instantiateEffectDead, 0.4f);
			}
			

            return;
        }
        rb.linearVelocity =   (IsGrounded()? moveSpeed : moveSpeed-7) * moveVector ;


		if (rb.linearVelocity == new Vector2(0,0))
		{
			AnimationPj.SetBool("Move", false);

		}

		

		if (dashEjecution)
		{
			dashTimer += Time.deltaTime;
			if (dashTimer >= 0.4f)
			{
				AnimationPj.SetBool("Dash", false);
				dashEjecution = false;
				dashTimer = 0f;
			}
		}
		else
		{
			rb.linearVelocity = (IsGrounded() ? moveSpeed : moveSpeed - 7) * moveVector;
		}

		AnimationPj.SetBool("Jump", !IsGrounded());

	}


    public void OnJump(InputAction.CallbackContext context)
	{
        if (IsDead || dashEjecution) return;
        if (IsGrounded())
		{
			
			if (rb.gravityScale == -30)
			{
				rb.gravityScale = 30;
				sp.flipY = false;
				
			}
			else
			{
				rb.gravityScale = -30;
				sp.flipY = true;
				
			}
			
		}
		
     
	}

    public void OnMovement(InputAction.CallbackContext context)
    {
        if (IsDead || dashEjecution) return;
		
			moveVector = context.ReadValue<Vector2>();
			AnimationPj.SetBool("Move", true);

			if (moveVector == new Vector2(1, 0))
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
		Debug.DrawRay(transform.position, (sp.flipY == false ? Vector2.down : Vector2.up) * rayLength, Color.red);
		RaycastHit2D hit = Physics2D.Raycast(transform.position, (sp.flipY == false ? Vector2.down : Vector2.up), rayLength, groundLayer);
		return hit.collider != null;
	}
	private void OnEnable()
	{
		inputSystem.Enable();
        GameEvents.onPlayerIsDead += HandlePlayerDeath;
    }
	private void OnDisable()
	{
		inputSystem.Disable();
        GameEvents.onPlayerIsDead -= HandlePlayerDeath;
    }

    public void OnDash(InputAction.CallbackContext context)
    {
        if (IsDead || dashEjecution) return;
        if (context.performed)
		{
			AnimationPj.SetBool("Dash", true);
			dashEjecution = true;
			float direction = moveVector.x;
			float forceD = Mathf.Lerp(0, forceDash, 0.7f);
			rb.AddForceX(forceD * direction);
			//rb.AddForce(new Vector2(forceD * direction, 1), ForceMode2D.Impulse);
		}
		
	}
    private void HandlePlayerDeath(bool isDead)
    {
        IsDead = isDead;

        if (effectDeadprefab != null)
        {
            instantiateEffectDead = Instantiate(effectDeadprefab, transform.position, quaternion.identity);
        }

        rb.linearVelocity = Vector2.zero;
        rb.gravityScale = 0;
        AnimationPj.SetBool("Move", false);
        AnimationPj.SetBool("Jump", false);
    }

}


