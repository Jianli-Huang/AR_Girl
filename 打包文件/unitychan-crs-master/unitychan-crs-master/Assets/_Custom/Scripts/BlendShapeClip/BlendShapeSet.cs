using System;
using System.Collections.Generic;
using UnityEngine;


[Serializable]
public class BlendShapeKey
{
    [SerializeField]
    public string Key;

    [SerializeField]
    public float Value;

    public BlendShapeKey(string key, float value)
    {
        Key = key;
        Value = value;
    }
}


[Serializable]
public class BlendShapeSet : ScriptableObject
{
    [SerializeField]
    public string SetName;

    [SerializeField]
    public List<BlendShapeKey> Values = new List<BlendShapeKey>();
}
