using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovement : MonoBehaviour
{
    Rigidbody2D rb2d;
    Animator animator;

    Vector2 lookDirection = new Vector2(0, -1);
    float moveSpeed = 3f;
    Vector2 moveInput;

    public bool pausePlayerMovement = false;

    void Start()
    {
        rb2d = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
    }

    void Update()
    {
        if (pausePlayerMovement) { return; }
        Walk();
    }

    void OnMove(InputValue value)
    {
        moveInput = value.Get<Vector2>();
    }

    void Walk()
    {
        rb2d.velocity = moveInput * moveSpeed;

        bool playerHasHorizontalSpeed = Mathf.Abs(rb2d.velocity.x) > Mathf.Epsilon;
        bool playerHasVerticalSpeed = Mathf.Abs(rb2d.velocity.y) > Mathf.Epsilon;
        animator.SetBool("isWalking", playerHasHorizontalSpeed || playerHasVerticalSpeed);

        if (!Mathf.Approximately(moveInput.x, 0.0f) || !Mathf.Approximately(moveInput.y, 0.0f))
        {
            lookDirection.Set(moveInput.x, moveInput.y);
            lookDirection.Normalize();
        }

        animator.SetFloat("LookX", lookDirection.x);
        animator.SetFloat("LookY", lookDirection.y);
    }

    public void PausePlayerMovement()
    {
        pausePlayerMovement = true;
        rb2d.velocity = new Vector2(0, 0);
        if (FindObjectOfType<HumanMovement>())
        {
            FindObjectOfType<HumanMovement>().StopHumanMovement();
        }
        animator.SetBool("isWalking", false);
    }
}
