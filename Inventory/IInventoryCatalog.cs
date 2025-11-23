using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IInventoryCatalog 
{
    Dictionary<int, ItemDetails> ItemDetailsDictionary { get; }
    ItemDetails GetItemDetails(int id);
}
