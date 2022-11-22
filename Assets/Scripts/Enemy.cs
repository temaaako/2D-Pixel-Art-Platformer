using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{

    [SerializeField] private float moveSpeed;

    private Rigidbody2D _rb;

    private Collider2D _platformEndCollider;


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

        _rb.velocity = new Vector2(moveSpeed, 0f);
        //bool onGround = Physics2D.Raycast(transform.position, Vector2.down, _groundLength, _groundLayer);
    }

    private void FlipSprite(float direction)
    {
        transform.localScale = new Vector2(Mathf.Sign(direction), transform.localScale.y);
    }
}
