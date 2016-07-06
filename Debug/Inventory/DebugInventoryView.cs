using DT;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
﻿using UnityEngine;
﻿using UnityEngine.UI;

namespace DT.GameEngine {
	public class DebugInventoryView : MonoBehaviour {
    // PRAGMA MARK - Button Callbacks
    public void HandleAddButtonTapped() {
      Type entityType = DTEntityUtil.EntitySubclasses[this._entityTypeDropdown.value];
      IIdList list = IdListLocator.GetListForType(entityType);
      List<int> ids = list.Ids().ToList();

      int selectedId = ids[this._idDropdown.value];

      IUserIdInventory inventory = UserIdInventoryLocator.GetInventoryForType(entityType);
      inventory.AddIdQuantity(selectedId, quantity: 1);
    }


		// PRAGMA MARK - Internal
    [Header("Outlets")]
		[SerializeField] private Dropdown _entityTypeDropdown;
		[SerializeField] private Dropdown _idDropdown;

    void Awake() {
      this._entityTypeDropdown.ClearOptions();
      this._idDropdown.ClearOptions();

      this._entityTypeDropdown.onValueChanged.AddListener(this.HandleEntityTypeChanged);

      this._entityTypeDropdown.AddOptions(DTEntityUtil.EntitySubclasses.Select(t => t.Name).ToList());
      this._entityTypeDropdown.value = 0;
      this.HandleEntityTypeChanged(0);
    }

    private void HandleEntityTypeChanged(int index) {
      if (!DTEntityUtil.EntitySubclasses.ContainsIndex(index)) {
        Debug.LogError("HandleEntityTypeChanged - index not in entity subclasses list!");
        return;
      }

      Type newEntityType = DTEntityUtil.EntitySubclasses[index];

      this._idDropdown.ClearOptions();
      IIdList list = IdListLocator.GetListForType(newEntityType);
      List<string> idOptions = list.Ids().Select(id => list.LoadById(id)).Select(entity => entity.GetDropdownTitle()).ToList();
      this._idDropdown.AddOptions(idOptions);
      this._idDropdown.value = 0;
    }
	}
}