using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class OpenChoseMap : MonoBehaviour
{
    public SaveLoadMenu _saveLoadMenu;
    public Button _btnValidate;
  
    void Awake()
    {
        _btnValidate.interactable = false;
    }

    void Start()
    {
        _saveLoadMenu.Open();
    }

    public void SelectedMap()
    {
        _btnValidate.interactable = true;
    }

    public void ButtonOkPressed()
    {
        GameManager.Instance.EType_Phase = PhaseType.EType_PickPhase;
        SceneManager.LoadScene(2);
    }
}
