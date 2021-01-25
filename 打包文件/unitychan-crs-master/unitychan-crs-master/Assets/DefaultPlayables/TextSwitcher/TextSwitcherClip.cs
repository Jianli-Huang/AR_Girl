using System;
using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.Timeline;

[Serializable]
public class TextSwitcherClip : PlayableAsset, ITimelineClipAsset
{
    public ClipCaps clipCaps
    {
        get { return ClipCaps.Blending; }
    }

    public string text;

    public override Playable CreatePlayable(PlayableGraph graph, GameObject owner)
    {
        var playable = ScriptPlayable<TextSwitcherBehaviour>.Create(graph);
        var behaviour = playable.GetBehaviour();
        behaviour.text = text;
        return playable;
    }
}
