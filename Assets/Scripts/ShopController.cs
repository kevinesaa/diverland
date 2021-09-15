using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Purchasing;
using UnityEngine.UI;

public class ShopController : MonoBehaviour
{
    public ProductServiceController productServiceController;

    public ErrorController errorControllerDialog;
    public SimpleObjectPool categoryButtonPool;
    public SimpleObjectPool itemButtonPool;
    public Transform contentCategoryPanel;
    public Transform contentItemPanel;
    public Transform contentMoreCoinsPanel;
    public Text coinsText;
    public Text coinsText2;
    public GameObject buyButton;
    public GameObject moreCoinsPanel;
    public GameObject loading;
    public ItemSelectedHolder itemSelectedHolder;
    public Animator handAnimator;
    public Animator buySuccessAnimator;

    private ServerChecker internetChecker;
    private ICoinsService coinServiceController;
    private IShopServiceIAP shopIAP;
    private IBuyShopItemService buyService;   
    private IDictionary<string, ShopItem> shopItemsDitionary;
    private IList<ShopCategory> shopCategories;
    private IList<ShopItem> coinsIAP;
    private ShopCategory CategorySelected { get; set; }
    private ShopItem ItemSelected { get; set; }
    private IList<ShopItem> shopItemsPendingIAPQueue;
    private int Coins { get; set; }
    private string user;
    private readonly Vector3 SCALE = new Vector3(1, 1, 1);
   
    void Start ()
    {
        internetChecker = new ServerChecker(this);
        internetChecker.OnCheckerFinish += InternetChecked;

        productServiceController.ProductListService.OnShopDataLoadSuccessful += OnShopDataLoadSuccessful;
        productServiceController.ProductListService.OnShopDataLoadedFail += OnShopDataLoadedFail;

        coinServiceController = new CoinLocalService();
        coinServiceController.OnCoinsLoadedSuccessful += OnGetCoinsSuccessful;
        coinServiceController.OnCoinsLoadedError  += OnGetCoinsFail;

        IAPController.Instance.ContainsItemsIAP += OnContainsItemsFromIAP;
        IAPController.Instance.OnIAPInitializedSuccessful += OnIAPInitSuccessful;
        IAPController.Instance.OnIAPInitializedFail += OnIAPInitFail;

        IAPController.Instance.OnIAPPurchasingSuccessful += OnPurchasingSuccessful;
        IAPController.Instance.OnIAPPurchasingFail += OnPurchasingFail;

        shopIAP=new ShopLocalServiceIAP();
        shopIAP.OnPurchaseSuccessful += ConfirmPurchasingIAP;
        shopIAP.OnPurchaseFail += OnFinishPurchasingFail;

        buyService = new BuyShopItemLocalService();
        buyService.OnPurchaseSuccessful += OnBuyWithCoinsSuccessful;
        buyService.OnPurchaseFail += OnBuyWithCoinsFail;

        shopItemsPendingIAPQueue = new List<ShopItem>();
        shopItemsDitionary = new Dictionary<string, ShopItem>();
        user = PlayerPrefs.GetString(Constants.USER, "");

        CheckInternetConnection();
        Button button = buyButton.GetComponent<Button>();
        button.onClick.AddListener(BuyItem);
        buyButton.SetActive(false);
        itemSelectedHolder.Show(false);
        CloseMoreCoins();
    }

    private void OnDestroy()
    {
        internetChecker.OnCheckerFinish -= InternetChecked;

        productServiceController.ProductListService.OnShopDataLoadSuccessful -= OnShopDataLoadSuccessful;
        productServiceController.ProductListService.OnShopDataLoadedFail -= OnShopDataLoadedFail;

        coinServiceController.OnCoinsLoadedSuccessful -= OnGetCoinsSuccessful;
        coinServiceController.OnCoinsLoadedError -= OnGetCoinsFail;

        IAPController.Instance.ContainsItemsIAP -= OnContainsItemsFromIAP;
        IAPController.Instance.OnIAPInitializedSuccessful -= OnIAPInitSuccessful;
        IAPController.Instance.OnIAPInitializedFail -= OnIAPInitFail;

        IAPController.Instance.OnIAPPurchasingSuccessful -= OnPurchasingSuccessful;
        IAPController.Instance.OnIAPPurchasingFail -= OnPurchasingFail;

        shopIAP.OnPurchaseSuccessful -= ConfirmPurchasingIAP;
        shopIAP.OnPurchaseFail -= OnFinishPurchasingFail;

        buyService.OnPurchaseSuccessful -= OnBuyWithCoinsSuccessful;
        buyService.OnPurchaseFail -= OnBuyWithCoinsFail;
    }

