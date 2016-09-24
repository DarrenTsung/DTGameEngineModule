using DT;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DT.GameEngine {
  public class DebugMenuView : MonoBehaviour {
    // PRAGMA MARK - Button Callbacks
    public void SetMenuItemsToHidden() {
      this._animator.SetBool("Expanded", false);
    }

    public void SetMenuItemsToExpanded() {
      this._animator.SetBool("Expanded", true);
    }


    // PRAGMA MARK - Internal
    [SerializeField] private GameObject _menuItemsContainer;
    [SerializeField] private GameObject _activeMenuItemContainer;

    private IList<DebugMenuItemView> _menuItemViews;
    private DebugMenuItem _activeItem;

    private Animator _animator;

    void Awake() {
      this._animator = this.GetRequiredComponent<Animator>();
      this.CreateMenuItems();
    }

    private void CreateMenuItems() {
      if (DebugMenuItem.MenuItems == null) {
        return;
      }

      if (DebugMenuItem.MenuItems.Length <= 0) {
        return;
      }

      List<DebugMenuItemView> menuItemViews = new List<DebugMenuItemView>();

      foreach (DebugMenuItem item in DebugMenuItem.MenuItems) {
        var itemView = ObjectPoolManager.Instantiate<DebugMenuItemView>("DebugMenuItem", parent: this._menuItemsContainer);
        itemView.Configure(item, this.HandleItemTapped);

        menuItemViews.Add(itemView);
      }

      this._menuItemViews = menuItemViews;
      this.HandleItemTapped(DebugMenuItem.MenuItems[0]);
      this.SetMenuItemsToExpanded();
    }

    private void HandleItemTapped(DebugMenuItem item) {
      foreach (DebugMenuItemView view in this._menuItemViews) {
        view.SetToggled(item == view.Item);
      }

      this.CleanupActiveItem();

      this._activeItem = item;
      this._activeItem.SetupOn(this._activeMenuItemContainer);
      this.SetMenuItemsToHidden();
    }

    private void CleanupActiveItem() {
      if (this._activeItem == null) {
        return;
      }

      this._activeItem.Cleanup();
      this._activeItem = null;
    }
  }
}