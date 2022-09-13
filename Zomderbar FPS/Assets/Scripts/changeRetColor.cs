using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class changeRetColor : MonoBehaviour
{
    public Camera cam;
    public float  shootdist;
    public Color  origColor = Color.black;
    public Image  reticle;

    private void Awake()
    {
        cam = Camera.main;
    }

    void Update()
    {
        reticle = gameManager.instance.currentGunHUD.transform.GetChild(2).gameObject.GetComponent<Image>();
        if (origColor == Color.black)
            origColor = reticle.GetComponent<Image>().color;

        shootdist = gameManager.instance.player.GetComponent<playerController>().shootDistance;        

        if (Physics.Raycast(cam.ViewportPointToRay(new Vector2(0.5f, 0.5f)), out RaycastHit hit, shootdist)) {
            if (hit.collider.GetComponent<IDamageable>() != null) {
                IDamageable isDamageable = hit.collider.GetComponent<IDamageable>();
                reticle.color = Color.white;
            } else {
                reticle.color = origColor;
            }
        }
    }
}
