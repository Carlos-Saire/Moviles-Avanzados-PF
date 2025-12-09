using Unity.Netcode;

public interface IMiniGame
{
    void SetMissionObject(MissionTrigger missionObj);
    void SetPlayer(SinglePlayer.PlayerController player);
}
