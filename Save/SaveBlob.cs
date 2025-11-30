using System.Collections.Generic;
[System.Serializable]
public class SaveBlob 
{
    [DataList]  //打好反射标记
    public List<RoomData> _rooms=new ();
    [DataList]
    public List<ProfileData> _profiles=new ();
}
