using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ForkliftScript : MonoBehaviour
{
    // Start is called before the first frame update
    public float MoveSpeed = 0.1f;
    private List<GameObject> Boxes = new List<GameObject>();
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        Move();
    }

    private void Move()
    {
        float move = MoveSpeed * Time.deltaTime;
        transform.position = Vector3.MoveTowards(transform.position, new Vector3(-3, transform.position.y, transform.position.z), move);
        MoveBoxes();
        if (transform.position.x == -3)
        {
            foreach (GameObject box in Boxes)
            {
                Destroy(box);
            }
            Destroy(gameObject);
        }

    }

    private void MoveBoxes()
    {
        float move = 30 * Time.deltaTime;
        for (int i = 0; i < Boxes.Count; i += 2)
        {
            Boxes[i].transform.position = Vector3.MoveTowards(Boxes[i].transform.position,
                                            new Vector3(transform.position.x - 2, transform.position.y - 0.85f + i / 2, transform.position.z), move);
            if (i + 1 != Boxes.Count)
                Boxes[i + 1].transform.position = Vector3.MoveTowards(Boxes[i + 1].transform.position,
                                            new Vector3(transform.position.x - 2 - 1, transform.position.y - 0.85f + i / 2, transform.position.z), move);
        }

    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.tag == "Player")
        {
            Color color = GetComponent<SpriteRenderer>().color;
            color.a = 0.5f;
            GetComponent<SpriteRenderer>().color = color;
            foreach (GameObject box in Boxes)
            {
                box.GetComponent<SpriteRenderer>().color = color;
            }
        }
        if (other.gameObject.layer == LayerMask.NameToLayer("DeadBox"))
        {
            Boxes.Add(other.gameObject);
        }
    }
    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.gameObject.tag == "Player")
        {
            Color color = GetComponent<SpriteRenderer>().color;
            color.a = 1.0f;
            GetComponent<SpriteRenderer>().color = color;
        }
    }
}
