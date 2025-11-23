using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IWeaponServe 
{
    bool EquipWeapon(int weaponId, int bagIndex = -1);
}
