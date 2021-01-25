using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.Timeline;


public class BlendShapeBehaviour : PlayableBehaviour
{
    public BlendShapeSet BlendShape;
}


public class BlendShapeMixerBehaviour : PlayableBehaviour
{
    BlendShapeTargets m_trackBinding;

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
        //m_trackBinding.alpha = m_initialValue;
    }

    Dictionary<string, float> m_valueMap = new Dictionary<string, float>();

    public override void ProcessFrame(Playable playable, FrameData info, object playerData)
    {
        m_trackBinding = playerData as BlendShapeTargets;
        if (m_trackBinding == null)
        {
            return;
        }

        if (!m_firstFrameHappened)
        {
            // 初期値の記憶
            //m_initialValue = m_trackBinding.alpha;
            m_firstFrameHappened = true;
        }

        // 全clipのalpha値をweightに応じて合計する
        int inputCount = playable.GetInputCount();
        m_valueMap.Clear();
        for (int i = 0; i < inputCount; ++i)
        {
            var inputWeight = playable.GetInputWeight(i);
            var inputPlayable = (ScriptPlayable<BlendShapeBehaviour>)playable.GetInput(i);
            var input = inputPlayable.GetBehaviour();

            if (input.BlendShape != null)
            {
                foreach (var x in input.BlendShape.Values)
                {
                    float value = 0;
                    if (m_valueMap.TryGetValue(x.Key, out value))
                    {
                        //
                    }
                    m_valueMap[x.Key] = value + x.Value * inputWeight;
                }
            }
        }

        // 反映
        foreach(var x in m_trackBinding.Renderers)
        {
            var mesh = x.sharedMesh;
            var count = mesh.blendShapeCount;
            for(int i=0; i<count; ++i)
            {
                float value = 0;
                if(m_valueMap.TryGetValue(mesh.GetBlendShapeName(i), out value))
                {
                    x.SetBlendShapeWeight(i, value);
                }
                else
                {
                    x.SetBlendShapeWeight(i, 0);
                }
            }
        }
    }
}


[Serializable]
public class BlendShapeClip : PlayableAsset, ITimelineClipAsset
{
    public ClipCaps clipCaps
    {
        get { return ClipCaps.Blending; }
    }

    public BlendShapeSet BlendShape;

    public override Playable CreatePlayable(PlayableGraph graph, GameObject owner)
    {
        var playable = ScriptPlayable<BlendShapeBehaviour>.Create(graph);

        // Playableにクリップの設定を反映する
        var behaviour = playable.GetBehaviour();
        if (BlendShape != null)
        {
            behaviour.BlendShape = ScriptableObject.Instantiate<BlendShapeSet>(BlendShape);
        }

        return playable;
    }
}

