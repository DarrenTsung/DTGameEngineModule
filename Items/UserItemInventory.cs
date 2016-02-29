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
    public UnityEvent OnUserItemsUpdated = new UnityEvent();
    [HideInInspector]
    public ItemQuantityUnityEvent OnGainedItemQuantity = new ItemQuantityUnityEvent();
    [HideInInspector]
    public ItemQuantityUnityEvent OnSpentItemQuantity = new ItemQuantityUnityEvent();

    public void GainItemQuantity(ItemQuantity gainQuantity) {
      this._itemInventory.GainItemQuantity(gainQuantity);
      this.OnUserItemsUpdated.Invoke();
      this.OnGainedItemQuantity.Invoke(gainQuantity);
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
      return this._itemInventory.CanSpendItemQuantity(neededQuantity);
    }

    public void SpendItemQuantityList(List<ItemQuantity> spendQuantities) {
      foreach (ItemQuantity spendQuantity in spendQuantities) {
        this.SpendItemQuantity(spendQuantity);
      }
    }

    public void SpendItemQuantity(ItemQuantity spendQuantity) {
      this._itemInventory.SpendItemQuantity(spendQuantity);
      this.OnUserItemsUpdated.Invoke();
      this.OnSpentItemQuantity.Invoke(spendQuantity);
    }


    // PRAGMA MARK - Internal
    [SerializeField]
    private ItemQuantityInventory _itemInventory = new ItemQuantityInventory();
  }
}