using UnityEngine;
using UnityEngine.UI;
using System.Collections;
#if TMPRO
using TMPro;

namespace DT.GameEngine {
  public class ItemQuantityView : MonoBehaviour {
    // PRAGMA MARK - Public Interface
    public void SetupWithItemQuantity(ItemQuantity itemQuantity) {
      Item item = Toolbox.GetInstance<ItemList>().LoadById(itemQuantity.itemId);
      this._image.sprite = item.displaySprite;
      this._text.SetText(string.Format("x{0}", itemQuantity.quantity));
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