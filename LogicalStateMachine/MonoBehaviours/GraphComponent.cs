using DT;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DT.GameEngine {
  public class GraphComponent : MonoBehaviour {
    // PRAGMA MARK - Internal
    [SerializeField] private bool _startOnEnable = true;
    [SerializeField] private Graph _graph;

    private void OnEnable() {
      if (this._startOnEnable) {
        this._graph.Start();
      }
    }
  }
}