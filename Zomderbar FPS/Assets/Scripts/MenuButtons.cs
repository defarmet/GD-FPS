using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuButtons : MonoBehaviour
{
    [SerializeField] private string sceneToLoad;
    public void LoadScene()
    {
        //SceneManager.LoadScene(sceneToLoad);
        SceneManager.LoadScene(sceneToLoad);
    }
}
