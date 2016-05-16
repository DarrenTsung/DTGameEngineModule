using DT;
using System;
using System.Collections;

namespace DT.GameEngine {
  public interface INode {
    NodeId Id { get; }

    void HandleEnter();
    void HandleExit();
  }
}