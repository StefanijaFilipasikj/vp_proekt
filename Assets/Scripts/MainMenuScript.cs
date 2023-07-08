using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;
using System.Text;
using System;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using System.Runtime.Serialization;

public class MainMenuScript : MonoBehaviour
{
    [SerializeField] GameObject TextField;
    [SerializeField] GameObject LeaderBoard;
    public static List<Player> Players = new List<Player>();
    public static Player ThisPlayer;
    private static bool HasLoaded = false;
    // Start is called before the first frame update
    private void Awake()
    {
        if (!HasLoaded)
        {
            OpenFile(Application.dataPath + "/save.savefile");
            HasLoaded = true;
        }

        Transform obj = transform.Find("Points");
        if (obj != null)
        {
            obj.gameObject.GetComponent<TextMeshProUGUI>().text = $"{ThisPlayer.Name} Points: {Scene.Points}";
            if (ThisPlayer.BestPoints < Scene.Points)
                ThisPlayer.BestPoints = Scene.Points;
        }
        if (obj != null)
        {
            StringBuilder sb = new StringBuilder();
            Players.Sort(new Comparison<Player>(ComparePlayer));
            for (int i = 0; i < Players.Count && i < 10; i++)
                sb.Append($"{i + 1}. " + Players[i].ToString());
            LeaderBoard.GetComponent<TextMeshProUGUI>().text = sb.ToString();
        }
    }
    public void SetName()
    {
        ThisPlayer = new Player(0, TextField.GetComponent<TextMeshProUGUI>().text);
        Players.Add(ThisPlayer);
    }
    public void LoadGame()
    {
        SceneManager.LoadScene("MainGameScene", LoadSceneMode.Single);
    }
    public void QuitGame()
    {
        SaveFile(Application.dataPath + "/save.savefile");
        Application.Quit();
    }
    private void OpenFile(string dataPath)
    {
        FileStream fs = new FileStream(dataPath, FileMode.Open);
        IFormatter formatter = new BinaryFormatter();
        Players = (List<Player>)formatter.Deserialize(fs);
        fs.Close();
    }
    private void SaveFile(string dataPath)
    {
        FileStream fs = new FileStream(dataPath, FileMode.OpenOrCreate);
        IFormatter formatter = new BinaryFormatter();
        formatter.Serialize(fs, Players);
        fs.Close();
    }

    public void SetGameToEasy()
    {
        Scene.ChanceForBlack = 98;
        Scene.ChanceForPowerUp = 90;
        Scene.generateTime = 3;
    }
    public void SetGameToMedium()
    {
        Scene.ChanceForBlack = 95;
        Scene.ChanceForPowerUp = 95;
        Scene.generateTime = 2;
    }
    public void SetGameToHard()
    {
        Scene.ChanceForBlack = 80;
        Scene.ChanceForPowerUp = 98;
        Scene.generateTime = 1;
    }

    private static int ComparePlayer(Player p1, Player p2)
    {
        return p2.BestPoints.CompareTo(p1.BestPoints);
    }
}
[Serializable]
public class Player
{
    public int BestPoints { get; set; }
    public string Name { get; set; }
    public Player(int p, string n)
    {
        BestPoints = p;
        Name = n;
    }
    public override string ToString()
    {
        return $"{Name}-{BestPoints}\n";
    }
}
