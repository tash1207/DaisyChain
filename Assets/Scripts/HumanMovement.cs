using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HumanMovement : MonoBehaviour
{
    GameObject daisy;
    Rigidbody2D rb2d;
    Animator animator;
    LineRenderer lineRenderer;

    Vector2 lookDirection = new Vector2(0, -1);
    float minVelocity = 0.5f;

    void Start()
    {
        daisy = FindObjectOfType<PlayerMovement>().gameObject;
        rb2d = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        lineRenderer = gameObject.transform.GetChild(0).GetComponent<LineRenderer>();
    }

    void Update()
    {
        bool playerHasHorizontalSpeed = Mathf.Abs(rb2d.velocity.x) > minVelocity;
        bool playerHasVerticalSpeed = Mathf.Abs(rb2d.velocity.y) > minVelocity;
        animator.SetBool("isWalking", playerHasHorizontalSpeed || playerHasVerticalSpeed);

        if (playerHasHorizontalSpeed || playerHasVerticalSpeed)
        {
            lookDirection.Set(rb2d.velocity.x, rb2d.velocity.y);
            lookDirection.Normalize();
        }

        animator.SetFloat("LookX", lookDirection.x);
        animator.SetFloat("LookY", lookDirection.y);

        Vector2 leashPositionHuman =
            new Vector2(transform.position.x, transform.position.y - 0.24f);
        Vector2 leashPositionDaisy =
            new Vector2(daisy.transform.position.x, daisy.transform.position.y - 0.3f);

        lineRenderer.SetPosition(0, leashPositionHuman);
        lineRenderer.SetPosition(1, leashPositionDaisy);
    }

    public void StopHumanMovement()
    {
        rb2d.velocity = new Vector2(0, 0);
        animator.SetBool("isWalking", false);
    }
}
