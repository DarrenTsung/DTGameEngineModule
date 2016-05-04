using UnityEngine;
using UnityEngine.UI;
using System.Collections;
#if TMPRO
using TMPro;

namespace DT.GameEngine {
  public class IdQuantityView : MonoBehaviour, IRecycleSetupSubscriber, IRecycleCleanupSubscriber {
    // PRAGMA MARK - Public Interface
    public void SetupWithIdQuantity<TEntity>(IdQuantity<TEntity> idQuantity) where TEntity : DTEntity {
      IViewIdQuantity viewIdQuantity = new ViewIdQuantity<TEntity>(idQuantity);

      DTEntity entity = viewIdQuantity.Entity;
      DisplayComponent displayComponent = entity.GetComponent<DisplayComponent>();
      if (displayComponent != null) {
        this._image.sprite = displayComponent.displaySprite;
      }

      if (viewIdQuantity.Quantity <= 0) {
        Debug.LogWarning("IdQuantityView - don't know how to show a quantity less than or equal to zero!");
      }

      if (viewIdQuantity.Quantity > 1) {
        this._textContainer.SetActive(true);
        this._text.SetText(string.Format("x{0}", viewIdQuantity.Quantity));
      } else {
        this._textContainer.SetActive(false);
      }
    }

    public void SetSize(Vector2 size) {
      this._containerTransform.sizeDelta = size;
    }

    public void SetImageScale(Vector2 scale) {
      this._image.transform.localScale = scale;
    }

    public void SetFontSize(float fontSize) {
      this._text.fontSize = fontSize;
    }


    // PRAGMA MARK - IRecycleSetupSubscriber Implementation
    public void OnRecycleSetup() {
      this._oldSize = this._containerTransform.sizeDelta;
      this._oldImageScale = this._image.transform.localScale;
      this._oldFontSize = this._text.fontSize;
    }


    // PRAGMA MARK - IRecycleCleanupSubscriber Implementation
    public void OnRecycleCleanup() {
      this._containerTransform.sizeDelta = this._oldSize;
      this._image.transform.localScale = this._oldImageScale;
      this._text.fontSize = this._oldFontSize;
    }


    // PRAGMA MARK - Internal
    [Header("Outlets")]
    [SerializeField]
    private RectTransform _containerTransform;

    [SerializeField]
    private Image _image;

    [SerializeField]
    private TMP_Text _text;
    [SerializeField]
    private GameObject _textContainer;

    private Vector2 _oldSize;
    private Vector2 _oldImageScale;
    private float _oldFontSize;
  }
}
#endif