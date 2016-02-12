using DT;
using UnityEngine;

namespace DT.GameEngine {
  public interface IDialog {
    bool DialogActive {
      get;
    }

    void StartDialog();
    void StopDialog();
    void RestartDialog();
    bool AdvanceDialogIfPossible();
    void LoadDialogModelFromTextSource(TextAsset textSource);
  }
}