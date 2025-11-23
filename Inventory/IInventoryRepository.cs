using System.Collections.Generic;

//运行时的库存数据接口
public interface IInventoryRepository 
{

   
  
    void AddWeapon(InventoryWeaponDetails weaponDetails);
    void AddFood(InventoryFoodDetails foodDetails);
    void AddBullet(InventoryBulletDetails bulletDetails);
    void AddMagazine(InventoryMagazineDetails magazineDetails);
    void AddCollection(InventoryItemDetails collectionDetails);
    void AddSpoils(InventoryItemDetails spoils);

    void RemoveFood(int id, int amount = 1);
    void RemoveBullet(int id, int amount = 1);
    void RemoveCollection(int id, int amount = 1);
    void RemoveWeaponAt(int index);
    void RemoveSpoilsAt(int index);
    void RemoveMagazineAt(int index);

    bool HasItem(int id, ItemType type);

    // 查询
    IReadOnlyList<InventoryWeaponDetails> GetWeapons();
    IReadOnlyList<InventoryFoodDetails> GetFoods();
    IReadOnlyList<InventoryBulletDetails> GetBullets();
    IReadOnlyList<InventoryMagazineDetails> GetMagazines();
    IReadOnlyList<InventoryItemDetails> GetCollections();
    IReadOnlyList<InventoryItemDetails> GetSpoils();
    void Save();
    void Load();

    bool MoveMagazineToUsable(int index);
    void RemoveUsableMagazine(int magazineIndex);
    bool MoveMagazineToBag(int usableIndex);
    void AddMagazineToUsable(InventoryMagazineDetails details);
    InventoryMagazineDetails[] GetUseableMagazineBag();
    bool TryGetBullet(int bulletId, out InventoryBulletDetails bullet);
    void RemoveBulletIfEmpty(int bulletId);

}
