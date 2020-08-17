using Mirror;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class Player : NetworkBehaviour
{
    private HUDMenu m_hudMenu = null;
    private HexGridMultiplayer m_hexgrid = null;

    [Header("Game info")]
    //[SerializeField] private Dictionary<int, CharacterManager> m_characters = new Dictionary<int, CharacterManager>();
    public Dictionary<int, CharacterManager> m_characters = new Dictionary<int, CharacterManager>();
    [SerializeField] private int _countMax = 20;


    [SyncVar]
    [SerializeField] private int m_currentCount = 0;

    [SerializeField] private int m_key = 0;

    [SyncVar]
    [SerializeField] private int m_nbCavalier = 0;

    [SyncVar]
    [SerializeField] private int m_nbSwordMan = 0;

    [SyncVar]
    [SerializeField] private int m_nbLancer = 0;

    [SyncVar]
    private string m_displayName = "Loading";
    

    public HUDMenu HudMenu { get => m_hudMenu; set => m_hudMenu = value; }
    public HexGridMultiplayer Hexgrid { get => m_hexgrid; set => m_hexgrid = value; }
    public int NbCavalier { get => m_nbCavalier; set => m_nbCavalier = value; }
    public int NbSwordMan { get => m_nbSwordMan; set => m_nbSwordMan = value; }
    public int NbLancer { get => m_nbLancer; set => m_nbLancer = value; }

    public override void OnStartClient()
    {
        DontDestroyOnLoad(gameObject);

        Room._gamePlayers.Add(this);

        Debug.Log("Add Client");
    }

    public override void OnStopClient()
    {
        Room._gamePlayers.Remove(this);
        Debug.Log("Remove Client");
    }

    public void SetDisplayName(string displayName)
    {
        this.m_displayName = displayName;
    }

    // Start is called before the first frame update
    void Awake()
    {
        DontDestroyOnLoad(gameObject);
        //GameManager.Instance._players.Add(this);
        m_currentCount = _countMax;
    }

    [Command]
    public void CmdAddCharacter(int index)
    {
        RpcAddCharacter(index);
    }

    [ClientRpc]
    public void RpcAddCharacter(int index)
    {
        if (hasAuthority)
        {
            CharacterManager character = GameManager.Instance.character[index];
            if (CheckEnoughPoint(character))
            {
                m_characters.Add(m_key, character);
                m_currentCount -= character._character._unitCost;
                SetHUDMenuUI(index, 1);
                m_key++;
            }
        }
    }

    [Command]
    public void CmdRemoveCharacter(int index)
    {
        RpcRemoveCharacter(index);
    }

    [ClientRpc]
    public void RpcRemoveCharacter(int index)
    {
        if (hasAuthority)
        {
            CharacterManager character = GameManager.Instance.character[index];
            int i = 0;
            foreach (var characterManager in m_characters)
            {
                if (characterManager.Value == character)
                {
                    m_characters.Remove(i);
                    break;
                }
                i++;
            }
            m_currentCount += character._character._unitCost;
            if (m_currentCount > _countMax)
                m_currentCount = _countMax;
            SetHUDMenuUI(index, -1);
        }
    }

    [Command]
    public void CmdLoadMap()
    {
        Debug.Log("Test Cmd");
        RpcLoadMap();
    }

    [ClientRpc]
    public void RpcLoadMap()
    {
        if(hasAuthority)
        {
            Debug.Log("Création Map");
            m_hexgrid.LoadMapAwake();
        }
    }

    public int GetAvailablePoint()
    {
        return m_currentCount;
    }

    public int GetMaxPoint()
    {
        return _countMax;
    }

    public bool CheckCavalier()
    {
        if (NbCavalier == 0)
            return false;
        return true;
    }

    public bool CheckSwordMan()
    {
        if (NbSwordMan == 0)
            return false;
        return true;
    }

    public bool CheckLancer()
    {
        if (NbLancer == 0)
            return false;
        return true;
    }

    public bool CheckEnoughPoint(CharacterManager character)
    {
        return m_currentCount - character._character._unitCost >= 0;
    }

    private void SetHUDMenuUI(int index, int valueToAdd)
    {
        switch (index)
        {
            case 0:
                NbCavalier += valueToAdd;
                break;
            case 1:
                NbSwordMan += valueToAdd;
                break;
            case 2:
                NbLancer += valueToAdd;
                break;
        }
        m_hudMenu.SetTextAvailablePoint();
        m_hudMenu.DisplayNumberTroop(index);
        m_hudMenu.CheckCavalier(GameManager.Instance.character[0]);
        m_hudMenu.CheckSwordMan(GameManager.Instance.character[1]);
        m_hudMenu.CheckLancer(GameManager.Instance.character[2]);
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

 
}
