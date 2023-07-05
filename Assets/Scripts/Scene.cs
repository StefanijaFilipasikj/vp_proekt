using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Scene : MonoBehaviour
{
    private float time;
    public int[] freeBlocks;
    private int[] sides;
    public List<List<GameObject>> Boxes;
    public GameObject box;
    [SerializeField] int numColumns = 21;
    [SerializeField] int numRows = 9;
    [SerializeField] float generateTime = 0.5f;
    [SerializeField] List<Sprite> Sprites = new List<Sprite>();
    // Start is called before the first frame update
    void Start()
    {
        time = 0;
        freeBlocks = new int[numColumns];
        Boxes = new List<List<GameObject>>();
        for (int i = 0; i < freeBlocks.Length; i++)
            freeBlocks[i] = 0;
        sides = new int[numColumns];
        for (int i = 0; i < numColumns; i++)
        {
            int side = Random.Range(0, 2);
            sides[i] = side;
            Boxes.Add(new List<GameObject>());
        }
    }

    // Update is called once per frame
    void Update()
    {
        time += Time.deltaTime;
        if (time >= generateTime)
        {
            GenerateBox();
            time = 0;
        }
    }

    public void CheckIfFullRow(int row)
    {
        bool f = true;
        for (int j = 0; j < numColumns; j++)
        {
            if (Boxes[j].Count <= row)
            {
                f = false;
                break;
            }
            BoxScript s = Boxes[j][row].GetComponent<BoxScript>();
            if (s.isDoneMovingY != true)
            {
                f = false;
                break;
            }
        }
        if (f)
        {
            DestroyRow(row);
        }
    }
    private void DestroyRow(int row)
    {
        for (int i = 0; i < numColumns; i++)
            Boxes[i][row].GetComponent<BoxScript>().Destroy();
    }
    private void GenerateBox()
    {
        int column = GetColumn();
        int side = sides[column];
        int spriteNum = Random.Range(0, 4);
        GameObject box1;
        if (side == 0)
            box1 = Instantiate(box, new Vector3(-1, 9, 0), Quaternion.identity);
        else
            box1 = Instantiate(box, new Vector3(22, 9, 0), Quaternion.identity);
        BoxScript script = box1.GetComponent<BoxScript>();
        script.Scene = this;
        box1.GetComponent<SpriteRenderer>().sprite = Sprites[spriteNum];
        script.side = side;
        script.column = column;
        script.row = -1;
        Boxes[column].Add(box1);
    }

    private int GetColumn()
    {
        int col = Random.Range(0, numColumns);
        col = GetNextFreeCol(col);
        return col;
    }

    private int GetNextFreeCol(int col)
    {
        int c = col;
        while (freeBlocks[col] >= numRows)
        {
            col++;
            if (col == numColumns)
                col = 0;
            if (col == c)
                return 0;
        }
        return col;
    }
    public void FreeBlock(int col, int row)
    {
        freeBlocks[col]--;
        Boxes[col].RemoveAt(row);
        for (int i = row; i < Boxes[col].Count; i++)
            Boxes[col][i].GetComponent<BoxScript>().row--;
    }
    public int GetFreeRow(int col)
    {
        return freeBlocks[col];
    }
}
