using System;
using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.Timeline;


public class AlphaBehaviour : PlayableBehaviour
{
    public float Alpha;
}


public class AlphaMixerBehaviour : PlayableBehaviour
{
    CanvasGroup m_trackBinding;

    bool m_firstFrameHappened;
    float m_initialValue;

    public override void OnGraphStop(Playable playable)
    {
        m_firstFrameHappened = false;
        if (m_trackBinding == null)
        {
            return;
        }
        // 初期値の復旧
        m_trackBinding.alpha = m_initialValue;
    }

    public override void ProcessFrame(Playable playable, FrameData info, object playerData)
    {
        m_trackBinding = playerData as CanvasGroup;
        if (m_trackBinding == null)
        {
            return;
        }

        if (!m_firstFrameHappened)
        {
            // 初期値の記憶
            m_initialValue = m_trackBinding.alpha;
            m_firstFrameHappened = true;
        }

        // 全clipのalpha値をweightに応じて合計する
        int inputCount = playable.GetInputCount();
        float alpha = 0;
        for (int i = 0; i < inputCount; ++i)
        {
            var inputWeight = playable.GetInputWeight(i);
            var inputPlayable = (ScriptPlayable<AlphaBehaviour>)playable.GetInput(i);
            var input = inputPlayable.GetBehaviour();
            alpha += input.Alpha * inputWeight;
        }

        // 反映
        m_trackBinding.alpha = alpha;
    }
}


[Serializable]
public class AlphaClip : PlayableAsset, ITimelineClipAsset
{
    public ClipCaps clipCaps
    {
        get { return ClipCaps.Blending; }
    }
   
    [SerializeField, Range(0, 1)]
    public float Alpha;

    public override Playable CreatePlayable(PlayableGraph graph, GameObject owner)
    {
        var playable =ScriptPlayable<AlphaBehaviour>.Create(graph);

        // Playableにクリップの設定を反映する
        var behaviour = playable.GetBehaviour();
        behaviour.Alpha = Alpha;

        return playable;
    }
}



