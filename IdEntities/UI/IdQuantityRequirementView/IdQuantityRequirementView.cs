using UnityEngine;
using UnityEngine.UI;
using System.Collections;
#if TMPRO
using TMPro;

namespace DT.GameEngine {
  public class IdQuantityRequirementView : MonoBehaviour {
    // PRAGMA MARK - Public Interface
    public void SetupWithRequiredIdQuantity<TEntity>(IdQuantity<TEntity> requiredIdQuantity) where TEntity : DTEntity, new() {
      IViewIdQuantity requiredViewIdQuantity = new ViewIdQuantity<TEntity>(requiredIdQuantity);

      this._requiredViewIdQuantity = requiredViewIdQuantity;

      DTEntity entity = this._requiredViewIdQuantity.Entity;
      DisplayComponent displayComponent = entity.GetComponent<DisplayComponent>();
      if (displayComponent != null) {
        this._requiredImage.sprite = displayComponent.displaySprite;
      }

      this.UpdateUserCount();
    }


    // PRAGMA MARK - Internal
    [Header("Outlets")]
    [SerializeField]
    private Image _requiredImage;
    [SerializeField]
    private TMP_Text _requiredLabel;

    private IViewIdQuantity _requiredViewIdQuantity;

    // TODO (darren): hook this up
    private void HandleUserQuantityUpdated() {
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