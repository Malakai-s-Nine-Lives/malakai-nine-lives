using UnityEngine;
using UnityEngine.Playables;

[RequireComponent(typeof(PlayableDirector))]
public class Timescale : MonoBehaviour
{
    public float timeScale = 1.0f;

    void Awake()
    {
        var director = GetComponent<PlayableDirector>();
        if (director != null)
        {
            director.played += SetSpeed;
            SetSpeed(director); // in case play on awake is set
        }
    }

    void SetSpeed(PlayableDirector director)
    {
        if (director != null && director.playableGraph.IsValid())
        {
            director.playableGraph.GetRootPlayable(0).SetSpeed(timeScale);
        }
    }

    void OnValidate()
    {
        SetSpeed(GetComponent<PlayableDirector>());
    }
}
