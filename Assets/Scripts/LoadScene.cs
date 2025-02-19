using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.SceneManagement;

public class LoadScene : MonoBehaviour
{
    public PlayableDirector playableDirector;

    void Start()
    {
        playableDirector.stopped += OnPlayableDirectorStopped;
    }

    void OnPlayableDirectorStopped(PlayableDirector director)
    {
        if (playableDirector == director)
        {
            SceneManager.LoadScene(1);
        }
    }
}
