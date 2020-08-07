

using Mirror;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class NetworkGamePlayerLobby : NetworkBehaviour
{
    [SyncVar]
    private string m_displayName = "Loading";

    private NetworkManagerLobby room;

    private NetworkManagerLobby Room
    {
        get
        {
            if (room != null) { return room; }
            return room = NetworkManager.singleton as NetworkManagerLobby;
        }
    }

    public override void OnStartClient()
    {
        DontDestroyOnLoad(gameObject);

        Room._gamePlayers.Add(this);
    }

    public override void OnNetworkDestroy()
    {
        Room._gamePlayers.Remove(this);
    }
  
    public void SetDisplayName(string displayName)
    {
        this.m_displayName = displayName;
    }
}
