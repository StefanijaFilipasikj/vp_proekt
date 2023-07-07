using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BombExploderScript : MonoBehaviour
{
    // Start is called before the first frame update
    public List<GameObject> Boxes = new List<GameObject>();
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.tag == "Box")
            Boxes.Add(other.gameObject);
    }
    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.gameObject.tag == "Box")
            Boxes.Remove(other.gameObject);
    }
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
