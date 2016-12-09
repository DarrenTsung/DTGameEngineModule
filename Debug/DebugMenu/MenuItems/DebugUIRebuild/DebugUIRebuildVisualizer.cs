using DT;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
﻿using UnityEngine;
﻿using UnityEngine.UI;

#if UNITY_EDITOR
using UnityEditor;
#endif

namespace DT.GameEngine {
	public static class DebugUIRebuildVisualizer {
    // PRAGMA MARK - Static Public Interface
    public static bool Enabled { get { return _enabled; } }

    public static void Enable() {
      // HACK (darren): add event handler to front of willRenderCanvases
      // so my callback always happens before the queues are cleared
      var fieldInfo = typeof(Canvas).GetField("willRenderCanvases", BindingFlags.Static | BindingFlags.NonPublic | BindingFlags.FlattenHierarchy);
      MulticastDelegate eventDelegate = (MulticastDelegate)fieldInfo.GetValue(null);
      if (eventDelegate != null) {
        var delegates = eventDelegate.GetInvocationList();
        foreach (var handler in delegates) {
          Canvas.willRenderCanvases -= (Canvas.WillRenderCanvases)handler;
        }

        Canvas.willRenderCanvases += DebugUIRebuildVisualizer.HandleWillRenderCanvases;
        foreach (var handler in delegates) {
          Canvas.willRenderCanvases += (Canvas.WillRenderCanvases)handler;
        }
      }

      MonoBehaviourHelper.OnUpdate += DebugUIRebuildVisualizer.HandleUpdate;

#if UNITY_EDITOR
      EditorApplication.hierarchyWindowItemOnGUI += DebugUIRebuildVisualizer.HandleHierarchyWindowItemOnGUI;
#endif
    }

    public static void Disable() {
      foreach (var canvasGroup in _canvasGroupLastDirtyTime.Keys) {
        if (canvasGroup == null) {
          continue;
        }

        canvasGroup.alpha = 1.0f;
      }

      Canvas.willRenderCanvases -= DebugUIRebuildVisualizer.HandleWillRenderCanvases;
      MonoBehaviourHelper.OnUpdate -= DebugUIRebuildVisualizer.HandleUpdate;

#if UNITY_EDITOR
      EditorApplication.hierarchyWindowItemOnGUI -= DebugUIRebuildVisualizer.HandleHierarchyWindowItemOnGUI;
#endif
    }


    // PRAGMA MARK - Static Internal
    private const float _kCanvasRebuildingAlpha = 0.5f;

    private static bool _enabled = false;

    private static IList<ICanvasElement> _graphicRebuildQueue = null;
    private static IList<ICanvasElement> _GraphicRebuildQueue {
      get {
        if (_graphicRebuildQueue == null) {
          FieldInfo fGraphic = typeof(CanvasUpdateRegistry).GetField("m_GraphicRebuildQueue", BindingFlags.Instance | BindingFlags.NonPublic);
          _graphicRebuildQueue = (IList<ICanvasElement>)fGraphic.GetValue(CanvasUpdateRegistry.instance);
        }
        return _graphicRebuildQueue;
      }
    }

    private static readonly Dictionary<GameObject, CanvasGroup> _canvasGroupMapping = new Dictionary<GameObject, CanvasGroup>();
    private static readonly Dictionary<CanvasGroup, float> _canvasGroupLastDirtyTime = new Dictionary<CanvasGroup, float>();
    private static readonly Dictionary<GameObject, float> _gameObjectLastDirtyTime = new Dictionary<GameObject, float>();

    private static float _lastUpdateTime;

    private static void HandleWillRenderCanvases() {
      float now = Time.realtimeSinceStartup;

      for (int i = 0; i < _GraphicRebuildQueue.Count; i++) {
        ICanvasElement elem = _GraphicRebuildQueue[i];

        Graphic graphic = elem as Graphic;
        if (graphic == null) {
          Debug.LogError("Don't know how to handle other types beyond Graphic!");
          continue;
        }

        Canvas canvas = graphic.canvas;
        if (canvas == null) {
          // NOTE (darren): this is a valid case
          // because sometimes the canvas can be destroyed
          continue;
        }

        var canvasGroup = _canvasGroupMapping.GetOrCreateCached(canvas.gameObject, g => g.GetOrAddComponent<CanvasGroup>());

        _canvasGroupLastDirtyTime[canvasGroup] = now;
        _gameObjectLastDirtyTime[graphic.gameObject] = now;
      }
    }

    private static void HandleUpdate() {
      float now = Time.realtimeSinceStartup;
      foreach (var kvp in _canvasGroupLastDirtyTime) {
        var canvasGroup = kvp.Key;
        float lastDirtyTime = kvp.Value;

        if (canvasGroup == null) {
          continue;
        }

        if (lastDirtyTime >= _lastUpdateTime - Mathf.Epsilon) {
          canvasGroup.alpha = _kCanvasRebuildingAlpha;
        } else {
          canvasGroup.alpha = 1.0f;
        }
      }

      _lastUpdateTime = now;
    }

#if UNITY_EDITOR
    private static void HandleHierarchyWindowItemOnGUI(int guid, Rect drawRect) {
      GameObject g = EditorUtility.InstanceIDToObject(guid) as GameObject;

      if (g == null || !_gameObjectLastDirtyTime.ContainsKey(g)) {
        return;
      }

      Color previousBackgroundColor = GUI.backgroundColor;

      float lastDirtyTime = _gameObjectLastDirtyTime[g];
      if (lastDirtyTime >= _lastUpdateTime - Mathf.Epsilon - 0.5f) {
        GUI.backgroundColor = Color.cyan.WithAlpha(0.3f);
        GUI.Box(drawRect, "");
        EditorApplication.RepaintHierarchyWindow();
      }

      GUI.backgroundColor = previousBackgroundColor;
    }
#endif
	}
}