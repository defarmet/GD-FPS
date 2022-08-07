using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class buttonFunctions : MonoBehaviour
{
	public void resume()
	{
		gameManager.instance.pause_game(false);
	}

	public void restart()
	{
		resume();
		SceneManager.LoadScene(SceneManager.GetActiveScene().name);
		
	}

	public void quit()
	{
		Application.Quit();
	}
}
