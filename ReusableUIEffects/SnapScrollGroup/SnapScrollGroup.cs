using DT;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.EventSystems;

namespace DT.GameEngine {
  public class SnapScrollGroup<T> {
    // PRAGMA MARK - Public Interface
    public delegate void ScrollChanged(T left, T right, float percentageBetween);
    public event ScrollChanged OnScrollChanged = delegate {};

    public SnapScrollGroup(SnapScrollGroupConfig config, IList<T> objects, Func<T, GameObject> recycledViewProvider) {
      if (config.IsInvalid()) {
        Debug.LogError("SnapScrollGroup - config is invalid, some part of set-up is incorrect!");
        return;
      }

      config.pointerHandler.Configure(this.HandleBeginDrag, this.HandleDrag, this.HandleEndDrag);

      this._leftShown = config.leftShown;
      this._rightShown = config.rightShown;

      this._snapDuration = config.snapDuration;

      this._axis = config.axis;

      Vector2 referenceResolution = ViewManagerLocator.Main.CanvasScaler.referenceResolution;
      if (config.axis == Axis.HORIZONTAL) {
        this._spacingOffset = new Vector2(config.spacing * referenceResolution.x, 0.0f);
      } else {
        this._spacingOffset = new Vector2(0.0f, config.spacing * referenceResolution.y);
      }

      this._objects = objects.ToArray();
      this._recycledViewProvider = recycledViewProvider;

      this.Select(index: 0, instant: true);
    }

    public void SelectFirst(Predicate<T> predicate, bool instant = false) {
      for (int i = 0; i < this._objects.Length; i++) {
        T obj = this._objects[i];
        if (predicate.Invoke(obj)) {
          this.Select(i, instant);
          return;
        }
      }

      Debug.LogWarning("SelectFirst - Failed to select object matching predicate!");
    }

    public T ShowingObject {
      get { return this._objects[this.ShowingIndex]; }
    }

    public bool IsStill {
      get { return Mathf.Approximately(this._currentIndex, this.ShowingIndex); }
    }


    // PRAGMA MARK - Internal
    private T[] _objects;
    private Func<T, GameObject> _recycledViewProvider;
    private Dictionary<int, GameObject> _showingViews = new Dictionary<int, GameObject>();
    private float _currentIndex;

    private int _leftShown;
    private int _rightShown;

    private float _snapDuration;

    private Axis _axis;
    private Vector2 _spacingOffset;

    private CoroutineWrapper _snapCoroutine;

    private int ShowingIndex {
      get { return this._objects.ClampIndex(Mathf.RoundToInt(this._currentIndex)); }
    }

    private void Select(int index, bool instant = false) {
      if (!this._objects.ContainsIndex(index)) {
        Debug.LogError("Select - called with invalid index! " + index);
        return;
      }

      if (instant) {
        this.SetCurrentIndex((float)index);
      } else {
        float startIndex = this._currentIndex;
        this._snapCoroutine = CoroutineWrapper.DoEaseEveryFrameForDuration(this._snapDuration, EaseType.QuadOut, (float t) => {
          this.SetCurrentIndex(Mathf.Lerp(startIndex, (float)index, t));
        });
      }
    }

    private void SetCurrentIndex(float index) {
      this._currentIndex = index;

      HashSet<int> shownIndices = this.GetIndicesShown().ToHashSet();

      foreach (KeyValuePair<int, GameObject> p in this._showingViews) {
        if (shownIndices.Contains(p.Key)) {
          continue;
        }

        // GameObject is not being shown, recycle
        GameObject g = p.Value;
        ObjectPoolManager.Recycle(g);
      }

      this._showingViews = this._showingViews.Where(p => shownIndices.Contains(p.Key)).ToDictionary();

      foreach (int shownIndex in this.GetIndicesShown()) {
        GameObject g = this.GetViewForIndex(shownIndex);
        g.transform.localPosition = this.GetLocalPositionForIndex(shownIndex);
      }

      int leftIndex = Mathf.FloorToInt(this._currentIndex);
      int rightIndex = Mathf.CeilToInt(this._currentIndex);

      T left = this._objects.SafeGet(leftIndex);
      T right = this._objects.SafeGet(rightIndex);

      float percentageBetween = this._currentIndex - leftIndex;
      this.OnScrollChanged.Invoke(left, right, percentageBetween);
    }

    private Vector3 GetLocalPositionForIndex(int index) {
      float relativeIndex = index - this._currentIndex;
      return this._spacingOffset * relativeIndex;
    }

    private IEnumerable<int> GetIndicesShown() {
      float leftBound = this._currentIndex - this._leftShown;
      float rightBound = this._currentIndex + this._rightShown;

      for (int i = Mathf.FloorToInt(leftBound); i <= Mathf.CeilToInt(rightBound); i++) {
        if (i < leftBound || i > rightBound) {
          continue;
        }

        if (!this._objects.ContainsIndex(i)) {
          continue;
        }

        yield return i;
      }
    }

    private GameObject GetViewForIndex(int index) {
      if (!this._showingViews.ContainsKey(index)) {
        T obj = this._objects[index];
        GameObject go = this._recycledViewProvider.Invoke(obj);
        this._showingViews[index] = go;
      }

      return this._showingViews[index];
    }


    // PRAGMA MARK - Internal : Dragging
    private const float kMaxDelta = 0.12f;

    private float _dragStartTime;
    private Vector2 _dragStartPosition;

    private void HandleBeginDrag(PointerEventData eventData) {
      if (this._snapCoroutine != null) {
        this._snapCoroutine.Stop();
        this._snapCoroutine = null;
      }

      this._dragStartPosition = eventData.position;
      this._dragStartTime = Time.time;
    }

    private void HandleDrag(PointerEventData eventData) {
      Vector2 vectorDelta = eventData.delta;
      Vector2 screenSize = new Vector2(Screen.width, Screen.height);
      vectorDelta = Vector2Util.InverseScale(vectorDelta, screenSize);

      float delta = Mathf.Clamp(this._axis.ApplicableValue(vectorDelta), -kMaxDelta, kMaxDelta);

      // NOTE (darren): we subtract here because we drag in the opposite direction we want to go
      this.SetCurrentIndex(this._currentIndex - delta);
    }

    private void HandleEndDrag(PointerEventData eventData) {
      // snap to closest index
      int closestIndex = Mathf.RoundToInt(this._currentIndex);

      Vector2 dragVector = eventData.position - this._dragStartPosition;
      HorizontalDirection dragDirection = HorizontalDirectionUtil.FromValue(this._axis.ApplicableValue(dragVector));
      HorizontalDirection snapDirection = HorizontalDirectionUtil.FromValue((float)closestIndex - this._currentIndex);

      // NOTE (darren): if we wanted to go left (dragDirection == right)
      // and we're already snapping left, don't need to add to index based on drag speed
      if (dragDirection.Flipped() != snapDirection) {
        // if dragged fast enough, add 1 or -1 to closest index
        float dragTime = Time.time - this._dragStartTime;

        float scaledDrag = Mathf.Abs(this._axis.ApplicableValue(dragVector / dragTime));
        float spacing = this._axis.ApplicableValue(this._spacingOffset);

        if (scaledDrag >= spacing / 2.0f) {
          closestIndex += dragDirection.Flipped().IntValue();
        }
      }

      closestIndex = this._objects.ClampIndex(closestIndex);
      this.Select(closestIndex);
    }
  }
}