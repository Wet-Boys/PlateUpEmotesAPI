using Kitchen;
using KitchenMods;
using PlateUpEmotesApi.Input;
using PlateUpEmotesApi.Logging;
using PlateUpEmotesApi.Utils;
using ILogger = PlateUpEmotesApi.Logging.Loggers.ILogger;

namespace PlateUpEmotesApi.Players;

public class EmotePlayerManager : GenericSystemBase, IModSystem
{
    private static readonly ILogger Logger = LogUtils.CreateDefaultLogger("EmotePlayerManager");
    public static EmotePlayerManager? Instance;

    private readonly Dictionary<Player, EmoteInputQueue> _inputQueues = new();

    protected override void Initialise()
    {
        Instance = this;
    }

    protected override void OnUpdate()
    {
        foreach (var (player, inputQueue) in _inputQueues)
            UpdateEmotePlayer(player, inputQueue);
    }

    internal void ReportNewInput(Player player, EmoteInputState inputState)
    {
        GetInputQueue(player).Enqueue(inputState);
    }

    private void UpdateEmotePlayer(Player player, EmoteInputQueue inputQueue)
    {
        inputQueue.ApplyUpdates();

        if (player.State == PlayerState.Null)
            return;

        if (EntityManager.RequireComponent<CEmoteInputData>(player.Entity, out _))
        {
            EntityManager.SetComponentData(player.Entity, new CEmoteInputData
            {
                State = inputQueue.State
            });
        }
    }

    private EmoteInputQueue GetInputQueue(Player player)
    {
        if (_inputQueues.TryGetValue(player, out var inputQueue))
            return inputQueue;
        
        Logger.Debug($"No EmoteInputQueue exists for player {player.Index}! Creating a new one...");
        
        _inputQueues[player] = new EmoteInputQueue();
        return _inputQueues[player];

    }
}