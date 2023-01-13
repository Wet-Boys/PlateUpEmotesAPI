using PlateUpEmotesApi.Input;

namespace PlateUpEmotesApi.Players;

public class EmoteInputQueue
{
    private const int QueueAggregationLimit = 5;

    private readonly Queue<EmoteInputState> _queue = new();
    private bool HasUpdates => _queue.Count > 0;
    private bool QueueOverLimit => _queue.Count > QueueAggregationLimit;

    public EmoteInputState State;

    public void ApplyUpdates()
    {
        if (HasUpdates)
        {
            if (QueueOverLimit)
            {
                EmoteInputState update = EmoteInputState.Neutral;
                
                foreach (var state in _queue)
                    update = update.ApplyUpdatedState(state, true);

                State = State.ApplyUpdatedState(update);
                _queue.Clear();
            }
            else
            {
                State = State.ApplyUpdatedState(_queue.Dequeue());
            }
        }
        else
        {
            State = State.ApplyUpdatedState(State);
            _queue.Clear();
        }
    }

    public void Enqueue(EmoteInputState update)
    {
        _queue.Enqueue(update);
    }
}