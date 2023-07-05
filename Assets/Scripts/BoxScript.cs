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
    [SerializeField] float moveSpeed = 0.1f;
    private bool HasChecked = false;
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
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
            row = Scene.GetFreeRow(column);
            Scene.freeBlocks[column]++;
        }

    }
    private void MoveToY()
    {
        float move = moveSpeed * Time.deltaTime;
        transform.position = Vector3.MoveTowards(transform.position, new Vector3(transform.position.x, row + 0.5f, transform.position.z), move);
        if (transform.position.y == row + 0.5f)
        {
            isDoneMovingY = true;
            if (!HasChecked)
            {
                //Scene.freeBlocks[column]++;
                Scene.CheckIfFullRow(row);
                HasChecked = true;
            }
        }
    }
    public void Destroy()
    {
        Scene.FreeBlock(column, row);
        Destroy(this.gameObject);
    }
    //0-left, 1-right
    public void Move(int side)
    {
        Debug.Log($"{Scene.freeBlocks[column]} == {row} | {column} != 0 | {Scene.freeBlocks[column - 1]} < {row}");
        if (column != 0 && Scene.freeBlocks[column] == row + 1 && Scene.freeBlocks[column - 1] <= row && side == 0)
        {
            /*Scene.freeBlocks[column]--;
            Scene.Boxes[column].RemoveAt(row);
            column--;
            Scene.Boxes[column].Add(this.gameObject);
            row = Scene.GetFreeRow(column);
            //Scene.freeBlocks[column]++;
            isDoneMovingX = false;
            isDoneMovingY = false;
            HasChecked = false;*/
        }
        else if (column != 0 && Scene.freeBlocks[column] == row + 1 && Scene.freeBlocks[column + 1] <= row && side == 1)
        {
            /*Scene.freeBlocks[column]--;
            Scene.Boxes[column].RemoveAt(row);
            column++;
            Scene.Boxes[column][row] = this.gameObject;
            row = Scene.GetFreeRow(column);
            //Scene.freeBlocks[column]++;
            isDoneMovingX = false;
            isDoneMovingY = false;
            HasChecked = false;*/
        }

    }
}
