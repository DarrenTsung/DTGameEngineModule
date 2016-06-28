using System;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;

namespace DT {
  public class AmountDelegate : MonoBehaviour {
    public virtual void SetAmount(int amount) {
      this.SetTextOutletWithAmount(amount);
    }


    // PRAGMA MARK - Internal
    [Header("Outlets")]
    [SerializeField] protected TextOutlet _textOutlet;

    protected virtual void SetTextOutletWithAmount(int amount) {
      this._textOutlet.Text = amount.ToString();
    }
  }
}