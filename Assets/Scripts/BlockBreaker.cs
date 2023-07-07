using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class BlockBreaker : MonoBehaviour
{
    // Start is called before the first frame update
    public PlayerMovement PMScript;
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (PMScript.IsGrounded() && other.gameObject.layer == LayerMask.NameToLayer("Ground"))
        {
            SceneManager.LoadScene("GameOverScene", LoadSceneMode.Single);

        }
        else if (!PMScript.IsGrounded() && other.gameObject.layer == LayerMask.NameToLayer("Ground") && !other.gameObject.GetComponent<BoxScript>().IsBlack)
        {
            other.gameObject.GetComponent<BoxScript>().Destroy();
        }
    }
}
