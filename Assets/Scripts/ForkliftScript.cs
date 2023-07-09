using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ForkliftScript : MonoBehaviour
{
    public float MoveSpeed = 0.1f;
    private List<GameObject> Boxes = new List<GameObject>(); // list of boxes to destroy

    // Update is called once per frame
    void Update()
    {
        Move();
    }
    //move forklift from right to left and destory the boxes when its done moving
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
                                            new Vector3(transform.position.x - 2, transform.position.y - 1.0f + i / 2, transform.position.z), move);
            if (i + 1 != Boxes.Count)
                Boxes[i + 1].transform.position = Vector3.MoveTowards(Boxes[i + 1].transform.position,
                                            new Vector3(transform.position.x - 2 - 1, transform.position.y - 1.0f + i / 2, transform.position.z), move);
        }

    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        //if coliding with player make the forklift transparent
        if (other.gameObject.tag == "Player")
        {
            Color color = GetComponent<SpriteRenderer>().color;
            color.a = 0.5f;
            GetComponent<SpriteRenderer>().color = color;
        }
        //if coliding with box add it to the list
        if (other.gameObject.layer == LayerMask.NameToLayer("DeadBox"))
        {
            Boxes.Add(other.gameObject);
        }
    }
    private void OnTriggerExit2D(Collider2D other)
    {
        //if player exiting colider, make the forklift normal
        if (other.gameObject.tag == "Player")
        {
            Color color = GetComponent<SpriteRenderer>().color;
            color.a = 1.0f;
            GetComponent<SpriteRenderer>().color = color;
        }
    }
}
