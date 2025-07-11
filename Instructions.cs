using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Instructions : MonoBehaviour
{
    void Start()
    {
        Cursor.visible = true;
    }
    public void PlayGame()
    {
        SceneManager.LoadSceneAsync(2);
    }
}
