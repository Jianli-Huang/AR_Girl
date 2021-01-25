//
// Reaktion - An audio reactive animation toolkit for Unity.
//
// Copyright (C) 2013, 2014 Keijiro Takahashi
//
// Permission is hereby granted, free of charge, to any person obtaining a copy of
// this software and associated documentation files (the "Software"), to deal in
// the Software without restriction, including without limitation the rights to
// use, copy, modify, merge, publish, distribute, sublicense, and/or sell copies of
// the Software, and to permit persons to whom the Software is furnished to do so,
// subject to the following conditions:
//
// The above copyright notice and this permission notice shall be included in all
// copies or substantial portions of the Software.
//
// THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
// IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY, FITNESS
// FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE AUTHORS OR
// COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER LIABILITY, WHETHER
// IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM, OUT OF OR IN
// CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE SOFTWARE.
//
using UnityEngine;
using UnityEngine.Timeline;

namespace Reaktion
{

    public class ConstantMotion : MonoBehaviour, ITimeControl
    {
        public enum TransformMode
        {
            Off, XAxis, YAxis, ZAxis, Arbitrary, Random
        };

        // A class for handling each transformation.
        [System.Serializable]
        public class TransformElement
        {
            public TransformMode mode = TransformMode.Off;
            public float velocity = 1;

            // Used only in the arbitrary mode.
            public Vector3 arbitraryVector = Vector3.up;

            // Affects velocity.
            public float randomness = 0;

            // Randomizer states.
            Vector3 randomVector;
            float randomScalar;

            public void Initialize()
            {
                randomVector = Random.onUnitSphere;
                randomScalar = Random.value;
            }

            // Get a vector corresponds to the current transform mode.
            public Vector3 Vector
            {
                get
                {
                    switch (mode)
                    {
                        case TransformMode.XAxis: return Vector3.right;
                        case TransformMode.YAxis: return Vector3.up;
                        case TransformMode.ZAxis: return Vector3.forward;
                        case TransformMode.Arbitrary: return arbitraryVector;
                        case TransformMode.Random: return randomVector;
                    }
                    return Vector3.zero;
                }
            }

            public float TimeDelta
            {
                get;
                set;
            }

            // Get the current delta value.
            public float Delta
            {
                get
                {
                    var scale = (1.0f - randomness * randomScalar);
                    return velocity * scale * TimeDelta;
                }
            }
        }

        public TransformElement position = new TransformElement();
        public TransformElement rotation = new TransformElement { velocity = 30 };
        public bool useLocalCoordinate = true;

        void Awake()
        {
            position.Initialize();
            rotation.Initialize();
        }

        double m_lastTime;

        public void SetTime(double time)
        {
            if (m_lastTime != 0)
            {
                if (time == m_lastTime)
                {
                    return;
                }
                var d = (float)(time - m_lastTime);
                position.TimeDelta = d;
                rotation.TimeDelta = d;

                if (position.mode != TransformMode.Off)
                {
                    if (useLocalCoordinate)
                        transform.localPosition += position.Vector * position.Delta;
                    else
                        transform.position += position.Vector * position.Delta;
                }

                if (rotation.mode != TransformMode.Off)
                {
                    var delta = Quaternion.AngleAxis(rotation.Delta, rotation.Vector);
                    if (useLocalCoordinate)
                        transform.localRotation = delta * transform.localRotation;
                    else
                        transform.rotation = delta * transform.rotation;
                }
            }
            m_lastTime = time;
        }

        public void OnControlTimeStart()
        {
            //throw new System.NotImplementedException();
        }

        public void OnControlTimeStop()
        {
            //throw new System.NotImplementedException();
        }
    }

} // namespace Reaktion
