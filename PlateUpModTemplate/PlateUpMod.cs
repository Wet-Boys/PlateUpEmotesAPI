﻿using HarmonyLib;
using UnityEngine;
using KitchenMods;
using UnityEngine;
using System.Collections.Generic;
using UnityEngine.Networking;
using System.Globalization;
using Kitchen;
using Unity.Entities;
using UnityEngine.Networking.Types;
using TwitchLib.Unity;

namespace EmotesAPI
{
    [HarmonyPatch]
    public static class PingDetectedPatch
    {
        [HarmonyPatch(typeof(Kitchen.MakePing), "Perform")]
        [HarmonyPostfix]
        public static void PingDetected_Postfix(ref InteractionData data)
        {
            Debug.Log("ping detected, playing random emote");
            int rand = UnityEngine.Random.Range(0, CustomEmotesAPI.allClipNames.Count);
            while (CustomEmotesAPI.blacklistedClips.Contains(rand))
            {
                rand = UnityEngine.Random.Range(0, CustomEmotesAPI.allClipNames.Count);
            }
            CustomEmotesAPI.PlayAnimation(CustomEmotesAPI.allClipNames[rand], data.Interactor);
            //TestClass t = World.DefaultGameObjectInjectionWorld.GetExistingSystem<TestClass>();
            //t.IncrementEventNumByOne(data.Interactor);
        }
    }
    public class CustomEmotesAPI : IModInitializer
    {
        public const string AUTHOR = "AUTHOR";
        public const string MOD_NAME = "MOD_NAME";
        public const string MOD_ID = $"com.{AUTHOR}.{MOD_NAME}";
        public static GameObject? playerPrefab;
        public void PostActivate(Mod mod)
        {
            Harmony.DEBUG = true;
            Harmony harmony = new Harmony(MOD_ID);

            harmony.PatchAll();
        }

        public void PreInject()
        {

        }

        public void PostInject()
        {

        }
        internal static List<string> allClipNames = new List<string>();
        internal static List<int> blacklistedClips = new List<int>();
        public static void BlackListEmote(string name)
        {
            for (int i = 0; i < allClipNames.Count; i++)
            {
                if (allClipNames[i] == name)
                {
                    blacklistedClips.Add(i);
                    return;
                }
            }
        }
        internal static void LoadResource(string resource)
        {
            Assets.AddBundle($"{resource}");
        }
        //internal static bool GetKey(ConfigEntry<KeyboardShortcut> entry)
        //{
        //    foreach (var item in entry.Value.Modifiers)
        //    {
        //        if (!Input.GetKey(item))
        //        {
        //            return false;
        //        }
        //    }
        //    return Input.GetKey(entry.Value.MainKey);
        //}
        //internal static bool GetKeyPressed(ConfigEntry<KeyboardShortcut> entry)
        //{
        //    foreach (var item in entry.Value.Modifiers)
        //    {
        //        if (!Input.GetKey(item))
        //        {
        //            return false;
        //        }
        //    }
        //    return Input.GetKeyDown(entry.Value.MainKey);
        //}
        public const string VERSION = "1.0.0";
        internal static float Actual_MSX = 69;
        public static CustomEmotesAPI instance;
        public static List<GameObject> audioContainers = new List<GameObject>();
        public static List<GameObject> activeAudioContainers = new List<GameObject>();

