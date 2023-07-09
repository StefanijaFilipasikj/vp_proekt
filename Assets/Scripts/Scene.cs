using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Scene : MonoBehaviour
{
    private float time; // time needed for random generation
    private int[] sides; // 0-left, 1-right, form which side a box i going to start depending on the column
    public List<List<GameObject>> Boxes; // two dimensional list
    public int numColumns = 21;
    public int numRows = 7;

    //static variables for comunication between scenes
    public static int ChanceForBlack = 95; // percent chance for black box
    public static int ChanceForPowerUp = 95; // precent chance for power-up
    public static int Points = 0;
    public static float generateTime = 1.0f; // time to generate each box in seconds

    //references
    [SerializeField] GameObject box; // reference to the box prefab
    [SerializeField] List<Sprite> Sprites = new List<Sprite>(); // refrence to each sprite for the boxes
    [SerializeField] GameObject PointsText; // refrence to the points textbox
    [SerializeField] GameObject PowerUp; // refrence to the power-up prefab

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

    // Generate a box with random color and column
    private void GenerateBox()
    {
        int rnd = Random.Range(0, 100); // percent chance
        int column = GetColumn();
        int side = sides[column]; // from which side the box is going to fall
        int spriteNum = Random.Range(0, 4);
        if (rnd > ChanceForBlack)
            spriteNum = 4;
        if (rnd > ChanceForPowerUp) // if enough for power-up, generate and return
        {
            GeneratePowerUp();
            return;
        }
        GameObject box1;
        if (side == 0) // spawn a box depending on which side it is going to be spawned; 0-left, 1-right
            box1 = Instantiate(box, new Vector3(-1, numRows + 0.5f, 0), Quaternion.identity);
        else
            box1 = Instantiate(box, new Vector3(22, numRows + 0.5f, 0), Quaternion.identity);

        BoxScript script = box1.GetComponent<BoxScript>();
        if (spriteNum == 4)
        {
            script.IsBlack = true;
        }
        //Initialize needed variables for the box script
        script.Scene = this;
        box1.GetComponent<SpriteRenderer>().sprite = Sprites[spriteNum];
        script.side = side;
        script.column = column;
        script.row = Boxes[column].Count;
    }

    //Generating random level with number of boxes between 15 and 25
    private void GenerateLevel()
    {
        int n = Random.Range(15, 25);
        for (int i = 0; i < n; i++)
        {
            int column = GetColumn();
            int row = Boxes[column].Count;
            int side = sides[column]; // from which side the box is going to fall
            int spriteNum = Random.Range(0, 4); // index for the sprite
            GameObject box1;
            box1 = Instantiate(box, new Vector3(column + 0.5f, row + 0.5f, 0), Quaternion.identity);
            BoxScript script = box1.GetComponent<BoxScript>();

            //Initialize needed variables for the box script
            script.Scene = this;
            box1.GetComponent<SpriteRenderer>().sprite = Sprites[spriteNum];
            script.side = side;
            script.column = column;
            script.row = row;
            script.isDoneMovingX = true;
            script.isDoneMovingY = true;
            script.HasChecked = true;

            //add box to matrix and delete the hook
            Boxes[column].Add(box1);
            Destroy(box1.transform.Find("HookSprite").gameObject);
        }
        //only the first row can be full so check it
        CheckIfFullRow(0);
    }

    // Function to generate power-up
    private void GeneratePowerUp()
    {
        int column = GetColumn();
        int side = sides[column];
        int type = Random.Range(0, 3);
        GameObject box1;
        if (side == 0)
            box1 = Instantiate(PowerUp, new Vector3(-1, numRows + 0.5f, 0), Quaternion.identity);
        else
            box1 = Instantiate(PowerUp, new Vector3(numColumns + 1, numRows + 0.5f, 0), Quaternion.identity);

        //Initialize needed variables for the power-up script
        PowerUpScript script = box1.GetComponent<PowerUpScript>();
        script.type = type;
        script.SetSprite();
        script.Scene = this;
        script.side = side;
        script.column = column;
        script.row = Boxes[column].Count;
    }

    //Check if the row is full and destory if full
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


    public void CheckIfFullColumn(int col)
    {
        if (Boxes[col].Count > numRows)
        {
            SceneManager.LoadScene("GameOverScene", LoadSceneMode.Single);
        }
    }

    //for each box in row play animation and destroy when animation full
    private void DestroyRow(int row)
    {
        List<GameObject> BoxesToDestory = new List<GameObject>(); // needed for animation
        for (int i = 0; i < numColumns; i++)
        {
            Boxes[i][row].GetComponent<BoxScript>().enabled = false;
            Boxes[i][row].layer = LayerMask.NameToLayer("DeadBox");
            Boxes[i][row].GetComponent<BoxCollider2D>().isTrigger = true;
            BoxesToDestory.Add(Boxes[i][row]);
            //make the box transparent
            Color color = Boxes[i][row].gameObject.GetComponent<SpriteRenderer>().color;
            color.a = 0.5f;
            Boxes[i][row].gameObject.GetComponent<SpriteRenderer>().color = color;
            //free the block and make other blocks in column fall
            FreeBlock(i, row);
        }
        DestroyRowScript s = this.gameObject.AddComponent<DestroyRowScript>();
        s.boxes = BoxesToDestory;
        s.IsDoneMoovingBoxes = false;
        Points += 100 * numColumns * 2;
    }

    private int GetColumn()
    {
        return Random.Range(0, numColumns);
    }

    //Free the block and make other blocks in the column fall
    public void FreeBlock(int col, int row)
    {
        Boxes[col].RemoveAt(row);
        for (int i = row; i < Boxes[col].Count; i++)
            Boxes[col][i].GetComponent<BoxScript>().row--;
    }
}
