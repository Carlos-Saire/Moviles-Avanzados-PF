using Unity.Netcode;
using UnityEngine;

public enum GameState
{
    Lobby,
    Game
}

public class GameStateManager : NetworkBehaviour
{
    public static GameStateManager Instance;

    public NetworkVariable<GameState> CurrentState = new NetworkVariable<GameState>(GameState.Lobby);

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        DontDestroyOnLoad(gameObject);
    }

    public void SetState(GameState newState)
    {
        if (!IsServer) return;
        CurrentState.Value = newState;
        Debug.Log("GAME STATE → " + newState);
    }
}
