using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IDataManager 
{
    IReadOnlyList<T> GetRoList<T>() where T:class,IDatable;

    void Load();
    void Save();
}
