using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class BlockBreaker : MonoBehaviour
{
    public PlayerMovement PMScript; //refrence to the player script

    //called when an object enters the trigger
    private void OnTriggerEnter2D(Collider2D other)
    {
        //if player is not jumping and a block fall, game over
        if (PMScript.IsGrounded() && other.gameObject.layer == LayerMask.NameToLayer("Ground"))
        {
            SceneManager.LoadScene("GameOverScene", LoadSceneMode.Single);

        }//if player is jumping and a block is not black, destroy it
        else if (!PMScript.IsGrounded() && other.gameObject.layer == LayerMask.NameToLayer("Ground") && !other.gameObject.GetComponent<BoxScript>().IsBlack)
        {
            other.gameObject.GetComponent<BoxScript>().Destroy();
        }
    }
}
