using UnityEngine;
using System.Collections;
using UnityEngine.EventSystems;
using System;

public class HexGameUI : MonoBehaviour
{

    public HexGrid grid;

    bool enter = false;

    HexCell currentCell;
    HexCell enemyCell;
    HexCell allyCell;

    [SerializeField] private GameObject m_uniteMenu;

    public CharacterManager selectedUnit;
    [SerializeField] CharacterManager enemyUnit;
    [SerializeField] CharacterManager allyUnit;

    public HexMapCamera m_mainCamera;

    private HUDInGame m_hudInGame;

    public GameObject _unitsMenu;

    public CharacterManager _characterAnimator;

    private void Awake()
    {
        m_mainCamera = FindObjectOfType<HexMapCamera>();
        m_hudInGame = FindObjectOfType<HUDInGame>();
        grid = FindObjectOfType<HexGrid>();
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
        if (GameManager.Instance.nbTour != 0 && GameManager.Instance.nbTour % 2 == 0 && !enter)
        {
            enter = true;
            if(GameManager.Instance.cycle == CyclePhase.PhaseDay)
            {
                GameManager.Instance.cycle = CyclePhase.PhaseNight;
                GameManager.Instance.SetNight();
                grid.Initialise();
            }
            else
            {
                GameManager.Instance.cycle = CyclePhase.PhaseDay;
                GameManager.Instance.SetDay();
                grid.Increase();
            }

        }
        if(GameManager.Instance.EType_StateAnim == AnimState.EType_IsNotPlaying)
        {
            if (GameManager.Instance.EType_Phase == PhaseType.EType_TurnPhasePlayerOne)
            {
                CheckEndGame();
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
                        else
                            SoundManager.PlaySound(Sound.ChooseUnit);
                    }
                    else if (selectedUnit  != null && selectedUnit.hasMoved != true)
                    {
                        if (Input.GetMouseButtonDown(1))
                        {
                            SoundManager.PlaySound(Sound.Move);
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
                CheckEndGame();
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
                        else
                            SoundManager.PlaySound(Sound.ChooseUnit);
                    }
                    else if (selectedUnit != null && selectedUnit.hasMoved != true)
                    {
                        if (Input.GetMouseButtonDown(1))
                        {
                            SoundManager.PlaySound(Sound.Move);
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
                                SoundManager.PlaySound(Sound.KabukiStick);
                                selectedUnit.SetHasMoved(true);
                                HUDInGame.Instance._panelPreview.SetHUDPreview(selectedUnit, enemyUnit, false);
                                break;
                            }
                        }
                        foreach (CharacterManager character in selectedUnit.m_enemyNeighborRange)
                        {
                            if (enemyUnit != null && enemyUnit == character)
                            {
                                SoundManager.PlaySound(Sound.KabukiStick);
                                selectedUnit.SetHasMoved(true);
                                HUDInGame.Instance._panelPreview.SetHUDPreview(selectedUnit, enemyUnit, true);
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

        
    }

    private bool CheckEndGame()
    {
        foreach(Player player in GameManager.Instance._players)
        {
            if (player.m_characters.Count == 0)
                EndGame(player);
        }
        return false;   
    }

    private void EndGame(Player player)
    {
        GameManager.Instance.EType_Phase = PhaseType.EType_EndGamePhase;
        HUDInGame.Instance.ActivePanelVictory();
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
        if (selectedUnit)
        {
            //SoundManager.PlaySound(Sound.ChooseUnit);

            //m_mainCamera.transform.position = new Vector3(currentCell.Position.x, transform.position.y, currentCell.Position.z);
        }
        if(!selectedUnit)
        {
           HUDInGame.Instance.UniteMenu.SetActive(true);
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

    public void EndTurn()
    {
        if (GameManager.Instance.EType_Phase == PhaseType.EType_TurnPhasePlayerOne)
        {
            EndTurn(GameManager.Instance._players[0]);
            StartTurn(GameManager.Instance._players[1]);
        }
        else
        {
            EndTurn(GameManager.Instance._players[1]);
            StartTurn(GameManager.Instance._players[0]);
        }
           
    }

    private void EndTurn(Player player)
    {
        if (GameManager.Instance.EType_Phase == PhaseType.EType_TurnPhasePlayerOne) HUDInGame.Instance.SetActiveTurnPlayer(false);
        else HUDInGame.Instance.SetActiveTurnPlayer(true);
        switch (GameManager.Instance.EType_Phase)
        {
            case PhaseType.EType_TurnPhasePlayerOne:
                GameManager.Instance.EType_Phase = PhaseType.EType_TurnPhasePlayerTwo;
                break;
            case PhaseType.EType_TurnPhasePlayerTwo:
                GameManager.Instance.EType_Phase = PhaseType.EType_TurnPhasePlayerOne;
                enter = false;
                GameManager.Instance.nbTour++;
                break;
        }

        SetHasAlreadyPlayedEquip(player);
    }

    public void StartTurn(Player player)
    {
        foreach (CharacterManager character in player.m_characters)
        {
            character.SetSpriteCounterAttack(false);
            character.SetHasAlreadyPlayed(false);
            character.SetHasAttacked(false);
            character.SetHasMoved(false);
        }
    }

    public void SetHasAlreadyPlayedEquip(Player player)
    {
        foreach (CharacterManager character in player.m_characters)
        {
            if (character.hasAttacked) character.SetSpriteCounterAttack(false);
            else character.SetSpriteCounterAttack(true);
            character.SetHasAlreadyPlayed(true);
            character.SetColorAlbedo(character.AlbedoColorTeam);
            character.DisableHighlight();
        }
    }

    void OnHoverEnter()
    {

        Debug.Log("On Unit");
    }

    void OnHoverExit()
    {
        Debug.Log("out Unit");
    }
}

