using UnityEngine;
using UnityEngine.UI;
using System.Collections;
#if TMPRO
using TMPro;

namespace DT.GameEngine {
  public class IdQuantityView : MonoBehaviour {
    // PRAGMA MARK - Public Interface
    public void SetupWithViewIdQuantity<TEntity>(IdQuantity<TEntity> idQuantity) where TEntity : DTEntity {
      IViewIdQuantity viewIdQuantity = new ViewIdQuantity<TEntity>(idQuantity);

      DTEntity entity = viewIdQuantity.Entity;
      DisplayComponent displayComponent = entity.GetComponent<DisplayComponent>();
      if (displayComponent != null) {
        this._image.sprite = displayComponent.displaySprite;
      }
      this._text.SetText(string.Format("x{0}", viewIdQuantity.Quantity));
    }


    // PRAGMA MARK - Internal
    [Header("Outlets")]
    [SerializeField]
    private Image _image;
    [SerializeField]
    private TMP_Text _text;
  }
}
#endif