using System;
using System.Collections;
using System.Collections.Generic;
﻿using UnityEngine;

namespace DT {
  public class ShowDismissEvents<T> : IShowDismissEvents<T> {
    // PRAGMA MARK - Public Interface 
    
    
    // PRAGMA MARK - Internal
    private event Action<T> _onStartShow = delegate {};
    private event Action<T> _onEndShow = delegate {};
    private event Action<T> _onStartDismiss = delegate {};
    private event Action<T> _onEndDismiss = delegate {};
    
    public void InvokeOnStartShow(T obj) {
      this._onStartShow(obj);
    }
    
    public void InvokeOnEndShow(T obj) {
      this._onEndShow(obj);
    }
    
    public void InvokeOnStartDismiss(T obj) {
      this._onStartDismiss(obj);
    }
    
    public void InvokeOnEndDismiss(T obj) {
      this._onEndDismiss(obj);
    }
    
    
    // PRAGMA MARK - IShowDismissEvents implementation
    public void AddShowDismissEvents(object subscriber) {
      IStartShowSubscriber<T> v = subscriber as IStartShowSubscriber<T>;
      if (v != null) {
        this._onStartShow += v.OnStartShow;
      }
      
      IEndShowSubscriber<T> x = subscriber as IEndShowSubscriber<T>;
      if (x != null) {
        this._onEndShow += x.OnEndShow;
      }
      
      IStartDismissSubscriber<T> y = subscriber as IStartDismissSubscriber<T>;
      if (y != null) {
        this._onStartDismiss += y.OnStartDismiss;
      }
      
      IEndDismissSubscriber<T> z = subscriber as IEndDismissSubscriber<T>;
      if (z != null) {
        this._onEndDismiss += z.OnEndDismiss;
      }
    }
    
    public void RemoveShowDismissEvents(object subscriber) {
      IStartShowSubscriber<T> v = subscriber as IStartShowSubscriber<T>;
      if (v != null) {
        this._onStartShow -= v.OnStartShow;
      }
      
      IEndShowSubscriber<T> x = subscriber as IEndShowSubscriber<T>;
      if (x != null) {
        this._onEndShow -= x.OnEndShow;
      }
      
      IStartDismissSubscriber<T> y = subscriber as IStartDismissSubscriber<T>;
      if (y != null) {
        this._onStartDismiss -= y.OnStartDismiss;
      }
      
      IEndDismissSubscriber<T> z = subscriber as IEndDismissSubscriber<T>;
      if (z != null) {
        this._onEndDismiss -= z.OnEndDismiss;
      }
    }
  }
}