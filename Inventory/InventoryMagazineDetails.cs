using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InventoryMagazineDetails:IDetails 
{
    private MagazineDetails _details;
    private int _bulletCount;   //子弹数量
    
    public MagazineDetails Details => _details;
    public int BulletCount => _bulletCount;
   

    public int ID => _details.ID;

    public int Amount =>1;

    public Sprite Icon =>_details.Icon;
    public const int EMPTY_BULLET_ID = -1;
    public InventoryMagazineDetails(MagazineDetails magazineDetails)
    {
        _details=magazineDetails;
        _bulletCount = 0;
      
    }
    //装填方法
    public void FillBullets(int bulletID,int count=1)
    {
       if(bulletID==_details.BulletID&&_bulletCount!=_details.MaxSize)
       {
            _bulletCount = Mathf.Min(_details.MaxSize,_bulletCount+count);
       }
    }
    public bool UseBullet()
    {
        if (_bulletCount == 0)
        {
            return false;
        }
        else
        {
            _bulletCount--;
            return true;
        }
    }
}
