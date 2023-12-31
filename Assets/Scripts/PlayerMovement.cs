using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class PlayerMovement : MonoBehaviour
{
    private float horizontalInput;
    public float speed = 5;
    public float jumpingPower = 10;
    public Animator animator;
    public bool isFacingRight = true;
    private Camera mainCamera;
    private float cameraWidth;
    //Refference to Rigidbody2d
    [SerializeField] private Rigidbody2D rb;
    [SerializeField] private Transform groundCheck;
    [SerializeField] private LayerMask groundLayer;
    [SerializeField] Canvas Canvas;
    private float GreenPotionTime;
    private float BluePotionTime;
    private void Start()
    {
        mainCamera = Camera.main;
        CalculateCameraWidth();
    }

    // Update is called once per frame
    void Update()
    {
        //move left and right
        horizontalInput = Input.GetAxisRaw("Horizontal"); //returns -1, 1 or 0, depending on the direction
        animator.SetFloat("Speed", Math.Abs(horizontalInput));
        //jump
        if ((Input.GetKeyDown(KeyCode.UpArrow) || Input.GetKeyDown(KeyCode.W)) && IsGrounded())
        {
            rb.velocity = new Vector2(rb.velocity.x, jumpingPower);
        }
        //if blue potion is active
        if (BluePotionTime > 0)
        {
            BluePotionTime -= Time.deltaTime;
            Canvas.transform.Find("TimeBluePotion").GetComponent<TextMeshProUGUI>().text = $"{(int)BluePotionTime}";
            if (BluePotionTime <= 0)
                DisableSpeedPowerUp();
        }
        //if green potion is active
        if (GreenPotionTime > 0)
        {
            GreenPotionTime -= Time.deltaTime;
            Canvas.transform.Find("TimeGreenPotion").GetComponent<TextMeshProUGUI>().text = $"{(int)GreenPotionTime}";
            if (GreenPotionTime <= 0)
                DisableJumpPowerUp();
        }
        //flip the sprite
        Flip();
    }

    private void DisableSpeedPowerUp()
    {
        speed = 5;
        transform.Find("SpeedParticleSystem").gameObject.SetActive(false);
        Canvas.transform.Find("BluePotionImage").gameObject.SetActive(false);
        Canvas.transform.Find("TimeBluePotion").gameObject.SetActive(false);
    }

    private void DisableJumpPowerUp()
    {
        jumpingPower = 12;
        rb.gravityScale = 5;
        transform.Find("JumpParticleSystem").gameObject.SetActive(false);
        Canvas.transform.Find("GreenPotionImage").gameObject.SetActive(false);
        Canvas.transform.Find("TimeGreenPotion").gameObject.SetActive(false);
    }

    public void EnableJumpPowerUp()
    {
        jumpingPower = 20;
        rb.gravityScale = 7;
        GreenPotionTime = 10f;
        transform.Find("JumpParticleSystem").gameObject.SetActive(true);
        Canvas.transform.Find("GreenPotionImage").gameObject.SetActive(true);
        Canvas.transform.Find("TimeGreenPotion").gameObject.SetActive(true);
    }
    public void EnableSpeedPowerUp()
    {
        speed = 10;
        BluePotionTime = 10f;
        transform.Find("SpeedParticleSystem").gameObject.SetActive(true);
        Canvas.transform.Find("BluePotionImage").gameObject.SetActive(true);
        Canvas.transform.Find("TimeBluePotion").gameObject.SetActive(true);
    }
    private void FixedUpdate()
    {
        rb.velocity = new Vector2(horizontalInput * speed, rb.velocity.y);
        CheckIfOffScreen();
    }

    private void Flip()
    {
        if (isFacingRight && horizontalInput < 0 || !isFacingRight && horizontalInput > 0)
        {
            isFacingRight = !isFacingRight;
            GetComponent<SpriteRenderer>().flipX = !GetComponent<SpriteRenderer>().flipX;
        }
    }

    public bool IsGrounded()
    {
        return Physics2D.OverlapCircle(groundCheck.position, 0.2f, groundLayer);
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
