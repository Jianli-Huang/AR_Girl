using UnityEngine;


public class IKUpdater : MonoBehaviour
{

    [SerializeField]
    Animator m_animator;

    [SerializeField]
    Transform m_lookAt;

    [SerializeField, Range(0, 1)]
    float m_lookAtWeight;

    private void Reset()
    {
        m_animator = GetComponent<Animator>();
        m_lookAt = Camera.main.transform;
    }

    private void OnAnimatorIK(int layerIndex)
    {
        Debug.Log("OnAnimatorIK");
        m_animator.SetLookAtWeight(m_lookAtWeight);
        m_animator.SetLookAtPosition(m_lookAt.position);
    }
}
