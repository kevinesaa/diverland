using System;
using UnityEngine;

public interface IInventoryService
{
    event Action<Inventory> OnLoadInventoryDataSuccessfully;
    event Action<string, string> OnLoadInventoryDataFail;
    void GetInventoryData(string user);

}
