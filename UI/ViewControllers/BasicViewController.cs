using System;
using System.Collections;
using System.Collections.Generic;
﻿using UnityEngine;
using UnityEngine.Events;

namespace DT {
  // NOTE: this view controller only handles presenting a single view 
  public abstract class BasicViewController : IViewController, IStartShowSubscriber<IView>, 
                                                               IEndShowSubscriber<IView>,
                                                               IStartDismissSubscriber<IView>,
                                                               IEndDismissSubscriber<IView> {
    // PRAGMA MARK - IViewController implementation
    public void Show() {
      this.InitializeViewIfNeededAndDo(() => {
        this._view.Show();
      });
    }
    
    public void Dismiss() {
      this.InitializeViewIfNeededAndDo(() => {
        this._view.Dismiss();
      });
    }
    
    
    // PRAGMA MARK - Internal
    protected ShowDismissEvents<IViewController> _showDismissEvents = new ShowDismissEvents<IViewController>();
    protected IView _view;
    protected string _viewPrefabName;
    
    protected void InitializeViewIfNeededAndDo(Action callback) {
      // load the view if uninitialized
      if (this._view == null) {
        this.InitializeView(() => {
          callback();
        });
      } else {
        callback();
      }
    }
    
    protected void InitializeView(Action callback) {
      PrefabLoader.InstantiatePrefab(this._viewPrefabName, (GameObject loadedObject) => {
        this._view = loadedObject.GetComponent<IView>();
        this._view.AddShowDismissEvents(this);
        
        // HACK: make this better Darren
        GameObject[] canvases = GameObject.FindGameObjectsWithTag("Canvas");
        loadedObject.transform.SetParent(canvases[0].transform);
        
        IContextContainer contextContainer = this._view as IContextContainer;
        if (contextContainer == null) {
          Debug.LogError("InitializeView: view is not an IContextContainer, probably should be!");
        } else {
          contextContainer.ProvideContext(this);
        }
        
        callback();
      });
    }
    
    
    // PRAGMA MARK - IStartShowSubscriber<IView> implementation
    protected UnityEvent _onEndShowOnce = new UnityEvent();
    
    public void OnStartShow(IView view) {
      this._showDismissEvents.InvokeOnStartShow(this);
    }
    
    // PRAGMA MARK - IEndShowSubscriber<IView> implementation
    public void OnEndShow(IView view) {
      this._onEndShowOnce.Invoke();
      this._onEndShowOnce.RemoveAllListeners();
      
      this._showDismissEvents.InvokeOnEndShow(this);
    }
    
    // PRAGMA MARK - IStartDismissSubscriber<IView> implementation
    public void OnStartDismiss(IView view) {
      this._showDismissEvents.InvokeOnStartDismiss(this);
    }
    
    // PRAGMA MARK - IEndDismissSubscriber<IView> implementation
    public void OnEndDismiss(IView view) {
      this._showDismissEvents.InvokeOnEndDismiss(this);
    }
    
    
    // PRAGMA MARK - IShowDismissEvents implementation
    public void AddShowDismissEvents(object subscriber) {
      this._showDismissEvents.AddShowDismissEvents(subscriber);
    }
    
    public void RemoveShowDismissEvents(object subscriber) {
      this._showDismissEvents.RemoveShowDismissEvents(subscriber);
    }
  }
}