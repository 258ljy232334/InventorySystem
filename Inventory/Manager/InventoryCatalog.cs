using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[CreateAssetMenu(fileName ="ItemCatalog",
    menuName ="SO_Assets/Inventory/Catalog")]
public class InventoryCatalog : ScriptableObject,IInventoryCatalog
{
    [SerializeField] private CollectionList _collectionCatalog;
    [SerializeField] private FoodList _foodCatalog;
    [SerializeField] private WeaponList _weaponCatalog;
    [SerializeField] private BulletList _bulletMagazineCatalog;
    [SerializeField] private MagazineList _magazineCatalog;

    private Dictionary<int, ItemDetails> _itemDetailsDictionary;
    public Dictionary<int, ItemDetails> ItemDetailsDictionary => _itemDetailsDictionary;
    private void OnEnable()
    {
        _itemDetailsDictionary= new Dictionary<int, ItemDetails>(300);
        foreach (var details in _collectionCatalog.ItemDetailList)
        {
            _itemDetailsDictionary.Add(details.ID, details);
        }
        foreach (var details in _foodCatalog.ItemDetailList)
        {
            _itemDetailsDictionary.Add(details.ID, details);
        }
        foreach (var details in _weaponCatalog.ItemDetailList)
        {
            _itemDetailsDictionary.Add(details.ID, details);
        }
        foreach (var details in _bulletMagazineCatalog.ItemDetailList)
        {
            _itemDetailsDictionary.Add(details.ID, details);
        }
        foreach (var details in _magazineCatalog.ItemDetailList)
        {
            _itemDetailsDictionary.Add(details.ID, details);
        }
    }
    public ItemDetails GetItemDetails(int id)
    {
        if( _itemDetailsDictionary.TryGetValue(id,out var res))
        {
            return res;
        }
        return null;
    }
}
