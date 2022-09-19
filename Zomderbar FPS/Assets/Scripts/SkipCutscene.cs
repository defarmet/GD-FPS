using System.Collections;
using System.Collections.Generic;
using UnityEngine.Playables;
using UnityEngine;

public class SkipCutscene : MonoBehaviour
{
    private PlayableDirector director;


    // Start is called before the first frame update
    void Start()
    {
        director = GetComponent<PlayableDirector>();
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetKeyDown(KeyCode.F))
        {
            SkipCinematic();
        }
    }

    void SkipCinematic()
    {
        director.time = director.playableAsset.duration;
        Destroy(this);
    }
}
