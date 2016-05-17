using DT;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DT.GameEngine {
  public class GraphComponent : MonoBehaviour {
    // PRAGMA MARK - Public Interface
    public GraphAsset graphAsset;


    // PRAGMA MARK - Internal
    [SerializeField] private bool _startOnEnable = true;
    [SerializeField] private Graph _graph;

    private void Awake() {
      if (this.graphAsset == null) {
        Debug.LogError("GraphComponent - missing linked graph asset!");
        this.enabled = false;
        return;
      }

      this._graph = this.graphAsset.graph;
    }

    private void OnEnable() {
      if (this.graphAsset == null) {
        return;
      }

      if (this._startOnEnable) {
        this._graph.Start();
      }
    }
  }
}