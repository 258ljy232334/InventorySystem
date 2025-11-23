
public interface IMagazineServe 
{
    bool MoveMagazineToUsable(int index);
    void RemoveUsableMagazine(int magazineIndex);
    bool MoveMagazineToBag(int usableIndex);
    void AddMagazineToUsable(InventoryMagazineDetails details);
    int LoadUsableMagazine(int usableIndex);
    (int index, bool isUsable) FindMagazine(int MagazineID);
}
