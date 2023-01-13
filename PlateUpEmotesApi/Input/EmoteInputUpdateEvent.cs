using MessagePack;

namespace PlateUpEmotesApi.Input;

[MessagePackObject]
public struct EmoteInputUpdateEvent
{
    [Key(0)]
    public int User;
    [Key(1)]
    public EmoteInputState State;
}