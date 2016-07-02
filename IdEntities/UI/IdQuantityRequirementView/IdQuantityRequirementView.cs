using DT;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;
#if TMPRO
using TMPro;

namespace DT.GameEngine {
  public class IdQuantityRequirementView : MonoBehaviour, IRecycleCleanupSubscriber {
    // PRAGMA MARK - Public Interface
    public void SetupWithRequiredIdQuantity(IIdQuantity requiredIdQuantity) {
      this._requiredIdQuantity = requiredIdQuantity;
      this._requiredIdQuantity.UserInventory.OnInventoryUpdated += this.HandleInventoryUpdated;

      this._requiredImage.sprite = this._requiredIdQuantity.Entity.DisplaySprite();
      this.UpdateUserCount();
    }

    public void SetSize(Vector2 size) {
      this._layoutElement.preferredWidth = size.x;
      this._layoutElement.preferredHeight = size.y;
    }


    // PRAGMA MARK - IRecycleCleanupSubscriber Implementation
    public void OnRecycleCleanup() {
      if (this._requiredIdQuantity != null) {
        this._requiredIdQuantity.UserInventory.OnInventoryUpdated -= this.HandleInventoryUpdated;
        this._requiredIdQuantity = null;
      }
    }


    // PRAGMA MARK - Internal
    [Header("Outlets")]
    [SerializeField]
    private Image _requiredImage;
    [SerializeField]
    private TMP_Text _requiredLabel;

    private IIdQuantity _requiredIdQuantity;
    private LayoutElement _layoutElement;

    void Awake() {
      this._layoutElement = this.GetRequiredComponent<LayoutElement>();
    }

    private void HandleInventoryUpdated() {
      this.UpdateUserCount();
    }

    private void UpdateUserCount() {
      if (this._requiredIdQuantity == null) {
        Debug.LogWarning("UpdateUserCount - called when required item quantity is null!");
        return;
      }

      this._requiredLabel.text = string.Format("{0} / {1}", this._requiredIdQuantity.UserQuantity(), this._requiredIdQuantity.Quantity);
    }
  }
}
#endif