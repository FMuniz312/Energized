using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;

[System.Serializable]
public class DataSerialization : MonoBehaviour
{
    #region Instance
    private static DataSerialization _i;
    public static DataSerialization Instance
    {
        get
        {
            if (_i == null)
            {

                _i = FindObjectOfType<DataSerialization>();

                if (_i == null)
                {
                    _i = new GameObject("SaveManagementGO", typeof(DataSerialization)).GetComponent<DataSerialization>();
                }

            }

            return _i;
        }

        private set { _i = value; }
    }
    #endregion

    public const string SaveFileName = "PlayerSaveData";

    public bool ResetData;

    public SaveDataContainer SaveDataContainer;
    private BinaryFormatter BinaryFormatter;

    private void Awake()
    {
      //  DontDestroyOnLoad(this.gameObject);
        BinaryFormatter = new BinaryFormatter();
        if (ResetData)
        {
            Save();
        }
        else
        {
            Load();
        }
    }

    public void Load()
    {
        try
        {
            string Path = Application.persistentDataPath + "/saves/" + SaveFileName + ".save";

            var fileStream = new FileStream(Path, FileMode.Open, FileAccess.Read);
            if (SaveDataContainer.instance.PlayerSaveData == null) SaveDataContainer.instance.PlayerSaveData = new PlayerSaveData();


            SaveDataContainer = (SaveDataContainer)BinaryFormatter.Deserialize(fileStream);
            fileStream.Close();
        }

        catch
        {
            Debug.Log("Não existe uma save ainda");

            Save();
        }


    }
    public void Save()
    {
        string Path = Application.persistentDataPath + "/saves/" + SaveFileName + ".save";

        if (!Directory.Exists(Application.persistentDataPath + "/saves"))
        {
            Directory.CreateDirectory(Application.persistentDataPath + "/saves");
        }

        if (SaveDataContainer.instance.PlayerSaveData == null) SaveDataContainer.instance.PlayerSaveData  = new PlayerSaveData();

        FileStream file = new FileStream(Path,FileMode.OpenOrCreate,FileAccess.Write);
        BinaryFormatter.Serialize(file, SaveDataContainer);
        file.Close();

    }

    
}
