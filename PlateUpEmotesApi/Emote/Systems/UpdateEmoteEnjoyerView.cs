using Kitchen;
using KitchenMods;
using PlateUpEmotesApi.Input;
using Unity.Collections;
using Unity.Entities;

namespace PlateUpEmotesApi.Emote.Systems;

public class UpdateEmoteEnjoyerView : ResponsiveViewSystemBase<EmoteEnjoyerView.ViewData, EmoteEnjoyerView.ResponseData>, IModSystem
{
    private EntityQuery _emoteEnjoyers;
    
    protected override void Initialise()
    {
        base.Initialise();
        _emoteEnjoyers = GetEntityQuery(new QueryHelper()
            .All(typeof(CPlayer), typeof(CEmoteInputData), typeof(CEmoteEnjoyer), typeof(CLinkedEmoteEnjoyerView)));
    }

    protected override void OnUpdate()
    {
        using var entities = _emoteEnjoyers.ToEntityArray(Allocator.TempJob);

        foreach (var entity in entities)
        {
            ViewIdentifier identifier = EntityManager.GetComponentData<CLinkedEmoteEnjoyerView>(entity).Identifier;
            CEmoteInputData emoteInputData = EntityManager.GetComponentData<CEmoteInputData>(entity);
            CPlayer player = EntityManager.GetComponentData<CPlayer>(entity);
            CEmoteEnjoyer emoteEnjoyer = EntityManager.GetComponentData<CEmoteEnjoyer>(entity);
            
            SendUpdate(identifier, new EmoteEnjoyerView.ViewData
            {
                Inputs = emoteInputData,
                EmoteEnjoyer = emoteEnjoyer,
                InputSource = player.InputSource,
                PlayerId = player.ID,
            });

            // Router.BroadcastUpdate(identifier, new EmoteEnjoyerView.ViewData
            // {
            //     Inputs = emoteInputData,
            //     EmoteEnjoyer = emoteEnjoyer,
            //     InputSource = player.InputSource,
            //     PlayerId = player.ID,
            // });

            EmoteEnjoyerView.ResponseData result = default;
            if (!ApplyUpdates(identifier, data => result.EmoteId = data.EmoteId, true))
                return;

            emoteEnjoyer.State.EmoteId = result.EmoteId;
            emoteEnjoyer.State.Playing = result.Playing;
            EntityManager.SetComponentData(entity, emoteEnjoyer);
        }
    }
}