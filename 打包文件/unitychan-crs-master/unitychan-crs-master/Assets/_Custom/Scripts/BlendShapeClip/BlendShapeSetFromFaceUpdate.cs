using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityChan;
using UnityEditor;
using UnityEngine;


public class BlendShapeSetFromFaceUpdate : MonoBehaviour
{
    [SerializeField]
    FaceUpdate m_faceUpdate;

    private void Reset()
    {
        m_faceUpdate = GetComponent<FaceUpdate>();
    }

    const string OUT_DIR = "Assets/_Custom/BlendShapeSet/";

#if UNITY_EDITOR
    static void CreateBlendShapeSet(AnimationClip clip)
    {
        var blendShapeSet = ScriptableObject.CreateInstance<BlendShapeSet>();
        blendShapeSet.SetName = clip.name;
        foreach (var binding in AnimationUtility.GetCurveBindings(clip))
        {
            var curve = AnimationUtility.GetEditorCurve(clip, binding);
            Debug.LogFormat("{0}: \"{1}\" {2}keys", binding.path, binding.propertyName, curve.keys.Length);

            if (curve.keys.Length > 0)
            {
                var key = curve.keys[0];
                /*
                var set = ScriptableObject.CreateInstance<BlendShapeKey>();
                set.Key = binding.propertyName;
                set.Value = key.value;
                blendShapeSet.Values.Add(set);
                */
                const string PREFIX = "blendShape.";
                if (binding.propertyName.StartsWith(PREFIX))
                {
                    blendShapeSet.Values.Add(new BlendShapeKey(binding.propertyName.Substring(PREFIX.Length), key.value));
                }
            }
        }

        if(!File.Exists(OUT_DIR))
        {
            Directory.CreateDirectory(OUT_DIR);
        }      
        AssetDatabase.CreateAsset(blendShapeSet, OUT_DIR+clip.name+".asset");
        AssetDatabase.Refresh();
    }

    private void Start()
    {
        foreach(var x in m_faceUpdate.animations)
        {
            CreateBlendShapeSet(x);
        }
    }
#endif
}
