using System.Collections;
using UnityEngine;
using UnityEngine.UI;

namespace DT.GameEngine {
  public class IdQuantityView : MonoBehaviour {
    private enum QuantityFormatterType {
      xQuantity,
      QuantityName,
    }

    // PRAGMA MARK - Public Interface
    public void Configure(IIdQuantity idQuantity) {
      DisplayComponent displayComponent = idQuantity.Entity.GetComponent<DisplayComponent>();
      if (displayComponent != null) {
        this._image.Sprite = displayComponent.displaySprite;
      }

      this.FormatText(idQuantity);
    }

    public BasicDisplayDelegate DisplayDelegate {
      get { return this._displayDelegate; }
    }

    public LayoutElement LayoutElement {
      get { return this._layoutElement; }
    }


    // PRAGMA MARK - Internal
    [Header("Outlets")]
    [SerializeField] private BasicDisplayDelegate _displayDelegate;
    [SerializeField] private LayoutElement _layoutElement;

    [Space]
    [SerializeField] private SpriteOutlet _image;

    [SerializeField] private TextOutlet _text;
    [SerializeField] private GameObject _textContainer;

    [Header("Properties")]
    [SerializeField] private QuantityFormatterType _formatterType = QuantityFormatterType.QuantityName;
    [SerializeField] private bool _hideTextIfQuantityIsZero = true;


    private void FormatText(IIdQuantity idQuantity) {
      if (this._hideTextIfQuantityIsZero && idQuantity.Quantity <= 0) {
        this._textContainer.SetActive(false);
        return;
      }

      this._textContainer.SetActive(true);
      switch (this._formatterType) {
        case QuantityFormatterType.xQuantity:
          this._text.Text = string.Format("x{0}", idQuantity.Quantity);
          break;
        case QuantityFormatterType.QuantityName:
        default:
        {
          string text = idQuantity.Quantity > 1 ? string.Format("{0} ", idQuantity.Quantity) : "";
          text += idQuantity.Entity.DisplayName();
          this._text.Text = text;
          break;
        }
      }
    }
  }
}
