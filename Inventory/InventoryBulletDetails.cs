using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InventoryBulletDetails :IDetails{
    private BulletDetails _details;
    private int _amount;
    public BulletDetails Details => _details;
    public int Amount => _amount;

    public int ID => _details.ID;

    public Sprite Icon => _details.Icon;

    public InventoryBulletDetails(BulletDetails bulletDetails,int amount=1)
    {
        _details=bulletDetails;
        _amount=amount;
    }
    public void AddAmount(int amount)
    {
        _amount+=amount;
    }
    public void ReduceAmount(int amount)
    {
        _amount=Mathf.Max(_amount-amount,0);
    }
}
