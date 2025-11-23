using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using UnityEngine;
using Zenject;

public class InventoryManager : MonoBehaviour, IInventoryManager
{
    [Inject]
    private IInventoryCatalog _catalog;
    [Inject]
    private IInventoryServe _serve;
    [Inject]
    private IInventoryRepository _repository;
    [Inject]
    private IWeaponManager _weaponManager;
    [Inject]
    private ITimeManager _timeManager;
    //取物品的泛型方法
    public IReadOnlyList<T> GetBag<T>(ItemType type) where T : class
    {
        return _serve.GetBag<T>(type);
    }  
    public void UseItem(int id,int index=0)
    {
       _serve.UseItem(id, index);
    }
    //存物品
    public void AddItem(int id, int count = 1)
    {
        _serve.AddItem(id, count);
    }
    public void AddInventoryItem<T>(T t) where T : class
    {
        _serve.AddInventoryItem(t);
    }
    public void RemoveItem(int id, int count = 1)
    {
      _serve.RemoveItem(id, count);
    }
    public void RemoveItemAt(int index, ItemType type)
    {
       _serve.RemoveItemAt(index, type);
    }
    public bool HasItem(int id)
    {
        return _serve.HasItem(id);
    }
    public int LoadUsableMagazine(int usableIndex)
    {
      return  _serve.LoadUsableMagazine(usableIndex);
    }
    

    public bool MoveMagazineToBag(int usableIndex)
    {
        return  _serve.MoveMagazineToBag(usableIndex);
    }

    public bool MoveMagazineToUsable(int index)
    {
        return _serve.MoveMagazineToUsable(index);
    }
    //把弹夹加入装备栏
    public void AddMagazineToUsable(InventoryMagazineDetails details)
    {
        _serve.AddMagazineToUsable(details);
    }
    
    
    //查询有没有对应的弹夹
    public (int index,bool isUsable) FindMagazine(int MagazineID)
    {
        return _serve.FindMagazine(MagazineID);
    }
    public void RemoveUsableMagazine(int magazineIndex)
    {
        _serve.RemoveUsableMagazine(magazineIndex);
    }
    public ItemDetails GetItemDetails(int id)
    {
        return _catalog.GetItemDetails(id);
    }

    public InventoryMagazineDetails[] GetUseableMagazine()
    {
        return _repository.GetUseableMagazineBag();
    }
}
