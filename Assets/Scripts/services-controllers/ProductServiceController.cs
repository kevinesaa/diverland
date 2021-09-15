using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProductServiceController : MonoBehaviour {

    public List<ShopCategoryLocal> shopCategoriesLocal;
    public IProductListService ProductListService { get; private set; }
    
    private void Awake()
    {
       ProductListService = new ProductListLocalService(shopCategoriesLocal); 
    }

    public void GetShopItems(string user)
    {
        StartCoroutine(LoadDataCoroutine(user));
    }

    private IEnumerator LoadDataCoroutine(string user)
    {
        ProductListService.GetShopItems(user);
        yield return null;
        StopCoroutine(LoadDataCoroutine(user));
    }
	
}
