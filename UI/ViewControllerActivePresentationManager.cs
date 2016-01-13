using System;
using System.Collections;
using System.Collections.Generic;
﻿using UnityEngine;

namespace DT {
	public class ViewControllerActivePresentationManager : MonoBehaviour {
		// PRAGMA MARK - Public Interface
    public IViewController ActiveViewController {
      get {
        return this._activeViewController;
      }
      private set {
        this._activeViewController = value;
      }
    }
    
		public void Present(IViewController vc) {
      if (this.ActiveViewController != null) {
        Debug.LogError("ActiveViewControllerManager->Present: attempting to present a view controller when there already is an active view controller!");
        return;
      }
      
      this.ActiveViewController = vc;
      this.ShowActiveViewController();
		}
    
    public void ReplaceActiveViewControllerWith(IViewController vc) {
      if (this.ActiveViewController == null) {
        Debug.LogError("ActiveViewControllerManager->ReplaceActiveViewControllerWith: called when there is no active view controller, probably something went wrong!");
        return;
      }
      
      this.DismissActiveViewController();
      this.Present(vc);
    }
    
    public void DismissActiveViewController() {
      this.ActiveViewController.Dismiss();
      this.ActiveViewController = null;
    }
    
		
		// PRAGMA MARK - Internal
    [SerializeField]
    private IViewController _activeViewController;
		
		protected void ShowActiveViewController() {
			this.ActiveViewController.Show();
		}
  }
}
