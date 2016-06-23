using DT;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;
#if TMPRO
using TMPro;

namespace DT.GameEngine {
  public class IdQuantityRequirementView : MonoBehaviour, IRecycleCleanupSubscriber {
    // PRAGMA MARK - Public Interface
    public void SetupWithRequiredIdQuantity<TEntity>(IdQuantity<TEntity> requiredIdQuantity) where TEntity : DTEntity {
      IViewIdQuantity requiredViewIdQuantity = new ViewIdQuantity<TEntity>(requiredIdQuantity);

      this._requiredViewIdQuantity = requiredViewIdQuantity;
      this._requiredViewIdQuantity.OnUserInventoryUpdated += this.HandleInventoryUpdated;

      DTEntity entity = this._requiredViewIdQuantity.Entity;
      DisplayComponent displayComponent = entity.GetComponent<DisplayComponent>();
      if (displayComponent != null) {
        this._requiredImage.sprite = displayComponent.displaySprite;
      }

      this.UpdateUserCount();
    }

    public void SetSize(Vector2 size) {
      this._layoutElement.preferredWidth = size.x;
      this._layoutElement.preferredHeight = size.y;
    }


    // PRAGMA MARK - IRecycleCleanupSubscriber Implementation
    public void OnRecycleCleanup() {
      if (this._requiredViewIdQuantity != null) {
        this._requiredViewIdQuantity.OnUserInventoryUpdated -= this.HandleInventoryUpdated;
        this._requiredViewIdQuantity = null;
      }
    }


    // PRAGMA MARK - Internal
    [Header("Outlets")]
    [SerializeField]
    private Image _requiredImage;
    [SerializeField]
    private TMP_Text _requiredLabel;

    private IViewIdQuantity _requiredViewIdQuantity;
    private LayoutElement _layoutElement;

    void Awake() {
      this._layoutElement = this.GetRequiredComponent<LayoutElement>();
    }

    private void HandleInventoryUpdated() {
      this.UpdateUserCount();
    }

    private void UpdateUserCount() {
      if (this._requiredViewIdQuantity == null) {
        Debug.LogWarning("UpdateUserCount - called when required view item quantity is null!");
        return;
      }

      this._requiredLabel.text = string.Format("{0} / {1}", this._requiredViewIdQuantity.UserQuantity, this._requiredViewIdQuantity.Quantity);
    }
  }
}
#endif