using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.SceneManagement;

public class LoadScene : MonoBehaviour
{
    // Start is called before the first frame update
    public void LoadMainGamePlay()
    {
        Debug.Log("Load");
        SceneManager.LoadScene("GamePlay", LoadSceneMode.Single);
    }

    public void skipButton()
    {
        PlayableDirector playableDirector = GetComponent<PlayableDirector>();
        playableDirector.time = playableDirector.playableAsset.duration;
    }
}
