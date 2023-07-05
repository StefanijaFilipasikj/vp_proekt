using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoxMoverRightScript : MonoBehaviour
{
    // Start is called before the first frame update
    public GameObject Player;
    private Animator animator;
    private PlayerMovement playerScript;
    void Start()
    {
        animator = Player.GetComponent<Animator>();
        playerScript = Player.GetComponent<PlayerMovement>();
    }

    // Update is called once per frame
    void Update()
    {

    }
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (animator.GetCurrentAnimatorStateInfo(0).IsName("PlayerMove"))
        {
            other.gameObject.GetComponent<BoxScript>().Move(1);
        }
    }
}
