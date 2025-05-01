using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Rendering.Universal; 

[RequireComponent(typeof(Rigidbody2D), typeof(TouchingDirections), typeof(Damageable))]
public class PlayerController : MonoBehaviour
{
    public float walkSpeed = 4f;
    public float runSpeed = 6f;
    public float airWalkSpeed = 3f;
    public float jumpImpulse = 6f;

    public Light2D torchLight;
    public AudioSource torchAudioSource;
    public AudioClip toggleTorchSound;
    private bool torchOn = false;
    Vector2 moveInput;
    TouchingDirections touchingDirections;
    Damageable damageable;

    public float CurrentMoveSpeed {
        get {
            if (CanMove) {
                if (IsMoving && !touchingDirections.IsOnWall) {
                    if (touchingDirections.IsGrounded) {
                        return IsRunning ? runSpeed : walkSpeed;
                    } else {
                        return airWalkSpeed;
                    }
                } else {
                    return 0;
                }
            } else {
                return 0;
            }
        }
    }

    [SerializeField] private bool _isMoving = false;
    public bool IsMoving {
        get => _isMoving;
        private set {
            _isMoving = value;
            animator.SetBool(AnimationStrings.isMoving, value);
        }
    }

    [SerializeField] private bool _isRunning = false;
    public bool IsRunning {
        get => _isRunning;
        set {
            _isRunning = value;
            animator.SetBool(AnimationStrings.isRunning, value);
        }
    }

    public bool _isFacingRight = true;
    public bool IsFacingRight {
        get => _isFacingRight;
        private set {
            _isFacingRight = value;
        }
    }

    public bool CanMove => animator.GetBool(AnimationStrings.canMove);
    public bool IsAlive => animator.GetBool(AnimationStrings.isAlive);

    Rigidbody2D rb;
    Animator animator;

    // === FLIP SOLUTIONS ===

    // Option A: If using SpriteRenderer
    // private SpriteRenderer spriteRenderer;

    // Option B: If using SkinnedMeshRenderer and bone rig
    public Transform rootBone; // Assign this manually in Inspector

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        touchingDirections = GetComponent<TouchingDirections>();
        damageable = GetComponent<Damageable>();

        // spriteRenderer = GetComponent<SpriteRenderer>(); // Uncomment if using SpriteRenderer
    }

    private void FixedUpdate()
    {
        if (!damageable.LockVelocity)
            rb.velocity = new Vector2(moveInput.x * CurrentMoveSpeed, rb.velocity.y);

        animator.SetFloat(AnimationStrings.yVelocity, rb.velocity.y);
    }

    public void onMove(InputAction.CallbackContext context)
    {
        moveInput = context.ReadValue<Vector2>();

        if (IsAlive)
        {
            IsMoving = moveInput != Vector2.zero;
            SetFacingDirection(moveInput);
        }
        else
        {
            IsMoving = false;
        }
    }

private void SetFacingDirection(Vector2 moveInput)
{
    if (moveInput.x != 0)
    {
        if (rootBone != null)
        {
            Vector3 scale = rootBone.localScale;
            scale.y = Mathf.Abs(scale.y) * (moveInput.x < 0 ? -1 : 1);
            rootBone.localScale = scale;
        }

        FlipTorchLight();
        Debug.Log("Flipping torch light");

        _isFacingRight = moveInput.x > 0;
    }
}

private void FlipTorchLight(){
    if (torchLight != null) return;
    if(IsFacingRight){
        torchLight.transform.localEulerAngles = new Vector3(0, 0, 0);
    } else {
        torchLight.transform.localEulerAngles = new Vector3(0, 180, 0);
    }
}


    public void onRun(InputAction.CallbackContext context)
    {
        if (context.started)
        {
            IsRunning = true;
        }
        else if (context.canceled)
        {
            IsRunning = false;
        }
    }

    public void onJump(InputAction.CallbackContext context)
    {
        if (context.started && touchingDirections.IsGrounded && CanMove)
        {
            animator.SetTrigger(AnimationStrings.jumpTrigger);
            rb.velocity = new Vector2(rb.velocity.x, jumpImpulse);
        }
    }

    public void onAttack(InputAction.CallbackContext context)
    {
        if (context.started)
        {
            animator.SetTrigger(AnimationStrings.attackTrigger);
        }
    }

    public void OnRangedAttack(InputAction.CallbackContext context)
    {
        if (context.started)
        {
            animator.SetTrigger(AnimationStrings.rangedAttackTrigger);
            Debug.Log("Ranged attack triggered");
        }
    }

    public void onTorchOn(InputAction.CallbackContext context)
    {
        if (context.started && torchLight != null)
        {
            torchLight.enabled = !torchLight.enabled;
        }

        if (torchAudioSource != null && toggleTorchSound != null)
        {
            torchAudioSource.PlayOneShot(toggleTorchSound);
        }
    }


    void OnCollisionEnter2D(Collision2D coll)
    {
        if (coll.gameObject.tag == "elevator")
        {
            transform.parent = coll.gameObject.transform;
        }
    }

    void OnCollisionExit2D(Collision2D coll)
    {
        if (coll.gameObject.tag == "elevator")
        {
            transform.parent = null;
        }
    }
}
