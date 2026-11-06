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
    [SerializeField] private float dashSpeed = 15f;
    [SerializeField] private float dashCooldown = 1.5f; // Tiempo de espera entre dashes
    [SerializeField] private GameObject effectDeadprefab;
    [SerializeField] private GameObject effectSpawnprefab;

    private GameObject instantiateEffectSpawn;
    private GameObject instantiateEffectDead;

    public float rayLength = 0.6f;
    public LayerMask groundLayer;

    private Animator AnimationPj;
    private SpriteRenderer sp;
    private float timeDead;

    private bool animationDeadStarted = false;
    private bool IsDead = false;
    private bool animationSpawnStarted = false;

    private float spawnTimer = 0f;
    private float spawnDuration = 0.4f;

    private bool dashEjecution = false;
    private float dashTimer = 0f;
    private float lastDashTime = -100f; // Controla cuándo se hizo el último dash

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
        rb.gravityScale = 1;

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
        colorState.a = colorState.a == 1f ? 0.0f : 1.0f;
        sp.color = colorState;
    }

    void Update()
    {
        if (!animationSpawnStarted)
        {
            spawnTimer += Time.deltaTime;
            if (spawnTimer >= spawnDuration)
            {
                animationSpawnStarted = true;
                ChangeColor();
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

        if (dashEjecution)
        {
            dashTimer += Time.deltaTime;
            if (dashTimer >= 0.3f)
            {
                AnimationPj.SetBool("Dash", false);
                dashEjecution = false;
                dashTimer = 0f;

                rb.linearVelocity = new Vector2(0, rb.linearVelocity.y);
            }
        }
        else
        {
            float currentSpeed = IsGrounded() ? moveSpeed : moveSpeed - 7f;
            rb.linearVelocity = new Vector2(moveVector.x * currentSpeed, rb.linearVelocity.y);

            AnimationPj.SetBool("Move", moveVector.x != 0);

            // La animación de salto se evalúa SOLO si no estamos haciendo un dash
            AnimationPj.SetBool("Jump", !IsGrounded());
        }
    }

    public void OnJump(InputAction.CallbackContext context)
    {
        if (IsDead || dashEjecution) return;
        if (IsGrounded() && context.performed)
        {
            rb.gravityScale = rb.gravityScale == -1 ? 1 : -1;
            sp.flipY = rb.gravityScale == -1;
        }
    }

    public void OnMovement(InputAction.CallbackContext context)
    {
        if (IsDead) return;

        // Quitamos el return de dashEjecution para que el juego siempre sepa si soltaste la tecla
        moveVector = context.ReadValue<Vector2>();

        // Solo giramos el sprite si no estamos en medio de un dash
        if (!dashEjecution)
        {
            if (moveVector.x > 0) sp.flipX = false;
            if (moveVector.x < 0) sp.flipX = true;
        }
    }

    public void OnDash(InputAction.CallbackContext context)
    {
        if (IsDead || dashEjecution) return;

        // Comprobamos si ha pasado el tiempo de cooldown desde el último dash
        if (context.performed && Time.time >= lastDashTime + dashCooldown)
        {
            dashEjecution = true;
            dashTimer = 0f;
            lastDashTime = Time.time; // Guardamos el momento exacto en el que se hizo el dash

            AnimationPj.SetBool("Dash", true);
            AnimationPj.SetBool("Move", false);
            AnimationPj.SetBool("Jump", false); // Apagamos el salto por si estábamos en el aire

            float direction = moveVector.x != 0 ? Mathf.Sign(moveVector.x) : (sp.flipX ? -1f : 1f);

            // Corregido el error de sintaxis en tu multiplicación
            rb.linearVelocity = new Vector2(direction * dashSpeed, rb.linearVelocity.y);
        }
    }

    bool IsGrounded()
    {
        Vector2 direction = sp.flipY ? Vector2.up : Vector2.down;
        Debug.DrawRay(transform.position, direction * rayLength, Color.red);
        RaycastHit2D hit = Physics2D.Raycast(transform.position, direction, rayLength, groundLayer);
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

    private void HandlePlayerDeath(bool isDead)
    {
        IsDead = isDead;

        if (effectDeadprefab != null)
        {
            instantiateEffectDead = Instantiate(effectDeadprefab, transform.position, Quaternion.identity);
        }

        rb.linearVelocity = Vector2.zero;
        rb.gravityScale = 0;
        AnimationPj.SetBool("Move", false);
        AnimationPj.SetBool("Jump", false);
        AnimationPj.SetBool("Dash", false);
    }
}