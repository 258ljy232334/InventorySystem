
public interface IDataStore 
{
    void Save(SaveBlob blob,string key="114514");
    SaveBlob Load(string key="114514");
}
