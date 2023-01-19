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
    }
    internal static void Import(GameObject go, string skeleton)
    {
        PlateUpEmotesManager.ImportArmature(go, Assets.Load<GameObject>(skeleton));
    }
    internal static void ChangeAnims()
    {
        //On.RoR2.SurvivorCatalog.Init += (orig) =>///COME BACK TO THIS?
        //{
        //    orig();
        //    if (!setup)
        //    {
        //        setup = true;
        //        ApplyAnimationStuff(RoR2Content.Survivors.Croco, "@CustomEmotesAPI_customemotespackage:assets/animationreplacements/acrid.prefab");

        //        ApplyAnimationStuff(RoR2Content.Survivors.Mage, "@CustomEmotesAPI_customemotespackage:assets/animationreplacements/artificer.prefab");
        //        RoR2Content.Survivors.Mage.bodyPrefab.GetComponentInChildren<BoneMapper>().scale = .9f;

        //        ApplyAnimationStuff(RoR2Content.Survivors.Captain, "@CustomEmotesAPI_customemotespackage:assets/animationreplacements/captain.prefab");
        //        RoR2Content.Survivors.Captain.bodyPrefab.GetComponentInChildren<BoneMapper>().scale = 1.1f;

        //        ApplyAnimationStuff(RoR2Content.Survivors.Engi, "@CustomEmotesAPI_customemotespackage:assets/animationreplacements/engi.prefab");
        //        RoR2Content.Survivors.Engi.bodyPrefab.GetComponentInChildren<BoneMapper>().scale = 1f;

        //        ApplyAnimationStuff(RoR2Content.Survivors.Loader, "@CustomEmotesAPI_customemotespackage:assets/animationreplacements/loader.prefab");
        //        RoR2Content.Survivors.Loader.bodyPrefab.GetComponentInChildren<BoneMapper>().scale = 1.2f;

        //        ApplyAnimationStuff(RoR2Content.Survivors.Merc, "@CustomEmotesAPI_customemotespackage:assets/animationreplacements/merc.prefab");
        //        RoR2Content.Survivors.Merc.bodyPrefab.GetComponentInChildren<BoneMapper>().scale = .95f;

        //        ApplyAnimationStuff(RoR2Content.Survivors.Toolbot, "@CustomEmotesAPI_customemotespackage:assets/animationreplacements/mult1.prefab");
        //        RoR2Content.Survivors.Toolbot.bodyPrefab.GetComponentInChildren<BoneMapper>().scale = 1.5f;

        //        ApplyAnimationStuff(RoR2Content.Survivors.Treebot, "@CustomEmotesAPI_customemotespackage:assets/animationreplacements/rex.prefab");


        //        ApplyAnimationStuff(RoR2Content.Survivors.Commando, "@CustomEmotesAPI_customemotespackage:assets/animationreplacements/commandoFixed.prefab");
        //        RoR2Content.Survivors.Commando.bodyPrefab.GetComponentInChildren<BoneMapper>().scale = .85f;

        //        ApplyAnimationStuff(RoR2Content.Survivors.Huntress, "@CustomEmotesAPI_customemotespackage:assets/animationreplacements/huntress2022.prefab");
        //        RoR2Content.Survivors.Huntress.bodyPrefab.GetComponentInChildren<BoneMapper>().scale = .9f;

        //        ApplyAnimationStuff(RoR2Content.Survivors.Bandit2, "@CustomEmotesAPI_customemotespackage:assets/animationreplacements/bandit.prefab");


        //        ApplyAnimationStuff(SurvivorCatalog.FindSurvivorDefFromBody(Addressables.LoadAssetAsync<GameObject>("RoR2/DLC1/VoidSurvivor/VoidSurvivorBody.prefab").WaitForCompletion()), "@CustomEmotesAPI_customemotespackage:assets/animationreplacements/voidsurvivor.prefab");
        //        SurvivorCatalog.FindSurvivorDefFromBody(Addressables.LoadAssetAsync<GameObject>("RoR2/DLC1/VoidSurvivor/VoidSurvivorBody.prefab").WaitForCompletion()).bodyPrefab.GetComponentInChildren<BoneMapper>().scale = .85f;

        //        ApplyAnimationStuff(SurvivorCatalog.FindSurvivorDefFromBody(Addressables.LoadAssetAsync<GameObject>("RoR2/DLC1/Railgunner/RailgunnerBody.prefab").WaitForCompletion()), "@CustomEmotesAPI_customemotespackage:assets/animationreplacements/railgunner.prefab");
        //        SurvivorCatalog.FindSurvivorDefFromBody(Addressables.LoadAssetAsync<GameObject>("RoR2/DLC1/Railgunner/RailgunnerBody.prefab").WaitForCompletion()).bodyPrefab.GetComponentInChildren<BoneMapper>().scale = 1.05f;

        //        ApplyAnimationStuff(Addressables.LoadAssetAsync<GameObject>("RoR2/Base/Heretic/HereticBody.prefab").WaitForCompletion(), "@CustomEmotesAPI_customemotespackage:assets/animationreplacements/heretricburried.prefab", 3);//this works



                EnemyArmatures();

        //        //CustomEmotesAPI.ImportArmature(Addressables.LoadAssetAsync<GameObject>("RoR2/Base/Brother/BrotherBody.prefab").WaitForCompletion(), Assets.Load<GameObject>("@CustomEmotesAPI_enemyskeletons:assets/myprioritiesarestraightnt/brother.prefab"));
        //        //CustomEmotesAPI.ImportArmature(Addressables.LoadAssetAsync<GameObject>("RoR2/Base/Brother/BrotherHurtBody.prefab").WaitForCompletion(), Assets.Load<GameObject>("@CustomEmotesAPI_enemyskeletons:assets/myprioritiesarestraightnt/brother.prefab"));

        //        //foreach (var item in SurvivorCatalog.allSurvivorDefs) //probably don't need this
        //        //{
        //        //    if (item.bodyPrefab.name == "RobPaladinBody" && Settings.Paladin.Value)
        //        //    {
        //        //        var skele = Assets.Load<GameObject>("@CustomEmotesAPI_fineilldoitmyself:assets/fineilldoitmyself/animPaladin.prefab");
        //        //        CustomEmotesAPI.ImportArmature(item.bodyPrefab, skele);
        //        //        skele.GetComponentInChildren<BoneMapper>().scale = 1.5f;
        //        //    }
        //        //}
        //    }
        //};
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