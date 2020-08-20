using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class HUDInGame : MonoBehaviour
{
    [Header("UI Spawn")]
    [SerializeField] private GameObject[] m_placeBtnSpawn;
    [SerializeField] private GameObject[] m_btnSpawn;


    // Start is called before the first frame update
    void Start()
    {
        AddButton(0);
    }

    void AddButtonSpawn(int index, int nbTroop)
    {
        int i = 0;
        foreach (GameObject go in m_placeBtnSpawn)
        {
            if (go.GetComponentInChildren<Button>() == null)
            {
                GameObject btnSpawn = Instantiate(m_btnSpawn[index], m_placeBtnSpawn[i].transform);
                btnSpawn.GetComponentInChildren<TextMeshProUGUI>().SetText(nbTroop.ToString());
                break;
            }
            i++;
        }
    }

    public void ChangeState(int indexPlayer)
    {
        GameManager.Instance.EType_Phase = PhaseType.EType_SpawnPhasePlayerTwo;
        RemoveSpawnButton();
        AddButton(indexPlayer);
    }

    public void RemoveSpawnButton()
    {
        foreach (ButtonSpawn btnSpawn in FindObjectsOfType<ButtonSpawn>())
        {
            btnSpawn.Destroy();  
        }
    }

    public void AddButton(int indexPlayer)
    {
        if (GameManager.Instance._players[0].NbCavalier != 0)
        {
            AddButtonSpawn(0, GameManager.Instance._players[indexPlayer].NbCavalier);
        }
        if (GameManager.Instance._players[0].NbSwordMan != 0)
        {
            AddButtonSpawn(1, GameManager.Instance._players[indexPlayer].NbSwordMan);
        }
        if (GameManager.Instance._players[0].NbLancer != 0)
        {
            AddButtonSpawn(2, GameManager.Instance._players[indexPlayer].NbLancer);
        }
    }
}
