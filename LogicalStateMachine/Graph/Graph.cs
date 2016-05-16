using DT;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DT.GameEngine {
  [Serializable]
  public class Graph : ScriptableObject, IGraph {
    // PRAGMA MARK - IGraph Implementation
    public void Start() {
      if (this._isActive) {
        Debug.LogWarning("Graph - Start called when already active!");
        return;
      }

      this._isActive = true;
      this.ResetContext();

      Node[] startingNodes = this._graphData.GetStartingNodes();
      if (startingNodes == null) {
        return;
      }

      foreach (Node node in startingNodes) {
        this.AddActiveNode(node);
      }
      this.CommitActiveNodeChanges();

      this.CheckActiveNodesTransitions();
    }

    public void Stop() {
      if (!this._isActive) {
        Debug.LogWarning("Graph - Stop called when not active!");
        return;
      }

      this._isActive = false;
      this.Reset();
    }

    public void Reset() {
      foreach (Node node in this._activeNodes) {
        this.RemoveActiveNode(node);
      }
      this.CommitActiveNodeChanges();

      this._isActive = false;
      this.ResetContext();
    }


    public void SetStartingNodes(IList<Node> nodes) {
      this._graphData.SetStartingNodes(nodes);
    }

    public void AddStartingContextParameter(GraphContextParameter contextParameter) {
      this._startingContextParameters.Add(contextParameter);
    }

    public Node MakeNode() {
      Node node = this._graphData.MakeNode();
      node.OnManualExit += this.HandleNodeManualExitTriggered;
      return node;
    }

    public void AddOutgoingTransitionForNode(Node node, NodeTransition nodeTransition) {
      this._graphData.AddOutgoingTransitionForNode(node, nodeTransition);
    }


    public bool IsNodeActive(Node node) {
      int changeValue = this._activeNodeChangeMap.GetValue(node);
      bool shouldBeAdded = changeValue > 0;
      bool shouldBeRemoved = changeValue < 0;

      if (shouldBeAdded) {
        return true;
      }

      if (shouldBeRemoved) {
        return false;
      }

      if (this._activeNodes.Contains(node)) {
        return true;
      }

      return false;
    }


    // PRAGMA MARK - Internal
    [SerializeField] private GraphData _graphData = new GraphData();
    [SerializeField] private List<GraphContextParameter> _startingContextParameters = new List<GraphContextParameter>();

    private IGraphContext _context;
    private bool _isActive = false;

    private HashSet<Node> _activeNodes = new HashSet<Node>();
    private CountMap<Node> _activeNodeChangeMap = new CountMap<Node>();

    private void HandleNodeManualExitTriggered(Node node) {
      if (!this.IsNodeActive(node)) {
        Debug.LogWarning("HandleNodeManualExitTriggered - node that is not active manually exited!");
        return;
      }

      this.CheckTransitions(node);
    }

    private void CheckActiveNodesTransitions() {
      foreach (Node node in this._activeNodes) {
        this.CheckTransitions(node);
      }
      this.CommitActiveNodeChanges();
    }

    private void CheckTransitions(Node node) {
      IList<NodeTransition> nodeTransitions = this.GetOutgoingTransitionsForNode(node);
      foreach (NodeTransition nodeTransition in nodeTransitions) {
        if (!nodeTransition.transition.AreConditionsMet()) {
          continue;
        }

        Node targetNode = this._graphData.LoadNodeById(nodeTransition.target);
        if (targetNode == null) {
          continue;
        }

        nodeTransition.transition.HandleTransitionUsed();
        this.RemoveActiveNode(node);
        this.AddActiveNode(targetNode);
        this.CheckTransitions(targetNode);
        break;
      }
    }

    private void RemoveActiveNode(Node node) {
      if (!this.IsNodeActive(node)) {
        Debug.LogError("Graph - RemoveActiveNode node was not active!");
        return;
      }

      this._activeNodeChangeMap.Decrement(node);
      node.HandleExit();
    }

    private void AddActiveNode(Node node) {
      if (node == null) {
        Debug.LogError("Graph - AddActiveNode node passed was null, stopping graph!");
        return;
      }

      if (this.IsNodeActive(node)) {
        return;
      }

      this._activeNodeChangeMap.Increment(node);
      node.HandleEnter();
    }

    private void CommitActiveNodeChanges() {
      foreach (KeyValuePair<Node, int> entry in this._activeNodeChangeMap) {
        Node node = entry.Key;

        bool shouldRemove = entry.Value < 0;
        bool shouldAdd = entry.Value > 0;

        if (shouldRemove) {
          this._activeNodes.Remove(node);
        }

        if (shouldAdd) {
          this._activeNodes.Add(node);
        }
      }

      this._activeNodeChangeMap.Clear();
    }

    private void ResetContext() {
      if (this._context != null) {
        this._context.OnContextUpdated -= this.CheckActiveNodesTransitions;
      }
      this._context = GraphContextFactoryLocator.MakeContext();
      this._context.OnContextUpdated += this.CheckActiveNodesTransitions;
      this._context.PopulateStartingContextParameters(this._startingContextParameters);
    }

    private IList<NodeTransition> GetOutgoingTransitionsForNode(Node node) {
      IList<NodeTransition> nodeTransitions = this._graphData.GetOutgoingTransitionsForNode(node);

      foreach (NodeTransition nodeTransition in nodeTransitions) {
        Transition transition = nodeTransition.transition;
        if (!transition.HasContext()) {
          TransitionContext transitionContext = new TransitionContext {
            graphContext = this._context,
            nodeContext = new GraphNodeContext(this, node)
          };

          transition.ConfigureWithContext(transitionContext);
        }
      }

      return nodeTransitions;
    }
  }
}