using DT;
using System;
using System.Collections;

namespace DT.GameEngine {
  [Serializable]
  public class Node : INode {
    // PRAGMA MARK - Public Interface
    public event Action OnEnter = delegate {};
    public event Action OnExit = delegate {};
    public event Action<Node> OnManualExit = delegate {};

    public Node(NodeId id) {
      this.Id = id;
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
      this.OnEnter.Invoke();
    }

    public void HandleExit() {
      this.IsManuallyExited = false;
      this.OnExit.Invoke();
    }

    public void TriggerManualExit() {
      this.IsManuallyExited = true;
      this.OnManualExit.Invoke(this);
    }


    // PRAGMA MARK - Internal
    // [SerializeField] private void
  }
}