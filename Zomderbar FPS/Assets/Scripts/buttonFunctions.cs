using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class buttonFunctions : MonoBehaviour
{
	public void resume()
	{
		if (gameManager.instance.isPaused)
        {
			gameManager.instance.isPaused = !gameManager.instance.isPaused;
			gameManager.instance.cursorUnlockUnpause();
		}
		
	}

	public void restart()
	{
		//resume();
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
}
