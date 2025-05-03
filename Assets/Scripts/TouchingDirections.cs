using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TouchingDirections : MonoBehaviour
{
    public ContactFilter2D castFilter;
    public float groundDistance = 0.05f;
    public float wallDistance = 0.2f;
    public float ceilingDistance = 0.05f;

    private CapsuleCollider2D touchingCol;
    private Animator animator;

    private PlayerController playerController;
    private Knight knight;

    private RaycastHit2D[] groundHits = new RaycastHit2D[5];
    private RaycastHit2D[] wallHits = new RaycastHit2D[5];
    private RaycastHit2D[] ceilingHits = new RaycastHit2D[5];

    [SerializeField] private bool _isGrounded;
    [SerializeField] private bool _isOnWall;
    [SerializeField] private bool _isOnCeiling;

    public bool IsGrounded
    {
        get => _isGrounded;
        private set
        {
            _isGrounded = value;
            if (animator != null) animator.SetBool(AnimationStrings.isGrounded, value);
        }
    }

    public bool IsOnWall
    {
        get => _isOnWall;
        private set
        {
            _isOnWall = value;
            if (animator != null) animator.SetBool(AnimationStrings.isOnWall, value);
        }
    }

    public bool IsOnCeiling
    {
        get => _isOnCeiling;
        private set
        {
            _isOnCeiling = value;
            if (animator != null) animator.SetBool(AnimationStrings.isOnCeiling, value);
        }
    }

    private Vector2 wallCheckDirection
    {
        get
        {
            if (playerController != null)
            {
                return playerController.IsFacingRight ? Vector2.right : Vector2.left;
            }
            else if (knight != null)
            {
                return knight.WalkDirection == Knight.WalkableDirection.Right ? Vector2.right : Vector2.left;
            }
            else
            {
                Debug.LogWarning("TouchingDirections: No direction source found.");
                return Vector2.right; // Default
            }
        }
    }

    private void Awake()
    {
        touchingCol = GetComponent<CapsuleCollider2D>();
        animator = GetComponent<Animator>();
        playerController = GetComponent<PlayerController>();
        knight = GetComponent<Knight>();
    }

    private void FixedUpdate()
    {
        IsGrounded = touchingCol.Cast(Vector2.down, castFilter, groundHits, groundDistance) > 0;
        IsOnWall = touchingCol.Cast(wallCheckDirection, castFilter, wallHits, wallDistance) > 0;
        IsOnCeiling = touchingCol.Cast(Vector2.up, castFilter, ceilingHits, ceilingDistance) > 0;
    }
}
