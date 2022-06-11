using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WalkingEnemy : Enemy
{
    [SerializeField] private float leftCap;
    [SerializeField] private float rightCap;
    [SerializeField] private LayerMask ground;
    [SerializeField] private float movementSpeed;

    private bool facingLeft = true;

    // Start is called before the first frame update
    protected override void Start()
    {
        base.Start();
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
            //Test to see if we are beyond the leftCap
            if (transform.position.x > leftCap)
            {
                //Make sure sprite is facing right location, and if is not, then face the right directon
                if (transform.localScale.x != 1)
                {
                    transform.localScale = new Vector3(1, 1);
                }
                rb.velocity = new Vector2(-movementSpeed, 0);
            }
            else
            {
                facingLeft = false;
            }
        }
        else
        {
            //Test to see if we are beyond the leftCap
            if (transform.position.x < rightCap)
            {
                //Make sure sprite is facing right location, and if is not, then face the right directon
                if (transform.localScale.x != -1)
                {
                    transform.localScale = new Vector3(-1, 1);
                }
                rb.velocity = new Vector2(movementSpeed, 0);
            }
            else
            {
                facingLeft = true;
            }
        }
    }
}