    #region internet check
    private void InternetChecked(bool status, string message)
    {
        loading.SetActive(false);
        if (status)
        {
            GetShopData();
        }
        else
        {
            errorControllerDialog.Show(true);
            errorControllerDialog.ShowCancelButton(false);
            errorControllerDialog.Setup(message, CheckInternetConnection);
        }
    }
    #endregion

    #region Shop Items Data
    private void OnShopDataLoadSuccessful(IList<ShopCategory> shopCategories)
    {
        this.shopCategories = shopCategories;
        this.coinsIAP = new List<ShopItem>();
        RefreshCategories();
        ShowItemsFromCategory(this.shopCategories[0]);
        foreach (ShopCategory category in this.shopCategories)
        {
            foreach (ShopItem item in category.ShopItems)
            {
                shopItemsDitionary[item.BaseItem.id]= item;
                if (item.isItemFromIAP && InventoryTypeItem.COIN.Equals(item.BaseItem.inventoryType))
                {
                    coinsIAP.Add(item);
                }
            }
        }
        loading.SetActive(false);
        InitIAP();        
    }

    private void OnShopDataLoadedFail(string error,string message)
    {
        loading.SetActive(false);
        errorControllerDialog.Show(true);
        errorControllerDialog.ShowCancelButton(false);
        errorControllerDialog.Setup("Shop data problem. \n"+message, GetShopData);
    }
    #endregion

    #region IAP init
    private void OnContainsItemsFromIAP(bool containsItems)
    {
        if (!containsItems)
            GetCoins();
    }

    private void OnIAPInitSuccessful()
    {
        loading.SetActive(false);
        RefreshItems();
        GetCoins();
    }

    private void OnIAPInitFail(string message)
    {
        loading.SetActive(false);
        errorControllerDialog.Show(true);
        errorControllerDialog.ShowCancelButton(false);
        errorControllerDialog.Setup("A store conection problem ocurred. \n" + message, InitIAP );
    }
    #endregion

    #region Coins Load
    private void OnGetCoinsSuccessful(int userTotalCoins)
    {
        loading.SetActive(false);
        Coins = userTotalCoins;
        coinsText.text = Coins.ToString();
        coinsText2.text = Coins.ToString();
    }

    private void OnGetCoinsFail(string error,string message)
    {
        loading.SetActive(false);
        errorControllerDialog.Show(true);
        errorControllerDialog.ShowCancelButton(false);
        errorControllerDialog.Setup("Problem to get your coins. \n" + message, GetCoins);
    }
    #endregion

    #region IAP purchasing
    private void OnPurchasingSuccessful(ShopItem shopItem)
    {
        shopItemsPendingIAPQueue.Add(shopItem);
        loading.SetActive(false);
        FinishPurchase();
    }

    private void OnPurchasingFail(ShopItem shopItem, PurchaseFailureReason failureReason)
    {
        loading.SetActive(false);
        if (!PurchaseFailureReason.UserCancelled.Equals( failureReason))
        {
            errorControllerDialog.Show(true);
            errorControllerDialog.ShowCancelButton(true);
            errorControllerDialog.Setup("IAP problem: Something failed in your purchase. " + failureReason, BuyFromIAP);
        }
    }
    #endregion

    #region Confirm IAP
    private void ConfirmPurchasingIAP(ShopItem shopItem)
    {
        ConfirmPendingPurchase();
        if (shopItemsPendingIAPQueue.Count == 0)
        {
            loading.SetActive(false);
            Coins += shopItem.coins;
            coinsText.text = Coins.ToString();
            coinsText2.text = Coins.ToString();
            itemSelectedHolder.Show(false);
            RefreshItems();

            if (ItemSelected != null)
            {
                handAnimator.SetTrigger("handOk");
                buySuccessAnimator.SetTrigger("show");
            }
        }
    }

    private void OnFinishPurchasingFail(string error, string message)
    {
        loading.SetActive(false);
        errorControllerDialog.Show(true);
        errorControllerDialog.ShowCancelButton(true);
        errorControllerDialog.Setup("Something failed while confirming your purchase.\n" + message, FinishPurchase);
    }
    #endregion

    #region buy with coins
    private void OnBuyWithCoinsSuccessful(int totalsCoins,ShopItem shopItem)
    {
        loading.SetActive(false);
        Coins = totalsCoins;
        coinsText.text = Coins.ToString();
        coinsText2.text = Coins.ToString();
        ItemSelected.UserQuantity = shopItem.UserQuantity;
        itemSelectedHolder.Show(false);
        handAnimator.SetTrigger("handOk");
        buySuccessAnimator.SetTrigger("show");
        RefreshItems();
    }

    private void OnBuyWithCoinsFail(string error, string message)
    {
        loading.SetActive(false);
        errorControllerDialog.Show(true);
        errorControllerDialog.ShowCancelButton(true);
        errorControllerDialog.Setup("Coins Problem: Something failed in your purchase.\n" + message, BuyWithCoins);
    }
    #endregion

