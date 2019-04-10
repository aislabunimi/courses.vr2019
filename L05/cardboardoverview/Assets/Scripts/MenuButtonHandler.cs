using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuButtonHandler : MonoBehaviour
{
    bool pointerInside = false;
    public string sceneToLoad = "";

    // Update is called once per frame
    void Update()
    {
        if(pointerInside && Input.GetButtonDown("Fire1"))
        {
            SceneManager.LoadScene(sceneToLoad);
        }
    }

    // Called everytime user look at target
    public void OnPointerEnter()
    {
        pointerInside = true;
    }

    // Called everytime user stop looking at target
    public void OnPointerExit()
    {
        pointerInside = false;
    }
}
