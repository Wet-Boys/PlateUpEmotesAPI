using PlateUpEmotesApi.Anim;
using PlateUpEmotesApi.Logging;
using Unity.Entities;
using UnityEngine;
using ILogger = PlateUpEmotesApi.Logging.Loggers.ILogger;

namespace PlateUpEmotesApi;

public static class PlateUpEmotesManager
{
    private static readonly ILogger Logger = LogUtils.CreateDefaultLogger("EmotesAPI");
    
    internal static List<string> allClipNames = new();
    internal static List<int> blacklistedClips = new();
    public static List<GameObject> audioContainers = new();
    public static List<GameObject> activeAudioContainers = new();

    #region Delegates

    public delegate void AnimationChangedDelegate(string newAnimation, BoneMapper mapper);

    #endregion

    #region Events

    public static event AnimationChangedDelegate? AnimationChanged;

    #endregion
    
    public static void ImportArmature(GameObject bodyPrefab, GameObject rigToAnimate, bool jank, int meshPos = 0, bool hideMeshes = true)
    {
        //rigToAnimate.GetComponent<Animator>().runtimeAnimatorController = GameObject.Instantiate<GameObject>(Assets.Load<GameObject>("@CustomEmotesAPI_customemotespackage:assets/animationreplacements/commando.prefab")).GetComponent<Animator>().runtimeAnimatorController;
        AnimationReplacements.ApplyAnimationStuff(bodyPrefab, rigToAnimate, meshPos, hideMeshes, jank);
    }
    public static void ImportArmature(GameObject bodyPrefab, GameObject rigToAnimate, int meshPos = 0, bool hideMeshes = true)
    {
        //rigToAnimate.GetComponent<Animator>().runtimeAnimatorController = GameObject.Instantiate<GameObject>(Assets.Load<GameObject>("@CustomEmotesAPI_customemotespackage:assets/animationreplacements/commando.prefab")).GetComponent<Animator>().runtimeAnimatorController;
        AnimationReplacements.ApplyAnimationStuff(bodyPrefab, rigToAnimate, meshPos, hideMeshes);
    }

    #region AddEmotes

    public static void AddNonAnimatingEmote(string emoteName, bool visible = true)
    {
        if (visible)
            allClipNames.Add(emoteName);
        BoneMapper.animClips.Add(emoteName, null);
    }
    
    public static void AddCustomAnimation(AnimationClipParams animationClipParams)
        {
            if (BoneMapper.animClips.ContainsKey(animationClipParams.animationClip[0].name))
            {
                Logger.Debug($"EmoteAPI: EmotesError: [{animationClipParams.animationClip[0].name}] is already defined as a custom emote but is trying to be added. Skipping");
                return;
            }
            if (!animationClipParams.animationClip[0].isHumanMotion)
            {
                Logger.Debug($"EmoteAPI: EmotesError: [{animationClipParams.animationClip[0].name}] is not a humanoid animation!");
                return;
            }
            if (animationClipParams.rootBonesToIgnore == null)
                animationClipParams.rootBonesToIgnore = Array.Empty<HumanBodyBones>();
            if (animationClipParams.soloBonesToIgnore == null)
                animationClipParams.soloBonesToIgnore = Array.Empty<HumanBodyBones>();
            if (animationClipParams._wwiseEventName == null)
                animationClipParams._wwiseEventName = new string[] { "" };
            if (animationClipParams._wwiseStopEvent == null)
                animationClipParams._wwiseStopEvent = new string[] { "" };
            if (animationClipParams.joinSpots == null)
                animationClipParams.joinSpots = Array.Empty<JoinSpot>();
            CustomAnimationClip clip = new CustomAnimationClip(animationClipParams.animationClip, animationClipParams.looping, animationClipParams._wwiseEventName, animationClipParams._wwiseStopEvent, animationClipParams.rootBonesToIgnore, animationClipParams.soloBonesToIgnore, animationClipParams.secondaryAnimation, animationClipParams.dimWhenClose, animationClipParams.stopWhenMove, animationClipParams.stopWhenAttack, animationClipParams.visible, animationClipParams.syncAnim, animationClipParams.syncAudio, animationClipParams.startPref, animationClipParams.joinPref, animationClipParams.joinSpots, animationClipParams.useSafePositionReset, animationClipParams.customName, animationClipParams.customPostEventCodeSync);
            if (animationClipParams.visible)
                allClipNames.Add(animationClipParams.animationClip[0].name);
            BoneMapper.animClips.Add(animationClipParams.animationClip[0].name, clip);
        }
    
