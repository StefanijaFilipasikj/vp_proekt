using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Scene : MonoBehaviour
{
    private float time;
    private int[] sides;
    public List<List<GameObject>> Boxes;
    public GameObject box;
    public int numColumns = 21;
    [SerializeField] int numRows = 9;
    [SerializeField] float generateTime = 0.5f;
    [SerializeField] List<Sprite> Sprites = new List<Sprite>();
    // Start is called before the first frame update
    void Start()
    {
        time = 0;
        Boxes = new List<List<GameObject>>();
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
    //Check if the row is full and destory the row if full
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
        {
            Boxes[i][row].GetComponent<BoxScript>().Destroy();
            FreeBlock(i, row);
        }

    }
    private void GenerateBox()
    {
        int column = GetColumn();
        int side = sides[column]; // from which side the box is going to fall
        int spriteNum = Random.Range(0, 4);
        GameObject box1;
        if (side == 0)
            box1 = Instantiate(box, new Vector3(-1, 9, 0), Quaternion.identity);
        else
            box1 = Instantiate(box, new Vector3(22, 9, 0), Quaternion.identity);
        BoxScript script = box1.GetComponent<BoxScript>();
        //Initialize needed variables for the box
        script.Scene = this;
        box1.GetComponent<SpriteRenderer>().sprite = Sprites[spriteNum];
        script.side = side;
        script.column = column;
        script.row = Boxes[column].Count;
    }

    private int GetColumn()
    {
        int col = Random.Range(0, numColumns);
        int c = col;
        while (Boxes[col].Count >= numRows) //get next free column if this column is full
        {
            col++;
            if (col == numColumns)
                col = 0;
            if (col == c)
                return 0;
        }
        return col;
    }
    //Free a non mooving block and make the boxes on top of the block fall
    public void FreeBlock(int col, int row)
    {
        Boxes[col].RemoveAt(row);
        for (int i = row; i < Boxes[col].Count; i++)
            Boxes[col][i].GetComponent<BoxScript>().row--;
    }
}
