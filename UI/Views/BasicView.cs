using System;
using System.Collections;
using System.Collections.Generic;
﻿using UnityEngine;

namespace DT {
  public class BasicView : MonoBehaviour, IView {
    // PRAGMA MARK - IView implementation
    public virtual void Show() {
      this._showDismissEvents.InvokeOnStartShow(this);
    }
    
    public virtual void Dismiss() {
      this._showDismissEvents.InvokeOnStartDismiss(this);
    }
    
    public virtual void EndShow() {
      this._showDismissEvents.InvokeOnEndShow(this);
    }
    
    public virtual void EndDismiss() {
      this._showDismissEvents.InvokeOnEndDismiss(this);
    }
    
    
    // PRAGMA MARK - Internal
    private ShowDismissEvents<IView> _showDismissEvents = new ShowDismissEvents<IView>();
    
    
    // PRAGMA MARK - IShowDismissEvents implementation
    public void AddShowDismissEvents(object subscriber) {
      this._showDismissEvents.AddShowDismissEvents(subscriber);
    }
    
    public void RemoveShowDismissEvents(object subscriber) {
      this._showDismissEvents.RemoveShowDismissEvents(subscriber);
    }
  }
}