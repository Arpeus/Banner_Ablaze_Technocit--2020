using UnityEngine;
using System.Collections;
using UnityEngine.EventSystems;

public class HexGameUI : MonoBehaviour
{

    public HexGrid grid;

    HexCell currentCell;
    HexCell enemyCell;

    CharacterManager selectedUnit;
    CharacterManager enemyUnit;

    HexMapCamera m_mainCamera;

    

    private void Awake()
    {
        m_mainCamera = FindObjectOfType<HexMapCamera>();
    }

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
            if(CheckUnitPlayed(GameManager.Instance._players[0]))
            {
                EndTurn(GameManager.Instance._players[0]);
            }
            if (!EventSystem.current.IsPointerOverGameObject())
            {
                if (Input.GetMouseButtonDown(0))
                {
                    DoSelection();
                    if (
                        selectedUnit != null && selectedUnit._playerNumberType == PlayerNumber.EType_PlayerTwo ||
                        selectedUnit != null && selectedUnit.hasAlreadyPlayed == true && selectedUnit._playerNumberType == PlayerNumber.EType_PlayerOne
                        )
                        selectedUnit = null;
                }
                else if (selectedUnit  != null && selectedUnit.hasAlreadyPlayed != true)
                {
                    if (Input.GetMouseButtonDown(1))
                    {
                        DoMove();
                        selectedUnit.Move();
                        //grid.ClearPath();
                        //UpdateCurrentCell();
                    }
                    else
                    {
                        DoPathfinding();
                    }
                }
            }
        }
        if (GameManager.Instance.EType_Phase == PhaseType.EType_TurnPhasePlayerTwo)
        {
            if (!EventSystem.current.IsPointerOverGameObject())
            {
                if (Input.GetMouseButtonDown(0))
                {
                    DoSelection();
                    if (
                        selectedUnit != null && selectedUnit._playerNumberType == PlayerNumber.EType_PlayerOne ||
                        selectedUnit != null && selectedUnit.hasAlreadyPlayed == true && selectedUnit._playerNumberType == PlayerNumber.EType_PlayerTwo
                        )
                        selectedUnit = null;
                }
                else if (selectedUnit != null)
                {
                    if (Input.GetMouseButtonDown(1))
                    {
                        DoMove();
                        selectedUnit.Wait();
                    }
                    else
                    {
                        DoPathfinding();
                    }
                }
            }
            if (CheckUnitPlayed(GameManager.Instance._players[1]))
            {
                EndTurn(GameManager.Instance._players[1]);
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
                            enemyUnit.TakeDamage(character);
                        }
                    }
                    selectedUnit.ClearEnemy();
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
        if (selectedUnit && selectedUnit._playerNumberType != PlayerNumber.EType_PlayerTwo)
        {

            m_mainCamera.transform.position = new Vector3(currentCell.Position.x, transform.position.y, currentCell.Position.z);

            Debug.Log(selectedUnit);
        }
    }

    public bool CheckUnitPlayed(Player player)
    {
        foreach(CharacterManager character in player.m_characters)
        {
            if (!character.hasAlreadyPlayed)
                return false;
        }
        return true;
    }

    private void EndTurn(Player player)
    {
        switch (GameManager.Instance.EType_Phase)
        {
            case PhaseType.EType_TurnPhasePlayerOne:
                GameManager.Instance.EType_Phase = PhaseType.EType_TurnPhasePlayerTwo;
                break;
            case PhaseType.EType_TurnPhasePlayerTwo:
                GameManager.Instance.EType_Phase = PhaseType.EType_TurnPhasePlayerOne;
                break;
        }

        foreach (CharacterManager character in player.m_characters)
        {
            character.hasAlreadyPlayed = true;
        }
        
    }
}