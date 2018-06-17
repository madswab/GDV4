using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class LobbyPlayer : NetworkLobbyPlayer {

    [SyncVar]
    public string playerName = "player";


    public override void OnClientEnterLobby(){
        base.OnClientEnterLobby();
        NetworkManagerV2.single.AddLobbyPlayer(this);
        //playerName = Login.instance.nickName;

        playerName = Random.Range(0, 100).ToString(); /////  name player
        StartCoroutine(DelayReady());
    }

    private IEnumerator DelayReady(){
        yield return new WaitForSeconds(1f);

        if (isLocalPlayer && isServer == false){
            SendReadyToBeginMessage();
        }

        if (isServer){
            NetworkManagerUI.single.startGameButton.SetActive(true);
        }

    }

    [Command]
    public void CmdBroadcastPlayername(uint playerID){
        LobbyPlayerInfo lpi = NetworkManagerV2.single.GetLobbyPlayer(playerID);

        if (lpi != null){
            Debug.Log(playerName);
            RpcBroadcastPlayername(playerID, lpi.lobbyPlayer.playerName);

        }
    }

    [ClientRpc]
    public void RpcBroadcastPlayername(uint playerID, string name){
        LobbyPlayerInfo lpi = NetworkManagerV2.single.GetLobbyPlayer(playerID);
        lpi.lobbyPlayer.playerName = name;
        lpi.UiIcon.Init(name);
    }
}
