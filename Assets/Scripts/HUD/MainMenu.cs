using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainMenu : MonoBehaviour
{
    [SerializeField] private NetworkManagerLobby m_networkManager = null;

    [Header("UI")]
    [SerializeField] private GameObject m_landingPagePanel = null;

    public void HostLobby()
    {
        m_networkManager.StartHost();

        m_landingPagePanel.SetActive(false);
    }
}

