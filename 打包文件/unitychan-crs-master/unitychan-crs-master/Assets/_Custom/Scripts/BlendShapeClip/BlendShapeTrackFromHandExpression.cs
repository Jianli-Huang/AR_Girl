#if UNITY_EDITOR
using UnityEditor;
#endif
using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.Timeline;


public class BlendShapeTrackFromHandExpression : MonoBehaviour
{
#if UNITY_EDITOR
    [SerializeField]
    AnimationClip m_clip;

    [SerializeField]
    TimelineAsset m_timeline;

    static T GetOrCreateTrack<T>(TimelineAsset tl, string name) where T : TrackAsset
    {
        foreach (var t in tl.GetOutputTracks())
        {
            if (t.name == name)
            {
                return t as T;
            }
        }

        Debug.LogFormat("CreateTrack: {0}", name);
        return tl.CreateTrack(typeof(T), null, name) as T;
    }

    static T CreatePlayableAsset<T>(TimelineAsset timelineAsset, string playableAssetName) where T : PlayableAsset
    {
        var playableAsset = (T)ScriptableObject.CreateInstance(typeof(T));
        playableAsset.name = playableAssetName;
        playableAsset.hideFlags = HideFlags.HideInHierarchy;
        AssetDatabase.AddObjectToAsset(playableAsset, timelineAsset);
        return playableAsset;
    }

    void Start()
    {
        var track = GetOrCreateTrack<BlendShapeTrack>(m_timeline, "blendshape");
        Debug.Log(track);

        // clear clips
        foreach (var c in track.GetClips())
        {
            m_timeline.DeleteClip(c);
        }

        var events = AnimationUtility.GetAnimationEvents(m_clip);
        for(int i=0; i<events.Length; ++i)
        {
            var x = events[i];
            Debug.LogFormat("{0} => {1}({2})", x.time, x.functionName, x.stringParameter);
            var playable = CreatePlayableAsset<BlendShapeClip>(m_timeline, x.stringParameter);

            playable.BlendShape = AssetDatabase.LoadAssetAtPath<BlendShapeSet>("Assets/_Custom/BlendShapeSet/"+x.stringParameter+".asset");

            var clip = track.CreateDefaultClip();
            clip.asset = playable;
            clip.start = x.time;
            if (i != events.Length - 1)
            {
                clip.duration = events[i+1].time - x.time;
            }
            else
            {
                clip.duration = m_clip.length - x.time;
            }
            clip.displayName = x.stringParameter;
        }

        AssetDatabase.SaveAssets();
    }
#endif
}
