using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IProductListService
{
    event Action<IList<ShopCategory>> OnShopDataLoadSuccessful;
    event Action<string, string> OnShopDataLoadedFail;
    void GetShopItems(string user);
}