        public static void AddCustomAnimation(AnimationClip[] animationClip, bool looping, string[]? _wwiseEventName = null, string[]? _wwiseStopEvent = null, HumanBodyBones[]? rootBonesToIgnore = null, HumanBodyBones[]? soloBonesToIgnore = null, AnimationClip[]? secondaryAnimation = null, bool dimWhenClose = false, bool stopWhenMove = false, bool stopWhenAttack = false, bool visible = true, bool syncAnim = false, bool syncAudio = false, int startPref = -1, int joinPref = -1, JoinSpot[]? joinSpots = null)
        {
            if (BoneMapper.animClips.ContainsKey(animationClip[0].name))
            {
                Logger.Debug($"EmoteAPI: EmotesError: [{animationClip[0].name}] is already defined as a custom emote but is trying to be added. Skipping");
                return;
            }
            if (!animationClip[0].isHumanMotion)
            {
                Logger.Debug($"EmoteAPI: EmotesError: [{animationClip[0].name}] is not a humanoid animation!");
                return;
            }
            if (rootBonesToIgnore == null)
                rootBonesToIgnore = Array.Empty<HumanBodyBones>();
            if (soloBonesToIgnore == null)
                soloBonesToIgnore = Array.Empty<HumanBodyBones>();
            if (_wwiseEventName == null)
                _wwiseEventName = new string[] { "" };
            if (_wwiseStopEvent == null)
                _wwiseStopEvent = new string[] { "" };
            if (joinSpots == null)
                joinSpots = Array.Empty<JoinSpot>();
            CustomAnimationClip clip = new CustomAnimationClip(animationClip, looping, _wwiseEventName, _wwiseStopEvent, rootBonesToIgnore, soloBonesToIgnore, secondaryAnimation, dimWhenClose, stopWhenMove, stopWhenAttack, visible, syncAnim, syncAudio, startPref, joinPref, joinSpots);
            if (visible)
                allClipNames.Add(animationClip[0].name);
            BoneMapper.animClips.Add(animationClip[0].name, clip);
        }
        
        public static void AddCustomAnimation(AnimationClip[] animationClip, bool looping, string _wwiseEventName = "", string _wwiseStopEvent = "", HumanBodyBones[]? rootBonesToIgnore = null, HumanBodyBones[]? soloBonesToIgnore = null, AnimationClip[]? secondaryAnimation = null, bool dimWhenClose = false, bool stopWhenMove = false, bool stopWhenAttack = false, bool visible = true, bool syncAnim = false, bool syncAudio = false, int startPref = -1, int joinPref = -1)
        {
            if (BoneMapper.animClips.ContainsKey(animationClip[0].name))
            {
                Logger.Debug($"EmoteAPI: EmotesError: [{animationClip[0].name}] is already defined as a custom emote but is trying to be added. Skipping");
                return;
            }
            if (!animationClip[0].isHumanMotion)
            {
                Logger.Debug($"EmoteAPI: EmotesError: [{animationClip[0].name}] is not a humanoid animation!");
                return;
            }
            if (rootBonesToIgnore == null)
                rootBonesToIgnore = Array.Empty<HumanBodyBones>();
            if (soloBonesToIgnore == null)
                soloBonesToIgnore = Array.Empty<HumanBodyBones>();
            string[] wwiseEvents = new string[] { _wwiseEventName };
            string[] wwiseStopEvents = new string[] { _wwiseStopEvent };
            CustomAnimationClip clip = new CustomAnimationClip(animationClip, looping, wwiseEvents, wwiseStopEvents, rootBonesToIgnore, soloBonesToIgnore, secondaryAnimation, dimWhenClose, stopWhenMove, stopWhenAttack, visible, syncAnim, syncAudio, startPref, joinPref);
            if (visible)
                allClipNames.Add(animationClip[0].name);
            BoneMapper.animClips.Add(animationClip[0].name, clip);
        }
        
