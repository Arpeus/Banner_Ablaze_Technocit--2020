using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;
    public int nbTour = 1;
    public List<Player> _players;
    public List<CharacterManager> _characters;

    public List<string> _nameAnimationattack;
    public string filepathMap = "";
    public PhaseType EType_Phase;
    public AnimState EType_StateAnim;
    [Header("Value Forest")]
    public int bonusForestDefense = 2;
    public int bonusForestDodge = -1;
    public int malusForestDefense = 10;
    [Header("Value Swamp")]
    public int malusSwampMove = -2;
    [Header("Value River")]
    public int malusRiverMove = -1;
    public int malusRiverDodge = -5;
    [Header("Value Castle")]
    public int bonusCastleDefense = 3;
    public int bonusCastleDodge = 5;
    [Header("Value Night")] 
    public int bonusNightDefense = 1;
    public int bonusNightDodge = 10;

    // Start is called before the first frame update
    void Awake()
    {
        if (Instance == null)
        {
            DontDestroyOnLoad(gameObject);
            Instance = this;
        }
        else
        {
            if (Instance != this)
            {
                Destroy(gameObject);
            }
        }
    }




}
