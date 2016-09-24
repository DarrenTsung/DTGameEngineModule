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
      IUserIdInventory inventory = this.GetSelectedInventory();
      int selectedId = this.GetSelectedId();

      inventory.AddIdQuantity(selectedId, quantity: 1);
      this.SyncInputFieldToInventory();
    }

    public void HandleRemoveButtonTapped() {
      IUserIdInventory inventory = this.GetSelectedInventory();
      int selectedId = this.GetSelectedId();

      inventory.RemoveIdQuantity(selectedId, quantity: 1);
      this.SyncInputFieldToInventory();
    }

    public void HandleApplyButtonTapped() {
      IUserIdInventory inventory = this.GetSelectedInventory();
      int selectedId = this.GetSelectedId();

      int inputAmount;
      try {
        inputAmount = Int32.Parse(this._amountInputField.text);
      } catch (Exception e) {
        Debug.LogError(string.Format("{0}: Parse Error: {1}", this._amountInputField.text, e));
        return;
      }

      inventory.SetIdQuantity(selectedId, inputAmount);
    }


		// PRAGMA MARK - Internal
    [Header("Outlets")]
		[SerializeField] private Dropdown _entityTypeDropdown;
		[SerializeField] private Dropdown _idDropdown;

    [SerializeField] private InputField _amountInputField;

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

      this.SyncInputFieldToInventory();
    }

    private IUserIdInventory GetSelectedInventory() {
      Type entityType = DTEntityUtil.EntitySubclasses[this._entityTypeDropdown.value];
      return UserIdInventoryLocator.GetInventoryForType(entityType);
    }

    private int GetSelectedId() {
      Type entityType = DTEntityUtil.EntitySubclasses[this._entityTypeDropdown.value];
      IIdList list = IdListLocator.GetListForType(entityType);
      List<int> ids = list.Ids().ToList();

      return ids[this._idDropdown.value];
    }

    private void SyncInputFieldToInventory() {
      IUserIdInventory inventory = this.GetSelectedInventory();
      int selectedId = this.GetSelectedId();

      this._amountInputField.text = inventory.GetCountOfId(selectedId).ToString();
    }
	}
}