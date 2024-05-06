using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Title : MonoBehaviour
{
    private void Start()
    {
        Cursor.visible = true;
    }
    public void StartGame()
    {
        PlayerPrefs.SetInt("PlayerHP", 120);
        SceneManager.LoadScene("WorldMap");

    }

    public void Quit()
    {
        Application.Quit();
    }
}
