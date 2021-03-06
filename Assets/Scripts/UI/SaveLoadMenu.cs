﻿using UnityEngine;
using UnityEngine.UI;
using System;
using System.IO;
using TMPro;

public class SaveLoadMenu : MonoBehaviour
{

    const int mapFileVersion = 5;

    public TextMeshProUGUI menuLabel, actionButtonLabel;

    public InputField nameInput;

    public RectTransform listContent;

    public SaveLoadItem itemPrefab;

    public HexGrid hexGrid;

    bool saveMode;

    private const string filePath = "./Map/";

    void Awake()
    {
        hexGrid = FindObjectOfType<HexGrid>();
    }

    public void Open()
    {
        menuLabel.text = "Load Map";
        actionButtonLabel.text = "Load";
        FillList();
    }

    public void SelectMap()
    {
        GameManager.Instance.filepathMap = Path.Combine(filePath, nameInput.text + ".map");
    }


    // ------------ Editor ---------------- 
    public void Open(bool saveMode)
    {
        this.saveMode = saveMode;
        if (saveMode)
        {
            menuLabel.text = "Save Map";
            actionButtonLabel.text = "Save";
        }
        else
        {
            menuLabel.text = "Load Map";
            actionButtonLabel.text = "Load";
        }
        FillList();
        gameObject.SetActive(true);
        HexMapCamera.Locked = true;
    }


    public void Close()
    {
        gameObject.SetActive(false);
        HexMapCamera.Locked = false;
    }

    public void Action()
    {
        string path = GetSelectedPath();
        if (path == null)
        {
            return;
        }
        if (saveMode)
        {
            Save(path);
        }
        else
        {
            Load(path);
        }
        Close();
    }

    public void SelectItem(string name)
    {
        nameInput.text = name;
    }

    public void Delete()
    {
        string path = GetSelectedPath();
        if (path == null)
        {
            return;
        }
        if (File.Exists(path))
        {
            File.Delete(path);
        }
        nameInput.text = "";
        FillList();
    }

    void FillList()
    {
        for (int i = 0; i < listContent.childCount; i++)
        {
            Destroy(listContent.GetChild(i).gameObject);
        }
        if (!Directory.Exists(filePath))
        {
            Directory.CreateDirectory(filePath);
        }
        string[] paths =
            Directory.GetFiles(filePath, "*.map");
        Array.Sort(paths);
        for (int i = 0; i < paths.Length; i++)
        {
            SaveLoadItem item = Instantiate(itemPrefab);
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

    void Save(string path)
    {
        using (
            BinaryWriter writer =
            new BinaryWriter(File.Open(path, FileMode.Create))
        )
        {
            writer.Write(mapFileVersion);
            hexGrid.Save(writer);
        }
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
}