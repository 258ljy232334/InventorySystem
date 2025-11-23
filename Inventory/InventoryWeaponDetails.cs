using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class InventoryWeaponDetails :IDetails
{
    private WeaponDetails _details;
    private int _curDurability;
    private InventoryMagazineDetails _currentMagazine;


    public WeaponDetails Details => _details;
    public int ID => _details.ID;
    public int Amount => 1;
    public Sprite Icon =>_details.Icon;

    public int CurDurability=>_curDurability;
    public InventoryMagazineDetails CurrentMagazine => _currentMagazine;
    public event Action<bool> OnRangeAttack;
    public event Action<bool> OnMeleeAttack;

    private float _timer;       //攻击计时器
    private IInventoryManager _inventoryManager;
    public InventoryWeaponDetails(WeaponDetails weaponDetails, IInventoryManager inventoryManager)
    {
        _details = weaponDetails;
        _timer = 0;
        _curDurability = _details.MaxDurability;
        _currentMagazine = null;
        _inventoryManager = inventoryManager;
    }
    
    public void Tick(float dt)
    {
        _timer=Mathf.Max(0, _timer-dt);
    }
    public bool ReLoadAtUsable(int magazineIndex)
    {
        ReturnEmptyMagazine();
        var bag = _inventoryManager.GetUseableMagazine();
        var temp = bag[magazineIndex];
        if (temp != null&&temp.Details.ID ==_details.MagazineID)
        {
            _currentMagazine = temp;
            _inventoryManager.RemoveUsableMagazine(magazineIndex);
            return true;
        }
        return false;
    }
    public void ReLoadAtBag(int index)
    {
        ReturnEmptyMagazine();
        var temp = _inventoryManager.GetUseableMagazine()[index];
        if (temp != null && temp.Details.ID== _details.MagazineID)
        {
            _currentMagazine = temp;
            _inventoryManager.RemoveItemAt(index,ItemType.Magazine);
            return;
        }
        return;
    }
    public bool CanAttack()
    {
        if(Details.WeaponType!=WeaponType.MeleeWeapon&&
         (_currentMagazine==null||_currentMagazine.BulletCount==0))
        {
            return false;
        }
        if(_curDurability<=0)
        {
            return false;
        }
        return _timer == 0;
    }
    public void Repair()
    {

    }
    //远程攻击
    public bool RangeAttack()
    {
        if (Details.WeaponType == WeaponType.MeleeWeapon) return false;
       
        if (_curDurability <= 0) return false;
        if (_timer > 0) return false;

        // 弹夹检查
        if (_currentMagazine == null || _currentMagazine.BulletCount == 0)
        {
            return false;
        }
        // 真正消耗
        _curDurability--;
        _currentMagazine.UseBullet();
        if(_currentMagazine.BulletCount==0)
        {
            ReturnEmptyMagazine();
        }
        _timer = Details.AttackInterval;
        Debug.Log("Fire");
        OnRangeAttack?.Invoke(true);   // 远程事件
        return true;

    }
    public bool ChargeUpMeleeAttack()
    {
        return false;
    }
    public bool MeleeAttack()
    {
        if (Details.WeaponType != WeaponType.MeleeWeapon)
        {
            return false;
        }
        if (_curDurability <= 0)
        {
            return false;
        }
        _curDurability--;
        OnMeleeAttack?.Invoke(true);
        Debug.Log("近战一次");
        _timer= Details.AttackInterval;
        return true;
    }
    //把弹夹送回背包
    private void ReturnEmptyMagazine()
    {
        if (_currentMagazine != null && _currentMagazine.BulletCount == 0)
        {
            _inventoryManager.AddMagazineToUsable(_currentMagazine);
            _currentMagazine = null;
        }
    }

   
}
