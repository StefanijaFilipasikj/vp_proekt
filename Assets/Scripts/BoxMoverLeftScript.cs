using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoxMoverLeftScript : MonoBehaviour
{
    // Start is called before the first frame update
    public GameObject Player;
    private Animator animator;
    private PlayerMovement playerScript;
    private List<Collider2D> Colliders = new List<Collider2D>();// needed to be able to check all the boxes in the trigger
    void Start()
    {
        animator = Player.GetComponent<Animator>();
        playerScript = Player.GetComponent<PlayerMovement>();
    }

    // Update is called once per frame
    void Update()
    {
        for (int i = Colliders.Count - 1; i >= 0; i--)
            if (animator.GetCurrentAnimatorStateInfo(0).IsName("PlayerMove"))
            {
                if (Colliders[i].gameObject == null)
                {
                    Colliders.RemoveAt(i);
                    continue;
                }
                Colliders[i].gameObject.GetComponent<BoxScript>().Move(0);
            }
    }
    //is only called once when the box enters the trigger
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (!Colliders.Contains(other) && other.gameObject.layer == LayerMask.NameToLayer("Ground"))
        {
            Colliders.Add(other);
        }
    }
    //is only called once when the box exits the trigger
    private void OnTriggerExit2D(Collider2D other)
    {
        if (Colliders.Contains(other) && other.gameObject.layer == LayerMask.NameToLayer("Ground"))
            Colliders.Remove(other);
    }
}
