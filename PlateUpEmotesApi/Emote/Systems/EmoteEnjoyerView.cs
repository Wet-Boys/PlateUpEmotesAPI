using Controllers;
using Kitchen;
using MessagePack;
using PlateUpEmotesApi.Anim;
using PlateUpEmotesApi.Input;
using UnityEngine;

namespace PlateUpEmotesApi.Emote.Systems;

public class EmoteEnjoyerView : ResponsiveObjectView<EmoteEnjoyerView.ViewData, EmoteEnjoyerView.ResponseData>
{
    public bool emoting;
    public int emoteId;
    public PlayerView? playerView;
    public BoneMapper? boneMapper;

    private ViewData _data;
    private bool _isMyPlayer;

    private void Awake()
    {
        playerView ??= GetComponent<PlayerView>();
        boneMapper ??= GetComponentInChildren<BoneMapper>();
    }

    private int RandomEmote()
    {
        int rand = UnityEngine.Random.Range(0, PlateUpEmotesManager.allClipNames.Count);
        
        while (PlateUpEmotesManager.blacklistedClips.Contains(rand))
            rand = UnityEngine.Random.Range(0, PlateUpEmotesManager.allClipNames.Count);

        return rand;
    }

    private void Update()
    {
        // Todo: implement emote wheel state

        if (_data.Inputs.State.EmoteWheelAction == ButtonState.Pressed)
        {
            emoteId = RandomEmote();
        }
    }

    protected override void UpdateData(ViewData data)
    {
        _isMyPlayer = data.InputSource == InputSourceIdentifier.Identifier;

        _data = data;
        
        if (emoteId != _data.EmoteEnjoyer.State.EmoteId)
            boneMapper!.PlayAnim(PlateUpEmotesManager.allClipNames[emoteId], -2);
        
        emoteId = _data.EmoteEnjoyer.State.EmoteId;
    }

    public override bool HasStateUpdate(out IResponseData? state)
    {
        state = null;
        
        if (!_isMyPlayer || emoteId == _data.EmoteEnjoyer.State.EmoteId)
            return false;

        state = new ResponseData
        {
            EmoteId = emoteId,
            Playing = emoting
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
        public CEmoteEnjoyer EmoteEnjoyer;
        [Key(2)]
        public int InputSource;
        [Key(3)]
        public int PlayerId;

        public bool IsChangedFrom(ViewData check)
        {
            return Inputs.State != check.Inputs.State ||
                   EmoteEnjoyer.State != check.EmoteEnjoyer.State ||
                   InputSource != check.InputSource ||
                   PlayerId != check.PlayerId;
        }

        public IUpdatableObject GetRelevantSubview(IObjectView view) => view.GetSubView<EmoteEnjoyerView>();
    }
    
    [MessagePackObject]
    public struct ResponseData : IResponseData
    {
        [Key(0)]
        public int EmoteId;
        [Key(1)]
        public bool Playing;
    }
}