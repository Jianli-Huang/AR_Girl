using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.Timeline;


[TrackColor(0, 1, 0)]
[TrackClipType(typeof(BlendShapeClip))]
[TrackBindingType(typeof(BlendShapeTargets))]
public class BlendShapeTrack : TrackAsset
{
    public override Playable CreateTrackMixer(PlayableGraph graph, GameObject go, int inputCount)
    {
        // Hack to set the display name of the clip
        foreach (var c in GetClips())
        {
            var clip = (BlendShapeClip)c.asset;
            if (clip.BlendShape != null)
            {
                c.displayName = clip.BlendShape.SetName;
            }
            else
            {
                c.displayName = "no";
            }
        }

        return ScriptPlayable<BlendShapeMixerBehaviour>.Create(graph, inputCount);
    }
}
