using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour, IDamaging
{

    [SerializeField] private float moveSpeed;

    private Rigidbody2D _rb;

    private Collider2D _platformEndCollider;

    bool IsFacingRight => transform.localScale.x > Mathf.Epsilon;

    [SerializeField] private float _damage = 1f;

    public float Damage => _damage;

    // Start is called before the first frame update
    void Start()
    {
        _rb= GetComponent<Rigidbody2D>(); 
        _platformEndCollider= GetComponent<Collider2D>();
    }

    // Update is called once per frame
    void Update()
    {

        Move();
        
    }

    private void Move()
    {
        if (IsFacingRight)
        {
            _rb.velocity = new Vector2(moveSpeed, 0f);
        }
        else
        {
            _rb.velocity = new Vector2(-moveSpeed, 0f);
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (IsFacingRight)
        {
            FlipSprite(-1f);
        }
        else
        {
            FlipSprite(1f);
        }
    }

    private void FlipSprite(float direction)
    {
        Debug.Log("Флип");
        transform.localScale = new Vector2(Mathf.Sign(direction), transform.localScale.y);
    }
}
