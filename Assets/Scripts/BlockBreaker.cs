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
        if (!PMScript.IsGrounded())
        {
            other.gameObject.GetComponent<BoxScript>().Destroy();
        }
        else
        {
            SceneManager.LoadScene("GameOverScene", LoadSceneMode.Single);
        }
    }
}
