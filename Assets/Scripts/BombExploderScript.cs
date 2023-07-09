using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BombExploderScript : MonoBehaviour
{

    public List<GameObject> Boxes = new List<GameObject>(); // all objects in the trigger
    //when a box enters the collider, add it to the list
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.tag == "Box")
            Boxes.Add(other.gameObject);
    }
    //when a box exits the collider, add it to the list
    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.gameObject.tag == "Box")
            Boxes.Remove(other.gameObject);
    }

    //disable scripts, destroy boxes and play animation
    public void Explode()
    {
        transform.parent.GetComponent<SpriteRenderer>().enabled = false;
        transform.parent.GetComponent<PowerUpScript>().enabled = false;
        GetComponent<ParticleSystem>().Play();
        for (int i = Boxes.Count - 1; i >= 0; i--)
        {
            Boxes[i].GetComponent<BoxScript>().Destroy();
        }
        Destroy(transform.parent.gameObject, 1f);
    }
}
