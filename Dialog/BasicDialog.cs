using DT;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DT.GameEngine {
  [System.Serializable]
  public class BasicDialogModel<TKeyframe> where TKeyframe : BasicDialogKeyframe {
    public TKeyframe[] keyframes;

    public int KeyframeCount {
      get { return this.keyframes.Length; }
    }

    public TKeyframe GetKeyframeForIndex(int index) {
      return this.keyframes.SafeGet(index);
    }
  }

  [System.Serializable]
  public class BasicDialogKeyframe {
    public string text;
  }

  [CustomExtensionInspector]
  public class BasicDialog<TDialogModel, TKeyframe> : MonoBehaviour, IDialog where TDialogModel : BasicDialogModel<TKeyframe>
                                                                             where TKeyframe : BasicDialogKeyframe {
    // PRAGMA MARK - IDialog Implementation
    public bool DialogActive {
      get { return this._dialogActive; }
      private set {
        if (this._dialogActive == value) {
          return;
        }

        this._dialogActive = value;
        this.HandleDialogActiveStateChange(this._dialogActive);
      }
    }

    [MakeButton]
    public void StartDialog() {
      this.DialogActive = true;
      this._dialogIndex = 0;
      this.HandleDialogIndexChange();
    }

    [MakeButton]
    public void StopDialog() {
      this.DialogActive = false;
    }

    [MakeButton]
    public void RestartDialog() {
      this.StopDialog();
      this.StartDialog();
    }

    [MakeButton]
    public bool AdvanceDialogIfPossible() {
      return this.IncrementDialogIndexIfPossible();
    }

    public void LoadDialogModelFromTextSource(TextAsset textSource) {
      this._dialog = JsonSerializable.DeserializeFromTextAsset<TDialogModel>(textSource);
    }

		// PRAGMA MARK - Internal
		protected TDialogModel _dialog;

    [Header("Debug Properties")]
    [SerializeField]
    private TextAsset _debugTextAsset;

		[Header("Read Only Properties")]
		[SerializeField, ReadOnly]
		protected bool _dialogActive = false;
		[SerializeField, ReadOnly]
		protected int _dialogIndex = 0;

    // optional
    protected virtual void HandleDialogIndexChange() { }

    // optional
    protected virtual void HandleDialogActiveStateChange(bool dialogActive) { }

    protected TKeyframe GetCurrentKeyframe() {
      return this._dialog.GetKeyframeForIndex(this._dialogIndex);
    }

    private bool IncrementDialogIndexIfPossible() {
			if (this._dialogIndex >= this._dialog.KeyframeCount - 1) {
        return false;
			}

			this._dialogIndex++;
      this.HandleDialogIndexChange();
      return true;
    }

    [MakeButton]
    protected void LoadDebugTextAsset() {
      this.LoadDialogModelFromTextSource(this._debugTextAsset);
    }
  }
}
