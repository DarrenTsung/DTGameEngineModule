using DT;
using System;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;

namespace DT.GameEngine {
  public class DebugMenuItemView : MonoBehaviour {
    // PRAGMA MARK - Public Interface
    public void Configure(DebugMenuItem item, Action<DebugMenuItem> tappedCallback) {
      this.Item = item;
      this._tappedCallback = tappedCallback;

      this._menuItemNameOutlet.Text = this.Item.Name;
    }

    public void SetToggled(bool toggled) {
      this._toggledButton.interactable = !toggled;
      this._buttonImage.raycastTarget = !toggled;
    }

    public DebugMenuItem Item {
      get; private set;
    }


    // PRAGMA MARK - Button Callbacks
    public void HandleTapped() {
      this._tappedCallback.Invoke(this.Item);
    }


    // PRAGMA MARK - Internal
    [SerializeField] private TextOutlet _menuItemNameOutlet;
    [SerializeField] private Button _toggledButton;
    [SerializeField] private Image _buttonImage;

    private Action<DebugMenuItem> _tappedCallback;
  }
}