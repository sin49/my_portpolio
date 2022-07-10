using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;

public class Ntworkmanager : NetworkManager
{
    int playernum = 0;
    public override void OnServerAddPlayer(NetworkConnection conn)
    {
        GameObject player = Instantiate(playerPrefab);
        player.GetComponent<playercontroller>().playernum = playernum;
        playernum++;
        NetworkServer.AddPlayerForConnection(conn, player);
        Debug.Log("playernum:"+playernum + "connenction");
    }
}
