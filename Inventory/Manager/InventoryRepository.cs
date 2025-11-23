using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Zenject;

public class InventoryRepository :MonoBehaviour,IInventoryRepository
{
    [Inject]
    private IInventoryServe _serve;
    private readonly  List<InventoryWeaponDetails> _weaponBag  = new();
    private readonly  List<InventoryItemDetails> _spoilsBag = new();
    private readonly List<InventoryItemDetails> _collectionBag  = new();
    private readonly List<InventoryFoodDetails> _foodBag  = new();
    private readonly List<InventoryBulletDetails> _bulletBag = new();
    private readonly List<InventoryMagazineDetails> _magazineBag = new();
    private  InventoryMagazineDetails[] _usableMagazineBag;

    /* 只读包装 → 0 GC */
    private IReadOnlyList<InventoryWeaponDetails> WeaponRo;
    private IReadOnlyList<InventoryItemDetails> SpoilsRo;
    private IReadOnlyList<InventoryItemDetails> CollectionRo;
    private IReadOnlyList<InventoryFoodDetails> FoodRo;
    private IReadOnlyList<InventoryBulletDetails> BulletRo;
    private IReadOnlyList<InventoryMagazineDetails> MagazineRo;
    private Dictionary<int, InventoryItemDetails> CollectionBag;
    private Dictionary<int, InventoryFoodDetails> FoodBag;
    private Dictionary<int, InventoryBulletDetails> BulletBag;//子弹背包
    public InventoryMagazineDetails[] UsableMagazineBag => _usableMagazineBag;
    [SerializeField]
    private int _usableMagazineCount;
    [Inject]
    public void Initialize()
    {
        WeaponRo = _weaponBag;
        SpoilsRo = _spoilsBag;
        CollectionRo = _collectionBag;
        FoodRo = _foodBag;
        BulletRo = _bulletBag;
        MagazineRo = _magazineBag;

        //初始化背包
        CollectionBag = new Dictionary<int, InventoryItemDetails>();
        FoodBag = new Dictionary<int, InventoryFoodDetails>();
        BulletBag = new Dictionary<int, InventoryBulletDetails>();
        _usableMagazineBag = new InventoryMagazineDetails[_usableMagazineCount];
    }

   

    public void RemoveFood(int id, int amount = 1)
    {
        if(FoodBag.TryGetValue(id, out InventoryFoodDetails food))
        {
            food.ReduceAmount(amount);
            if(food.Amount<=0)
            {
                FoodBag.Remove(id);
                _foodBag.Remove(food);
            }
        }
    }

    public void RemoveBullet(int id, int amount = 1)
    {
        if (BulletBag.TryGetValue(id, out InventoryBulletDetails bullet))
        {
            bullet.ReduceAmount(amount);
            if (bullet.Amount <= 0)
            {
                BulletBag.Remove(id);
                _bulletBag.Remove(bullet);
            }
        }
    }

    public void RemoveCollection(int id, int amount = 1)
    {
        if(CollectionBag.TryGetValue(id,out var collection))
        {
            collection.ReduceAmount(amount);
            if(collection.Amount<=0)
            {
                CollectionBag.Remove(id);
                _collectionBag.Remove(collection);
            }
        }
    }

    public void RemoveWeaponAt(int index)
    {
       if(index>=0&&index<_weaponBag.Count)
        {
            _weaponBag.RemoveAt(index);
        }
    }

    public void RemoveSpoilsAt(int index)
    {
        if(index>=0&&index<_spoilsBag.Count)
        {
            _spoilsBag.RemoveAt(index);
        }
    }

    public void RemoveMagazineAt(int index)
    {
        if (index >= 0 && index < _magazineBag.Count)
        {
            _magazineBag.RemoveAt(index);
        }
    }

    public bool HasItem(int id, ItemType type)
     => type switch
     {
         ItemType.Food => FoodBag.ContainsKey(id),
         ItemType.Bullet => BulletBag.ContainsKey(id),
         ItemType.Collection => CollectionBag.ContainsKey(id),
         ItemType.Weapon => HasWeapon(id),
         ItemType.Magazine => HasMagazine(id),
         ItemType.SpoilsOfWar => HasSpoils(id),
         _ => false
     };
    public IReadOnlyList<InventoryWeaponDetails> GetWeapons()
    {
        return WeaponRo;
    }

    public IReadOnlyList<InventoryFoodDetails> GetFoods()
    {
        return FoodRo;
    }

    public IReadOnlyList<InventoryBulletDetails> GetBullets()
    {
       return BulletRo;
    }

