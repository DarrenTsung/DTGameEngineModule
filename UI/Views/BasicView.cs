using System;
using System.Collections;
using System.Collections.Generic;
﻿using UnityEngine;

namespace DT {
  public class BasicView<TViewController> : MonoBehaviour, IView, IContextContainer<TViewController> where TViewController : IViewController {
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
    
    
    // PRAGMA MARK - IContextContainer, IContextContainer<TViewController> implementation
    public void ProvideContext(object viewController) {
      TViewController context = default(TViewController);
      
      try {
        context = (TViewController)viewController;
      } catch (InvalidCastException) {
        Debug.LogError("BasicView->ProvideContext: invaild cast to TViewController: " + typeof(TViewController).Name + "!");
      }
      
      this.ProvideContext(context);
    }
    
    public void ProvideContext(TViewController viewController) {
      if (viewController == null) {
        Debug.LogError("ProvideContext: invalid context for BasicView!");
        return;
      }
      
      this._viewController = viewController;
    }
    
    
    // PRAGMA MARK - Internal
    protected TViewController _viewController;
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