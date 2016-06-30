using System;
using System.Collections;
using System.Collections.Generic;
﻿using UnityEngine;
using UnityEngine.Events;

namespace DT {
  // NOTE: this view controller only handles presenting a single view
  public abstract class BasicViewController<TView> : IViewController, IStartShowSubscriber<IView>,
                                                                      IEndShowSubscriber<IView>,
                                                                      IStartDismissSubscriber<IView>,
                                                                      IEndDismissSubscriber<IView>
                                                                      where TView : MonoBehaviour, IView {
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
    protected TView _view;
    protected string _viewPrefabName;

    protected void InitializeViewIfNeededAndDo(Action callback) {
      // load the view if uninitialized
      if (this._view == null) {
        this.InitializeView();
      }

      callback.Invoke();
    }

    protected void InitializeView() {
      GameObject loadedObject = ObjectPoolManager.Instantiate(this._viewPrefabName);

      this._view = loadedObject.GetComponent<TView>();
      this._view.AddShowDismissEvents(this);

      ViewManagerLocator.Main.AttachView(loadedObject);

      IContextContainer contextContainer = this._view as IContextContainer;
      if (contextContainer == null) {
        Debug.LogError("InitializeView: view is not an IContextContainer, probably should be!");
      } else {
        contextContainer.ProvideContext(this);
      }

      this.OnViewInitialized();
    }

    protected void RecycleView() {
      if (this._view == null) {
        Debug.LogWarning("RecycleView called with no view!");
        return;
      }

      this._view.RemoveShowDismissEvents(this);
      ObjectPoolManager.Recycle(this._view.gameObject);
      this._view = null;
    }


    // PRAGMA MARK - IStartShowSubscriber<IView> implementation
    protected UnityEvent _onStartShowOnce = new UnityEvent();
    protected UnityEvent _onEndShowOnce = new UnityEvent();
    protected UnityEvent _onStartDismissOnce = new UnityEvent();
    protected UnityEvent _onEndDismissOnce = new UnityEvent();

    public virtual void OnStartShow(IView view) {
      this._onStartShowOnce.Invoke();
      this._onStartShowOnce.RemoveAllListeners();

      this._showDismissEvents.InvokeOnStartShow(this);
    }

    // PRAGMA MARK - IEndShowSubscriber<IView> implementation
    public virtual void OnEndShow(IView view) {
      this._onEndShowOnce.Invoke();
      this._onEndShowOnce.RemoveAllListeners();

      this._showDismissEvents.InvokeOnEndShow(this);
    }

    // PRAGMA MARK - IStartDismissSubscriber<IView> implementation
    public virtual void OnStartDismiss(IView view) {
      this._onStartDismissOnce.Invoke();
      this._onStartDismissOnce.RemoveAllListeners();

      this._showDismissEvents.InvokeOnStartDismiss(this);
    }

    // PRAGMA MARK - IEndDismissSubscriber<IView> implementation
    public virtual void OnEndDismiss(IView view) {
      this._onEndDismissOnce.Invoke();
      this._onEndDismissOnce.RemoveAllListeners();

      this._showDismissEvents.InvokeOnEndDismiss(this);
      this.RecycleView();
    }


    // PRAGMA MARK - IShowDismissEvents implementation
    public void AddShowDismissEvents(object subscriber) {
      this._showDismissEvents.AddShowDismissEvents(subscriber);
    }

    public void RemoveShowDismissEvents(object subscriber) {
      this._showDismissEvents.RemoveShowDismissEvents(subscriber);
    }


    // PRAGMA MARK - Internal
    // optional
    protected virtual void OnViewInitialized() {
    }
  }
}