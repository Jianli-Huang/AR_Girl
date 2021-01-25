using UnityEngine;
using UnityEngine.Timeline;


public class MoveTarget : MonoBehaviour, ITimeControl
{
    [SerializeField]
    Cinemachine.CinemachineVirtualCamera m_vm;

    [SerializeField]
    float m_duration = 1.0f;

    #region Pan
    [SerializeField]
    GameObject m_panTargeet;

    public enum PanType
    {
        Fix,
        PanLeft,
        PanRight,
        PanUp,
        PanDown,
    }

    [SerializeField]
    PanType m_type;

    [SerializeField]
    float m_length = 1.0f;

    Vector3 m_start;
    Vector3 m_end;
    #endregion

    public void SetTime(double time)
    {
        transform.position = Vector3.Lerp(m_start, m_end, (float)(time / m_duration));
    }

    public void OnControlTimeStart()
    {
        switch (m_type)
        {
            case PanType.Fix:
                m_start = m_end = m_panTargeet.transform.position;
                break;

            case PanType.PanLeft:
                {
                    var xaxis = Vector3.Cross(m_vm.transform.forward, Vector3.up);
                    var half = xaxis * m_length / 2;
                    m_start = m_panTargeet.transform.position + half;
                    m_end = m_panTargeet.transform.position - half;
                }
                break;
        }
    }

    public void OnControlTimeStop()
    {
    }
}
