using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InventoryItemDetails :IDetails
{
    protected ItemDetails _details;
    protected int _amount; 

    public ItemDetails Details {  get { return _details; } }
    public int Amount {  get { return _amount; } }
    public Sprite Icon {  get { return _details.Icon; } }
    public int ID { get { return Details.ID; } }

    public InventoryItemDetails(ItemDetails itemDetails,int amount=1)
    {
        _amount=amount;
        _details = itemDetails;
        
    }
    public void AddAmount(int count=1)
    {
        _amount += count;
    }
    public void ReduceAmount(int count = 1)
    {
        if(_amount<count)
        {
            Debug.LogError("多删除了物体");
        }
        _amount -= count;
    }
    
}
