using System;
using System.Collections;
using System.Collections.Generic;
﻿using UnityEngine;

namespace DT {
	public enum VCPriority {
		HIGH = 100,
		MEDIUM = 1000,
		LOW = 10000
	}
	
	public enum VCPresentationType {
		IMMEDIATE,
		QUEUED
	}
	
	public class ViewControllerQueuedPresentationManager : MonoBehaviour, IEndDismissSubscriber<IViewController> {
		// PRAGMA MARK - Public Interface
		public void Present(IViewController vc, VCPresentationType presentationType, VCPriority priority = VCPriority.LOW) {
			switch (presentationType) {
				case VCPresentationType.QUEUED:
					if (_showingViewControllers.Count > 0) {
						_viewControllerQueue.Enqueue(vc, (double)priority);
					} else {
						this.Show(vc);
					}
					break;
				case VCPresentationType.IMMEDIATE:
				default:
					this.Show(vc);
					break;
			}
		}
		
		
		// PRAGMA MARK - Internal
		protected SimplePriorityQueue<IViewController> _viewControllerQueue = new SimplePriorityQueue<IViewController>();
		protected HashSet<IViewController> _showingViewControllers = new HashSet<IViewController>();
		
		protected void Update() {
#if UNITY_EDITOR
			this.UpdateInspectorViewControllerQueue();
#endif
		}
		
		protected void Show(IViewController vc) {
			vc.AddShowDismissEvents(this);
			
			_showingViewControllers.Add(vc);
			vc.Show();
		}
		
		
		// PRAGMA MARK - IEndDismissSubscriber implementation
		public void OnEndDismiss(IViewController vc) {
			vc.RemoveShowDismissEvents(this);
			
			_showingViewControllers.Remove(vc);
			if (_showingViewControllers.Count <= 0 && this._viewControllerQueue.Count > 0) {
				IViewController nextViewController = this._viewControllerQueue.Dequeue();
				this.Show(nextViewController);
			}
		}
		
		
		// PRAGMA MARK - Inspector Debug Logic
#if UNITY_EDITOR
		[SerializeField]
		private List<InspectorDisplayViewController> _inspectorViewControllerQueue = new List<InspectorDisplayViewController>();
		private int _inspectorViewControllerQueueCount;
		
		private void UpdateInspectorViewControllerQueue() {
			int viewControllerQueueCount = this._viewControllerQueue.Count;
			if (this._inspectorViewControllerQueueCount != viewControllerQueueCount) {
				this._inspectorViewControllerQueueCount = viewControllerQueueCount;
			} else {
				return;
			}
			
			this._inspectorViewControllerQueue.Clear();
			
			foreach (IViewController vc in this._viewControllerQueue) {
				double priority = this._viewControllerQueue.DebugGetPriorityForItem(vc);
				this._inspectorViewControllerQueue.Add(new InspectorDisplayViewController(vc, priority));
			}
		}
#endif
	}
	
#if UNITY_EDITOR
	[Serializable]
	public struct InspectorDisplayViewController {
		public string className;
		public double priority;
		
		public InspectorDisplayViewController(IViewController vc, double priority) {
			this.className = vc.GetType().Name;
			this.priority = priority;
		}
	}
#endif
}