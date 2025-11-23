using System;
using System.Collections.Generic;
using UnityEngine;

public class InventoryFoodDetails :IDetails
{
    private readonly FoodDetails _details;
    private readonly ITimeManager _timeManager;
    private int _amount;
    private readonly LinkedList<(int entryDay, int count)> _expiredDayList;

    public FoodDetails Details => _details;
    public int Amount => _amount;

    public int ID => _details.ID;

    public Sprite Icon => _details.Icon;

    public InventoryFoodDetails(FoodDetails itemDetails, ITimeManager _timeManager,int amount = 1)
    {
        _details = itemDetails ?? throw new ArgumentNullException(nameof(itemDetails));
        _timeManager = _timeManager ?? throw new ArgumentNullException(nameof(_timeManager));
        _amount = amount;
        _expiredDayList = new LinkedList<(int, int)>();
        _expiredDayList.AddLast((_timeManager.GetCurDay(), amount));
    }
    /// <summary> 增加库存：同天合并到尾块，否则新尾巴 </summary>
    public void AddAmount(int count = 1)
    {
        if (count <= 0) throw new ArgumentOutOfRangeException(nameof(count));

        int today = _timeManager.GetCurDay();

        if (_expiredDayList.Count > 0 && _expiredDayList.Last!.Value.entryDay == today)
        {
            // 改尾块
            var lastNode = _expiredDayList.Last!;
            lastNode.Value = (today, lastNode.Value.count + count);
        }
        else
        {
            _expiredDayList.AddLast((today, count));
        }
        _amount += count;
    }

    /// <summary> 减少库存：从老块开始扣，扣完删节点 </summary>
    public void ReduceAmount(int count = 1)
    {
        if (count <= 0) throw new ArgumentOutOfRangeException(nameof(count));
        if (count > _amount) throw new InvalidOperationException(
            $"尝试移除 {count}，但当前只有 {_amount}");

        _amount -= count;

        while (count > 0)
        {
            var firstNode = _expiredDayList.First!;
            var (day, cnt) = firstNode.Value;
            if (cnt <= count)
            {
                // 整块拿光
                count -= cnt;
                _expiredDayList.RemoveFirst();
            }
            else
            {
                // 拿一部分
                firstNode.Value = (day, cnt - count);
                count = 0;
            }
        }
    }

    /// <summary> 只计算过期数量，不删 </summary>
    public int CalcExpiredCount()
    {
        int day = _timeManager.GetCurDay();
        int life = _details.ShelfLife;
        int expiredCount = 0;

        foreach (var (entryDay, cnt) in _expiredDayList)
        {
            if (day - entryDay > life)
            {
                expiredCount += cnt;
            }
            else
            {
                break;            // 链表按时间有序，后面更不可能过期
            }
        }
        return expiredCount;
    }
    /// <summary> 原子地移除并返回过期数量 </summary>
    public int RemoveExpired()
    {
        int day = _timeManager.GetCurDay();
        int life = _details.ShelfLife;
        int expiredCount = 0;

        while (_expiredDayList.Count > 0 && day - _expiredDayList.First!.Value.entryDay > life)
        {
            expiredCount += _expiredDayList.First.Value.count;
            _expiredDayList.RemoveFirst();
        }
        _amount -= expiredCount;
        return expiredCount;
    }
}
