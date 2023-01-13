using Controllers;
using MessagePack;

namespace PlateUpEmotesApi.Input;

[MessagePackObject]
public struct EmoteInputState : IEquatable<EmoteInputState>
{
    public static readonly EmoteInputState Neutral = new()
    {
        EmoteWheelAction = ButtonState.Up
    };
    
    [Key(1)]
    public ButtonState EmoteWheelAction;

    public EmoteInputState ApplyUpdatedState(EmoteInputState update, bool aggregation = false)
    {
        EmoteInputState result = default(EmoteInputState);
        result.EmoteWheelAction = CombineButtonState(EmoteWheelAction, update.EmoteWheelAction, aggregation);
        return result;
    }

    private ButtonState CombineButtonState(ButtonState prevState, ButtonState nextState, bool aggregation = false)
    {
        bool prevActive = prevState is ButtonState.Held or ButtonState.Pressed;
        bool nextActive = nextState is ButtonState.Held or ButtonState.Pressed;

        if (prevState is ButtonState.Consumed)
            return nextActive ? ButtonState.Consumed : ButtonState.Up;

        if (aggregation)
            return nextActive | prevActive ? ButtonState.Held : ButtonState.Up;

        return nextActive
            ? prevActive
                ? ButtonState.Held
                : ButtonState.Pressed
            : prevActive
                ? ButtonState.Released
                : ButtonState.Up;
    }

    public bool Equals(EmoteInputState other)
    {
        return EmoteWheelAction == other.EmoteWheelAction;
    }

    public override bool Equals(object? obj)
    {
        return obj is EmoteInputState other && Equals(other);
    }

    public override int GetHashCode()
    {
        int hashCode = (int)EmoteWheelAction;
        return hashCode;
    }

    public static bool operator ==(EmoteInputState left, EmoteInputState right)
    {
        return left.Equals(right);
    }

    public static bool operator !=(EmoteInputState left, EmoteInputState right)
    {
        return !left.Equals(right);
    }
}