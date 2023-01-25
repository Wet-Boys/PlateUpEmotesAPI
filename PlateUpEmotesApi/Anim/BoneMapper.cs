using Kitchen;
using UnityEngine;
using UnityEngine.Animations;

namespace PlateUpEmotesApi.Anim;

public class BoneMapper : MonoBehaviour
{
    public static List<AudioClip[]> startEvents = new();
    internal static List<GameObject> audioObjects = new();
    public SkinnedMeshRenderer smr1, smr2;
    public Animator a1, a2;
    public List<BonePair> pairs = new();
    public float timer = 0;
    public GameObject model;
    List<string> ignore = new();
    bool twopart = false;
    public static Dictionary<string, CustomAnimationClip?> animClips = new();
    public CustomAnimationClip? currentClip = null;
    public CustomAnimationClip? prevClip = null;
    internal static float Current_MSX = 69;
    internal static List<BoneMapper> allMappers = new();
    internal static List<WorldProp> allWorldProps = new();
    public bool local = false;
    internal static bool moving = false;
    internal static bool attacking = false;
    public bool jank = false;
    public List<GameObject> props = new();
    public float scale = 1.0f;
    internal int desiredEvent = 0;
    public int currEvent = 0;
    public float autoWalkSpeed = 0;
    public bool overrideMoveSpeed = false;
    public bool autoWalk = false;
    public GameObject? currentEmoteSpot = null;
    public bool worldProp = false;
    public bool ragdolling = false;
    public GameObject? bodyPrefab;
    public int uniqueSpot = -1;
    public bool preserveProps = false;
    public bool preserveParent = false;
    internal bool useSafePositionReset = false;
    public List<EmoteLocation> emoteLocations = new();
    List<string> dontAnimateUs = new();
    public static List<AudioSource> audioSources = new();

