using System.Collections;
﻿using UnityEngine;

namespace DT {
  public interface IShowDismissEvents<T> {
    void AddShowDismissEvents(object subscriber);
    void RemoveShowDismissEvents(object subscriber);
	}
  
  public interface IStartShowSubscriber<T> {
    void OnStartShow(T obj);
  }
  
  public interface IEndShowSubscriber<T> {
    void OnEndShow(T obj);
  }
  
  public interface IStartDismissSubscriber<T> {
    void OnStartDismiss(T obj);
  }
  
  public interface IEndDismissSubscriber<T> {
    void OnEndDismiss(T obj);
  }
}