        public static void AddCustomAnimation(AnimationClip animationClip, bool looping, string _wwiseEventName = "", string _wwiseStopEvent = "", HumanBodyBones[]? rootBonesToIgnore = null, HumanBodyBones[]? soloBonesToIgnore = null, AnimationClip secondaryAnimation = null, bool dimWhenClose = false, bool stopWhenMove = false, bool stopWhenAttack = false, bool visible = true, bool syncAnim = false, bool syncAudio = false)
        {
            if (BoneMapper.animClips.ContainsKey(animationClip.name))
            {
                Logger.Debug($"EmoteAPI: EmotesError: [{animationClip.name}] is already defined as a custom emote but is trying to be added. Skipping");
                return;
            }
            if (!animationClip.isHumanMotion)
            {
                Logger.Debug($"EmoteAPI: EmotesError: [{animationClip.name}] is not a humanoid animation!");
                return;
            }
            if (rootBonesToIgnore == null)
                rootBonesToIgnore = Array.Empty<HumanBodyBones>();
            if (soloBonesToIgnore == null)
                soloBonesToIgnore = Array.Empty<HumanBodyBones>();
            AnimationClip[] animationClips = new AnimationClip[] { animationClip };
            AnimationClip[]? secondaryClips = null;
            if (secondaryAnimation)
            {
                secondaryClips = new AnimationClip[] { secondaryAnimation };
            }
            string[] wwiseEvents = new string[] { _wwiseEventName };
            string[] wwiseStopEvents = new string[] { _wwiseStopEvent };
            CustomAnimationClip clip = new CustomAnimationClip(animationClips, looping, wwiseEvents, wwiseStopEvents, rootBonesToIgnore, soloBonesToIgnore, secondaryClips, dimWhenClose, stopWhenMove, stopWhenAttack, visible, syncAnim, syncAudio);
            if (visible)
                allClipNames.Add(animationClip.name);
            BoneMapper.animClips.Add(animationClip.name, clip);
        }

    #endregion
    
    

    public static void PlayAnimation(string animationName, int playerId, int pos = -2)
    {
        Logger.Debug($"Under normal circumstances, player {playerId} would be emoting \"{animationName}\" right now");
    }
    
    internal static void Changed(string newAnimation, BoneMapper mapper) //is a neat game made by a developer who endorses nsfw content while calling it a fine game for kids
    {
        foreach (var item in EmoteLocation.emoteLocations)
        {
            if (item.emoter == mapper)
            {
                try
                {
                    item.emoter = null;
                    item.SetVisible(true);
                }
                catch (Exception)
                {
                    // ignored
                }
            }
        }
        //mapper.transform.parent.GetComponent<CharacterModel>().body.RecalculateStats();///COME BACK TO THIS?
        
        AnimationChanged?.Invoke(newAnimation, mapper);
        
        //TODO: evaluate if localMapper is necessary for this game.  
        // if (newAnimation != "none")
        // {
        //     if (mapper == localMapper/* && Settings.HideJoinSpots.Value*/)
        //     {
        //         EmoteLocation.HideAllSpots();
        //     }
        // }
        // else
        // {
        //     if (mapper == localMapper/* && Settings.HideJoinSpots.Value*/)
        //     {
        //         EmoteLocation.ShowAllSpots();
        //     }
        // }
    }
}