    public void PlayAnim(string s, int pos, int eventNum)
    {
        desiredEvent = eventNum;
        PlayAnim(s, pos);
    }
    public void PlayAnim(string s, int pos)
    {
        if (s != "none")
        {
            if (!animClips.ContainsKey(s))
            {
                Debug.Log($"EmoteAPI: No emote bound to the name [{s}]");
                return;
            }
            try
            {
                animClips[s].ToString();
            }
            catch (Exception)
            {
                PlateUpEmotesManager.Changed(s, this);
                return;
            }
        }
        a2.enabled = true;
        dontAnimateUs.Clear();
        try
        {
            currentClip.clip[0].ToString();
            try
            {
                CustomAnimationClip.syncPlayerCount[currentClip.syncPos]--;
                RemoveAudioObject();
                if (audioSources[currentClip.syncPos].isPlaying && CustomAnimationClip.syncPlayerCount[currentClip.syncPos] == 0)
                {
                    audioSources[currentClip.syncPos].Stop();
                    //audioSources[currentClip.syncPos].clip = startEvents[currentClip.syncPos][currEvent];
                }
                if (startEvents[currentClip.syncPos][currEvent] != null)
                {
                    if (!currentClip.syncronizeAudio)
                    {
                        audioSources[currentClip.syncPos].Stop();
                    }


                    if (CustomAnimationClip.syncPlayerCount[currentClip.syncPos] == 0 && currentClip.syncronizeAudio)
                    {
                        audioSources[currentClip.syncPos].Stop();
                    }
                }
                if (uniqueSpot != -1 && CustomAnimationClip.uniqueAnimations[currentClip.syncPos][uniqueSpot])
                {
                    CustomAnimationClip.uniqueAnimations[currentClip.syncPos][uniqueSpot] = false;
                    uniqueSpot = -1;
                }
            }
            catch (Exception e)
            {
                Debug.Log($"EmoteAPI: had issue turning off audio before new audio played: {e}\n Notable items for debugging: [currentClip: {currentClip}] [currentClip.syncPos: {currentClip.syncPos}] [currEvent: {currEvent}] [audioSources[currentClip.syncPos]: {audioSources[currentClip.syncPos]}]");
            }
        }
        catch (Exception)
        {

        }
        currEvent = 0;
        if (s != "none")
        {
            prevClip = currentClip;
            currentClip = animClips[s];
            try
            {
                currentClip.clip[0].ToString();
            }
            catch (Exception)
            {
                return;
            }
            if (pos == -2)
            {
                if (CustomAnimationClip.syncPlayerCount[animClips[s].syncPos] == 0)
                {
                    pos = animClips[s].startPref;
                }
                else
                {
                    pos = animClips[s].joinPref;
                }
            }
            if (pos == -2)
            {
                for (int i = 0; i < CustomAnimationClip.uniqueAnimations[currentClip.syncPos].Count; i++)
                {
                    if (!CustomAnimationClip.uniqueAnimations[currentClip.syncPos][i])
                    {
                        pos = i;
                        uniqueSpot = pos;
                        CustomAnimationClip.uniqueAnimations[currentClip.syncPos][uniqueSpot] = true;
                        break;
                    }
                }
                if (uniqueSpot == -1)
                {
                    pos = -1;
                }
            }
            if (pos == -1)
            {
                pos = UnityEngine.Random.Range(0, currentClip.clip.Length);
            }
            LockBones();
        }
        if (s == "none")
        {
            a2.Play("none", -1, 0f);
            twopart = false;
            prevClip = currentClip;
            currentClip = null;
            NewAnimation(null);
            PlateUpEmotesManager.Changed(s, this);

            return;
        }
        AnimatorOverrideController animController = new AnimatorOverrideController(a2.runtimeAnimatorController);
        CustomAnimationClip.syncPlayerCount[currentClip.syncPos]++;
        //Debug.Log($"EmoteAPI: --------------  adding audio object {currentClip.syncPos}");
        AddAudioObject();
        if (currentClip.syncronizeAnimation && CustomAnimationClip.syncPlayerCount[currentClip.syncPos] == 1)
        {
            CustomAnimationClip.syncTimer[currentClip.syncPos] = 0;
        }
        if (startEvents[currentClip.syncPos][currEvent] != null)
        {
            if (CustomAnimationClip.syncPlayerCount[currentClip.syncPos] == 1 && currentClip.syncronizeAudio)
            {
                if (desiredEvent != -1)
                    currEvent = desiredEvent;
                else
                    currEvent = UnityEngine.Random.Range(0, startEvents[currentClip.syncPos].Length);
                foreach (var item in allMappers)
                {
                    item.currEvent = currEvent;
                }
                if (currentClip.customPostEventCodeSync != null)
                {
                    currentClip.customPostEventCodeSync.Invoke(this);
                }
                else if (!audioSources[currentClip.syncPos].isPlaying)
                {
                    audioSources[currentClip.syncPos].PlayOneShot(startEvents[currentClip.syncPos][currEvent]);
                    audioSources[currentClip.syncPos].volume = .05f;
                    //TODO Sync up audio volume with a slider
                }
            }
            else if (!currentClip.syncronizeAudio)
            {
                currEvent = UnityEngine.Random.Range(0, startEvents[currentClip.syncPos].Length);
                if (currentClip.customPostEventCodeNoSync != null)
                {
                    currentClip.customPostEventCodeNoSync.Invoke(this);
                }
                else
                {

                    audioSources[currentClip.syncPos].PlayOneShot(startEvents[currentClip.syncPos][currEvent]);
                    audioSources[currentClip.syncPos].volume = .05f;
                }
            }
            //audioObjects[currentClip.syncPos].transform.localPosition = Vector3.zero;
        }
        SetAnimationSpeed(1);
        if (currentClip.secondaryClip != null && currentClip.secondaryClip.Length != 0)
        {
            if (true)
            {
                if (CustomAnimationClip.syncTimer[currentClip.syncPos] > currentClip.clip[pos].length)
                {
                    animController["Floss"] = currentClip.secondaryClip[pos];
                    a2.runtimeAnimatorController = animController;
                    a2.Play("Loop", -1, ((CustomAnimationClip.syncTimer[currentClip.syncPos] - currentClip.clip[pos].length) % currentClip.secondaryClip[pos].length) / currentClip.secondaryClip[pos].length);
                }
                else
                {
                    animController["Dab"] = currentClip.clip[pos];
                    animController["nobones"] = currentClip.secondaryClip[pos];
                    a2.runtimeAnimatorController = animController;
                    a2.Play("PoopToLoop", -1, (CustomAnimationClip.syncTimer[currentClip.syncPos] % currentClip.clip[pos].length) / currentClip.clip[pos].length);
                }
            }
        }
        else if (currentClip.looping)
        {
            animController["Floss"] = currentClip.clip[pos];
            a2.runtimeAnimatorController = animController;
            if (currentClip.clip[pos].length != 0)
            {
                a2.Play("Loop", -1, (CustomAnimationClip.syncTimer[currentClip.syncPos] % currentClip.clip[pos].length) / currentClip.clip[pos].length);
            }
            else
            {
                a2.Play("Loop", -1, 0);
            }
        }
        else
        {
            animController["Default Dance"] = currentClip.clip[pos];
            a2.runtimeAnimatorController = animController;
            a2.Play("Poop", -1, (CustomAnimationClip.syncTimer[currentClip.syncPos] % currentClip.clip[pos].length) / currentClip.clip[pos].length);
        }
        twopart = false;
        NewAnimation(currentClip.joinSpots);
        PlateUpEmotesManager.Changed(s, this);
    }
    public void SetAnimationSpeed(float speed)
    {
        a2.speed = speed;
    }
    internal void NewAnimation(JoinSpot[]? locations)
    {
        try
        {
            emoteLocations.Clear();
            autoWalkSpeed = 0;
            autoWalk = false;
            overrideMoveSpeed = false;
            if (parentGameObject && !preserveParent)
            {
                //TODO Make sure we don't have to teleport players after they had a parentGameObject which drags them around
                //Vector3 OHYEAHHHHHH = transform.parent.GetComponent<CharacterModel>().body.gameObject.transform.position;
                //OHYEAHHHHHH = (TeleportHelper.FindSafeTeleportDestination(transform.parent.GetComponent<CharacterModel>().body.gameObject.transform.position, transform.parent.GetComponent<CharacterModel>().body, RoR2Application.rng)) ?? OHYEAHHHHHH;
                //Vector3 result = OHYEAHHHHHH;
                //RaycastHit raycastHit = default(RaycastHit);
                //Ray ray = new Ray(OHYEAHHHHHH + Vector3.up * 2f, Vector3.down);
                //float maxDistance = 4f;
                //if (Physics.SphereCast(ray, transform.parent.GetComponent<CharacterModel>().body.radius, out raycastHit, maxDistance, LayerIndex.world.mask))
                //{
                //    result.y = ray.origin.y - raycastHit.distance;
                //}
                //float bodyPrefabFootOffset = Util.GetBodyPrefabFootOffset(bodyPrefab);
                //result.y += bodyPrefabFootOffset;
                ////CapsuleCollider capsule = transform.parent.GetComponent<CharacterModel>().body.gameObject.GetComponent<CapsuleCollider>();
                //if (!useSafePositionReset/* && capsule*/)
                //{
                //    Rigidbody rigidbody = transform.parent.GetComponent<Rigidbody>();
                //    rigidbody.AddForce(new Vector3(0, 1, 0), ForceMode.VelocityChange);
                //    //transform.parent.GetComponent<PlayerView>().gameObject.transform.position += new Vector3(.1f, 2, .1f);
                //    //transform.parent.GetComponent<PlayerView>().transform.position += new Vector3(0, 10, 0);
                //}
                //else
                //{
                //    transform.parent.transform.position = result;
                //    //transform.parent.GetComponent<CharacterModel>().body.GetComponent<PlayerView>().modelBaseTransform.position = result;
                //}
                parentGameObject = null;
            }
            useSafePositionReset = true;
            if (preserveParent)
            {
                preserveParent = false;
            }
            else
            {
                transform.parent.GetComponent<PlayerView>().transform.localEulerAngles = Vector3.zero;
                //if (transform.parent.GetComponent<CharacterModel>().body.GetComponent<CharacterDirection>())
                //{
                //    transform.parent.GetComponent<CharacterModel>().body.GetComponent<CharacterDirection>().enabled = true;
                //}
                if (ogScale != new Vector3(-69, -69, -69))
                {
                    transform.parent.localScale = ogScale;
                    ogScale = new Vector3(-69, -69, -69);
                }
                if (ogLocation != new Vector3(-69, -69, -69))
                {
                    transform.parent.localPosition = ogLocation;
                    ogLocation = new Vector3(-69, -69, -69);
                }
                if (transform.parent.GetComponent<CapsuleCollider>())
                {
                    transform.parent.GetComponent<CapsuleCollider>().enabled = true;
                }
                //if (transform.parent.GetComponent<CharacterModel>().body.GetComponent<KinematicCharacterController.KinematicCharacterMotor>() && !transform.parent.GetComponent<CharacterModel>().body.GetComponent<KinematicCharacterController.KinematicCharacterMotor>().enabled)
                //{
                //    Vector3 desired = transform.parent.GetComponent<CharacterModel>().body.gameObject.transform.position;
                //    transform.parent.GetComponent<CharacterModel>().body.GetComponent<KinematicCharacterController.KinematicCharacterMotor>().enabled = true;
                //    transform.parent.GetComponent<CharacterModel>().body.GetComponent<KinematicCharacterController.KinematicCharacterMotor>().SetPosition(desired);
                //}
            }
            if (preserveProps)
            {
                preserveProps = false;
            }
            else
            {
                foreach (var item in props)
                {
                    if (item)
                        GameObject.Destroy(item);
                }
                props.Clear();
            }
            if (locations != null)
            {
                for (int i = 0; i < locations.Length; i++)
                {
                    SpawnJoinSpot(locations[i]);
                }
            }
        }
        catch (Exception e)
        {
            Debug.Log($"EmoteAPI: error during new animation: {e}");
        }
    }
    public void ScaleProps()
    {
        foreach (var item in props)
        {
            if (item)
            {
                Transform t = item.transform.parent;
                item.transform.SetParent(null);
                item.transform.localScale = new Vector3(scale * 1.15f, scale * 1.15f, scale * 1.15f);
                item.transform.SetParent(t);
            }
        }
    }
    //void AddIgnore(DynamicBone dynbone, Transform t)
    //{
    //    for (int i = 0; i < t.childCount; i++)
    //    {
    //        if (!dynbone.m_Exclusions.Contains(t.GetChild(i)))
    //        {
    //            ignore.Add(t.GetChild(i).name);
    //            AddIgnore(dynbone, t.GetChild(i));
    //        }
    //    }
    //}
    void Start()
    {
        if (worldProp)
        {
            return;
        }
        allMappers.Add(this);
        if (audioSources.Count == 0)
        {
            foreach (var _clip in startEvents)
            {
                GameObject obj = new GameObject();
                if (_clip[0] != null)
                {
                    obj.name = $"{_clip[0].name}_AudioObject";
                }
                GameObject.DontDestroyOnLoad(obj);
                obj.AddComponent<AudioSource>();
                BoneMapper.audioSources.Add(obj.GetComponent<AudioSource>());
                BoneMapper.audioObjects.Add(obj);
            }
        }
        int offset = 0;
        bool nuclear = true;
        if (nuclear)
        {
            foreach (var smr1bone in smr1.bones) //smr1 is the emote skeleton
            {
                foreach (var smr2bone in smr2.bones) //smr2 is the main skinned mesh renderer, which will receive parent constraints
                {
                    //Debug.Log($"EmoteAPI: --------------  {smr2bone.gameObject.name}   {smr1bone.gameObject.name}      {smr2bone.GetComponent<ParentConstraint>()}");
                    if (smr1bone.name == smr2bone.name/* + "_CustomEmotesAPIBone"*/ && !smr2bone.GetComponent<ParentConstraint>())
                    {
                        var s = new ConstraintSource();
                        s.sourceTransform = smr1bone;
                        s.weight = 1;
                        smr2bone.gameObject.AddComponent<ParentConstraint>().AddSource(s);
                        //smr1bone.name = smr1bone.name.Remove(smr1bone.name.Length - "_CustomEmotesAPIBone".Length);
                    }
                }
            }
        }
        if (jank)
        {
            for (int i = 0; i < smr2.bones.Length; i++)
            {
                try
                {
                    if (smr2.bones[i].gameObject.GetComponent<ParentConstraint>())
                    {
                        //Debug.Log($"EmoteAPI: -{i}---------{smr2.bones[i].gameObject}");
                        smr2.bones[i].gameObject.GetComponent<ParentConstraint>().constraintActive = true;
                    }
                }
                catch (Exception e)
                {
                    Debug.Log($"EmoteAPI: {e}");
                }
            }
        }
    }
    public GameObject parentGameObject;
    bool positionLock, rotationLock, scaleLock;
    public void AssignParentGameObject(GameObject youAreTheFather, bool lockPosition, bool lockRotation, bool lockScale, bool scaleAsBandit = true, bool disableCollider = true)
    {
        if (parentGameObject)
        {
            NewAnimation(null);
        }
        ogLocation = transform.parent.localPosition;
        ogScale = transform.parent.localScale;
        if (scaleAsBandit)
            scaleDiff = ogScale / scale;
        else
            scaleDiff = ogScale;

        //RoR2.Navigation.NodeGraph nodeGraph = SceneInfo.instance.GetNodeGraph(RoR2.Navigation.MapNodeGroup.GraphType.Ground);
        //List<RoR2.Navigation.NodeGraph.Node> nodes = new List<RoR2.Navigation.NodeGraph.Node>(nodeGraph.nodes);
        //nodes.Add(new RoR2.Navigation.NodeGraph.Node
        //{
        //    position = youAreTheFather.transform.position,
        //    forbiddenHulls = HullMask.None,
        //    flags = RoR2.Navigation.NodeFlags.None,
        //});
        //nodeGraph.nodes = nodes.ToArray();

        parentGameObject = youAreTheFather;
        //parentGameObject.transform.position += new Vector3(0, 0.060072f, 0);
        //startingPos = transform.position/* + new Vector3(0, 0.060072f, 0)*/;
        positionLock = lockPosition;
        rotationLock = lockRotation;
        scaleLock = lockScale;
        transform.parent.GetComponent<CapsuleCollider>().enabled = !disableCollider;
        //transform.parent.GetComponent<CharacterModel>().body.GetComponent<KinematicCharacterController.KinematicCharacterMotor>().enabled = !lockPosition;
        if (disableCollider && currentEmoteSpot)
        {
            currentEmoteSpot.GetComponent<EmoteLocation>().validPlayers--;
            currentEmoteSpot.GetComponent<EmoteLocation>().SetColor();
            currentEmoteSpot = null;
        }
    }
    float interval = 0;
    Vector3 ogScale = new Vector3(-69, -69, -69);
    Vector3 ogLocation = new Vector3(-69, -69, -69);
    Vector3 scaleDiff = Vector3.one;
    float rtpcUpdateTimer = 0;
    void DimmingChecks()
    {
        float closestDimmingSource = 20f;
        foreach (var item in allMappers)
        {
            try
            {
                if (item)
                {
                    if (item.a2.enabled && item.currentClip.dimAudio)
                    {
                        closestDimmingSource = 0f; break; //cry about it
                    }
                }
            }
            catch (Exception)
            {
            }
        }
        SetRTPCInDimming(closestDimmingSource);
    }
    void SetRTPCInDimming(float closestDimmingSource)
    {
        //TODO lower ingame music whenever a diming emote is playing (no dimming spheres because this game doesn't work like that)
        //if (closestDimmingSource < 20f && Settings.DimmingSpheres.Value && Settings.EmotesVolume.Value > 0)
        //{
        //    Current_MSX = Mathf.Lerp(Current_MSX, (closestDimmingSource / 20f) * CustomEmotesAPI.Actual_MSX, Time.deltaTime * 3);
        //    AkSoundEngine.SetRTPCValue("Volume_MSX", Current_MSX);
        //}
        //else if (Current_MSX != CustomEmotesAPI.Actual_MSX)
        //{
        //    Current_MSX = Mathf.Lerp(Current_MSX, CustomEmotesAPI.Actual_MSX, Time.deltaTime * 3);
        //    AkSoundEngine.SetRTPCValue("Volume_MSX", Current_MSX);
        //}
    }
    void LocalFunctions()
    {
        //AudioFunctions();
        DimmingChecks();
    }
    void GetLocal()
    {
        try
        {
            //TODO verify there are no references to CustomEmotesAPI.localMapper cause it doesn't work in this game
        }
        catch (Exception)
        {
        }
    }
    void TwoPartThing()
    {
        if (a2.GetCurrentAnimatorStateInfo(0).IsName("none"))
        {
            if (!twopart)
            {
                twopart = true;
            }
            else
            {
                if (a2.enabled)
                {
                    if (smr2.transform.parent.gameObject.name == "mdlVoidSurvivor" || smr2.transform.parent.gameObject.name == "mdlMage" || smr2.transform.parent.gameObject.name == "mdlJinx" || smr2.transform.parent.gameObject.name.StartsWith("mdlHouse"))
                    {
                        smr2.transform.parent.gameObject.SetActive(false);
                        smr2.transform.parent.gameObject.SetActive(true);
                    }
                    if (!jank)
                    {
                        UnlockBones();
                    }
                }
                //Debug.Log($"EmoteAPI: ----------{a1}");
                if (!ragdolling)
                {
                    a1.enabled = true;
                }
                a2.enabled = false;
                try
                {
                    currentClip.clip.ToString();
                    PlateUpEmotesManager.Changed("none", this);
                    NewAnimation(null);
                    CustomAnimationClip.syncPlayerCount[currentClip.syncPos]--;
                    RemoveAudioObject();
                    if (startEvents[currentClip.syncPos][currEvent] != null)
                    {
                        if (!currentClip.syncronizeAudio)
                        {
                            audioSources[currentClip.syncPos].Stop();
                        }

                        if (CustomAnimationClip.syncPlayerCount[currentClip.syncPos] == 0 && currentClip.syncronizeAudio)
                        {
                            audioSources[currentClip.syncPos].Stop();
                        }
                    }
                    prevClip = currentClip;
                    currentClip = null;
                }
                catch (Exception)
                {
                }
            }
        }
        else
        {
            //a1.enabled = false;
            twopart = false;
        }
    }
    void HealthAndAutoWalk()
    {
        if (autoWalkSpeed != 0)
        {
            if (overrideMoveSpeed)
            {
                transform.parent.GetComponent<PlayerView>().Speed = autoWalkSpeed;
            }
            if (autoWalk)
            {
                var field = typeof(PlayerView).GetField("MovementVector", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance);
                //var currVector = (Vector3)field.GetValue(transform.parent.GetComponent<PlayerView>());
                field.SetValue(transform.parent.GetComponent<PlayerView>(), transform.forward * autoWalkSpeed);
                //transform.parent.GetComponent<PlayerView>() = transform.parent.GetComponent<CharacterModel>().body.GetComponent<CharacterDirection>().forward * autoWalkSpeed;
            }
        }
    }
    void WorldPropAndParent()
    {
        if (parentGameObject)
        {
            if (positionLock)
            {
                //transform.parent.GetComponent<CharacterModel>().body.gameObject.transform.position = parentGameObject.transform.position + new Vector3(0, 1, 0);
                transform.parent.GetComponent<PlayerView>().transform.position = parentGameObject.transform.position;
                transform.parent.GetComponent<Rigidbody>().velocity = Vector3.zero;
            }
            if (rotationLock)
            {
                //transform.parent.GetComponent<CharacterModel>().body.GetComponent<CharacterDirection>().enabled = false;
                transform.parent.GetComponent<PlayerView>().transform.eulerAngles = parentGameObject.transform.eulerAngles;
            }
            if (scaleLock)
            {
                transform.parent.localScale = new Vector3(parentGameObject.transform.localScale.x * scaleDiff.x, parentGameObject.transform.localScale.y * scaleDiff.y, parentGameObject.transform.localScale.z * scaleDiff.z);
            }
        }
    }
    void Update()
    {
        if (worldProp)
        {
            return;
        }
        WorldPropAndParent();
        if (local)
        {
            LocalFunctions();
        }
        else
        {
            GetLocal();
        }
        TwoPartThing();
        HealthAndAutoWalk();
    }
    public int SpawnJoinSpot(JoinSpot joinSpot)
    {
        //TODO JOIN SPOTS
        //props.Add(GameObject.Instantiate(Assets.Load<GameObject>("@CustomEmotesAPI_customemotespackage:assets/emotejoiner/JoinVisual.prefab")));
        props[props.Count - 1].transform.SetParent(transform);
        //Vector3 scal = transform.lossyScale;
        //props[props.Count - 1].transform.localPosition = new Vector3(joinSpot.position.x / scal.x, joinSpot.position.y / scal.y, joinSpot.position.z / scal.z);
        //props[props.Count - 1].transform.localEulerAngles = joinSpot.rotation;
        //props[props.Count - 1].transform.localScale = new Vector3(joinSpot.scale.x / scal.x, joinSpot.scale.y / scal.y, joinSpot.scale.z / scal.z);
        props[props.Count - 1].name = joinSpot.name;
        //foreach (var rend in props[props.Count - 1].GetComponentsInChildren<SkinnedMeshRenderer>())
        //{
        //    rend.material.shader = CustomEmotesAPI.standardShader;
        //}
        EmoteLocation location = props[props.Count - 1].AddComponent<EmoteLocation>();
        location.joinSpot = joinSpot;
        location.owner = this;
        emoteLocations.Add(location);
        return props.Count - 1;
    }
    public void JoinEmoteSpot()
    {
        int spot = 0;
        for (; spot < currentEmoteSpot.transform.parent.GetComponentsInChildren<EmoteLocation>().Length; spot++)
        {
            if (currentEmoteSpot.transform.parent.GetComponentsInChildren<EmoteLocation>()[spot] == currentEmoteSpot.GetComponent<EmoteLocation>())
            {
                break;
            }
        }
        if (currentEmoteSpot.GetComponent<EmoteLocation>().owner.worldProp)
        {//TODO Make sync spots work with new system
            //new SyncSpotJoinedToHost(transform.parent.GetComponent<CharacterModel>().body.GetComponent<NetworkIdentity>().netId, currentEmoteSpot.transform.parent.GetComponent<NetworkIdentity>().netId, true, spot).Send(R2API.Networking.NetworkDestination.Server);
        }
        else
        {
            //new SyncSpotJoinedToHost(transform.parent.GetComponent<CharacterModel>().body.GetComponent<NetworkIdentity>().netId, currentEmoteSpot.transform.parent.parent.GetComponent<CharacterModel>().body.GetComponent<NetworkIdentity>().netId, false, spot).Send(R2API.Networking.NetworkDestination.Server);
        }
    }
    public void RemoveProp(int propPos)
    {
        GameObject.Destroy(props[propPos]);
    }
    public void SetAutoWalk(float speed, bool overrideBaseMovement, bool autoWalk)
    {
        autoWalkSpeed = speed;
        overrideMoveSpeed = overrideBaseMovement;
        this.autoWalk = autoWalk;
    }
    public void SetAutoWalk(float speed, bool overrideBaseMovement)
    {
        autoWalkSpeed = speed;
        overrideMoveSpeed = overrideBaseMovement;
        autoWalk = true;
    }
    void FixedUpdate()
    {
        //if (autoWalkSpeed != 0)
        //{
        //    if (overrideMoveSpeed)
        //    {
        //        transform.parent.GetComponent<CharacterModel>().body.GetComponent<InputBankTest>().moveVector = transform.parent.GetComponent<CharacterModel>().body.GetComponent<CharacterDirection>().forward * autoWalkSpeed;
        //    }
        //    else
        //    {
        //        transform.parent.GetComponent<CharacterModel>().body.GetComponent<CharacterMotor>().moveDirection = transform.parent.GetComponent<CharacterModel>().body.GetComponent<CharacterDirection>().forward * autoWalkSpeed;
        //    }
        //}
    }
    void AddAudioObject()
    {
        // TODO COME BACK TO THIS?
        // PlateUpEmotesManager.audioContainers[currentClip.syncPos].GetComponent<AudioContainer>().playingObjects.Add(this.gameObject);
    }
    void RemoveAudioObject()
    {
        //try
        //{
        //    PlateUpEmotesManager.audioContainers[currentClip.syncPos].GetComponent<AudioContainer>().playingObjects.Remove(this.gameObject);
        //}
        //catch (Exception e)
        //{
        //    Debug.Log($"EmoteAPI: failed to remove object {this.gameObject} from playingObjects: {e}");
        //}
    }
    void OnDestroy()
    {
        try
        {
            currentClip.clip[0].ToString();
            NewAnimation(null);
            if (CustomAnimationClip.syncPlayerCount[currentClip.syncPos] > 0)
            {
                CustomAnimationClip.syncPlayerCount[currentClip.syncPos]--;
                RemoveAudioObject();
            }
            if (startEvents[currentClip.syncPos][currEvent] != null)
            {
                if (!currentClip.syncronizeAudio)
                {
                    audioSources[currentClip.syncPos].Stop();
                }

                if (CustomAnimationClip.syncPlayerCount[currentClip.syncPos] == 0 && currentClip.syncronizeAudio)
                {
                    audioSources[currentClip.syncPos].Stop();
                }
            }
            if (uniqueSpot != -1 && CustomAnimationClip.uniqueAnimations[currentClip.syncPos][uniqueSpot])
            {
                CustomAnimationClip.uniqueAnimations[currentClip.syncPos][uniqueSpot] = false;
                uniqueSpot = -1;
            }
            BoneMapper.allMappers.Remove(this);
            prevClip = currentClip;
            currentClip = null;
        }
        catch (Exception e)
        {
            //Debug.Log($"EmoteAPI: Had issues when destroying bonemapper: {e}");
            BoneMapper.allMappers.Remove(this);
        }
    }
    public void UnlockBones()
    {
        for (int i = 0; i < smr2.bones.Length; i++)
        {
            try
            {
                if (smr2.bones[i].gameObject.GetComponent<ParentConstraint>())
                {
                    smr2.bones[i].gameObject.GetComponent<ParentConstraint>().constraintActive = false;
                }
            }
            catch (Exception)
            {
                break;
            }
        }
    }
    public void LockBones()
    {
        bool footL = false;
        bool footR = false;
        bool upperLegR = false;
        bool upperLegL = false;
        bool lowerLegR = false;
        bool lowerLegL = false;
        foreach (var item in currentClip.soloIgnoredBones)
        {
            if (item == HumanBodyBones.LeftFoot)
            {
                footL = true;
            }
            if (item == HumanBodyBones.RightFoot)
            {
                footR = true;
            }
            if (item == HumanBodyBones.LeftLowerLeg)
            {
                lowerLegL = true;
            }
            if (item == HumanBodyBones.LeftUpperLeg)
            {
                upperLegL = true;
            }
            if (item == HumanBodyBones.RightLowerLeg)
            {
                lowerLegR = true;
            }
            if (item == HumanBodyBones.RightUpperLeg)
            {
                upperLegR = true;
            }
            if (a2.GetBoneTransform(item))
                dontAnimateUs.Add(a2.GetBoneTransform(item).name);
        }
        foreach (var item in currentClip.rootIgnoredBones)
        {

            if (item == HumanBodyBones.LeftUpperLeg || item == HumanBodyBones.Hips)
            {
                upperLegL = true;
                lowerLegL = true;
                footL = true;
            }
            if (item == HumanBodyBones.RightUpperLeg || item == HumanBodyBones.Hips)
            {
                upperLegR = true;
                lowerLegR = true;
                footR = true;
            }
            if (a2.GetBoneTransform(item))
                dontAnimateUs.Add(a2.GetBoneTransform(item).name);
            foreach (var bone in a2.GetBoneTransform(item).GetComponentsInChildren<Transform>())
            {

                dontAnimateUs.Add(bone.name);
            }
        }
        bool left = upperLegL && lowerLegL && footL;
        bool right = upperLegR && lowerLegR && footR;
        Transform LeftLegIK = null;
        Transform RightLegIK = null;
        if (!jank)
        {
            for (int i = 0; i < smr2.bones.Length; i++)
            {
                try
                {
                    if (right && (smr2.bones[i].gameObject.ToString() == "IKLegTarget.r (UnityEngine.GameObject)" || smr2.bones[i].gameObject.ToString() == "FootControl.r (UnityEngine.GameObject)"))
                    {
                        RightLegIK = smr2.bones[i];
                    }
                    else if (left && (smr2.bones[i].gameObject.ToString() == "IKLegTarget.l (UnityEngine.GameObject)" || smr2.bones[i].gameObject.ToString() == "FootControl.l (UnityEngine.GameObject)"))
                    {
                        LeftLegIK = smr2.bones[i];
                    }
                    if (smr2.bones[i].gameObject.GetComponent<ParentConstraint>() && !dontAnimateUs.Contains(smr2.bones[i].name))
                    {
                        //Debug.Log($"EmoteAPI: -{i}---------{smr2.bones[i].gameObject}");
                        smr2.bones[i].gameObject.GetComponent<ParentConstraint>().constraintActive = true;
                    }
                    else if (dontAnimateUs.Contains(smr2.bones[i].name))
                    {
                        //Debug.Log($"EmoteAPI: dontanimateme-{i}---------{smr2.bones[i].gameObject}");
                        smr2.bones[i].gameObject.GetComponent<ParentConstraint>().constraintActive = false;
                    }
                }
                catch (Exception e)
                {
                    Debug.Log($"EmoteAPI: {e}");
                }
            }
            if (left && LeftLegIK)//we can leave ik for the legs
            {
                if (LeftLegIK.gameObject.GetComponent<ParentConstraint>())
                    LeftLegIK.gameObject.GetComponent<ParentConstraint>().constraintActive = false;
                foreach (var item in LeftLegIK.gameObject.GetComponentsInChildren<Transform>())
                {
                    if (item.gameObject.GetComponent<ParentConstraint>())
                        item.gameObject.GetComponent<ParentConstraint>().constraintActive = false;
                }
            }
            if (right && RightLegIK)
            {
                if (RightLegIK.gameObject.GetComponent<ParentConstraint>())
                    RightLegIK.gameObject.GetComponent<ParentConstraint>().constraintActive = false;
                foreach (var item in RightLegIK.gameObject.GetComponentsInChildren<Transform>())
                {
                    if (item.gameObject.GetComponent<ParentConstraint>())
                        item.gameObject.GetComponent<ParentConstraint>().constraintActive = false;
                }
            }
        }
        else
        {
            a1.enabled = false;
        }
    }
}