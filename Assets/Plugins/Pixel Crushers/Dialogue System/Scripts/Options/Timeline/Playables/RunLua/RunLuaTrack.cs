// Recompile at 2024/5/4 12:29:57
#if USE_TIMELINE
#if UNITY_2017_1_OR_NEWER
// Copyright (c) Pixel Crushers. All rights reserved.

using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.Timeline;

namespace PixelCrushers.DialogueSystem
{

    [TrackColor(0.855f, 0.8623f, 0.87f)]
    [TrackClipType(typeof(RunLuaClip))]
    [TrackBindingType(typeof(GameObject))]
    public class RunLuaTrack : TrackAsset
    {
        public override Playable CreateTrackMixer(PlayableGraph graph, GameObject go, int inputCount)
        {
            return ScriptPlayable<RunLuaMixerBehaviour>.Create(graph, inputCount);
        }
    }
}
#endif
#endif