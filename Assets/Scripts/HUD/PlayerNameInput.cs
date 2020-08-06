using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class PlayerNameInput : MonoBehaviour
{
    [Header("UI")]
    [SerializeField] private TMP_InputField m_nameInputField = null;
    [SerializeField] private Button m_btnContinue = null;

    public static string _displayName { get; private set; }

    private const string PlayerPrefsNameKey = "PlayerName";

    private void Start()
    {
        SetUpInputField();
    }

    private void SetUpInputField()
    {
        if (!PlayerPrefs.HasKey(PlayerPrefsNameKey))
        {
            return;

        }
        string defaultName = PlayerPrefs.GetString(PlayerPrefsNameKey);

        m_nameInputField.text = defaultName;

        SetPlayerName(defaultName);
    }

    public void SetPlayerName(string name)
    {
        m_btnContinue.interactable = !string.IsNullOrEmpty(name);
    }

    public void SavePlayerName()
    {
        _displayName = m_nameInputField.text;

        PlayerPrefs.SetString(PlayerPrefsNameKey, _displayName);
    }
}
