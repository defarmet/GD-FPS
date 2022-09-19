using System.Collections;
using System.Collections.Generic;
using UnityEngine.Playables;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Assertions;

public class SkipCutscene : MonoBehaviour
{
    private PlayableDirector director;
    public Image fillImage;
    public GameObject pressKeyCanvas;
    //public RectTransform holderRect;
    //Vector2 startHolderScale;

    //public Vector2 targetHolderScale = new Vector2(1.2f, 1.2f);

    public float activeSpeed = .5f;
    float coldownSpeed = 0.6f;
    bool functionTriggered = false;

    // Start is called before the first frame update
    void Awake()
    {
        director = GetComponent<PlayableDirector>();
        Assert.IsNotNull(fillImage);
        //Assert.IsNotNull(holderRect);

        //startHolderScale = holderRect.localScale;
        fillImage.fillAmount = 0;
    }

    // Update is called once per frame
    void Update()
    {
        fillImage.fillAmount += (Input.GetKey(KeyCode.F) && !functionTriggered ? activeSpeed : -coldownSpeed) * Time.deltaTime;
        if(!functionTriggered && fillImage.fillAmount == 1)
        {
            functionTriggered = true;
            SkipCinematic();
            Destroy(pressKeyCanvas);
        }
    }

    void SkipCinematic()
    {
        director.time = director.playableAsset.duration;
        Destroy(this);
    }
}
