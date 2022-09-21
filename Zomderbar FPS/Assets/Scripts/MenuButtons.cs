using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuButtons : MonoBehaviour
{
    [SerializeField] string sceneToLoad;

    public void LoadScene()
    {
        SceneManager.LoadScene(sceneToLoad);
    }

    private void cursorunlock() //the only reason this was added was for an animator event to unlock the cursor after the infirmary level
    {
        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.Confined;
    }
}
