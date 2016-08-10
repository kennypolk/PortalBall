using UnityEngine;
using Prototype.NetworkLobby;
using UnityEngine.Networking;

public class PlayerLobbyHook : LobbyHook
{
    public override void OnLobbyServerSceneLoadedForPlayer(NetworkManager manager, GameObject lobbyPlayer, GameObject gamePlayer)
    {
        var lobby = lobbyPlayer.GetComponent<LobbyPlayer>();
        var player = gamePlayer.GetComponent<Player>();

        player.name = lobby.name;
        player.Color = lobby.playerColor;
    }
}
