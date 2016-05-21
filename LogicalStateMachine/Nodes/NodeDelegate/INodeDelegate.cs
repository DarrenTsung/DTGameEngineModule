using DT;
using System.Collections;

namespace DT.GameEngine {
  public interface INodeDelegate {
    void HandleEnter();
    void HandleExit();
  }
}
