using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoxScript : MonoBehaviour
{
    // Start is called before the first frame update
    public bool isDoneMovingX = false;
    public bool isDoneMovingY = false;
    public int side;
    public int column;
    public int row;
    public Scene Scene;
    public float moveSpeed = 0.1f;
    public bool HasChecked = false;
    public bool IsBlack = false;
    void Start()
    {

    }
    // Update is called once per frame
    void Update()
    {
        //first move to the column, then to the row
        if (!isDoneMovingX)
            MoveToX();
        if (isDoneMovingX)
            MoveToY();
    }

    private void MoveToX()
    {
        float move = moveSpeed * Time.deltaTime;
        if (side == 0)
        {
            transform.position = Vector3.MoveTowards(transform.position, new Vector3(column + 0.5f, transform.position.y, transform.position.z), move);
        }
        else if (side == 1)
        {
            transform.position = Vector3.MoveTowards(transform.position, new Vector3(column + 0.5f, transform.position.y, transform.position.z), move);
        }
        if (transform.position.x == column + 0.5f)
        {
            isDoneMovingX = true;
            Transform hook = transform.Find("HookSprite");
            if (hook != null)
                Destroy(hook.gameObject);
        }

    }
    private void MoveToY()
    {
        float move = moveSpeed * Time.deltaTime;
        if (!isDoneMovingY)
            row = Scene.Boxes[column].Count;
        transform.position = Vector3.MoveTowards(transform.position, new Vector3(transform.position.x, row + 0.5f, transform.position.z), move);
        if (transform.position.y == row + 0.5f)
        {
            isDoneMovingY = true;
            if (!HasChecked) // add the box to the Scene and check if the row is full. This needs to be done only once
            {
                Scene.Boxes[column].Add(this.gameObject);
                Scene.CheckIfFullColumn(column);
                Scene.CheckIfFullRow(row);
                HasChecked = true;
            }
        }
    }
    public void Destroy()
    {
        Scene.Points += 100;
        if (isDoneMovingY)
            Scene.FreeBlock(column, row);
        Destroy(this.gameObject);
    }
    //0-left, 1-right
    public void Move(int side)
    {
        //Check if this is the top block of the column and if the block can move 
        if (!IsBlack)
        {
            if (column != 0 && Scene.Boxes[column].Count - 1 == row && Scene.Boxes[column - 1].Count - 1 < row && side == 0)
            {
                Scene.Boxes[column].RemoveAt(row);
                column--;
                isDoneMovingX = false;
                isDoneMovingY = false;
                HasChecked = false;
            }
            else if (column != Scene.numColumns - 1 && Scene.Boxes[column].Count - 1 == row && Scene.Boxes[column + 1].Count - 1 < row && side == 1)
            {
                Scene.Boxes[column].RemoveAt(row);
                column++;
                isDoneMovingX = false;
                isDoneMovingY = false;
                HasChecked = false;
            }
        }
    }
}