        public void Awake()
        {
            instance = this;
            //DebugClass.SetLogger(base.Logger);
            //CustomEmotesAPI.LoadResource("customemotespackage");
            //CustomEmotesAPI.LoadResource("fineilldoitmyself");
            //CustomEmotesAPI.LoadResource("enemyskeletons");
            //if (!BepInEx.Bootstrap.Chainloader.PluginInfos.ContainsKey("com.gemumoddo.MoistureUpset"))
            //{
            //}
            //CustomEmotesAPI.LoadResource("moisture_animationreplacements"); // I don't remember what's in here that makes importing emotes work, don't @ me
            //Settings.RunAll();
            //Register.Init();
            float WhosSteveJobs = 69420;
            //if (Settings.DontTouchThis.Value < 101) ///COME BACK TO THIS?
            //{
            //    WhosSteveJobs = Settings.DontTouchThis.Value;
            //}
            //On.RoR2.SceneCatalog.OnActiveSceneChanged += (orig, self, scene) =>  ///COME BACK TO THIS?
            //{
            //    //orig(self, scene);
            //    //AkSoundEngine.SetRTPCValue("Volume_Emotes", Settings.EmotesVolume.Value);
            //    if (allClipNames != null)
            //    {
            //        ScrollManager.SetupButtons(allClipNames);
            //    }
            //    //AkSoundEngine.SetRTPCValue("Volume_MSX", Actual_MSX);
            //    for (int i = 0; i < CustomAnimationClip.syncPlayerCount.Count; i++)
            //    {
            //        CustomAnimationClip.syncTimer[i] = 0;
            //        CustomAnimationClip.syncPlayerCount[i] = 0;
            //    }
            //    //if (scene.name == "title" && WhosSteveJobs < 101)
            //    //{
            //    //    AkSoundEngine.SetRTPCValue("Volume_MSX", WhosSteveJobs);
            //    //    Actual_MSX = WhosSteveJobs;
            //    //    WhosSteveJobs = 69420;
            //    //}
            //    foreach (var item in BoneMapper.allMappers)
            //    {
            //        try
            //        {
            //            foreach (var thing in audioContainers)
            //            {
            //                AkSoundEngine.StopAll(thing);
            //            }
            //            if (item)
            //            {
            //                item.audioObjects[item.currentClip.syncPos].transform.localPosition = new Vector3(0, -10000, 0);
            //                AkSoundEngine.PostEvent(BoneMapper.stopEvents[item.currentClip.syncPos][item.currEvent], item.audioObjects[item.currentClip.syncPos]);
            //            }
            //        }
            //        catch (System.Exception e)
            //        {
            //            Debug.Log($"EmoteAPI: Error when cleaning up audio on scene exit: {e}");
            //        }
            //    }
            //    BoneMapper.allMappers.Clear();
            //    localMapper = null;
            //    EmoteLocation.visibile = true;
            //};
            //On.RoR2.AudioManager.VolumeConVar.SetString += (orig, self, newValue) =>///COME BACK TO THIS?
            //{
            //    orig(self, newValue);
            //    //Volume_MSX
            //    if (self.GetFieldValue<string>("rtpcName") == "Volume_MSX" && WhosSteveJobs > 100)
            //    {
            //        Actual_MSX = float.Parse(newValue, CultureInfo.InvariantCulture);
            //        BoneMapper.Current_MSX = Actual_MSX;
            //        Settings.DontTouchThis.Value = float.Parse(newValue, CultureInfo.InvariantCulture);
            //    }
            //};
            //On.RoR2.PlayerCharacterMasterController.FixedUpdate += (orig, self) =>///COME BACK TO THIS?
            //{
            //    orig(self);
            //    if (CustomEmotesAPI.GetKey(Settings.EmoteWheel))
            //    {
            //        if (self.hasEffectiveAuthority && self.GetFieldValue<InputBankTest>("bodyInputs"))
            //        {
            //            bool newState = false;
            //            bool newState2 = false;
            //            bool newState3 = false;
            //            bool newState4 = false;
            //            LocalUser localUser;
            //            Rewired.Player player;
            //            CameraRigController cameraRigController;
            //            bool doIt = false;
            //            if (!self.networkUser)
            //            {
            //                localUser = null;
            //                player = null;
            //                cameraRigController = null;
            //                doIt = false;
            //            }
            //            else
            //            {
            //                localUser = self.networkUser.localUser;
            //                player = self.networkUser.inputPlayer;
            //                cameraRigController = self.networkUser.cameraRigController;
            //                doIt = localUser != null && player != null && cameraRigController && !localUser.isUIFocused && cameraRigController.isControlAllowed;
            //            }
            //            if (doIt)
            //            {
            //                newState = player.GetButton(7) && !CustomEmotesAPI.GetKey(Settings.EmoteWheel); //left click
            //                newState2 = player.GetButton(8) && !CustomEmotesAPI.GetKey(Settings.EmoteWheel); //right click
            //                newState3 = player.GetButton(9);
            //                newState4 = player.GetButton(10);
            //                self.GetFieldValue<InputBankTest>("bodyInputs").skill1.PushState(newState);
            //                self.GetFieldValue<InputBankTest>("bodyInputs").skill2.PushState(newState2);
            //                BoneMapper.attacking = newState || newState2 || newState3 || newState4;
            //                BoneMapper.moving = self.GetFieldValue<InputBankTest>("bodyInputs").moveVector != Vector3.zero || player.GetButton(4);
            //            }
            //        }
            //    }
            //};
        }
        //public static int RegisterWorldProp(GameObject worldProp, JoinSpot[] joinSpots)///COME BACK TO THIS?
        //{
        //    worldProp.AddComponent<NetworkIdentity>();
        //    worldProp.RegisterNetworkPrefab();
        //    worldProp.AddComponent<BoneMapper>().worldProp = true;
        //    var handler = worldProp.AddComponent<WorldPropSpawnHandler>();
        //    handler.propPos = BoneMapper.allWorldProps.Count;
        //    BoneMapper.allWorldProps.Add(new WorldProp(worldProp, joinSpots));
        //    return BoneMapper.allWorldProps.Count - 1;
        //}
        //internal static Shader standardShader = Addressables.LoadAssetAsync<GameObject>("RoR2/Base/Commando/CommandoBody.prefab").WaitForCompletion().GetComponentInChildren<SkinnedMeshRenderer>().material.shader;
        public static GameObject SpawnWorldProp(int propPos)
        {
            BoneMapper.allWorldProps[propPos].prop.GetComponent<WorldPropSpawnHandler>().propPos = propPos;
            return GameObject.Instantiate(BoneMapper.allWorldProps[propPos].prop);
        }

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
                Debug.Log($"EmoteAPI: EmotesError: [{animationClipParams.animationClip[0].name}] is already defined as a custom emote but is trying to be added. Skipping");
                return;
            }
            if (!animationClipParams.animationClip[0].isHumanMotion)
            {
                Debug.Log($"EmoteAPI: EmotesError: [{animationClipParams.animationClip[0].name}] is not a humanoid animation!");
                return;
            }
            if (animationClipParams.rootBonesToIgnore == null)
                animationClipParams.rootBonesToIgnore = new HumanBodyBones[0];
            if (animationClipParams.soloBonesToIgnore == null)
                animationClipParams.soloBonesToIgnore = new HumanBodyBones[0];
            if (animationClipParams._wwiseEventName == null)
                animationClipParams._wwiseEventName = new string[] { "" };
            if (animationClipParams._wwiseStopEvent == null)
                animationClipParams._wwiseStopEvent = new string[] { "" };
            if (animationClipParams.joinSpots == null)
                animationClipParams.joinSpots = new JoinSpot[0];
            CustomAnimationClip clip = new CustomAnimationClip(animationClipParams.animationClip, animationClipParams.looping, animationClipParams._wwiseEventName, animationClipParams._wwiseStopEvent, animationClipParams.rootBonesToIgnore, animationClipParams.soloBonesToIgnore, animationClipParams.secondaryAnimation, animationClipParams.dimWhenClose, animationClipParams.stopWhenMove, animationClipParams.stopWhenAttack, animationClipParams.visible, animationClipParams.syncAnim, animationClipParams.syncAudio, animationClipParams.startPref, animationClipParams.joinPref, animationClipParams.joinSpots, animationClipParams.useSafePositionReset, animationClipParams.customName, animationClipParams.customPostEventCodeSync);
            if (animationClipParams.visible)
                allClipNames.Add(animationClipParams.animationClip[0].name);
            BoneMapper.animClips.Add(animationClipParams.animationClip[0].name, clip);
        }
        public static void AddCustomAnimation(AnimationClip[] animationClip, bool looping, string[] _wwiseEventName = null, string[] _wwiseStopEvent = null, HumanBodyBones[] rootBonesToIgnore = null, HumanBodyBones[] soloBonesToIgnore = null, AnimationClip[] secondaryAnimation = null, bool dimWhenClose = false, bool stopWhenMove = false, bool stopWhenAttack = false, bool visible = true, bool syncAnim = false, bool syncAudio = false, int startPref = -1, int joinPref = -1, JoinSpot[] joinSpots = null)
        {
            if (BoneMapper.animClips.ContainsKey(animationClip[0].name))
            {
                Debug.Log($"EmoteAPI: EmotesError: [{animationClip[0].name}] is already defined as a custom emote but is trying to be added. Skipping");
                return;
            }
            if (!animationClip[0].isHumanMotion)
            {
                Debug.Log($"EmoteAPI: EmotesError: [{animationClip[0].name}] is not a humanoid animation!");
                return;
            }
            if (rootBonesToIgnore == null)
                rootBonesToIgnore = new HumanBodyBones[0];
            if (soloBonesToIgnore == null)
                soloBonesToIgnore = new HumanBodyBones[0];
            if (_wwiseEventName == null)
                _wwiseEventName = new string[] { "" };
            if (_wwiseStopEvent == null)
                _wwiseStopEvent = new string[] { "" };
            if (joinSpots == null)
                joinSpots = new JoinSpot[0];
            CustomAnimationClip clip = new CustomAnimationClip(animationClip, looping, _wwiseEventName, _wwiseStopEvent, rootBonesToIgnore, soloBonesToIgnore, secondaryAnimation, dimWhenClose, stopWhenMove, stopWhenAttack, visible, syncAnim, syncAudio, startPref, joinPref, joinSpots);
            if (visible)
                allClipNames.Add(animationClip[0].name);
            BoneMapper.animClips.Add(animationClip[0].name, clip);
        }
        public static void AddCustomAnimation(AnimationClip[] animationClip, bool looping, string _wwiseEventName = "", string _wwiseStopEvent = "", HumanBodyBones[] rootBonesToIgnore = null, HumanBodyBones[] soloBonesToIgnore = null, AnimationClip[] secondaryAnimation = null, bool dimWhenClose = false, bool stopWhenMove = false, bool stopWhenAttack = false, bool visible = true, bool syncAnim = false, bool syncAudio = false, int startPref = -1, int joinPref = -1)
        {
            if (BoneMapper.animClips.ContainsKey(animationClip[0].name))
            {
                Debug.Log($"EmoteAPI: EmotesError: [{animationClip[0].name}] is already defined as a custom emote but is trying to be added. Skipping");
                return;
            }
            if (!animationClip[0].isHumanMotion)
            {
                Debug.Log($"EmoteAPI: EmotesError: [{animationClip[0].name}] is not a humanoid animation!");
                return;
            }
            if (rootBonesToIgnore == null)
                rootBonesToIgnore = new HumanBodyBones[0];
            if (soloBonesToIgnore == null)
                soloBonesToIgnore = new HumanBodyBones[0];
            string[] wwiseEvents = new string[] { _wwiseEventName };
            string[] wwiseStopEvents = new string[] { _wwiseStopEvent };
            CustomAnimationClip clip = new CustomAnimationClip(animationClip, looping, wwiseEvents, wwiseStopEvents, rootBonesToIgnore, soloBonesToIgnore, secondaryAnimation, dimWhenClose, stopWhenMove, stopWhenAttack, visible, syncAnim, syncAudio, startPref, joinPref);
            if (visible)
                allClipNames.Add(animationClip[0].name);
            BoneMapper.animClips.Add(animationClip[0].name, clip);
        }
        public static void AddCustomAnimation(AnimationClip animationClip, bool looping, string _wwiseEventName = "", string _wwiseStopEvent = "", HumanBodyBones[] rootBonesToIgnore = null, HumanBodyBones[] soloBonesToIgnore = null, AnimationClip secondaryAnimation = null, bool dimWhenClose = false, bool stopWhenMove = false, bool stopWhenAttack = false, bool visible = true, bool syncAnim = false, bool syncAudio = false)
        {
            if (BoneMapper.animClips.ContainsKey(animationClip.name))
            {
                Debug.Log($"EmoteAPI: EmotesError: [{animationClip.name}] is already defined as a custom emote but is trying to be added. Skipping");
                return;
            }
            if (!animationClip.isHumanMotion)
            {
                Debug.Log($"EmoteAPI: EmotesError: [{animationClip.name}] is not a humanoid animation!");
                return;
            }
            if (rootBonesToIgnore == null)
                rootBonesToIgnore = new HumanBodyBones[0];
            if (soloBonesToIgnore == null)
                soloBonesToIgnore = new HumanBodyBones[0];
            AnimationClip[] animationClips = new AnimationClip[] { animationClip };
            AnimationClip[] secondaryClips = null;
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

        //public static void PlayAnimation(string animationName, int pos = -2)
        //{
        //    var identity = NetworkUser.readOnlyLocalPlayersList[0].master?.GetBody().gameObject.GetComponent<NetworkIdentity>();
        //    new SyncAnimationToServer(identity.netId, animationName, pos).Send(R2API.Networking.NetworkDestination.Server);
        //}
        //public static void PlayAnimation(string animationName, BoneMapper mapper, int pos = -2)
        //{
        //    var identity = mapper.transform.parent.GetComponent<CharacterModel>().body.GetComponent<NetworkIdentity>();
        //    new SyncAnimationToServer(identity.netId, animationName, pos).Send(R2API.Networking.NetworkDestination.Server);
        //}
        public static void PlayAnimation(string animationName, Entity interactor, int pos = -2)
        {
            Debug.Log($"Under normal circumstances, {interactor} would be emoting right now");
            //var identity = NetworkUser.readOnlyLocalPlayersList[0].master?.GetBody().gameObject.GetComponent<NetworkIdentity>();
            //new SyncAnimationToServer(identity.netId, animationName, pos).Send(R2API.Networking.NetworkDestination.Server);

            GameObject bodyObject = Util.FindNetworkObject(netId);
            if (!bodyObject)
            {
                Debug.Log($"EmoteAPI: Body is null!!!");
            }

            Debug.Log($"EmoteAPI: Recieved message to play {animationName} on client. Playing on {bodyObject.GetComponent<PlayerView>().transform}");

            bodyObject.GetComponent<PlayerView>().transform.GetComponentInChildren<BoneMapper>().PlayAnim(animation, position, eventNum);// rune look at this
        }
        public static BoneMapper localMapper = null;
        static BoneMapper nearestMapper = null;
        public static CustomAnimationClip GetLocalBodyCustomAnimationClip()
        {
            if (localMapper)
            {
                return localMapper.currentClip;
            }
            else
            {
                return null;
            }
        }
        public static BoneMapper[] GetAllBoneMappers()
        {
            return BoneMapper.allMappers.ToArray();
        }
        public delegate void AnimationChanged(string newAnimation, BoneMapper mapper);
        public static event AnimationChanged animChanged;
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
                    catch (System.Exception)
                    {
                    }
                }
            }
            //mapper.transform.parent.GetComponent<CharacterModel>().body.RecalculateStats();///COME BACK TO THIS?
            if (animChanged != null)
            {
                animChanged(newAnimation, mapper);
            }
            if (newAnimation != "none")
            {
                if (mapper == localMapper/* && Settings.HideJoinSpots.Value*/)///COME BACK TO THIS?
                {
                    EmoteLocation.HideAllSpots();
                }
            }
            else
            {
                if (mapper == localMapper/* && Settings.HideJoinSpots.Value*/)///COME BACK TO THIS?
                {
                    EmoteLocation.ShowAllSpots();
                }
            }
        }
        public delegate void JoinedEmoteSpotBody(GameObject emoteSpot, BoneMapper joiner, BoneMapper host);
        public static event JoinedEmoteSpotBody emoteSpotJoined_Body;
        internal static void JoinedBody(GameObject emoteSpot, BoneMapper joiner, BoneMapper host)
        {
            emoteSpotJoined_Body(emoteSpot, joiner, host);
        }
        public delegate void JoinedEmoteSpotProp(GameObject emoteSpot, BoneMapper joiner, BoneMapper host);
        public static event JoinedEmoteSpotProp emoteSpotJoined_Prop;
        internal static void JoinedProp(GameObject emoteSpot, BoneMapper joiner, BoneMapper host)
        {
            emoteSpotJoined_Prop(emoteSpot, joiner, host);
        }
        public delegate void AnimationJoined(string joinedAnimation, BoneMapper joiner, BoneMapper host);
        public static event AnimationJoined animJoined;
        internal static void Joined(string joinedAnimation, BoneMapper joiner, BoneMapper host)
        {
            animJoined(joinedAnimation, joiner, host);
        }

        void Update()
        {
            //if (GetKeyPressed(Settings.RandomEmote))///COME BACK TO THIS?
            //{
            //    int rand = UnityEngine.Random.Range(0, allClipNames.Count);
            //    while (blacklistedClips.Contains(rand))
            //    {
            //        rand = UnityEngine.Random.Range(0, allClipNames.Count);
            //    }
            //    //foreach (var item in BoneMapper.allMappers)
            //    //{
            //    //    PlayAnimation(allClipNames[rand], item);
            //    //}
            //    PlayAnimation(allClipNames[rand]);
            //}
            //if (GetKeyPressed(Settings.JoinEmote))///COME BACK TO THIS?
            //{
            //    try
            //    {
            //        if (localMapper)
            //        {
            //            if (localMapper.currentEmoteSpot)
            //            {
            //                localMapper.JoinEmoteSpot();
            //            }
            //            else
            //            {
            //                foreach (var mapper in BoneMapper.allMappers)
            //                {
            //                    try
            //                    {
            //                        if (mapper != localMapper)
            //                        {
            //                            if (!nearestMapper && (mapper.currentClip.syncronizeAnimation || mapper.currentClip.syncronizeAudio))
            //                            {
            //                                nearestMapper = mapper;
            //                            }
            //                            else if (nearestMapper)
            //                            {
            //                                if ((mapper.currentClip.syncronizeAnimation || mapper.currentClip.syncronizeAudio) && Vector3.Distance(localMapper.transform.position, mapper.transform.position) < Vector3.Distance(localMapper.transform.position, nearestMapper.transform.position))
            //                                {
            //                                    nearestMapper = mapper;
            //                                }
            //                            }
            //                        }
            //                    }
            //                    catch (System.Exception)
            //                    {
            //                    }
            //                }
            //                if (nearestMapper)
            //                {
            //                    PlayAnimation(nearestMapper.currentClip.clip[0].name);
            //                    Joined(nearestMapper.currentClip.clip[0].name, localMapper, nearestMapper); //this is not networked and only sent locally FYI
            //                }
            //                nearestMapper = null;
            //            }
            //        }
            //    }
            //    catch (System.Exception e)
            //    {
            //        Debug.Log($"EmoteAPI: had issue while attempting to join an emote as a client: {e}\nNotable info: [nearestMapper: {nearestMapper}] [localMapper: {localMapper}]");
            //        try
            //        {
            //            nearestMapper.currentClip.ToString();
            //            Debug.Log($"EmoteAPI: [nearestMapper.currentClip: {nearestMapper.currentClip.ToString()}] [nearestMapper.currentClip.clip[0]: {nearestMapper.currentClip.clip[0]}]");
            //        }
            //        catch (System.Exception)
            //        {
            //        }
            //    }
            //}
            AudioFunctions();
        }

        void AudioFunctions()
        {
            //for (int i = 0; i < CustomEmotesAPI.audioContainers.Count; i++)///COME BACK TO THIS?
            //{
            //    AudioContainer ac = CustomEmotesAPI.audioContainers[i].GetComponent<AudioContainer>();
            //    if (ac.playingObjects.Count != 0)
            //    {
            //        AkPositionArray ak = new AkPositionArray((uint)ac.playingObjects.Count);
            //        foreach (var item in ac.playingObjects)
            //        {
            //            ak.Add(item.transform.position, new Vector3(1, 0, 0), new Vector3(0, 1, 0));
            //        }
            //        AkSoundEngine.SetMultiplePositions(CustomEmotesAPI.audioContainers[i], ak, (ushort)ak.Count, AkMultiPositionType.MultiPositionType_MultiDirections);
            //    }
            //}
            for (int i = 0; i < CustomAnimationClip.syncPlayerCount.Count; i++)
            {
                if (CustomAnimationClip.syncPlayerCount[i] != 0)
                {
                    CustomAnimationClip.syncTimer[i] += Time.deltaTime;
                }
            }
        }
    }
    //[HarmonyPatch]
    //public static class Scene_Changed_Patch
    //{
    //    [HarmonyPatch(typeof(UnityEngine.SceneManagement.SceneManager), nameof(UnityEngine.SceneManagement.SceneManager.activeSceneChanged))]
    //    [HarmonyPostfix]
    //    public static void SceneChanged_Postfix()
    //    {
    //        //do my stuff
    //    }
    //}
}