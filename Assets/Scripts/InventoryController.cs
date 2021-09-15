using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InventoryController : MonoBehaviour {

    public ErrorController errorController;
    public Transform contentItemsPanel;
    public GameObject loading;
    public GameObject detailsCategory;
    public GameObject emptyCategory;
    public SimpleObjectPool itemsPool;
    public InventorySelectedItemHolder selectedItemHolder;
    public EquipmentHolder equipmentHolder;
    public UnityEngine.UI.Text coinsText;
    public SwipeController characterSwipeController;
    public CharacterSelectController characterSelectController;
    public Sprite defaultCharacterImage;

    private string user;
    private ICoinsService coinsService;
    private IInventoryService inventoryService;
    private IEquipmentService equipmentService;
    private Equipment equipment;
    private IDictionary<InventoryTypeItem, InventoryItem> equipmentDictionary;
    private IDictionary<InventoryTypeItem, IDictionary<string , InventoryItem>> itemsCategories;
    private InventoryItem defaultCharacter;
    private Animator emptyCategoryAnimator;

    private readonly Vector3 SCALE = new Vector3(1, 1, 1);

    void Start ()
    {
        InitDefaultCharacter();
        selectedItemHolder.Inventory = this;

        emptyCategoryAnimator = emptyCategory.GetComponent<Animator>();

        itemsCategories = new Dictionary<InventoryTypeItem, IDictionary<string, InventoryItem>>();
        equipmentDictionary = new Dictionary<InventoryTypeItem, InventoryItem>();

        inventoryService = new InventoryServiceLocal();
        inventoryService.OnLoadInventoryDataSuccessfully += OnLoadInventoryDataSuccessfully;
        inventoryService.OnLoadInventoryDataFail += OnLoadInventoryDataFail;

        characterSwipeController.OnSwipeAction += OnSwipe;

        equipmentService = new EquipmentServiceLocal();
        equipmentService.OnUpdateEquipment += OnEquimentUpdate;

        coinsService = new CoinLocalService();
        coinsService.OnCoinsLoadedSuccessful += OnCoinsLoadedSuccessful;
        coinsService.OnCoinsLoadedError += OnCoinsLoadedFail;

        user = PlayerPrefs.GetString(Constants.USER, "");
        GetCoins();
    }    

    private void OnDestroy()
    {
        inventoryService.OnLoadInventoryDataSuccessfully -= OnLoadInventoryDataSuccessfully;
        inventoryService.OnLoadInventoryDataFail -= OnLoadInventoryDataFail;
        equipmentService.OnUpdateEquipment -= OnEquimentUpdate;
        coinsService.OnCoinsLoadedSuccessful -= OnCoinsLoadedSuccessful;
        coinsService.OnCoinsLoadedError -= OnCoinsLoadedFail;
    }

    private void GetCoins()
    {
        loading.SetActive(true);
        coinsService.GetUserCoins(user);
    }

    private void GetInventory()
    {
        loading.SetActive(true);
        inventoryService.GetInventoryData(user);
    }

    private void OnCoinsLoadedSuccessful(int coins)
    {
        loading.SetActive(false);
        coinsText.text = coins.ToString();
        inventoryService.GetInventoryData(user);
    }

    private void OnCoinsLoadedFail(string error, string message)
    {
        loading.SetActive(false);
        errorController.Show(true);
        errorController.ShowCancelButton(false);
        errorController.Setup("Something failed loading the inventory\n" + message, GetCoins);
    }

    private void OnLoadInventoryDataSuccessfully(Inventory inventory)
    {
        loading.SetActive(false);
        AddItemToCategory(defaultCharacter);
        foreach (InventoryItem item in inventory.items)
        {
            if(item.quantity>0)
                AddItemToCategory(item);
        }
        InitCharacters();
        equipmentService.GetEquipment();
    }

    private void OnLoadInventoryDataFail(string error, string message)
    {
        loading.SetActive(false);
        errorController.Show(true);
        errorController.ShowCancelButton(false);
        errorController.Setup("Something failed loading the inventory\n"+message,  GetInventory);
    }

    private void OnEquimentUpdate(Equipment equipment)
    {
        this.equipment = equipment;
        UpdateEquimentDictionary(InventoryTypeItem.BODY_PART1, this.equipment.bodyPart1);
        UpdateEquimentDictionary(InventoryTypeItem.BODY_PART2, this.equipment.bodyPart2);
        UpdateEquimentDictionary(InventoryTypeItem.BODY_PART3, this.equipment.bodyPart3);
        UpdateEquimentDictionary(InventoryTypeItem.HAND, this.equipment.hand);
        equipmentHolder.Set(equipmentDictionary);
        detailsCategory.SetActive(false);
    }

    private void UpdateEquimentDictionary(InventoryTypeItem categoryID, string itemId)
    {
        InventoryItem item = null;
        if (itemsCategories.ContainsKey(categoryID))
        { 
            IDictionary<string, InventoryItem> category = itemsCategories[categoryID];
            if (!string.IsNullOrEmpty(itemId))
                item = category[itemId];
        }
        equipmentDictionary[categoryID] = item;
    }

    private void InitDefaultCharacter()
    {
        defaultCharacter = new InventoryItem();
        defaultCharacter.quantity = 1;
        defaultCharacter.baseItem.SetImage(defaultCharacterImage);
        defaultCharacter.baseItem.id = Constants.DEFAULT_CHARACTER_ID;
        defaultCharacter.baseItem.name = "Tony";
        defaultCharacter.baseItem.inventoryType = InventoryTypeItem.CHARACTER_PLAYER;
    }

    public void ShowCategory(int inventoryType)
    {
        ShowCategory((InventoryTypeItem)inventoryType);
    }

    public void ShowCategory(InventoryTypeItem inventoryType)
    {
        if (itemsCategories.ContainsKey(inventoryType))
        {
            detailsCategory.SetActive(true);
            IDictionary<string, InventoryItem> items = itemsCategories[inventoryType];
            RefreshItems(items);
        }
        else
        {
            emptyCategory.SetActive(true);
            emptyCategoryAnimator.SetTrigger("enter");
        }
    }

    public void ShowItem(InventoryItem item)
    {
        selectedItemHolder.Show(item, IsEquip(item));
    }

    public void EquipItem(InventoryItem item)
    {
        equipmentService.EquipItem(item);
    }

    public void UnequipItem(InventoryItem item)
    {
        equipmentService.UnequipItem(item);
    }

    public void ChangeCharacter(InventoryItem character)
    {
        string characterID = character.baseItem.id;
        PlayerPrefs.SetString(Constants.CHARACTER, characterID);
        PlayerPrefs.Save();
    }

    private void AddItemToCategory(InventoryItem item)
    {
        if (!itemsCategories.ContainsKey(item.baseItem.inventoryType))
        {
            itemsCategories.Add(item.baseItem.inventoryType, new Dictionary<string, InventoryItem>());
        }

        IDictionary<string, InventoryItem> dataDictionary = itemsCategories[item.baseItem.inventoryType];
        
        dataDictionary[item.baseItem.id] = item;
        itemsCategories[item.baseItem.inventoryType] = dataDictionary;
    }

    private void OnSwipe(Vector2 start, Vector2 end)
    {
        if (end.x > start.x)
            characterSelectController.NextRigth();
        else
            characterSelectController.NextLeft();
    }

    private void InitCharacters()
    {
        string characterSelect = PlayerPrefs.GetString(Constants.CHARACTER, Constants.DEFAULT_CHARACTER_ID);
        IDictionary<string, InventoryItem> characters = itemsCategories[InventoryTypeItem.CHARACTER_PLAYER];
        characterSelectController.Setup(characters, characterSelect,this);
    }

    private void RefreshItems(IDictionary<string, InventoryItem> items)
    {
        DeleteItems();
        selectedItemHolder.Clear();
        AddItems(items);
    }

    private void DeleteItems()
    {
        while (contentItemsPanel.childCount > 0)
        {
            GameObject child = contentItemsPanel.GetChild(0).gameObject;
            itemsPool.ReturnObject(child);
        }
    }

    private void AddItems(IDictionary<string, InventoryItem> items)
    {
        foreach (InventoryItem it in items.Values)
        {
            GameObject button = itemsPool.GetObject();
            button.transform.localScale = SCALE;
            button.transform.SetParent(contentItemsPanel, false);
            InventoryItemHolder holder = button.GetComponent<InventoryItemHolder>();
            bool p = IsEquip(it);
            holder.Setup(it, this, p );
            if (p)
                ShowItem(it);
        }
    }

    private bool IsEquip(InventoryItem item)
    {
        string itemId = item.baseItem.id;
        InventoryTypeItem type = item.baseItem.inventoryType;
        InventoryItem equipmentItem = equipmentDictionary[type];

        return equipmentItem != null && itemId.Equals(equipmentItem.baseItem.id);
    }
}
