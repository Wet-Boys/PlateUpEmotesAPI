using Kitchen;
using MessagePack;
using Unity.Entities;

namespace PlateUpEmotesApi.Input;

[MessagePackObject]
public struct CEmoteInputData : IComponentData, IViewData.ICheckForChanges<CEmoteInputData>
{
    [Key(1)]
    public EmoteInputState State;

    public bool IsChangedFrom(CEmoteInputData check)
    {
        return State != check.State;
    }
}