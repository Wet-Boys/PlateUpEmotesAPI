using Kitchen;
using KitchenMods;
using PlateUpEmotesApi.Input;
using PlateUpEmotesApi.Logging;
using PlateUpEmotesApi.Utils;
using Unity.Collections;
using Unity.Entities;
using UnityEngine;
using ILogger = PlateUpEmotesApi.Logging.Loggers.ILogger;

namespace PlateUpEmotesApi.Emote.Systems;

public class InitializeEmoteEnjoyers : GenericSystemBase, IModSystem
{
    private static readonly ILogger Logger = LogUtils.CreateDefaultLogger("InitializeEmoteEnjoyers");
    
    private EntityQuery _emoteUnEnjoyers;
    private AddNewViews? _addNewViews;

    protected override void Initialise()
    {
        base.Initialise();
        
        _emoteUnEnjoyers = GetEntityQuery(new QueryHelper()
            .All(typeof(CPlayer), typeof(CEmoteInputData), typeof(CLinkedView))
            .None(typeof(CEmoteEnjoyer), typeof(CLinkedEmoteEnjoyerView)));

        _addNewViews = World.GetExistingSystem<AddNewViews>();
    }

    protected override void OnUpdate()
    {
        using var entities = _emoteUnEnjoyers.ToEntityArray(Allocator.TempJob);
        foreach (var entity in entities)
        {
            EntityManager.AddComponent<CEmoteEnjoyer>(entity);
            CLinkedView linkedView = EntityManager.GetComponentData<CLinkedView>(entity);
            
            GameObject playerObject = EntityViewManager.EntityViews[linkedView.Identifier].GameObject;
            IObjectView view = playerObject.GetComponent<EmoteEnjoyerView>();

            ViewIdentifier identifier = _addNewViews!.InvokeMethod<ViewIdentifier>("GetNewIdentifier");
            EntityManager.AddComponent<CLinkedEmoteEnjoyerView>(entity);
            EntityManager.SetComponentData(entity, new CLinkedEmoteEnjoyerView
            {
                Identifier = identifier
            });
            
            EntityViewManager.EntityViews.Add(identifier, view);
            
            Logger.Debug($"Added linked emote view {identifier.Identifier}");
        }
    }
}