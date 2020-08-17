using UnityEngine;
using UnityEngine.UI;
using System;
using System.IO;

public class SaveLoadMenuMultiplayer : MonoBehaviour
{

    const int mapFileVersion = 5;

    public Text menuLabel, actionButtonLabel;

    public InputField nameInput;

    public RectTransform listContent;

    public SaveLoadItemMultiplayer itemPrefab;

    public HexGridMultiplayer hexGrid;

    private const string filePath = "./Map/";

    public void Close()
    {
        gameObject.SetActive(false);
        //HexMapCamera.Locked = false;
    }

    public void SelectItem(string name)
    {
        nameInput.text = name;
    }

    void FillList()
    {
        for (int i = 0; i < listContent.childCount; i++)
        {
            Destroy(listContent.GetChild(i).gameObject);
        }
        if (!Directory.Exists(filePath))
        {
            //if it doesn't, create it
            Directory.CreateDirectory(filePath);
        }
        string[] paths =
            Directory.GetFiles(filePath, "*.map");
        Array.Sort(paths);
        for (int i = 0; i < paths.Length; i++)
        {
            SaveLoadItemMultiplayer item = Instantiate(itemPrefab);
            item.menu = this;
            item.MapName = Path.GetFileNameWithoutExtension(paths[i]);
            item.transform.SetParent(listContent, false);
        }
    }

    string GetSelectedPath()
    {
        string mapName = nameInput.text;
        if (mapName.Length == 0)
        {
            return null;
        }
        return Path.Combine(filePath, mapName + ".map");
    }

    void Load(string path)
    {
        if (!File.Exists(path))
        {
            Debug.LogError("File does not exist " + path);
            return;
        }
        using (BinaryReader reader = new BinaryReader(File.OpenRead(path)))
        {
            int header = reader.ReadInt32();
            if (header <= mapFileVersion)
            {
                hexGrid.Load(reader, header);
                HexMapCamera.ValidatePosition();
            }
            else
            {
                Debug.LogWarning("Unknown map format " + header);
            }
        }
    }

    public void OpenLoadMenu()
    { 
        menuLabel.text = "Load Map";
        actionButtonLabel.text = "Load";
        FillList();
        gameObject.SetActive(true);
    }
}