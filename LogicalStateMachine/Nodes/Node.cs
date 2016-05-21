using DT;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DT.GameEngine {
  [Serializable]
  public class Node : INode, ISerializationCallbackReceiver {
    // PRAGMA MARK - Public Interface
    public event Action OnEnter = delegate {};
    public event Action OnExit = delegate {};
    public event Action<Node> OnManualExit = delegate {};

    public Node(NodeId id) {
      this.Id = id;
    }

    public void AddNodeDelegate(INodeDelegate nodeDelegate) {
      this._nodeDelegates.Add(nodeDelegate);
    }

    public void RemoveNodeDelegate(INodeDelegate nodeDelegate) {
      bool successful = this._nodeDelegates.Remove(nodeDelegate);
      if (!successful) {
        Debug.LogError("Node - RemoveNodeDelegate called with node delegate not found!");
      }
    }

    public IList<INodeDelegate> GetNodeDelegates() {
      return this._nodeDelegates;
    }


    // PRAGMA MARK - INode Implementation
    public NodeId Id {
      get; private set;
    }

    public bool IsManuallyExited {
      get; private set;
    }

    public void HandleEnter() {
      this.IsManuallyExited = false;
      foreach (INodeDelegate nodeDelegate in this._nodeDelegates) {
        nodeDelegate.HandleEnter();
      }
      this.OnEnter.Invoke();
    }

    public void HandleExit() {
      this.IsManuallyExited = false;
      foreach (INodeDelegate nodeDelegate in this._nodeDelegates) {
        nodeDelegate.HandleExit();
      }
      this.OnExit.Invoke();
    }

    public void TriggerManualExit() {
      this.IsManuallyExited = true;
      this.OnManualExit.Invoke(this);
    }


    // PRAGMA MARK - ISerializationCallbackReceiver Implementation
    public void OnAfterDeserialize() {
      this._nodeDelegates = new List<INodeDelegate>();
      foreach (string serializedNodeDelegate in this._serializedNodeDelegates) {
        INodeDelegate nodeDelegate = JsonSerialization.DeserializeGeneric<INodeDelegate>(serializedNodeDelegate);
        if (nodeDelegate != null) {
          this._nodeDelegates.Add(nodeDelegate);
        }
      }
    }

    public void OnBeforeSerialize() {
      this._serializedNodeDelegates.Clear();
      foreach (INodeDelegate nodeDelegate in this._nodeDelegates) {
        string serializedNodeDelegate = JsonSerialization.SerializeGeneric(nodeDelegate);
        if (serializedNodeDelegate != null) {
          this._serializedNodeDelegates.Add(serializedNodeDelegate);
        }
      }
    }


    // PRAGMA MARK - Internal
    [SerializeField] private List<string> _serializedNodeDelegates = new List<string>();

    private List<INodeDelegate> _nodeDelegates;
  }
}