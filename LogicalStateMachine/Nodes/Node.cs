using DT;
using System;
using System.Collections;

namespace DT.GameEngine {
  [Serializable]
  public class Node : INode {
    // PRAGMA MARK - Public Interface
    public event Action OnEnter = delegate {};
    public event Action OnExit = delegate {};
    public event Action OnManualExit = delegate {};

    public Node(int id) {
      this.Id = new NodeId(id);
    }


    // PRAGMA MARK - INode Implementation
    public NodeId Id {
      get; private set;
    }

    public bool IsManuallyExited {
      get; private set;
    }

    public void HandleEnter() {
      this.OnEnter.Invoke();
    }

    public void HandleExit() {
      this.OnExit.Invoke();
    }

    public void TriggerManualExit() {
      this.IsManuallyExited = true;
      this.OnManualExit.Invoke();
    }


    // PRAGMA MARK - Internal
    // [SerializeField] private void
  }
}