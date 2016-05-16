using DT;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace DT.GameEngine {
  [Serializable]
  public class GraphData {
    public static NodeTransition[] kEmptyNodeTransitionArray = new NodeTransition[0];

    public Node LoadNodeById(NodeId id) {
      if (!this.HasNodeWithId(id)) {
        Debug.LogWarning("GraphData - LoadNodeById has no node with id: " + id + "!");
        return null;
      }
      return this.CachedNodeDataMapping[id].node;
    }

    public Node[] GetStartingNodes() {
      if (this._startingNodeIds == null) {
        return null;
      }
      return (from id in this._startingNodeIds select this.LoadNodeById(id)).ToArray();
    }

    public void SetStartingNodes(IList<Node> nodes) {
      this._startingNodeIds = nodes.Select(node => node.Id).ToArray();
    }

    public Node MakeNode() {
      Node node = new Node(this.CachedHighestNodeId + 1);
      this._nodeDatas.Add(new NodeData(node));

      // If no starting nodes, set starting node to first node created
      if (this._startingNodeIds == null) {
        this._startingNodeIds = new NodeId[] { node.Id };
      }

      this.ClearCached();
      return node;
    }

    public void RemoveNode(Node node) {
      if (!this.HasNodeWithId(node.Id)) {
        Debug.LogWarning("GraphData - RemoveNode has no node with id: " + node.Id + "!");
        return;
      }

      NodeData nodeData = this.CachedNodeDataMapping[node.Id];
      bool successful = this._nodeDatas.Remove(nodeData);
      if (!successful) {
        Debug.LogWarning("GraphData - RemoveNode called with node not contained in the graph!");
        return;
      }

      this.ClearCached();
    }

    public IList<NodeTransition> GetOutgoingTransitionsForNode(Node node) {
      if (!this.HasNodeWithId(node.Id)) {
        Debug.LogWarning("GraphData - GetOutgoingTransitionsForNode has no node with id: " + node.Id + "!");
        return kEmptyNodeTransitionArray;
      }

      return this.CachedNodeDataMapping[node.Id].outgoingTransitions;
    }

    public void AddOutgoingTransitionForNode(Node node, NodeTransition nodeTransition) {
      if (!this.HasNodeWithId(node.Id)) {
        Debug.LogWarning("GraphData - AddOutgoingTransitionForNode has no node with id: " + node.Id + "!");
        return;
      }

      this.CachedNodeDataMapping[node.Id].outgoingTransitions.Add(nodeTransition);
    }


    // PRAGMA MARK - Internal
    [SerializeField] List<NodeData> _nodeDatas = new List<NodeData>();
    [SerializeField] NodeId[] _startingNodeIds;

    private int? _cachedHighestNodeId;
    private int CachedHighestNodeId {
      get {
        if (this._cachedHighestNodeId == null) {
          if (this._nodeDatas.IsNullOrEmpty()) {
            this._cachedHighestNodeId = 0;
          } else {
            this._cachedHighestNodeId = this._nodeDatas.Select(data => data.node.Id).Max();
          }
        }

        return this._cachedHighestNodeId.Value;
      }
    }

    private Dictionary<NodeId, NodeData> _cachedNodeDataMapping;
    private Dictionary<NodeId, NodeData> CachedNodeDataMapping {
      get {
        if (this._cachedNodeDataMapping == null) {
          this._cachedNodeDataMapping = new Dictionary<NodeId, NodeData>();
          foreach (NodeData nodeData in this._nodeDatas) {
            this._cachedNodeDataMapping[nodeData.node.Id] = nodeData;
          }
        }

        return this._cachedNodeDataMapping;
      }
    }

    private void ClearCached() {
      this._cachedNodeDataMapping = null;
      this._cachedHighestNodeId = null;
    }

    private bool HasNodeWithId(NodeId id) {
      return this.CachedNodeDataMapping.ContainsKey(id);
    }
  }
}