    public void RefreshCategories()
    {
        DeleteCategories();
        AddCategories();
    }

    public void RefreshItems()
    {
        DeleteItems();
        AddItems();
    }

    public void ShowItemsFromCategory(ShopCategory shopCategory)
    {
        if(this.CategorySelected !=null && this.CategorySelected.Equals(shopCategory))
        {
            DeleteItems();
            buyButton.SetActive(false);
            itemSelectedHolder.Show(false);
            this.ItemSelected = null;
            this.CategorySelected = null;
        }
        else
        {
            this.CategorySelected = shopCategory;
            RefreshItems();
        }
    }

    public void ShowItem(ShopItem shopItem)
    {
        CloseMoreCoins();
        this.ItemSelected = shopItem;
        itemSelectedHolder.Show(true);
        itemSelectedHolder.Setup(this.ItemSelected);
        buyButton.SetActive(!shopItem.Bought);
    }

    public void BuyItem()
    {
        if (ItemSelected != null)
        {
            if (ItemSelected.isItemFromIAP)
            {
                loading.SetActive(true);
                BuyFromIAP();
            }
            else
            {
                if (Coins >= ItemSelected.coins)
                {
                    loading.SetActive(true);
                    BuyWithCoins();
                }
                else
                {
                    itemSelectedHolder.Show(false);
                    ShowMoreCoins();
                }
            }
        }
    }

    private void CheckInternetConnection()
    {
        loading.SetActive(true);
        internetChecker.CheckWithGoogle();
    }

    private void GetShopData()
    {
        loading.SetActive(true);
        productServiceController.GetShopItems(user);
    }

    private void InitIAP()
    {
        loading.SetActive(true);
        IAPController.Instance.InitializePurchasing(this.shopCategories);
    }

    private void GetCoins()
    {
        loading.SetActive(true);
        coinServiceController.GetUserCoins(user);
    }

    private void BuyWithCoins()
    {
        loading.SetActive(true);
        buyService.BuyItem(user, this.ItemSelected);
    }

    private void BuyFromIAP()
    {
        loading.SetActive(true);
        IAPController.Instance.BuyProductID(ItemSelected.idFromIAP);
    }

    private void FinishPurchase()
    {
        loading.SetActive(true);
        if (shopItemsPendingIAPQueue.Count > 0)
            shopIAP.Buy(user, shopItemsPendingIAPQueue[0]);
        else
            loading.SetActive(false);
    }

    private void ConfirmPendingPurchase()
    {
        ShopItem itemToConfirm = shopItemsPendingIAPQueue[0];
        IAPController.Instance.ConfirmPendingPurchase(itemToConfirm.idFromIAP);
        shopItemsPendingIAPQueue.RemoveAt(0);
        if (shopItemsPendingIAPQueue.Count > 0)
            FinishPurchase();
    }

    public void CloseMoreCoins()
    {
        moreCoinsPanel.SetActive(false);
    }

    private void ShowMoreCoins()
    {
        moreCoinsPanel.SetActive(true);
        DeleteCoinsIAP();
        AddMoreCoinsButtons();
    }

    private void DeleteCategories()
    {
        DeleteChildren(contentCategoryPanel,categoryButtonPool);
    }

    private void DeleteItems()
    {
        DeleteChildren(contentItemPanel, itemButtonPool);
    }

    private void DeleteCoinsIAP()
    {
        DeleteChildren(contentMoreCoinsPanel, itemButtonPool);
    }

    private void DeleteChildren(Transform transform, SimpleObjectPool pool)
    {
        while (transform.childCount > 0)
        {
            GameObject child = transform.GetChild(0).gameObject;
            pool.ReturnObject(child);
        }
    }
    
    private void AddCategories()
    {
        if (shopCategories != null)
        {
            AddChildren<ShopCategory>(shopCategories, contentCategoryPanel, categoryButtonPool);
        }
    }

    private void AddItems()
    {
        if (CategorySelected != null && CategorySelected.ShopItems != null)
        {
            AddChildren<ShopItem>(CategorySelected.ShopItems, contentItemPanel, itemButtonPool);
        }
    }

    private void AddMoreCoinsButtons()
    {
        if(coinsIAP!=null && coinsIAP.Count>0)
            AddChildren<ShopItem>(coinsIAP, contentMoreCoinsPanel, itemButtonPool);
    }

    private void AddChildren<T>(IList<T> dataSource, Transform parent, SimpleObjectPool pool)
    {
        foreach( T data in dataSource)
        {
            GameObject button = pool.GetObject();
            button.transform.localScale = SCALE;
            button.transform.SetParent(parent,false);
            IButtonHolder<T> holder = button.GetComponent<IButtonHolder <T> >();
            holder.Setup(data, this);
        }
    }
}
