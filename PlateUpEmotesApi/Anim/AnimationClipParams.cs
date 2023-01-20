using UnityEngine;

namespace PlateUpEmotesApi.Anim;

public class AnimationClipParams
{
    public AnimationClip[] animationClip;
    public bool looping;
    public List<AudioClip> audioClips = new List<AudioClip>();
    public HumanBodyBones[]? rootBonesToIgnore = null;
    public HumanBodyBones[]? soloBonesToIgnore = null;
    public AnimationClip[]? secondaryAnimation = null;
    public bool visible = true;
    public bool syncAnim = false;
    public bool syncAudio = false;
    public int startPref = -1;
    public int joinPref = -1;
    public bool dimAudio = false;
    public JoinSpot[]? joinSpots = null;
    public string customName = "NO_CUSTOM_NAME";
    public Action<BoneMapper>? customPostEventCodeSync = null;
    public Action<BoneMapper>? customPostEventCodeNoSync = null;
}