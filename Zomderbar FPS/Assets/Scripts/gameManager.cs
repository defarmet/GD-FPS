using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class gameManager : MonoBehaviour
{

	public static gameManager instance;
	public GameObject player;
	
	public GameObject pauseMenu;
	public GameObject currentMenu;

	bool pauseState = false;

	void Awake()
	{
		instance = this;
		player = GameObject.FindGameObjectWithTag("Player");
	}

	void Update()
	{
		if (Input.GetButtonDown("Cancel")) {
			currentMenu = pauseMenu;
			pause_game(!pauseState);
		}
	}

	public void pause_game(bool p)
	{
		Cursor.visible = p;
		pauseState = p;
		if (p) {
			Cursor.lockState = CursorLockMode.Confined;
			Time.timeScale = 0;
		} else {
			Cursor.lockState = CursorLockMode.Locked;
			Time.timeScale = 1;
		}
		currentMenu.SetActive(p);
	}
}
