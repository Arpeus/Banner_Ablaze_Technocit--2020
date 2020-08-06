using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class JoinLobbyMenu : MonoBehaviour
{
    [SerializeField] private NetworkManagerLobby m_networkManager = null;

    [SerializeField] private GameObject m_landingPanePanel = null;
    [SerializeField] private TMP_InputField m_ipAdressInputField = null;
    [SerializeField] private Button m_btnJoin = null;
    
    void OnEnable()
    {
        NetworkManagerLobby.OnClientConnected += HandleClientConnected;
        NetworkManagerLobby.OnClientDisconnected += HandleClientDisconnected;
    }

    void OnDisable()
    {
        NetworkManagerLobby.OnClientConnected -= HandleClientConnected;
        NetworkManagerLobby.OnClientDisconnected -= HandleClientDisconnected;
    }

    public void JoinLobby()
    {
        string ipAddress = m_ipAdressInputField.text;

        m_networkManager.networkAddress = ipAddress;
        m_networkManager.StartClient();

        m_btnJoin.interactable = false;
    }

    private void HandleClientConnected()
    {
        m_btnJoin.interactable = true;

        gameObject.SetActive(false);
        m_landingPanePanel.SetActive(false);
    }

    private void HandleClientDisconnected()
    {
        m_btnJoin.interactable = true;
    }
}
