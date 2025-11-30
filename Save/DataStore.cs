using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using UnityEngine;

public class DataStore : IDataStore
{
    private ISaveSerializer _serializer = new JsonUtilitySerializer();
    private string GetPath(string key) =>
      Path.Combine(Application.persistentDataPath, $"{key}.txt");

    public async void Save(SaveBlob blob, string key = "114514")
    {
        //活得路径
        var path = GetPath(key);
        var tmp = path + ".tmp";

        //写入暂时文件
        await Task.Run(() =>
        {
            Directory.CreateDirectory(Path.GetDirectoryName(path)!);
            File.WriteAllText(tmp, _serializer.Serialize(blob));
        });

        File.Replace(tmp, path, null);   // 原子替换
    }

    public SaveBlob Load(string key = "114514")
    {
        string path = GetPath(key);
        if (!File.Exists(path))
        {
            Debug.Log($"[DataStore] No file → {path} , returning empty blob");
            return new SaveBlob();          // 首次运行给个空壳
        }

        string json = File.ReadAllText(path);
        SaveBlob blob = _serializer.Deserialize<SaveBlob>(json);
        Debug.Log($"[DataStore] Loaded ← {path}");
        return blob;
    }
}
