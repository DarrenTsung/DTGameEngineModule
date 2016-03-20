using UnityEngine;
using UnityEngine.UI;
using System.Collections;
#if TMPRO
using TMPro;

namespace DT.GameEngine {
  public class ItemQuantityRequirementView : MonoBehaviour {
    // PRAGMA MARK - Public Interface
    public void SetupWithRequiredItemQuantity(ItemQuantity requiredItemQuantity) {
      Item item = ItemList.Instance.LoadById(requiredItemQuantity.itemId);
      this._requiredItemImage.sprite = item.displaySprite;

      this._requiredItemQuantity = requiredItemQuantity;

      this.UpdateUserCount();
    }


    // PRAGMA MARK - Internal
    [Header("Outlets")]
    [SerializeField]
    private Image _requiredItemImage;
    [SerializeField]
    private TMP_Text _requiredLabel;

    private ItemQuantity _requiredItemQuantity;

    private void HandleUserItemsUpdated() {
      this.UpdateUserCount();
    }

    private void UpdateUserCount() {
      if (this._requiredItemQuantity == null) {
        Debug.LogWarning("UpdateUserCount - called when required item quantity is null!");
        return;
      }

      int userCount = UserItemInventory.Instance.GetCountOfItemId(this._requiredItemQuantity.itemId);

      this._requiredLabel.text = string.Format("{0} / {1}", userCount, this._requiredItemQuantity.quantity);
    }
  }
}
#endif