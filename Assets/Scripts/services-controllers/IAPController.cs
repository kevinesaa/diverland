using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Purchasing;

public class IAPController : MonoBehaviour, IStoreListener
{
    public event Action<bool> ContainsItemsIAP;
    public event Action OnIAPInitializedSuccessful;
    public event Action<string> OnIAPInitializedFail;
    public event Action<ShopItem> OnIAPPurchasingSuccessful;
    public event Action<ShopItem, PurchaseFailureReason> OnIAPPurchasingFail;
    public static IAPController Instance { get; private set; }
    private static IStoreController m_StoreController;          // The Unity Purchasing system.
    private static IExtensionProvider m_StoreExtensionProvider; // The store-specific Purchasing subsystems.
    private IDictionary<string, ShopItem> productsFromIAP;

    public bool ContainItemsFromIAP { get; private set; }
    
    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
            Destroy(gameObject);
    }

    private void Start()
    {
        productsFromIAP = new Dictionary<string, ShopItem>();
    }
    

    public void InitializePurchasing(IList<ShopCategory> shopCategories)
    {
        ContainItemsFromIAP = false;
        AddItemsFromIAP(shopCategories);
        if (ContainsItemsIAP != null)
                ContainsItemsIAP(ContainItemsFromIAP);

        if (IsInitialized())
        {
            InitProductPrices();
            return;
        }
        
        if (ContainItemsFromIAP)
        {
            ConfigurationBuilder builder = ConfigurationBuilder.Instance(StandardPurchasingModule.Instance());
            foreach (ShopItem item in productsFromIAP.Values)
            {
                ProductType productType = ProductType.Consumable;
                switch (item.typeFromIAP)
                {
                    case ShopItem.TypeFromIAP.NonConsumable: productType = ProductType.NonConsumable; break;
                    case ShopItem.TypeFromIAP.Subscription: productType = ProductType.Subscription; break;
                }
                builder.AddProduct(item.idFromIAP, productType);
            }
            UnityPurchasing.Initialize(this, builder);
        }
    }

    private void AddItemsFromIAP(IList<ShopCategory> shopCategories)
    {
        foreach (ShopCategory category in shopCategories)
        {
            if (category.ShopItems != null)
            {
                foreach (ShopItem item in category.ShopItems)
                {
                    if (item.isItemFromIAP)
                    {
                        productsFromIAP[item.idFromIAP] = item;
                        ContainItemsFromIAP = true;
                    }
                }
            }
        }
    }

    public bool IsInitialized()
    {
        return m_StoreController != null && m_StoreExtensionProvider != null;
    }

    public void ConfirmPendingPurchase(string productIdIAP)
    {
        if (IsInitialized())
        {
            Product product = m_StoreController.products.WithID(productIdIAP);

            if (product != null && product.availableToPurchase)
            {
                Debug.Log(string.Format("Purchasing product asychronously: '{0}'", product.definition.id));
                m_StoreController.ConfirmPendingPurchase(product);
            }
            else
            {
                Debug.Log("BuyProductID: FAIL. Not purchasing product, either is not found or is not available for purchase");
            }
        }
        else
        {
            Debug.Log("BuyProductID FAIL. Not initialized.");
        }
    }


    public void BuyProductID(string productIdIAP)
    {
        if (IsInitialized())
        {
            Product product = m_StoreController.products.WithID(productIdIAP);

            if (product != null && product.availableToPurchase)
            {
                Debug.Log(string.Format("Purchasing product asychronously: '{0}'", product.definition.id));
                m_StoreController.InitiatePurchase(product);
            }
            else
            {
                Debug.Log("BuyProductID: FAIL. Not purchasing product, either is not found or is not available for purchase");
            }
        }
        else
        {
            Debug.Log("BuyProductID FAIL. Not initialized.");
        }
    }

    public void RestorePurchases()
    {
        if (!IsInitialized())
        {
            Debug.Log("RestorePurchases FAIL. Not initialized.");
            return;
        }

        if (Application.platform == RuntimePlatform.IPhonePlayer ||
            Application.platform == RuntimePlatform.OSXPlayer)
        {
            Debug.Log("RestorePurchases started ...");

        
            var apple = m_StoreExtensionProvider.GetExtension<IAppleExtensions>();
            apple.RestoreTransactions((result) => {
            Debug.Log("RestorePurchases continuing: " + result + ". If no further messages, no purchases available to restore.");
            });
        }
        else
        {
            Debug.Log("RestorePurchases FAIL. Not supported on this platform. Current = " + Application.platform);
        }
    }

    //  
    // --- IStoreListener
    //
    public void OnInitialized(IStoreController controller, IExtensionProvider extensions)
    {
        Debug.Log("OnInitialized: PASS");
        m_StoreController = controller;
        m_StoreExtensionProvider = extensions;
        InitProductPrices();
    }

    private void InitProductPrices()
    {
        foreach (Product product in m_StoreController.products.all)
        {
            productsFromIAP[product.definition.id].PriceStringIAP = product.metadata.localizedPriceString;
        }
        if (OnIAPInitializedSuccessful != null)
            OnIAPInitializedSuccessful();
    }

    public void OnInitializeFailed(InitializationFailureReason error)
    {
        Debug.Log("OnInitializeFailed InitializationFailureReason:" + error);
        if (OnIAPInitializedFail != null)
            OnIAPInitializedFail(error.ToString());
    }

    public PurchaseProcessingResult ProcessPurchase(PurchaseEventArgs args)
    {
        if (OnIAPPurchasingSuccessful != null)
            OnIAPPurchasingSuccessful(productsFromIAP[args.purchasedProduct.definition.id]);

        return PurchaseProcessingResult.Pending;
    }


    public void OnPurchaseFailed(Product product, PurchaseFailureReason failureReason)
    {
        Debug.Log(string.Format("OnPurchaseFailed: FAIL. Product: '{0}', PurchaseFailureReason: {1}", product.definition.storeSpecificId, failureReason));
        if (OnIAPInitializedFail != null)
            OnIAPPurchasingFail(productsFromIAP[product.definition.id], failureReason);
    }
}