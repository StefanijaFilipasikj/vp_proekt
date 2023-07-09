using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestroyRowScript : MonoBehaviour
{
    public GameObject Forklift; // reference to forklift prefab
    public List<GameObject> boxes = new List<GameObject>(); // reference to the boxes needed to destroy
    public bool IsDoneMoovingBoxes = true; // if animation is finished
    private GameObject ForkliftObj;

    // Start is called before the first frame update
    void Start()
    {
        Forklift = GetComponent<DestroyRowScript>().Forklift;
    }

    // Update is called once per frame
    void Update()
    {
        if (!IsDoneMoovingBoxes)
            if (!Destroy()) // if the boxes are done moving, spawn a forklift and destory the script
            {
                IsDoneMoovingBoxes = true;
                ForkliftObj = Instantiate(Forklift, new Vector3(22.5f, 0.6f, 0), Quaternion.identity);
                Destroy(this);
            }
    }

    //move all boxes to the row for pickup and return false if all boxes are moved
    public bool Destroy()
    {
        bool f = false;
        foreach (GameObject box in boxes)
        {
            if (box.transform.position.y != (-1 + 0.5f))
                f = true;
            box.transform.position = Vector3.MoveTowards(box.transform.position, new Vector3(box.transform.position.x, -1 + 0.5f, box.transform.position.z),
                                                        box.GetComponent<BoxScript>().moveSpeed * Time.deltaTime);
        }
        return f;
    }
}
