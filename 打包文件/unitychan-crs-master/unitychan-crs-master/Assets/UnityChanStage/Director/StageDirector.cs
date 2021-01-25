using UnityEngine;


public class StageDirector : MonoBehaviour
{
    #region AnimationClip Events
    public void StartMusic()
    {
        Debug.Log("StartMusic");
    }

    public void ActivateProps()
    {
        Debug.Log("ActivateProps");
    }

    public void SwitchCamera(int index)
    {
        Debug.LogFormat("SwitchCamera: {0}", index);
    }

    public void StartAutoCameraChange()
    {
        Debug.Log("StartAutoCameraChange");
    }

    public void StopAutoCameraChange()
    {
        Debug.Log("StopAutoCameraChange");
    }

    public void EndPerformance()
    {
        Debug.Log("EndPerformance");
    }
    #endregion
}
