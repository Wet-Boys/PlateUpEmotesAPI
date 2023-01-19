using Kitchen;
using KitchenMods;
using MessagePack;

namespace PlateUpEmotesApi.Emote;

[MessagePackObject]
public struct CEmoteEnjoyer : IModComponent, IViewData.ICheckForChanges<CEmoteEnjoyer>
{
    [Key(1)]
    public EmotingState State;
    
    public bool IsChangedFrom(CEmoteEnjoyer check)
    {
        return State != check.State;
    }
}