    public IReadOnlyList<InventoryMagazineDetails> GetMagazines()
    {
        return MagazineRo;
    }

    public IReadOnlyList<InventoryItemDetails> GetCollections()
    {
       return CollectionRo;
    }

    public IReadOnlyList<InventoryItemDetails> GetSpoils()
    {
       return SpoilsRo;
    }

    public void AddWeapon(InventoryWeaponDetails weaponDetails)
    {
       _weaponBag.Add(weaponDetails);
    }

    public void AddFood(InventoryFoodDetails foodDetails)
    {
       if(FoodBag.TryGetValue(foodDetails.ID,out var food))
        {
            food.AddAmount(foodDetails.Amount);
        }
       else
        {
            FoodBag.Add(foodDetails.ID, foodDetails);
            _foodBag.Add(foodDetails);
        }
    }

    public void AddBullet(InventoryBulletDetails bulletDetails)
    {
        if (BulletBag.TryGetValue(bulletDetails.ID, out var bullet))
        {
            bullet.AddAmount(bulletDetails.Amount);
        }
        else
        {
            Debug.Log("捡到了");
            BulletBag.Add(bulletDetails.ID, bulletDetails);
            _bulletBag.Add(bulletDetails);
        }
    }

    public void AddMagazine(InventoryMagazineDetails magazineDetails)
    {
       _magazineBag.Add(magazineDetails);
    }

    public void AddCollection(InventoryItemDetails collectionDetails)
    {
        if (CollectionBag.TryGetValue(collectionDetails.ID, out var collection))
        {
            collection.AddAmount(collectionDetails.Amount);
        }
        else
        {
            CollectionBag.Add(collectionDetails.ID, collectionDetails);
            _collectionBag.Add(collectionDetails);
        }
    }
    private bool HasWeapon(int id) => _weaponBag.Exists(w => w.Details.ID == id);
    private bool HasMagazine(int id) => _magazineBag.Exists(m => m.Details.ID == id);
    private bool HasSpoils(int id) => _spoilsBag.Exists(s => s.Details.ID == id);

    public void AddSpoils(InventoryItemDetails spoils)
    {
       _spoilsBag.Add(spoils);
    }
    public void Save()
    {

    }
    public void Load()
    {

    }

    public bool MoveMagazineToUsable(int index)
    {
        if (index < 0 || index >= _magazineBag.Count)
        {
            return false;
        }
        var mag = _magazineBag[index];
        if (mag == null) return false;
        int empty = Array.FindIndex(_usableMagazineBag, m => m == null);
        if (empty == -1) return false;          

        _usableMagazineBag[empty] = mag;
        _magazineBag.Remove(mag);
        _serve.CallUpdateInventory(ItemType.Magazine);
        _serve.CallUpdateInventory(ItemType.UsableMagazine);
        return true;
    }

    public void RemoveUsableMagazine(int magazineIndex)
    {
        if (magazineIndex >= _usableMagazineBag.Length || magazineIndex < 0)
        {
            return;
        }
        if (_usableMagazineBag[magazineIndex] == null)
        {
            return;
        }
        else
        {
            _usableMagazineBag[magazineIndex] = null;
            _serve.CallUpdateInventory(ItemType.UsableMagazine);
        }
    }

    public bool MoveMagazineToBag(int usableIndex)
    {
        if ((uint)usableIndex >= _usableMagazineBag.Length) return false;
        var mag = _usableMagazineBag[usableIndex];
        if (mag == null) return false;
        _magazineBag.Add(mag);
        _usableMagazineBag[usableIndex] = null;
        _serve.CallUpdateInventory(ItemType.Magazine);
        _serve.CallUpdateInventory(ItemType.UsableMagazine);
        return true;
    }

    public void AddMagazineToUsable(InventoryMagazineDetails details)
    {
        for (int i = 0; i < _usableMagazineBag.Length; i++)
        {
            if (_usableMagazineBag[i] == null)
            {
                _usableMagazineBag[i] = details;
                _serve.CallUpdateInventory(ItemType.UsableMagazine);
                return;
            }
        }
        AddMagazine(details);
    }
    public bool TryGetBullet(int bulletId, out InventoryBulletDetails bullet) =>
    BulletBag.TryGetValue(bulletId, out bullet);

    public void RemoveBulletIfEmpty(int bulletId)
    {
        if (BulletBag.TryGetValue(bulletId, out var b) && b.Amount <= 0)
        {
            BulletBag.Remove(bulletId);
            _bulletBag.Remove(b);        // 同步列表
        }
    }

    public InventoryMagazineDetails[] GetUseableMagazineBag()
    {
       return _usableMagazineBag;
    }
}
