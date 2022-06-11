using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveableArea : MonoBehaviour
{
    [SerializeField] private float leftCap;
    [SerializeField] private float rightCap;
    protected Rigidbody2D rb;

    private bool facingLeft = true;

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {
        Move();
    }

    private void Move()
    {
        if (facingLeft)
        {
            if (transform.position.x > leftCap)
            {
                rb.velocity = new Vector2(-2, 0);
            }
            else
            {
                facingLeft = false;
            }
        }
        else
        {
            if (transform.position.x < rightCap)
            {
                rb.velocity = new Vector2(2, 0);
            }
            else
            {
                facingLeft = true;
            }
        }
    }
}
