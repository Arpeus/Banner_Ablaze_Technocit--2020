using UnityEngine;
using System.Collections;
using UnityEngine.EventSystems;

public class HexGameUI : MonoBehaviour
{

    public HexGrid grid;

    bool enter = false;

    HexCell currentCell;
    HexCell enemyCell;
    HexCell allyCell;

    [SerializeField] private GameObject m_uniteMenu;

    [SerializeField] CharacterManager selectedUnit;
    [SerializeField] CharacterManager enemyUnit;
    [SerializeField] CharacterManager allyUnit;

    HexMapCamera m_mainCamera;

    private HUDInGame m_hudInGame;

    public GameObject _unitsMenu;



    private void Awake()
    {
        m_mainCamera = FindObjectOfType<HexMapCamera>();
        m_hudInGame = FindObjectOfType<HUDInGame>();
        
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
        /*if (GameManager.Instance.nbTour % 2 == 0 && !enter)
        {
            enter = true;
            foreach(HexCell hexcell in grid.cells)
            {
                hexcell.Visible = false;
            }
            SetVisibleAroundPlayer();
        }*/
        if (GameManager.Instance.EType_Phase == PhaseType.EType_TurnPhasePlayerOne)
        {
            if(CheckUnitPlayed(GameManager.Instance._players[0]))
            {
                EndTurn(GameManager.Instance._players[0]);
                StartTurn(GameManager.Instance._players[1]);
            }
            if (!EventSystem.current.IsPointerOverGameObject())
            {
                if (Input.GetMouseButtonDown(0))
                {
                    DoSelection();
                    if (
                        selectedUnit != null && selectedUnit._playerNumberType == PlayerNumber.EType_PlayerTwo ||
                        selectedUnit != null && selectedUnit.hasMoved == true && selectedUnit._playerNumberType == PlayerNumber.EType_PlayerOne
                        )
                        selectedUnit = null;
                }
                else if (selectedUnit  != null && selectedUnit.hasMoved != true)
                {
                    if (Input.GetMouseButtonDown(1))
                    {
                        DoMove();
                        selectedUnit.SetHasMoved(true);
                        grid.ClearPath();
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
            if (CheckUnitPlayed(GameManager.Instance._players[1]))
            {
                EndTurn(GameManager.Instance._players[1]);
                StartTurn(GameManager.Instance._players[0]);
            }
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
                else if (selectedUnit != null && selectedUnit.hasMoved != true)
                {
                    if (Input.GetMouseButtonDown(1))
                    {
                        DoMove();
                        selectedUnit.SetHasMoved(true);
                        grid.ClearPath();
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
                            selectedUnit.SetHasMoved(true);
                            enemyUnit.TakeDamage(selectedUnit);
                            selectedUnit.SetHasAlreadyPlayed(true);
                            selectedUnit.ClearEnemy();
                            selectedUnit.SetStateTurn();
                            break;
                        }
                    }
                    foreach (CharacterManager character in selectedUnit.m_enemyNeighborRange)
                    {
                        if (enemyUnit != null && enemyUnit == character)
                        {
                            selectedUnit.SetHasMoved(true);
                            enemyUnit.TakeDamageRange(selectedUnit);
                            selectedUnit.SetHasAlreadyPlayed(true);
                            selectedUnit.ClearEnemy();
                            selectedUnit.SetStateTurn();
                            break;
                        }
                    }

                    enemyUnit = null;
                }
            }
        }
        if (GameManager.Instance.EType_Phase == PhaseType.EType_HealPhase)
        {
            if (!EventSystem.current.IsPointerOverGameObject())
            {
                if (Input.GetMouseButtonDown(0))
                {
                    DoSelectionAlly();
                    CharacterHealer tmpCharacter = selectedUnit as CharacterHealer;
                    foreach (CharacterManager character in tmpCharacter._allyNeighbor)
                    {
                        if (allyUnit != null && allyUnit == character && allyUnit != selectedUnit)
                        {
                            selectedUnit.SetHasMoved(true);
                            allyUnit.ReceiveHeal(selectedUnit);
                            selectedUnit.SetHasAlreadyPlayed(true);
                            tmpCharacter.ClearAlly();
                            break;
                        }
                    }

                    allyUnit = null;
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

    void DoSelectionAlly()
    {
        grid.ClearPath();
        UpdateAllyCell();
        if (allyCell)
        {
            allyUnit = allyCell.CharacterManager;
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

            m_mainCamera._targetPosition = new Vector3(currentCell.Position.x, transform.position.y, currentCell.Position.z);

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

    bool UpdateAllyCell()
    {
        HexCell cell = grid.GetCell(Camera.main.ScreenPointToRay(Input.mousePosition));
        if (cell != allyCell)
        {
            allyCell = cell;
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
        if(!selectedUnit)
        {
            _unitsMenu.SetActive(true);
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
                GameManager.Instance.nbTour++;
                break;
        }

        SetHasAlreadyPlayedEquip(player);
    }

    public void StartTurn(Player player)
    {
        foreach (CharacterManager character in player.m_characters)
        {
            character.SetHasAlreadyPlayed(false);
            character.SetHasAttacked(false);
            character.SetHasMoved(false);
        }
    }

    public void SetHasAlreadyPlayedEquip(Player player)
    {
        foreach (CharacterManager character in player.m_characters)
        {
            character.SetHasAlreadyPlayed(true);
        }
    }

    private void SetVisibleAroundPlayer()
    {
        switch (GameManager.Instance.EType_Phase)
        {
            case PhaseType.EType_TurnPhasePlayerOne:
                SetVisibleAroundPlayer(GameManager.Instance._players[0]);
                break;
            case PhaseType.EType_TurnPhasePlayerTwo:
                SetVisibleAroundPlayer(GameManager.Instance._players[1]);
                break;
        }
    }

    private void SetVisibleAroundPlayer(Player player)
    {
        foreach(CharacterManager character in player.m_characters)
        {
            character.SetVisibleAround();
        } 
    }
}

