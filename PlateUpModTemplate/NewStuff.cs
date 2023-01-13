using HarmonyLib;
using Kitchen;
using KitchenMods;
using MessagePack;
using System;
using System.Collections.Generic;
using System.Text;
using Unity.Collections;
using Unity.Entities;
using UnityEngine;

namespace EmotesAPI
{
    [HarmonyPatch(typeof(GameCreator), "PerformInitialSetup")]
    public static class GameCreator_InitialSetupPatch
    {
        private static int what = 0;
        [HarmonyPostfix]
        public static void Postfix(GameCreator __instance)
        {
            var fieldInfo = typeof(GameCreator).GetField("Directory", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance);
            AssetDirectory dir = (AssetDirectory)fieldInfo.GetValue(__instance);

            CustomEmotesAPI.playerPrefab = dir.ViewPrefabs[ViewType.Player];

            if (CustomEmotesAPI.playerPrefab is null)
            {
                Debug.Log("I'm killing myself");
                throw new NullReferenceException();
            }

            CustomEmotesAPI.playerPrefab.AddComponent<EmoteView>();

            CustomEmotesAPI.LoadResource("assets.morbman");
            AnimationReplacements.RunAll();
            CustomEmotesAPI.AddNonAnimatingEmote("none");
        }
    }
    public class AddCEmoteDataToPlayers : GenericSystemBase, IModSystem
    {
        private EntityQuery EmoteEnjoyers;
        protected override void Initialise()
        {
            base.Initialise();
            EmoteEnjoyers = GetEntityQuery(new QueryHelper()
                    .All(typeof(CPlayer))
                    .None(
                        typeof(CEmoteData)
                    ));
        }

        protected override void OnUpdate()
        {
            var joyers = EmoteEnjoyers.ToEntityArray(Allocator.TempJob);
            foreach (var joyer in joyers)
            {
                EntityManager.AddComponent<CRequiresView>(joyer);
                EntityManager.SetComponentData(joyer, new CRequiresView
                {
                    Type = ViewType.Player,
                    ViewMode = ViewMode.World
                });
                EntityManager.AddComponent<CEmoteData>(joyer);
            }
            joyers.Dispose();
        }
    }
    public struct CEmoteData : IModComponent
    {
        public char animation;
        public int position;
        public int eventNum;
    }


    public class EmoteView : UpdatableObjectView<EmoteView.ViewData>
    {
        public int helpmeplease = 69;
        public class UpdateView : IncrementalViewSystemBase<ViewData>, IModSystem
        {
            private EntityQuery Views;

            protected override void Initialise()
            {
                base.Initialise();
                Views = GetEntityQuery(new QueryHelper().All(typeof(CLinkedView), typeof(CEmoteData)));
            }

            protected override void OnUpdate()
            {
                using var views = Views.ToComponentDataArray<CLinkedView>(Allocator.Temp);
                using var components = Views.ToComponentDataArray<CEmoteData>(Allocator.Temp);

                for (var i = 0; i < components.Length; i++)
                {
                    var view = views[i];
                    var data = components[i];

                    SendUpdate(view, new ViewData
                    {
                        animation = data.animation,
                        position = data.position,
                        eventNum = data.eventNum
                    }, MessageType.SpecificViewUpdate);
                }
            }
        }

        // you must mark your ViewData as MessagePackObject and mark each field with a key
        // if you don't, the game will run locally but fail in multiplayer
        [MessagePackObject]
        public struct ViewData : ISpecificViewData, IViewData.ICheckForChanges<ViewData>
        {
            [Key(0)] public char animation;
            [Key(1)] public int position;
            [Key(2)] public int eventNum;

            // this tells the game how to find this subview within a prefab
            // GetSubView<T> is a cached method that looks for the requested T in the view and its children
            public IUpdatableObject GetRelevantSubview(IObjectView view) => view.GetSubView<EmoteView>();

            // this is used to determine if the data needs to be sent again
            public bool IsChangedFrom(ViewData check) => animation != check.animation ||
                                                         position != check.position ||
                                                         eventNum != check.eventNum;
        }

        // this receives the updated data from the ECS backend whenever a new update is sent
        // in general, this should update the state of the view to match the values in view_data
        // ideally ignoring all current state; it's possible that not all updates will be received so
        // you should avoid relying on previous state where possible
        protected override void UpdateData(ViewData view_data)
        {
            helpmeplease = view_data.eventNum;
            // perform the update here
            // this is a Unity MonoBehavior so we can do normal Unity things here
        }
    }

    public class TestClass : GenericSystemBase, IModSystem
    {
        private CEmoteData _data;
        protected override void OnUpdate()
        {

        }
        public void IncrementEventNumByOne(Entity eeeeeeeeeeeeeeee)
        {
            bool sex = Require(eeeeeeeeeeeeeeee, out _data);
            if (sex)
            {
                _data.eventNum++;
                Debug.Log($"{eeeeeeeeeeeeeeee}'s eventNum is now {_data.eventNum}");
                EntityManager.SetComponentData(eeeeeeeeeeeeeeee, _data);
            }
        }
    }
}
