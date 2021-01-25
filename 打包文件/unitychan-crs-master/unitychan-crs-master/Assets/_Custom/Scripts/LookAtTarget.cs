using System.Collections;
using System.Collections.Generic;
using UnityChan;
using UnityEngine;

public class LookAtTarget : MonoBehaviour
{
    [SerializeField]
    DeltaLookAtAxis[] m_eyeLocators;

    void LookAt(Transform eye, Transform target)
    {
        var dir = (target.position - eye.position).normalized;
        var d = Quaternion.FromToRotation(eye.forward, dir);
        eye.transform.rotation *= d;
    }

    void Update()
    {
        foreach (var x in m_eyeLocators)
        {
            LookAt(x.transform, transform);
        }
    }
}
