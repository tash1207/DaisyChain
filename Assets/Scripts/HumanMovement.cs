using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HumanMovement : MonoBehaviour
{
    Rigidbody2D rb2d;
    Animator animator;

    Vector2 lookDirection = new Vector2(0, -1);

    void Start()
    {
        rb2d = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
    }

    void Update()
    {
        // TODO: Determine if connecting this to Daisy's X, Y, and movement is better.

        bool playerHasHorizontalSpeed = Mathf.Abs(rb2d.velocity.x) > Mathf.Epsilon;
        bool playerHasVerticalSpeed = Mathf.Abs(rb2d.velocity.y) > Mathf.Epsilon;
        animator.SetBool("isWalking", playerHasHorizontalSpeed || playerHasVerticalSpeed);

        if (!Mathf.Approximately(rb2d.velocity.x, 0.0f) || !Mathf.Approximately(rb2d.velocity.y, 0.0f))
        {
            lookDirection.Set(rb2d.velocity.x, rb2d.velocity.y);
            lookDirection.Normalize();
        }

        animator.SetFloat("LookX", lookDirection.x);
        animator.SetFloat("LookY", lookDirection.y);
    }

    public void StopHumanMovement()
    {
        rb2d.velocity = new Vector2(0, 0);
        animator.SetBool("isWalking", false);
    }
}
