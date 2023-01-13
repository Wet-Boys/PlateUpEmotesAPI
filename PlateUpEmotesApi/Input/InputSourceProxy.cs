using Controllers;
using PlateUpEmotesApi.Logging;
using PlateUpEmotesApi.Systems;
using PlateUpEmotesApi.Utils;
using Sirenix.Utilities;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.LowLevel;
using ILogger = PlateUpEmotesApi.Logging.Loggers.ILogger;

namespace PlateUpEmotesApi.Input;

internal class InputSourceProxy
{
    private static readonly ILogger Logger = LogUtils.CreateDefaultLogger("InputSourceProxy");
    
    private static InputSourceProxy? _instance;
    public static InputSourceProxy Instance => _instance ?? throw new Exception("Failed to get instance!");
    
    internal InputSource Target { get; }
    private readonly Dictionary<int, CachedState> _stateCache = new();
    private Dictionary<int, PlayerData> _players;
    private Dictionary<int, EmoteInputState> _inputStates;

    public event EventHandler<EmoteInputUpdateEvent> OnEmoteInputUpdate = delegate { };
    
    internal InputSourceProxy(InputSource target)
    {
        Target = target;
        
        _instance?.Dispose();
        _instance = this;

        OnEmoteInputUpdate += EmoteRouterManager.OnEmoteInputUpdate;
        
        Logger.Debug("New InputSource instantiated!");
    }

    internal void Setup(bool registerAsDefault)
    {
        Logger.Debug("Setup");

        _inputStates = new Dictionary<int, EmoteInputState>();
        FetchPrivateData();
    }

    internal void Destroy()
    {
        Logger.Debug("Destroy");
    }

    public void InputSourceUpdate(int playerId, UserInputData data)
    {
        InputActionMap map = data.Map;
        EmoteInputState oldState = _inputStates[playerId];
        EmoteInputState newState = GetEmoteInputData(map, oldState);
        _inputStates[playerId] = newState;
        
        if (!map.devices.HasValue || map.devices.GetValueOrDefault().Count <= 0)
            return;
            
        SetInputUpdate(playerId, newState);
    }

    private void SetInputUpdate(int playerId, EmoteInputState state)
    {
        float realTime = Time.realtimeSinceStartup;
        if (!_stateCache.TryGetValue(playerId, out var cachedState) || !(realTime - cachedState.Time < 3f) || !(state == cachedState.State))
        {
            _stateCache[playerId] = new CachedState
            {
                State = state,
                Time = realTime
            };

            OnEmoteInputUpdate(null, new EmoteInputUpdateEvent
            {
                User = playerId,
                State = state
            });
        }
    }

    private EmoteInputState GetEmoteInputData(InputActionMap map, EmoteInputState oldState)
    {
        EmoteInputState result = default(EmoteInputState);
        result.EmoteWheelAction = GetButtonState(map, EmoteControls.EmotesWheel, oldState.EmoteWheelAction);
        return result;
    }
    
    private ButtonState GetButtonState(InputActionMap map, string action, ButtonState oldState)
    {
        var inputAction = map.FindAction(action);
        float value = inputAction.ReadValue<float>();
        bool pressed = value > 0.5f;
        
        if (oldState == ButtonState.Consumed)
        {
            return pressed ? ButtonState.Consumed : ButtonState.Up;
        }
        if (pressed)
        {
            if (oldState != ButtonState.Held && oldState != ButtonState.Pressed)
            {
                return ButtonState.Pressed;
            }
            return ButtonState.Held;
        }
        if (oldState != 0 && oldState != ButtonState.Released)
        {
            return ButtonState.Released;
        }
        return ButtonState.Up;
    }

    internal void NewDeviceUsed(InputControl control, InputEventPtr eventPtr)
    {
        FetchPrivateData();

        _players
            .Select(kvp => kvp.Key)
            .Where(playerId => !_inputStates.ContainsKey(playerId))
            .ForEach(playerId => _inputStates.Add(playerId, default));
    }

    private void FetchPrivateData()
    {
        _players = Target.GetField<Dictionary<int, PlayerData>>("Players");
    }

    private void Dispose()
    {
        
    }
    
    private struct CachedState
    {
        public EmoteInputState State;
        public float Time;
    }
}