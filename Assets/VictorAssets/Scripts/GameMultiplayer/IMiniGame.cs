using Unity.Netcode;

public interface IMiniGame
{
    void SetMissionObject(NetworkObject missionObj);
    void SetPlayer(SinglePlayer.PlayerController player);
}
