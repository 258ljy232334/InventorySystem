using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Zenject;

public class MagazineServe : MonoBehaviour,IMagazineServe
{
    [Inject]
    private IInventoryRepository _repository;
    [Inject]
    private IInventoryServe _serve;

    private bool _isReloading;

    [SerializeField]
    private float _reloadInterval = 0.15f;
    public bool MoveMagazineToUsable(int index)
    {
        return _repository.MoveMagazineToUsable(index);
    }

    public void RemoveUsableMagazine(int magazineIndex)
    {
        _repository.RemoveUsableMagazine(magazineIndex);
    }

    public bool MoveMagazineToBag(int usableIndex)
    {
        return _repository.MoveMagazineToBag(usableIndex);
    }

    public void AddMagazineToUsable(InventoryMagazineDetails details)
    {
        _repository.AddMagazineToUsable(details);
    }

    public int LoadUsableMagazine(int usableIndex)
    {
        if (_isReloading) return 0;                       // 正在换弹，直接拒绝
        var usableBag = _repository.GetUseableMagazineBag();
        if ((uint)usableIndex >= usableBag.Length) return 0;
        var mag = usableBag[usableIndex];
        if (mag == null) return 0;

        int bulletID = mag.Details.BulletID;
        if (!_repository.TryGetBullet(bulletID,out var bullet)||bullet.Amount==0)
            return 0;

        int canLoad = mag.Details.MaxSize - mag.BulletCount;
        if (canLoad <= 0) return 0;

        int toLoad = Mathf.Min(canLoad, bullet.Amount);
        _isReloading = true;
        StartCoroutine(StartReload(mag, bulletID, toLoad));
        return toLoad;
    }
    private IEnumerator StartReload(InventoryMagazineDetails mag,int bulletID, int toLoad)
    {
        //等待一帧
        yield return null;          // 一帧后再真正写数据，避免 UI 刷新冲突
        for (int i = 0; i < toLoad; i++)
        {
            if (!_repository.TryGetBullet(bulletID, out var bullet) ||
            bullet.Amount <= 0 ||
            mag.BulletCount >= mag.Details.MaxSize)
                break;

            mag.FillBullets(bulletID, 1);
            bullet.ReduceAmount(1);

            // ③ 字典+列表同步清理
            _repository.RemoveBulletIfEmpty(bulletID);

            _serve.CallUpdateInventory(ItemType.Bullet);
            _serve.CallUpdateInventory(ItemType.UsableMagazine);
            yield return new WaitForSeconds(_reloadInterval);
        }

        // 循环结束（正常或提前 break）都解锁
        _isReloading = false;      // 4. 解锁
    }
    public void StopReload() => _isReloading = false;
    public (int index, bool isUsable) FindMagazine(int MagazineID)
    {
        int bestIndex = -1;
        bool bestIsUsable = false;
        int bestCount = 0;

        // 1. 先在“可用区”里搜
        InventoryMagazineDetails[] bag = _repository.GetUseableMagazineBag();
        for (int i = 0; i < bag.Length; i++)
        {
            var m = bag[i];
            if (m != null &&
                m.Details.ID == MagazineID &&
                m.BulletCount > bestCount)        // 只取“严格更多”
            {
                bestCount = m.BulletCount;
                bestIndex = i;
                bestIsUsable = true;
            }
        }
        //在可用区找到了
        if (bestIndex != -1)
        {
            return (bestIndex, true);
        }
        // 2. 再在“备用区”里搜
        var magazineBag=_repository.GetMagazines();

        for (int i = 0; i < magazineBag.Count; i++)
        {
            var m = magazineBag[i];
            if (m.Details.ID == MagazineID &&
                m.BulletCount > bestCount)
            {
                bestCount = m.BulletCount;
                bestIndex = i;
                bestIsUsable = false;
            }
        }
        return (bestIndex, bestIsUsable);
    }
}
