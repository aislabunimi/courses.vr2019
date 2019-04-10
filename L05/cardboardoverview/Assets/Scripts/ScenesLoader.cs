using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ScenesLoader : MonoBehaviour
{

    public void GoToMainMenu()
    {
        SceneManager.LoadScene("Menu");
    }

    public void LoadScene(string id)
    {
        SceneManager.LoadScene("Step" + id);
    }
}
