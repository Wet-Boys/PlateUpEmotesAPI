using MessagePack;

namespace PlateUpEmotesApi.Emote;

[MessagePackObject]
public struct EmotingState : IEquatable<EmotingState>
{
    public static readonly EmotingState Neutral = new() { Playing = false, EmoteId = -1 };
    
    [Key(1)]
    public bool Playing;
    
    [Key(2)]
    public int EmoteId;
    
    [Key(3)]
    public int Pos;

    public string GetAnimName()
    {
        return PlateUpEmotesManager.allClipNames[EmoteId];
    }
    
    public bool Equals(EmotingState other)
    {
        return Playing == other.Playing &&
               EmoteId == other.EmoteId &&
               Pos == other.Pos;
    }

    public override bool Equals(object? obj)
    {
        return obj is EmotingState other && Equals(other);
    }

    public override int GetHashCode()
    {
        int hashcode = EmoteId;
        hashcode = (hashcode * 397) ^ (Playing ? 1 : 0);
        hashcode = (hashcode * 397) ^ Pos;
        return hashcode;
    }

    public static bool operator ==(EmotingState left, EmotingState right)
    {
        return left.Equals(right);
    }

    public static bool operator !=(EmotingState left, EmotingState right)
    {
        return !left.Equals(right);
    }
}