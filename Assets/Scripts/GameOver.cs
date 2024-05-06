using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameOver : MonoBehaviour
{
    private void Start()
    {
        Cursor.visible = true;
    }
    public void Restart()
    {
        PlayerPrefs.SetInt("PlayerHP", 120);
        SceneManager.LoadScene("WorldMap");
    }

    public void Quit()
    {
        Application.Quit();
    }
}
