using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IInventoryServe 
{
    IReadOnlyList<T> GetBag<T>(ItemType type) where T : class;
    ItemDetails GetItemDetails(int id);
    void AddItem(int id, int count = 1);
    void AddInventoryItem<T>(T t) where T : class;
    void RemoveItem(int id, int count = 1);
    void RemoveItemAt(int index, ItemType type);
    bool HasItem(int id);
    void CallUpdateInventory(ItemType itemType);
    bool MoveMagazineToUsable(int index);
    void RemoveUsableMagazine(int magazineIndex);
    bool MoveMagazineToBag(int usableIndex);
    void AddMagazineToUsable(InventoryMagazineDetails details);
    int LoadUsableMagazine(int usableIndex);
    (int index, bool isUsable) FindMagazine(int magazineID);
    void UseItem(int id, int index);
}
