using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class checkpoint : MonoBehaviour
{
    TextMeshProUGUI text;

    void Start()
    {
        text = gameManager.instance.checkpointHUD.GetComponent<TextMeshProUGUI>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player")) {
            gameManager.instance.playerSpawnPoint.transform.position = transform.position;
            StartCoroutine(show_text());
        }
    }

    IEnumerator show_text()
    {
        text.alpha = 255;
        yield return new WaitForSeconds(3);
        text.alpha = 0;
    }
}
