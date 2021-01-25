#if UNITY_EDITOR
using UnityEditor;
#endif
using System;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.Timeline;


public class TextSwitcherTrackUtil : MonoBehaviour
{
#if UNITY_EDITOR
    [SerializeField]
    TextAsset m_src;

    [SerializeField]
    string trackname = "Lyrics";

    [SerializeField]
    TimelineAsset m_dst;

    T GetOrCreateTrack<T>(TimelineAsset tl, string name) where T: TrackAsset
    {
        foreach(var t in tl.GetOutputTracks())
        {
            if (t.name == name)
            {
                return t as T;
            }
        }

        Debug.LogFormat("CreateTrack: {0}", name);
        return tl.CreateTrack(typeof(T), null, name) as T;
    }


    class Lyrics
    {
        public TimeSpan Start;
        public TimeSpan Duration;
        public string Text="";

        public Lyrics()
        {

        }

        public Lyrics(string line)
        {
            //Debug.Log(line);
            var colon = line.IndexOf(':');
            var minutes = line.Substring(1, colon - 1);
            var end = line.IndexOf(']');
            var seconds = line.Substring(colon + 1, end - colon - 1);
            //Debug.LogFormat("{0}, {1}", minutes, seconds);
            var time = TimeSpan.FromMinutes(int.Parse(minutes)) + TimeSpan.FromSeconds(int.Parse(seconds));
            Start = time;
            Text = line.Substring(end + 1);
        }

        public override string ToString()
        {
            return String.Format("[{0}][{1}]{2}", Start, Duration, Text);
        }

        public bool AddLine(string line)
        {
            if (string.IsNullOrEmpty(line))
            {
                return false;
            }
            if (line[0] == '[')
            {
                var colon = line.IndexOf(':');
                var minutes = line.Substring(1, colon-1);
                var end = line.IndexOf(']');
                var seconds = line.Substring(colon + 1, end - colon-1);
                //Debug.LogFormat("{0}, {1}", minutes, seconds);
                var time = TimeSpan.FromMinutes(int.Parse(minutes)) +  TimeSpan.FromSeconds(int.Parse(seconds));

                Duration = time - Start;

                return true;
            }
            else
            {
                Text += line;
                return false;
            }
        }
    }

    IEnumerable<Lyrics> ParseLyrics(string lyrics)
    {
        Lyrics current = new Lyrics();
        using (var r = new StringReader(lyrics))
        {
            while (true)
            {
                var line = r.ReadLine();
                if (line == null)
                {
                    break;
                }

                if (current.AddLine(line))
                {
                    if (!string.IsNullOrEmpty(current.Text))
                    {
                        yield return current;
                    }
                    current = new Lyrics(line);
                }
            }
        }
        if (!string.IsNullOrEmpty(current.Text))
        {
            yield return current;
        }
    }

    static T CreatePlayableAsset<T>(TimelineAsset timelineAsset, string playableAssetName) where T : PlayableAsset
    {
        var playableAsset = (T)ScriptableObject.CreateInstance(typeof(T));
        playableAsset.name = playableAssetName;
        playableAsset.hideFlags = HideFlags.HideInHierarchy;
        AssetDatabase.AddObjectToAsset(playableAsset, timelineAsset);
        return playableAsset;
    }

    private void Start()
    {
        var track = GetOrCreateTrack<TextSwitcherTrack>(m_dst, trackname);
        Debug.Log(track);

        // clear clips
        foreach(var c in track.GetClips())
        {
            m_dst.DeleteClip(c);
        }

        //var graph = PlayableGraph.Create();

        foreach (var clip in ParseLyrics(m_src.text))
        {
            Debug.LogFormat("{0}", clip);

            //continue;
            var textSwitcher = CreatePlayableAsset<TextSwitcherClip>(m_dst, clip.Text);

            textSwitcher.text = clip.Text;

            var timelineClip = track.CreateDefaultClip();
            timelineClip.asset = textSwitcher;
            timelineClip.start = clip.Start.TotalSeconds;
            timelineClip.duration = clip.Duration.TotalSeconds;
            timelineClip.displayName = clip.Text;
            timelineClip.asset = textSwitcher;
        }
        AssetDatabase.SaveAssets();
    }
#endif
}
