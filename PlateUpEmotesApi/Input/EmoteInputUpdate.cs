using Controllers;
using Kitchen;
using MessagePack;

namespace PlateUpEmotesApi.Input;

[MessagePackObject]
public struct EmoteInputUpdate : ICommandUpdate
{
    [Key(0)]
    public SourceIdentifier SourceIdentifier { get; set; }
    [Key(1)]
    public EmoteInputUpdateEvent Data;

    public static implicit operator EmoteInputUpdateEvent(EmoteInputUpdate u) => u.Data;

    public static implicit operator EmoteInputUpdate(EmoteInputUpdateEvent e)
    {
        EmoteInputUpdate result = default(EmoteInputUpdate);
        result.Data = e;
        result.SourceIdentifier = InputSourceIdentifier.Identifier;
        return result;
    }
}