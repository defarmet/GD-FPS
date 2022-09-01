using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class checkpoint : MonoBehaviour
{
    [SerializeField] int             textSpeed;
                     TextMeshProUGUI text;
                     bool            showText;

    void Start()
    {
        text = gameManager.instance.checkpointHUD.GetComponent<TextMeshProUGUI>();
    }

    void Update()
    {
        float speed = Time.deltaTime;
        if (showText) {
            if (255 - text.alpha > speed)
                text.alpha += speed;
            else
                text.alpha = 255;
        } else {
            if (text.alpha > speed)
                text.alpha -= speed;
            else
                text.alpha = 0;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            gameManager.instance.playerSpawnPoint.transform.position = transform.position;
            StartCoroutine(show_text());
        }
    }

    IEnumerator show_text()
    {
        showText = true;
        yield return new WaitForSeconds(3);
        showText = false;
    }
}
