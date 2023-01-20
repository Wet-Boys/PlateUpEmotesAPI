using UnityEngine;

namespace PlateUpEmotesApi.Anim;

public class CustomAnimationClip
{
    public AnimationClip[] clip; 
    public AnimationClip[]? secondaryClip; 
    internal bool looping;
    internal List<AudioClip> audioClips= new List<AudioClip>();
    internal List<HumanBodyBones> soloIgnoredBones;
    internal List<HumanBodyBones> rootIgnoredBones;
    internal bool visibility;
    public int startPref, joinPref;
    public JoinSpot[]? joinSpots;
    public string customName;
    public Action<BoneMapper>? customPostEventCodeSync;
    public Action<BoneMapper> customPostEventCodeNoSync;


    internal bool syncronizeAnimation;
    internal bool syncronizeAudio;
    public int syncPos;
    public static List<float> syncTimer = new List<float>();
    public static List<int> syncPlayerCount = new List<int>();
    public static List<List<bool>> uniqueAnimations = new List<List<bool>>();
    public bool dimAudio;

    internal CustomAnimationClip(
        AnimationClip[] _clip, 
        bool _loop, 
        HumanBodyBones[]? rootBonesToIgnore = null, 
        HumanBodyBones[]? soloBonesToIgnore = null, 
        AnimationClip[]? _secondaryClip = null, 
        bool visible = true, 
        bool syncAnim = false, 
        bool syncAudio = false,
        int startPreference = -1, 
        int joinPreference = -1, 
        JoinSpot[]? _joinSpots = null, 
        string customName = "NO_CUSTOM_NAME", 
        Action<BoneMapper>? _customPostEventCodeSync = null, 
        Action<BoneMapper> _customPostEventCodeNoSync = null, 
        List<AudioClip> audioClips = null,
        bool dimAudio = false)
    {
        if (rootBonesToIgnore == null)
            rootBonesToIgnore = new HumanBodyBones[0];
        if (soloBonesToIgnore == null)
            soloBonesToIgnore = new HumanBodyBones[0];
        clip = _clip;
        secondaryClip = _secondaryClip;
        looping = _loop;
        visibility = visible;
        joinPref = joinPreference;
        startPref = startPreference;
        customPostEventCodeSync = _customPostEventCodeSync;
        customPostEventCodeNoSync = _customPostEventCodeNoSync;
        //int count = 0;
        //float timer = 0;
        //if (_wwiseEventName != "" && _wwiseStopEvent == "")
        //{
        //    //Debug.Log($"EmoteAPI: Error #2: wwiseEventName is declared but wwiseStopEvent isn't skipping sound implementation for [{clip.name}]");
        //}
        //else if (_wwiseEventName == "" && _wwiseStopEvent != "")
        //{
        //    //Debug.Log($"EmoteAPI: Error #3: wwiseStopEvent is declared but wwiseEventName isn't skipping sound implementation for [{clip.name}]");
        //}
        //else if (_wwiseEventName != "")
        //{
        //    //if (!_shouldSyncronize)
        //    //{
        //    BoneMapper.stopEvents.Add(_wwiseStopEvent);
        //    //}
        //    wwiseEvent = _wwiseEventName;
        //}
        this.audioClips = audioClips;
        if (audioClips == null)
        {
            audioClips = new List<AudioClip>();
        }
        BoneMapper.audioSources.Add(new AudioSource());
        BoneMapper.startEvents.Add(audioClips.ToArray());
        if (soloBonesToIgnore.Length != 0)
        {
            soloIgnoredBones = new List<HumanBodyBones>(soloBonesToIgnore);
        }
        else
        {
            soloIgnoredBones = new List<HumanBodyBones>();
        }

        if (rootBonesToIgnore.Length != 0)
        {
            rootIgnoredBones = new List<HumanBodyBones>(rootBonesToIgnore);
        }
        else
        {
            rootIgnoredBones = new List<HumanBodyBones>();
        }
        syncronizeAnimation = syncAnim;
        syncronizeAudio = syncAudio;
        syncPos = syncTimer.Count;
        syncTimer.Add(0);
        syncPlayerCount.Add(0);
        List<bool> bools = new List<bool>();
        for (int i = 0; i < _clip.Length; i++)
        {
            bools.Add(false);
        }
        uniqueAnimations.Add(bools);

        if (_joinSpots == null)
            _joinSpots = new JoinSpot[0];
        joinSpots = _joinSpots;
        this.customName = customName;
        this.dimAudio= dimAudio;
    }
}