using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.Timeline;

[TrackColor(0, 0, 0)]
[TrackClipType(typeof(AlphaClip))]
[TrackBindingType(typeof(CanvasGroup))]
public class AlphaTrack : TrackAsset
{
    public override Playable CreateTrackMixer(PlayableGraph graph, GameObject go, int inputCount)
    {
        // Hack to set the display name of the clip
        foreach (var c in GetClips())
        {
            var clip = (AlphaClip)c.asset;
            c.displayName = string.Format("{0:0.00}", clip.Alpha);
        }

        return ScriptPlayable<AlphaMixerBehaviour>.Create(graph, inputCount);
    }

    public override void GatherProperties(PlayableDirector director, IPropertyCollector driver)
    {
#if UNITY_EDITOR
        var trackBinding = director.GetGenericBinding(this) as CanvasGroup;
        if (trackBinding == null)
            return;

        var serializedObject = new UnityEditor.SerializedObject(trackBinding);
        var iterator = serializedObject.GetIterator();
        while (iterator.NextVisible(true))
        {
            if (iterator.hasVisibleChildren)
                continue;

            driver.AddFromName<CanvasGroup>(trackBinding.gameObject, iterator.propertyPath);
        }
#endif
        base.GatherProperties(director, driver);
    }
}
