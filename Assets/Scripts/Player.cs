using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Windows;

public class Player : MonoBehaviour
{
    //Config
    [SerializeField] private float _moveSpeed = 1f;
    [SerializeField] private float _jumpSpeed = 1f;
    [SerializeField] private float _climbSpeed = 1f;
    [SerializeField] private float _health = 3f;
    [SerializeField] private float _groundLength = 0.6f;


    //Properties
    public bool IsAlive => _health > 0f;

    public Action Dead;

    //Cached component references
    private Rigidbody2D _rb;
    private PlayerInput _input;
    private Vector2 _direction;
    private Animator _animator;
    private Collider2D _collider;
    private float _gravityScaleAtStart;

    [SerializeField] private LayerMask _groundLayer;
    [SerializeField] private LayerMask _ladderLayer;
    [SerializeField] private Vector2 _deathKick = new Vector2(25f,25f);
    private const string Climnbing = "Climbing";
    private const string Running = "Running";
    private const string Dying = "Dying";

    private void OnEnable()
    {
        _input.Player.Enable();
        Dead += OnDeath;
    }
    private void OnDisable()
    {
        _input.Player.Disable();
        Dead -= OnDeath;
    }

    private void Awake()
    {
        _input = new PlayerInput();
        _input.Player.Jump.performed += context => Jump();
    }

    private void Start()
    {
        _rb = GetComponent<Rigidbody2D>();
        _animator = GetComponent<Animator>();
        _collider = GetComponent<Collider2D>();
        _gravityScaleAtStart = _rb.gravityScale;
        
    }

    public void ApplyDamage(float damage)
    {
        if (damage<0)
        {
            throw new ArgumentOutOfRangeException(nameof(damage));
        }
        _health -= damage;
        Debug.Log("health "+_health);
        if (IsAlive == false)
        {
            Dead?.Invoke();
        }
    }


    private void Move(float direction)
    {

        Vector2 playerVelocity = new Vector2(direction * _moveSpeed, _rb.velocity.y);
        _rb.velocity = playerVelocity;

        bool playerHasHorizontalSpeed = Mathf.Abs(_rb.velocity.x) > Mathf.Epsilon;

        if (playerHasHorizontalSpeed)
        {
            FlipSprite(direction);
        }

        _animator.SetBool(Running, playerHasHorizontalSpeed);
    }

    private void FlipSprite(float direction)
    {
        transform.localScale = new Vector2(Mathf.Sign(direction), transform.localScale.y);
    }


    private void Jump()
    {
        bool onGround = Physics2D.Raycast(transform.position, Vector2.down, _groundLength, _groundLayer);
        if (onGround)
        {
            Vector2 jumpVelocityToAdd = new Vector2(0f, _jumpSpeed);
            _rb.velocity += jumpVelocityToAdd;
        }
    }

    private void ClimbLadder(float direction)
    {
        if (!_collider.IsTouchingLayers(_ladderLayer))
        {
            _animator.SetBool(Climnbing, false);
            _rb.gravityScale = _gravityScaleAtStart;
            return;
        }

        Vector2 playerVelocity = new Vector2(_rb.velocity.x, direction * _climbSpeed);
        _rb.velocity = playerVelocity;
        _rb.gravityScale = 0f;

        bool playerHasVerticalSpeed = Mathf.Abs(_rb.velocity.y) > Mathf.Epsilon;
        _animator.SetBool(Climnbing, true);
    }




    private void FixedUpdate()
    {

        _direction = _input.Player.Move.ReadValue<Vector2>();
        Move(_direction.x);
        ClimbLadder(_direction.y);
    }

    private void OnDeath()
    {
        _input.Disable();
        _animator.SetTrigger(Dying);
        _rb.velocity = _deathKick;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        Enemy enemy = collision.gameObject.GetComponent<Enemy>();


        if ( enemy!=null)
        {
            ApplyDamage(enemy.GetDamage);
        }
    }


    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawLine(transform.position, transform.position+Vector3.down*_groundLength);
    }
}
