using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public interface IInventoryManager
{
    InventoryMagazineDetails[] GetUseableMagazine();
    IReadOnlyList<T> GetBag<T>(ItemType type) where T : class;
    void UseItem(int id, int index = 0);
    void AddItem(int id, int count = 1);
    void AddInventoryItem<T>(T t) where T : class;
    void RemoveItem(int id, int count = 1);
    void RemoveItemAt(int index, ItemType type);
    bool HasItem(int id);
    bool MoveMagazineToBag(int usableIndex);
    int LoadUsableMagazine(int usableIndex);
    (int index, bool isUsable) FindMagazine(int bulletID);
    void AddMagazineToUsable(InventoryMagazineDetails details);
    ItemDetails GetItemDetails(int id);
    void RemoveUsableMagazine(int magazineIndex);
}
