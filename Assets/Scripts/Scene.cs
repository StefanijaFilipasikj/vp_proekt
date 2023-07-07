using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class Scene : MonoBehaviour
{
    private float time;
    private int[] sides;
    public List<List<GameObject>> Boxes;
    public GameObject box;
    public int numColumns = 21;
    [SerializeField] int numRows = 7;
    [SerializeField] List<Sprite> Sprites = new List<Sprite>();
    [SerializeField] GameObject PointsText;
    [SerializeField] GameObject PowerUp;
    public static int ChanceForBlack = 90;
    public static int ChanceForPowerUp = 95;
    public static int Points = 0;
    public static float generateTime = 0.5f;
    // Start is called before the first frame update
    void Start()
    {
        time = 0;
        Points = 0;
        Boxes = new List<List<GameObject>>();
        sides = new int[numColumns];
        for (int i = 0; i < numColumns; i++)
        {
            int side = Random.Range(0, 2);
            sides[i] = side;
            Boxes.Add(new List<GameObject>());
        }
        GenerateLevel();
    }

    private void GenerateLevel()
    {
        int n = Random.Range(15, 25);
        GeneratePowerUp();
        for (int i = 0; i < n; i++)
        {
            int column = GetColumn();
            int row = Boxes[column].Count;
            int side = sides[column];
            int spriteNum = Random.Range(0, 4);
            GameObject box1;
            box1 = Instantiate(box, new Vector3(column + 0.5f, row + 0.5f, 0), Quaternion.identity);
            BoxScript script = box1.GetComponent<BoxScript>();
            //Initialize needed variables for the box
            script.Scene = this;
            box1.GetComponent<SpriteRenderer>().sprite = Sprites[spriteNum];
            script.side = side;
            script.column = column;
            script.row = row;
            script.isDoneMovingX = true;
            script.isDoneMovingY = true;
            script.HasChecked = true;
            Boxes[column].Add(box1);
            Destroy(box1.transform.Find("HookSprite").gameObject);
        }
        CheckIfFullRow(0);
    }

    private void GeneratePowerUp()
    {
        int column = GetColumn();
        int side = sides[column]; // from which side the box is going to fall
        int type = Random.Range(0, 3);
        GameObject box1;
        if (side == 0)
            box1 = Instantiate(PowerUp, new Vector3(-1, numRows + 0.5f, 0), Quaternion.identity);
        else
            box1 = Instantiate(PowerUp, new Vector3(numColumns + 1, numRows + 0.5f, 0), Quaternion.identity);

        //Initialize needed variables for the box
        PowerUpScript script = box1.GetComponent<PowerUpScript>();
        script.type = type;
        script.SetSprite();
        script.Scene = this;
        script.side = side;
        script.column = column;
        script.row = Boxes[column].Count;
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
        PointsText.GetComponent<TextMeshProUGUI>().text = $"Points: {Points}";
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
        List<GameObject> BoxesToDestory = new List<GameObject>();
        for (int i = 0; i < numColumns; i++)
        {
            Boxes[i][row].GetComponent<BoxScript>().enabled = false;
            Boxes[i][row].layer = LayerMask.NameToLayer("DeadBox");
            Boxes[i][row].GetComponent<BoxCollider2D>().isTrigger = true;
            BoxesToDestory.Add(Boxes[i][row]);
            Color color = Boxes[i][row].gameObject.GetComponent<SpriteRenderer>().color;
            color.a = 0.5f;
            Boxes[i][row].gameObject.GetComponent<SpriteRenderer>().color = color;
            FreeBlock(i, row);
        }
        DestroyRowScript s = this.gameObject.AddComponent<DestroyRowScript>();
        s.boxes = BoxesToDestory;
        s.IsDoneMoovingBoxes = false;
        Points += 100 * numColumns * 2;
    }
    private void GenerateBox()
    {
        int rnd = Random.Range(0, 100);
        int column = GetColumn();
        int side = sides[column]; // from which side the box is going to fall
        int spriteNum = Random.Range(0, 4);
        if (rnd > ChanceForBlack)
            spriteNum = 4;
        if (rnd > ChanceForPowerUp)
        {
            GeneratePowerUp();
            return;
        }
        GameObject box1;
        if (side == 0)
            box1 = Instantiate(box, new Vector3(-1, numRows + 0.5f, 0), Quaternion.identity);
        else
            box1 = Instantiate(box, new Vector3(22, numRows + 0.5f, 0), Quaternion.identity);

        BoxScript script = box1.GetComponent<BoxScript>();
        if (spriteNum == 4)
        {
            script.IsBlack = true;
            Color c = box1.GetComponent<SpriteRenderer>().color;
            c.r = 0.0f;
            c.g = 0.0f;
            c.b = 0.0f;
            box1.GetComponent<SpriteRenderer>().color = c;
        }
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
        while (Boxes[col].Count > numRows) //get next free column if this column is full
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
