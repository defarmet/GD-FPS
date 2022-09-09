using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class buttonFunctions : MonoBehaviour
{
    public void resume()
    {
        if (gameManager.instance.isPaused) {
            gameManager.instance.isPaused = !gameManager.instance.isPaused;
            gameManager.instance.cursorUnlockUnpause();
        }
    }

    public void restart()
    {
        gameManager.instance.cursorUnlockUnpause();
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    public void Respawn()
    {
        gameManager.instance.playerScript.resetHP();
        gameManager.instance.playerScript.respawn();
        gameManager.instance.cursorUnlockUnpause();
    }

    public void quit()
    {
        Application.Quit();
    }

    public void save()
    {
        gameManager.instance.close_settings();
    }

    public void settings()
    {
        gameManager.instance.open_settings();
    }
    
    public void playGame()
    {
        gameManager.statsStart = new List<gunStats>(gameManager.instance.playerScript.gunstat);
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
        gameManager.instance.cursorUnlockUnpause();
    }
   
    public void playShowcase()
    {
        SceneManager.LoadScene("ShowcaseScene");
    }
    
    public void quitToMenu()
    {
        SceneManager.LoadScene("MainMenu");
        Time.timeScale = 1.0f;
    }
}
