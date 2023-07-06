using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestroyRowScript : MonoBehaviour
{
    public Scene Scene;
    public GameObject Forklift;
    public List<GameObject> boxes = new List<GameObject>();
    public bool IsDoneMoovingBoxes = true;
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
            if (!Destroy())
            {
                IsDoneMoovingBoxes = true;
                ForkliftObj = Instantiate(Forklift, new Vector3(22.5f, 0.6f, 0), Quaternion.identity);
                Destroy(this);
            }
    }

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
