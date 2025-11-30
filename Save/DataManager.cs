using System.Collections.Generic;
using UnityEngine;
using Zenject;

public class DataManager : MonoBehaviour, IDataManager,IInitializable
{
    [Inject]
    private IDataStore _store;
    [Inject]
    private IDataRepo _repo;
    [Inject]
    private IDataServe _serve;
    private SaveBlob _saveBlob;
    //初始化加载
    public void Initialize()
    {
        Load(); //初始化执行加载方法
    }

    public IReadOnlyList<T> GetRoList<T>() where T :class, IDatable
    {
        return _repo.GetRoList<T>();
    }

    public void Save()
    {
        _repo.SaveBlobData(_saveBlob);
        _store.Save(_saveBlob);
    }
    public void Load()
    {
        _saveBlob= _store.Load();
        _repo.Initialize(_saveBlob);
    }
}

   

