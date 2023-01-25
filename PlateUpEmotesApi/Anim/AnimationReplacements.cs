using Kitchen;
using PlateUpEmotesApi.Logging;
using PlateUpEmotesApi.Utils;
using UnityEngine;
using ILogger = PlateUpEmotesApi.Logging.Loggers.ILogger;

namespace PlateUpEmotesApi.Anim;

internal static class AnimationReplacements
{
    private static readonly ILogger Logger = LogUtils.CreateDefaultLogger("AnimationReplacements");

    internal static GameObject g;
    internal static void RunAll()
    {
        ChangeAnims();
    }
    internal static bool setup = false;
    internal static void EnemyArmatures()
    {
        if (Prefabs.PlayerPrefab is null)
            throw new NullReferenceException();

        Import(Prefabs.PlayerPrefab, "morbman.prefab");
        Prefabs.PlayerPrefab.GetComponentInChildren<BoneMapper>().scale = .5f;
    }
    internal static void Import(GameObject go, string skeleton)
    {
        PlateUpEmotesManager.ImportArmature(go, Assets.Load<GameObject>(skeleton));
    }
    internal static void ChangeAnims()
    {
        EnemyArmatures();
    }
    //internal static void ApplyAnimationStuff(SurvivorDef index, string resource, int pos = 0)
    //{
    //    ApplyAnimationStuff(index.bodyPrefab, resource, pos);
    //}
    internal static void ApplyAnimationStuff(GameObject bodyPrefab, string resource, int pos = 0)
    {
        GameObject animcontroller = Assets.Load<GameObject>(resource);
        ApplyAnimationStuff(bodyPrefab, animcontroller, pos);
    }
    internal static void ApplyAnimationStuff(GameObject bodyPrefab, GameObject animcontroller, int pos = 0, bool hidemeshes = true, bool jank = false)
    {
        try
        {
            if (!animcontroller.GetComponentInChildren<Animator>().avatar.isHuman)
            {
                Debug.Log($"EmoteAPI: {animcontroller}'s avatar isn't humanoid, please fix it in unity!");
                return;
            }
        }
        catch (Exception e)
        {
            Debug.Log($"EmoteAPI: Had issue checking if avatar was humanoid: {e}");
            throw;
        }
        try
        {
            if (hidemeshes)
            {
                foreach (var item in animcontroller.GetComponentsInChildren<SkinnedMeshRenderer>())
                {
                    item.sharedMesh = null;
                }
                foreach (var item in animcontroller.GetComponentsInChildren<MeshFilter>())
                {
                    item.sharedMesh = null;
                }
            }
        }
        catch (Exception e)
        {
            Debug.Log($"EmoteAPI: Had trouble while hiding meshes: {e}");
            throw;
        }
        try
        {
            animcontroller.transform.parent = bodyPrefab.GetComponent<PlayerView>().transform;
            animcontroller.transform.localPosition = Vector3.zero;
            animcontroller.transform.localEulerAngles = Vector3.zero;
            animcontroller.transform.localScale = Vector3.one;
        }
        catch (Exception e)
        {
            Debug.Log($"EmoteAPI: Had trouble setting emote skeletons parent: {e}");
            throw;
        }
        SkinnedMeshRenderer smr1;
        SkinnedMeshRenderer smr2;
        try
        {
            smr1 = animcontroller.GetComponentsInChildren<SkinnedMeshRenderer>()[pos];
        }
        catch (Exception e)
        {
            Debug.Log($"EmoteAPI: Had trouble setting emote skeletons SkinnedMeshRenderer: {e}");
            throw;
        }
        try
        {
            smr2 = bodyPrefab.GetComponent<PlayerView>().transform.GetComponentsInChildren<SkinnedMeshRenderer>()[pos];
        }
        catch (Exception e)
        {
            Debug.Log($"EmoteAPI: Had trouble setting the original skeleton's skinned mesh renderer: {e}");
            throw;
        }
        try
        {
            int matchingBones = 0;
            while (true)
            {
                foreach (var smr1bone in smr1.bones)
                {
                    foreach (var smr2bone in smr2.bones)
                    {
                        if (smr1bone.name == smr2bone.name)
                        {
                            matchingBones++;
                        }
                    }
                }
                if (matchingBones < 5 && pos + 1 < bodyPrefab.GetComponent<PlayerView>().transform.GetComponentsInChildren<SkinnedMeshRenderer>().Length)
                {
                    pos++;
                    smr2 = bodyPrefab.GetComponent<PlayerView>().transform.GetComponentsInChildren<SkinnedMeshRenderer>()[pos];
                    matchingBones = 0;
                }
                else
                {
                    break;
                }
            }
        }
        catch (Exception e)
        {
            Debug.Log($"EmoteAPI: Had issue while checking matching bones: {e}");
            throw;
        }
        var test = animcontroller.AddComponent<BoneMapper>();
        try
        {
            test.jank = jank;
            test.smr1 = smr1;
            test.smr2 = smr2;
            test.bodyPrefab = bodyPrefab;
            test.a1 = bodyPrefab.GetComponent<PlayerView>().transform.GetComponentInChildren<Animator>();
            test.a2 = animcontroller.GetComponentInChildren<Animator>();
        }
        catch (Exception e)
        {
            Debug.Log($"EmoteAPI: Had issue when setting up BoneMapper settings 1: {e}");
            throw;
        }
        try
        {
            //var nuts = Assets.Load<GameObject>("@CustomEmotesAPI_customemotespackage:assets/animationreplacements/bandit.prefab");
            //float banditScale = Vector3.Distance(nuts.GetComponentInChildren<Animator>().GetBoneTransform(HumanBodyBones.Head).position, nuts.GetComponentInChildren<Animator>().GetBoneTransform(HumanBodyBones.LeftFoot).position);
            //float currScale = Vector3.Distance(animcontroller.GetComponentInChildren<Animator>().GetBoneTransform(HumanBodyBones.Head).position, animcontroller.GetComponentInChildren<Animator>().GetBoneTransform(HumanBodyBones.LeftFoot).position);
            //test.scale = currScale / banditScale;
            test.model = bodyPrefab.GetComponent<PlayerView>().transform.gameObject;
        }
        catch (Exception e)
        {
            Debug.Log($"EmoteAPI: Had issue when setting up BoneMapper settings 2: {e}");
            throw;
        }
    }
}