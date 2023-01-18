using Controllers;
using Kitchen;
using MessagePack;
using PlateUpEmotesApi.Input;
using UnityEngine;

namespace PlateUpEmotesApi.Emote.Systems;

public class EmoteEnjoyerView : ResponsiveObjectView<EmoteEnjoyerView.ViewData, EmoteEnjoyerView.ResponseData>
{
    public int emoteId;
    public PlayerView? playerView;

    private ViewData _data;
    private bool _isMyPlayer;

    private void Awake()
    {
        playerView ??= GetComponent<PlayerView>();
    }

    private void Update()
    {
        if (_data.Inputs.State.EmoteWheelAction == ButtonState.Pressed)
            emoteId++;
    }

    protected override void UpdateData(ViewData data)
    {
        _isMyPlayer = data.InputSource == InputSourceIdentifier.Identifier;
        _data = data;
    }

    public override bool HasStateUpdate(out IResponseData? state)
    {
        state = null;
        
        if (!_isMyPlayer)
            return false;

        state = new ResponseData
        {
            EmoteId = emoteId
        };
        return true;
    }

    protected override void UpdatePosition()
    {
        
    }

    public override void SetParent(Transform newParent, bool setActive = true)
    {
        
    }

    public override void SetPosition(UpdateViewPositionData pos)
    {
        
    }

    public override CPosition GetPosition()
    {
        return playerView!.GetPosition();
    }

    [MessagePackObject]
    public struct ViewData : ISpecificViewData, IViewData.ICheckForChanges<ViewData>
    {
        [Key(0)]
        public CEmoteInputData Inputs;
        [Key(1)]
        public int InputSource;
        
        public bool IsChangedFrom(ViewData check)
        {
            return Inputs.State != check.Inputs.State &&
                   InputSource != check.InputSource;
        }

        public IUpdatableObject GetRelevantSubview(IObjectView view) => view.GetSubView<EmoteEnjoyerView>();
    }
    
    [MessagePackObject]
    public struct ResponseData : IResponseData
    {
        [Key(0)]
        public int EmoteId;
    }
}