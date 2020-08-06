using Mirror;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class NetworkRoomPlayerLobby : NetworkBehaviour
{
    [Header("UI")]
    [SerializeField] private GameObject m_lobbyUI = null;
    [SerializeField] private TMP_Text[] m_playerNameTexts = new TMP_Text[2];
    [SerializeField] private TMP_Text[] m_playerReadyTexts = new TMP_Text[2];
    [SerializeField] private Button m_btnStartGame = null;

    [SyncVar(hook = nameof(HandleDisplayNameChanged))]
    public string _displayName = "Loading";
    [SyncVar(hook = nameof(HandleReadyStatusChanged))]
    public bool _isReady = false;

    private bool m_isLeader;

    public bool IsLeader
    {
        set
        {
            m_isLeader = value;
            m_btnStartGame.gameObject.SetActive(value);
        }
    }

    private NetworkManagerLobby room;

    private NetworkManagerLobby Room
    {
        get
        {
            if (room != null) { return room; }
            return room = NetworkManager.singleton as NetworkManagerLobby;
        }
    }

    public override void OnStartAuthority()
    {
        CmdSetDisplayName(PlayerNameInput._displayName);

        m_lobbyUI.SetActive(true);
    }

    public override void OnStartClient()
    {
        Room._roomPlayers.Add(this);

        UpdateDisplay();
    }

    public override void OnNetworkDestroy()
    {
        Room._roomPlayers.Remove(this);

        UpdateDisplay();
    }

    public void HandleReadyStatusChanged(bool oldValue, bool newValue)
    {
        UpdateDisplay();
    }

    public void HandleDisplayNameChanged(string oldValue, string newValue)
    {
        UpdateDisplay();
    }

    public void UpdateDisplay()
    {
        if(!hasAuthority)
        {
            foreach(var player in Room._roomPlayers)
            {
                if(player.hasAuthority)
                {
                    player.UpdateDisplay();
                    break;
                }
            }

            return;
        }

        for (int i = 0; i < m_playerNameTexts.Length; i++)
        {
            m_playerNameTexts[i].text = "Waiting for players";
            m_playerReadyTexts[i].text = string.Empty;
        }

        for(int i = 0; i < Room._roomPlayers.Count; i++)
        {
            m_playerNameTexts[i].text = Room._roomPlayers[i]._displayName;
            m_playerReadyTexts[i].text = Room._roomPlayers[i]._isReady ?
                "<color=green>Ready</color>" :
                "<color=red>Not Ready</color>";
        }
    }

    public void HandleReadyToStart(bool readyToStart)
    {
        if (!m_isLeader) { return; }

        m_btnStartGame.interactable = readyToStart;
    }

    [Command]
    private void CmdSetDisplayName(string displayName)
    {
        _displayName = displayName;
    }

    [Command]
    public void CmdReadyUp()
    {
        _isReady = !_isReady;

        Room.NotifyPlayersOfReadyState();
    }

    [Command]
    public void CmdStartGame()
    {
        if (Room._roomPlayers[0].connectionToClient != connectionToClient) { return;  };
    }
}
