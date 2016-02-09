using DT;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace DT.GameEngine {
  public class ItemQuantityUnityEvent : UnityEvent<ItemQuantity> {}

  public class UserItemInventory : Singleton<UserItemInventory> {
    // PRAGMA MARK - Public Interface
    [HideInInspector]
    public UnityEvent UserItemsUpdated = new UnityEvent();
    [HideInInspector]
    public ItemQuantityUnityEvent GainedItemQuantity = new ItemQuantityUnityEvent();
    [HideInInspector]
    public ItemQuantityUnityEvent SpentItemQuantity = new ItemQuantityUnityEvent();

    public void GainItemQuantity(ItemQuantity gainQuantity) {
      ItemQuantity userItemQuantity = this.GetItemQuantityForItemId(gainQuantity.itemId);
      userItemQuantity.quantity += gainQuantity.quantity;
      this.UserItemsUpdated.Invoke();
      this.GainedItemQuantity.Invoke(gainQuantity);
    }

    public bool CanSpendItemQuantityList(List<ItemQuantity> neededQuantities) {
      foreach (ItemQuantity neededQuantity in neededQuantities) {
        if (!this.CanSpendItemQuantity(neededQuantity)) {
          return false;
        }
      }

      return true;
    }

    public bool CanSpendItemQuantity(ItemQuantity neededQuantity) {
      ItemQuantity userItemQuantity = this.GetItemQuantityForItemId(neededQuantity.itemId);
      return userItemQuantity.quantity >= neededQuantity.quantity;
    }

    public void SpendItemQuantityList(List<ItemQuantity> spendQuantities) {
      foreach (ItemQuantity spendQuantity in spendQuantities) {
        this.SpendItemQuantity(spendQuantity);
      }
    }

    public void SpendItemQuantity(ItemQuantity spendQuantity) {
      ItemQuantity userItemQuantity = this.GetItemQuantityForItemId(spendQuantity.itemId);
      if (userItemQuantity.quantity < spendQuantity.quantity) {
        Debug.LogWarning("SpendItemQuantity: Can't spend " + spendQuantity.quantity + " of item id: " + spendQuantity.itemId + "!");
        return;
      }

      userItemQuantity.quantity -= spendQuantity.quantity;
      this.UserItemsUpdated.Invoke();
      this.SpentItemQuantity.Invoke(spendQuantity);
    }


    // PRAGMA MARK - Internal
    [SerializeField]
    List<ItemQuantity> _userItemList = new List<ItemQuantity>();

    Dictionary<int, ItemQuantity> _userItemMap = new Dictionary<int, ItemQuantity>();

    private void Awake() {
      this.AddItemsMissingInList();
      this.RefreshMap();
    }

    private void AddItemsMissingInList() {
      foreach (Item item in Toolbox.GetInstance<ItemList>()) {
        this.CreateItemQuantityForItemIdIfNotFound(item.Id);
      }
    }

    private void CreateItemQuantityForItemIdIfNotFound(int itemId) {
      foreach (ItemQuantity itemQuantity in this._userItemList) {
        if (itemQuantity.itemId == itemId) {
          return;
        }
      }

      // not found in list, make new
      ItemQuantity newItemQuantity = new ItemQuantity(itemId, 0);
      this._userItemList.Add(newItemQuantity);
    }

    private void RefreshMap() {
      this._userItemMap.Clear();

      foreach (ItemQuantity itemQuantity in this._userItemList) {
        this._userItemMap[itemQuantity.itemId] = itemQuantity;
      }
    }

    private ItemQuantity GetItemQuantityForItemId(int itemId) {
      return this._userItemMap.SafeGet(itemId);
    }
  }
}