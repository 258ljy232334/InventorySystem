using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Zenject;

public class WeaponServe : MonoBehaviour, IWeaponServe
{
    [Inject]
    private IInventoryManager _inventoryManager;
    [Inject]
    private IWeaponManager _weaponManager;
    public bool EquipWeapon(int weaponId, int bagIndex = -1)
    {
        var weapons = _inventoryManager.GetBag<InventoryWeaponDetails>(ItemType.Weapon);
        if (bagIndex < 0)
        {
            for (int i = 0; i < weapons.Count; i++)
            {
                if (weaponId == weapons[i].ID)
                {
                    bagIndex = i;
                    break;
                }
            }
        }
        if ((uint)bagIndex >= weapons.Count) return false;
        var weapon = weapons[bagIndex];
        _weaponManager.ChangeWeapon(weapon);          // 表现切换
        _inventoryManager.RemoveItemAt(bagIndex, ItemType.Weapon);
        return true;
    }

}
