using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public class MainMenuScript : MonoBehaviour
{
    // Start is called before the first frame update
    private void Awake()
    {
        Transform obj = transform.Find("Points");
        if (obj != null)
        {
            obj.gameObject.GetComponent<TextMeshProUGUI>().text = $"Points: {Scene.Points}";
        }
    }
    public void LoadGame()
    {
        SceneManager.LoadScene("MainGameScene", LoadSceneMode.Single);
    }
    public void QuitGame()
    {
        Application.Quit();
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
}
