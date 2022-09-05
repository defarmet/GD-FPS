using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class WinCondition : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter(Collider other)
    {
        gameManager.instance.winMenu.SetActive(true);
        gameManager.instance.currentMenuOpen = gameManager.instance.winMenu;
        gameManager.instance.cursorLockPause();

        //Menu Navigation
        //clear selected object first
        EventSystem.current.SetSelectedGameObject(null);

        //set new selected object
        EventSystem.current.SetSelectedGameObject(gameManager.instance.winFirstButton);
    }
}
