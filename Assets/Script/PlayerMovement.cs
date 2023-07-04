using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    private float horizontalInput;
    public float speed = 5;
    public float jumpingPower = 10;
    private bool isFacingRight = true;
    private Camera mainCamera;
    private float cameraWidth;

    //Refference to Rigidbody2d
    [SerializeField] private Rigidbody2D rb;
    [SerializeField] private Transform groundCheck;
    [SerializeField] private LayerMask groundLayer;

    private void Start()
    {
        mainCamera = Camera.main;
        CalculateCameraWidth();
    }

    // Update is called once per frame
    void Update()
    {
        
        horizontalInput = Input.GetAxisRaw("Horizontal"); //returns -1, 1 or 0, depending on the direction

        if((Input.GetKeyDown(KeyCode.UpArrow) || Input.GetKeyDown(KeyCode.W)) && IsGrounded())
        {
            rb.velocity = new Vector2(rb.velocity.x, jumpingPower);
        }

        Flip();
    }

    private void FixedUpdate()
    {
        rb.velocity =  new Vector2 (horizontalInput * speed, rb.velocity.y);
        CheckIfOffScreen();
    }

    private void Flip()
    {
        if (isFacingRight && horizontalInput < 0 || !isFacingRight && horizontalInput > 0)
        {
            isFacingRight = !isFacingRight;
            Vector3 localScale = transform.localScale;
            localScale.x *= -1;
            transform.localScale = localScale;
        }
    }

    private bool IsGrounded()
    {
        return Physics2D.OverlapCircle(groundCheck.position, 0.2f , groundLayer);
    }
    private void CalculateCameraWidth()
    {
        cameraWidth = mainCamera.orthographicSize * 2f * mainCamera.aspect;
    }
    private void CheckIfOffScreen()
    {
        float playerWidth = CalculatePlayerWidth();

        float leftBoundary = mainCamera.ViewportToWorldPoint(new Vector3(0, 0, transform.position.z)).x + playerWidth / 2f;
        float rightBoundary = mainCamera.ViewportToWorldPoint(new Vector3(1, 0, transform.position.z)).x - playerWidth / 2f;

        float clampedXPosition = Mathf.Clamp(transform.position.x, leftBoundary, rightBoundary);
        transform.position = new Vector3(clampedXPosition, transform.position.y, transform.position.z);
    }
    private float CalculatePlayerWidth()
    {
        Renderer playerRenderer = GetComponent<Renderer>();
        Bounds playerBounds = playerRenderer.bounds;
        float playerWidth = playerBounds.size.x;
        float pivotOffset = playerBounds.center.x - transform.position.x;

        return playerWidth - pivotOffset;
    }
}
