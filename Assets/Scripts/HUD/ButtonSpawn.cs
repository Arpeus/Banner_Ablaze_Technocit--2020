using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ButtonSpawn : MonoBehaviour
{
    public int index;
    [SerializeField] private HexGrid m_hexGrid = null;
    public TextMeshProUGUI _currentNumberTroop = null;
    private Player player;
    int tmpNbTroop;

    public Player Player { get => player; set => player = value; }

    void Start()
    {
        m_hexGrid = FindObjectOfType<HexGrid>();
        _currentNumberTroop.SetText(player.Nbtroops[index].ToString());
        tmpNbTroop = player.Nbtroops[index];
    }

    public void SetPrefab(int index)
    {
        m_hexGrid.unitPrefab = GameManager.Instance._characters[index];
    }

    public bool Decrement(int index)
    {
        tmpNbTroop--;
        _currentNumberTroop.SetText(tmpNbTroop.ToString());
        if (tmpNbTroop == 0)
        {
            GetComponentInChildren<Button>().interactable = false;
            return true;
        }

        return false;
    }

    public void Destroy()
    {
        DestroyImmediate(gameObject);
    }
}
