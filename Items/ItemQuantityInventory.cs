using DT;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace DT.GameEngine {
  [Serializable]
  public class ItemQuantityInventory : IEnumerable<ItemQuantity> {
    // PRAGMA MARK - Public Interface
    public void GainItemQuantity(ItemQuantity gainQuantity) {
      ItemQuantity itemQuantity = this.GetItemQuantityForItemId(gainQuantity.itemId);
      itemQuantity.quantity += gainQuantity.quantity;
    }

    public bool CanSpendItemQuantityList(IEnumerable<ItemQuantity> neededQuantities) {
      foreach (ItemQuantity neededQuantity in neededQuantities) {
        if (!this.CanSpendItemQuantity(neededQuantity)) {
          return false;
        }
      }

      return true;
    }

    public bool CanSpendItemQuantity(ItemQuantity neededQuantity) {
      ItemQuantity itemQuantity = this.GetItemQuantityForItemId(neededQuantity.itemId);
      return itemQuantity.quantity >= neededQuantity.quantity;
    }

    public void SpendItemQuantityList(IEnumerable<ItemQuantity> spendQuantities) {
      foreach (ItemQuantity spendQuantity in spendQuantities) {
        this.SpendItemQuantity(spendQuantity);
      }
    }

    public void SpendItemQuantity(ItemQuantity spendQuantity) {
      ItemQuantity itemQuantity = this.GetItemQuantityForItemId(spendQuantity.itemId);
      if (itemQuantity.quantity < spendQuantity.quantity) {
        Debug.LogWarning("SpendItemQuantity: Can't spend " + spendQuantity.quantity + " of item id: " + spendQuantity.itemId + "!");
        return;
      }

      itemQuantity.quantity -= spendQuantity.quantity;
    }

    public int GetCountOfItemId(int itemId) {
      return this.GetItemQuantityForItemId(itemId).quantity;
    }


    // PRAGMA MARK - IEnumerable<ItemQuantity> Implementation
    IEnumerator IEnumerable.GetEnumerator() {
      return this.GetEnumerator();
    }

    public IEnumerator<ItemQuantity> GetEnumerator() {
      return this._itemList.GetEnumerator();
    }


    // PRAGMA MARK - Internal
    [SerializeField]
    private List<ItemQuantity> _itemList = new List<ItemQuantity>();

    private Dictionary<int, ItemQuantity> _itemMap = new Dictionary<int, ItemQuantity>();
    private bool _initialized = false;

    private void RefreshMap() {
      this._itemMap.Clear();

      foreach (ItemQuantity itemQuantity in this._itemList) {
        this._itemMap[itemQuantity.itemId] = itemQuantity;
      }
    }

    private ItemQuantity GetItemQuantityForItemId(int itemId) {
      if (!this._initialized) {
        this.RefreshMap();
        this._initialized = true;
      }

      if (!this._itemMap.ContainsKey(itemId)) {
        Item item = Toolbox.GetInstance<ItemList>().LoadById(itemId);
        if (item == null) {
          Debug.LogWarning("GetItemQuantityForItemId - called with invalid item id: " + itemId + "!");
          return null;
        }

        ItemQuantity itemQuantity = new ItemQuantity(itemId, quantity: 0);
        this._itemList.Add(itemQuantity);
        this._itemMap[itemId] = itemQuantity;
      }

      return this._itemMap[itemId];
    }
  }
}