using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Zenject;

public class InventoryServe : MonoBehaviour, IInventoryServe
{
    [Inject]
    private IInventoryRepository _repository;
    [Inject]
    private IInventoryCatalog _catalog;
    [Inject]
    private IInventoryManager _inventoryManager;
    [Inject]
    private ITimeManager _timeManager;
    [Inject]
    private IWeaponServe _weaponServe;
    [Inject]
    private IMagazineServe _magazineServe;
    //用来抛脏事件
    private HashSet<ItemType> _dirty = new HashSet<ItemType>();

    private void LateUpdate()
    {
        if (_dirty.Count == 0) return;
        foreach (var t in _dirty) InventoryEvent.CallUpdateAllItem(t);
        _dirty.Clear();
    }
    public IReadOnlyList<T> GetBag<T>(ItemType type) where T : class
         => type switch
         {
             ItemType.Weapon =>_repository.GetWeapons() as IReadOnlyList<T>,
             ItemType.SpoilsOfWar => _repository.GetSpoils() as IReadOnlyList<T>,
             ItemType.Collection => _repository.GetCollections() as IReadOnlyList<T>,
             ItemType.Food => _repository.GetFoods() as IReadOnlyList<T>,
             ItemType.Bullet => _repository.GetBullets() as IReadOnlyList<T>,
             ItemType.Magazine => _repository.GetMagazines() as IReadOnlyList<T>,
             _ => null
         };
    //存物品
    public void AddItem(int id, int count = 1)
    { 
        var det =_catalog.GetItemDetails(id);
        if (det == null)
        {
            return;
        }

        switch (det.Type)
        {
            case ItemType.Weapon:
                for (int i = 0; i < count; i++)
                    _repository.AddWeapon(new InventoryWeaponDetails(det as WeaponDetails, _inventoryManager));
                break;
            case ItemType.Food:
                InventoryFoodDetails foodDetails = new InventoryFoodDetails(det as FoodDetails, _timeManager, count);
                _repository.AddFood(foodDetails);
                break;
            case ItemType.Bullet:
                InventoryBulletDetails bulletDetails = new InventoryBulletDetails(det as BulletDetails, count);
                _repository.AddBullet(bulletDetails);
                break;
            case ItemType.SpoilsOfWar:
                for (int i = 0; i < count; i++)
                {
                   _repository.AddSpoils(new InventoryItemDetails(det));
                }
                break;
            case ItemType.Magazine:
                for (int i = 0; i < count; i++)
                {
                  _repository.AddMagazine(new InventoryMagazineDetails(det as MagazineDetails));
                }
                break;
            case ItemType.Collection:
                    _repository.AddCollection(new InventoryItemDetails(det, count));
                break;
        }
        _dirty.Add(det.Type);
    }
    public void AddInventoryItem<T>(T t) where T : class
    {
        switch (t)
        {
            case InventoryItemDetails item:
                 if(item.Details.Type==ItemType.Collection)
                {
                   _repository.AddCollection(item);
                }
                else
                {
                   _repository.AddSpoils(item);
                }
                    _dirty.Add(item.Details.Type);
                break;

            case InventoryFoodDetails food:
                _repository.AddFood(food);
                _dirty.Add(ItemType.Food);
                break;

            case InventoryWeaponDetails weapon:
                _repository.AddWeapon(weapon);
                _dirty.Add(ItemType.Weapon);
                break;

            case InventoryMagazineDetails mag:
                _repository.AddMagazine(mag);
                _dirty.Add(ItemType.Magazine);
                break;
        }
    }
    public void RemoveItem(int id, int count = 1)
    {
        var det = _catalog.GetItemDetails(id);
        if (det == null) return;

        switch (det.Type)
        {
            case ItemType.Food:
                _repository.RemoveFood(id, count);
                break;
            case ItemType.Bullet:
                _repository.RemoveBullet(id, count);
                break;
            case ItemType.Collection:
                _repository.RemoveCollection(id, count);
                break;
            default:
                Debug.LogWarning("不支持该类型");
                return;
        }
        _dirty.Add(det.Type);
    }
    public void RemoveItemAt(int index, ItemType type)
    {
        switch (type)
        {
            case ItemType.Weapon:
                _repository.RemoveWeaponAt(index);
                break;

            case ItemType.SpoilsOfWar:
                _repository.RemoveSpoilsAt(index);
                break;
            case ItemType.Magazine:
                _repository.RemoveMagazineAt(index);
                break;
            default:
                Debug.LogWarning($"RemoveItemAt 不支持类型 {type}");
                return;
        }
        _dirty.Add(type);
    }
    public bool HasItem(int id)
    {
        ItemDetails itemDetails = _catalog.GetItemDetails(id);
        if (itemDetails == null)
        {
            return false;
        }
        return _repository.HasItem(id,itemDetails.Type);
    }

    public void UseItem(int id, int index)
    {
        ItemDetails itemDetails = _catalog.GetItemDetails(id);
        if (itemDetails == null)
        {
            return;
        }
        switch (itemDetails.Type)
        {
            case ItemType.Weapon:
                _weaponServe.EquipWeapon(id,index);
                break;

            case ItemType.Food:
                break;
            case ItemType.Collection:
                break;
            case ItemType.Bullet:
                break;
            case ItemType.Magazine:
                MoveMagazineToUsable(index);
                break;
            default:
                break;
        }
    }

    public void CallUpdateInventory(ItemType itemType)
    {
       _dirty.Add(itemType);
    }

    public ItemDetails GetItemDetails(int id)
    {
       return _catalog.GetItemDetails(id);
    }

    public bool MoveMagazineToUsable(int index)
    {
        return _magazineServe.MoveMagazineToUsable(index);
    }

    public void RemoveUsableMagazine(int magazineIndex)
    {
      _magazineServe.RemoveUsableMagazine(magazineIndex);
    }

    public bool MoveMagazineToBag(int usableIndex)
    {
        return _magazineServe.MoveMagazineToBag(usableIndex);
    }

    public void AddMagazineToUsable(InventoryMagazineDetails details)
    {
       _magazineServe.AddMagazineToUsable(details);
    }

    public (int index, bool isUsable) FindMagazine(int magazineID)
    {
        return _magazineServe.FindMagazine(magazineID);
    }

    public int LoadUsableMagazine(int usableIndex)
    {
        return _magazineServe.LoadUsableMagazine(usableIndex);
    }
    
}
