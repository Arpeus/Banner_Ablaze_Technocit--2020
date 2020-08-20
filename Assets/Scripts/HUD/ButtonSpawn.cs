using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ButtonSpawn : MonoBehaviour
{
    public int index;
    [SerializeField] private HexGrid m_hexGrid = null;
    public TextMeshProUGUI _currentNumberTroop = null;  
    void Awake()
    {
        m_hexGrid = FindObjectOfType<HexGrid>();
    }

    public void SetPrefab(int index)
    {
        m_hexGrid.unitPrefab = GameManager.Instance._characters[index];
    }

    public bool Decrement(int index)
    {
        int tmpNbTroop = int.Parse(_currentNumberTroop.text);
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
