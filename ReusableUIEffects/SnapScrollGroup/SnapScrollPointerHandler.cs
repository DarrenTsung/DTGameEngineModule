using DT;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

namespace DT.GameEngine {
  public class SnapScrollPointerHandler : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler {
    // PRAGMA MARK - Public Interface
    public void Configure(Action<PointerEventData> handleBeginDrag, Action<PointerEventData> handleDrag, Action<PointerEventData> handleEndDrag) {
      this._handleBeginDrag = handleBeginDrag;
      this._handleDrag = handleDrag;
      this._handleEndDrag = handleEndDrag;
    }


    // PRAGMA MARK - IBeginDragHandler Implementation
    public void OnBeginDrag(PointerEventData eventData) {
      if (this._handleBeginDrag == null) {
        Debug.LogError("SnapScrollPointerHandler - no callback for OnBeginDrag!");
        return;
      }

      this._handleBeginDrag.Invoke(eventData);
    }


    // PRAGMA MARK - IDragHandler Implementation
    public void OnDrag(PointerEventData eventData) {
      if (this._handleDrag == null) {
        Debug.LogError("SnapScrollPointerHandler - no callback for OnDrag!");
        return;
      }

      this._handleDrag.Invoke(eventData);
    }


    // PRAGMA MARK - IEndDragHandler Implementation
    public void OnEndDrag(PointerEventData eventData) {
      if (this._handleEndDrag == null) {
        Debug.LogError("SnapScrollPointerHandler - no callback for OnEndDrag!");
        return;
      }

      this._handleEndDrag.Invoke(eventData);
    }


    // PRAGMA MARK - Internal
    private Action<PointerEventData> _handleBeginDrag;
    private Action<PointerEventData> _handleDrag;
    private Action<PointerEventData> _handleEndDrag;
  }
}