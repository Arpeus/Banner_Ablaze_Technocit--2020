using UnityEngine;
using UnityEngine.EventSystems;

public class HexGameUI : MonoBehaviour
{

    public HexGrid grid;

    HexCell currentCell;
    HexCell enemyCell;

    CharacterManager selectedUnit;
    CharacterManager enemyUnit;

    public void SetEditMode(bool toggle)
    {
        enabled = !toggle;
        grid.ShowUI(!toggle);
        grid.ClearPath();
        if (toggle)
        {
            Shader.EnableKeyword("HEX_MAP_EDIT_MODE");
        }
        else
        {
            Shader.DisableKeyword("HEX_MAP_EDIT_MODE");
        }
    }

    void Update()
    {
        if(GameManager.Instance.EType_Phase == PhaseType.EType_TurnPhasePlayerOne)
        {
            if (!EventSystem.current.IsPointerOverGameObject())
            {
                if (Input.GetMouseButtonDown(0))
                {
                    DoSelection();
                    if (selectedUnit != null && selectedUnit._playerNumberType == PlayerNumber.EType_PlayerTwo)
                        selectedUnit = null;
                }
                else if (selectedUnit  != null)
                {
                    if (Input.GetMouseButtonDown(1))
                    {
                        DoMove();
                    }
                    else
                    {
                        DoPathfinding();
                    }
                }
            }
        }
        if (GameManager.Instance.EType_Phase == PhaseType.EType_AttackPhase)
        {
            if (!EventSystem.current.IsPointerOverGameObject())
            {
                if (Input.GetMouseButtonDown(0))
                {
                    DoSelectionEnemy();
                    foreach (CharacterManager character in selectedUnit.m_enemyNeighbor)
                    {
                        
                        if (enemyUnit != null && enemyUnit == character)
                        {
                            enemyUnit.DoDamage(character);
                        }
                    }
                    enemyUnit = null;
                }
            }
        }
    }

    void DoSelection()
    {
        grid.ClearPath();
        UpdateCurrentCell();
        if (currentCell)
        {
            selectedUnit = currentCell.CharacterManager;
            CheckAllyUnit();
        }
    }

    void DoSelectionEnemy()
    {
        grid.ClearPath();
        UpdateEnemyCell();
        if (enemyCell)
        {
            enemyUnit = enemyCell.CharacterManager;
        }
    }

    void DoPathfinding()
    {
        if (UpdateCurrentCell())
        {
            if (currentCell && selectedUnit.IsValidDestination(currentCell))
            {
                grid.FindPath(selectedUnit.Location, currentCell, selectedUnit);
            }
            else
            {
                grid.ClearPath();
            }
        }
    }

    void DoMove()
    {
        if (grid.HasPath)
        {
            selectedUnit.Travel(grid.GetPath());
            grid.ClearPath();
        }
    }

    bool UpdateCurrentCell()
    {
        HexCell cell = grid.GetCell(Camera.main.ScreenPointToRay(Input.mousePosition));
        if (cell != currentCell)
        {
            currentCell = cell;
            return true;
        }
        return false;
    }

    bool UpdateEnemyCell()
    {
        HexCell cell = grid.GetCell(Camera.main.ScreenPointToRay(Input.mousePosition));
        if (cell != enemyCell)
        {
            enemyCell = cell;
            return true;
        }
        return false;
    }

    void CheckAllyUnit()
    {
        if (selectedUnit)
        {
            Debug.Log(selectedUnit);
        }
    }
}