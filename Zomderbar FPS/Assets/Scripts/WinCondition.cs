using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;

public class WinCondition : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if(SceneManager.GetActiveScene().buildIndex + 1 < SceneManager.sceneCountInBuildSettings)
        {
            gameManager.instance.winMenu.SetActive(true);
            gameManager.instance.currentMenuOpen = gameManager.instance.winMenu;
            gameManager.instance.cursorLockPause();

            /*
             * Menu Navigation
             * Clear selected object first.
             */
            EventSystem.current.SetSelectedGameObject(null);

            /*
             * Set new selected object
             */
            EventSystem.current.SetSelectedGameObject(gameManager.instance.winFirstButton);
        }
        else
        {
            gameManager.instance.gameCompleteMenu.SetActive(true);
            gameManager.instance.currentMenuOpen = gameManager.instance.gameCompleteMenu;
            gameManager.instance.cursorLockPause();

            /*
             * Menu Navigation
             * Clear selected object first.
             */
            EventSystem.current.SetSelectedGameObject(null);

            /*
             * Set new selected object
             */
            EventSystem.current.SetSelectedGameObject(gameManager.instance.completeFirstButton);
        }
        
    }
}
