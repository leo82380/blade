using System.Collections.Generic;

public class PlayerStateMachine
{
    public PlayerState CurrentState { get; private set; }
    public Dictionary<PlayerStateEnum, PlayerState> stateDictionary;

    private Player _player;

    public PlayerStateMachine()
    {
        stateDictionary = new Dictionary<PlayerStateEnum, PlayerState>();
    }

    public void Initialize(PlayerStateEnum startState, Player player)
    {
        _player = player;
        CurrentState = stateDictionary[startState];
        CurrentState.Enter();
    }

    public void ChangeState(PlayerStateEnum newState)
    {
        if (_player.CanStateChangeable == false) return;

        CurrentState.Exit(); //현재 상태를 나가고
        CurrentState = stateDictionary[newState]; //새로운 상태로 업데이트 하고
        CurrentState.Enter(); //새로운 상태로 진입한다.
    }

    public void AddState(PlayerStateEnum stateEnum, PlayerState state)
    {
        stateDictionary.Add(stateEnum, state);
    }
}
