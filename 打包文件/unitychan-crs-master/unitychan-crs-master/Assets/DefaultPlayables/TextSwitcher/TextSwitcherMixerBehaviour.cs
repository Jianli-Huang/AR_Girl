using System;
using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.Timeline;
using UnityEngine.UI;

public class TextSwitcherMixerBehaviour : PlayableBehaviour
{
    string m_DefaultText;

    Text m_TrackBinding;
    bool m_FirstFrameHappened;

    public override void ProcessFrame(Playable playable, FrameData info, object playerData)
    {
        m_TrackBinding = playerData as Text;

        if (m_TrackBinding == null)
            return;

        if (!m_FirstFrameHappened)
        {
            m_DefaultText = m_TrackBinding.text;
            m_FirstFrameHappened = true;
        }

        int inputCount = playable.GetInputCount ();

        float totalWeight = 0f;
        float greatestWeight = 0f;
        int currentInputs = 0;

        for (int i = 0; i < inputCount; i++)
        {
            float inputWeight = playable.GetInputWeight(i);
            ScriptPlayable<TextSwitcherBehaviour> inputPlayable = (ScriptPlayable<TextSwitcherBehaviour>)playable.GetInput(i);
            TextSwitcherBehaviour input = inputPlayable.GetBehaviour ();
            
            totalWeight += inputWeight;

            if (inputWeight > greatestWeight)
            {
                m_TrackBinding.text = input.text;
                greatestWeight = inputWeight;
            }

            if (!Mathf.Approximately (inputWeight, 0f))
                currentInputs++;
        }

        if (currentInputs != 1 && 1f - totalWeight > greatestWeight)
        {
            m_TrackBinding.text = m_DefaultText;
        }
    }

    public override void OnGraphStop (Playable playable)
    {
        if (m_TrackBinding != null)
        {
            m_TrackBinding.text = m_DefaultText;
        }
        m_FirstFrameHappened = false;
    }
}
