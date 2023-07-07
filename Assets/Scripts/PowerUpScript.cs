using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PowerUpScript : MonoBehaviour
{
    public bool isDoneMovingX = false;
    public bool isDoneMovingY = false;
    public int type = 0;
    public int side;
    public int column;
    public int row;
    public float moveSpeed = 0.1f;
    public Scene Scene;
    public PlayerMovement Player;
    [SerializeField] List<Sprite> Sprites = new List<Sprite>();
    // Start is called before the first frame update
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
            Transform hook = transform.Find("HookSprite");
            if (hook != null)
                Destroy(hook.gameObject);
        }

    }
    public void SetSprite()
    {
        gameObject.GetComponent<SpriteRenderer>().sprite = Sprites[type];
        if (type != 0)
        {
            Destroy(transform.GetChild(1).gameObject);
        }
        Player = FindAnyObjectByType<PlayerMovement>();
    }
    private void MoveToY()
    {
        float move = moveSpeed * Time.deltaTime;
        row = Scene.Boxes[column].Count;
        transform.position = Vector3.MoveTowards(transform.position, new Vector3(transform.position.x, row + 0.5f, transform.position.z), move);
        if (transform.position.y == row + 0.5f)
        {
            isDoneMovingY = true;
        }
    }
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.tag == "Player")
        {
            if (type == 0)
            {
                transform.GetChild(0).GetComponent<BombExploderScript>().Explode();
            }
            else if (type == 1)
            {
                Player.EnableJumpPowerUp();
                Destroy(this.gameObject);
            }
            else if (type == 2)
            {
                Player.EnableSpeedPowerUp();
                Destroy(this.gameObject);
            }
        }

    }
}
