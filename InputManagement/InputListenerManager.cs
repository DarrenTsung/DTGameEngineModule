using DT;
using System;
using System.Collections;
using System.Collections.Generic;
﻿using UnityEngine;

#if IN_CONTROL
using InControl;

namespace DT {
	public class InputListenerManager : MonoBehaviour {
		protected InputListenerManager() {}
		
		
		// PRAGMA MARK - Public Interface
		public bool InputDisabled;
		
		// note: listeners invoke all callbacks when they begin to be pressed (single time each press, no matter how long)
		public void ListenForType(InputControlType type, Action<InputDevice> callback) {
			this.ListenersForType(type).Add(callback);
		}
		
		public void RemoveCallbackForType(InputControlType type, Action<InputDevice> callback) {
			this.StartCoroutine(this.RemoveCallBackForTypeAtEndOfFrame(type, callback));
		}
		
		
		// PRAGMA MARK - Internal
		protected Dictionary<InputControlType, HashSet<Action<InputDevice>>> _listenersForInputTypes;
		
		protected void Awake() {
			_listenersForInputTypes = new Dictionary<InputControlType, HashSet<Action<InputDevice>>>();
		}
		
		protected void Update() {
			if (!this.InputDisabled) {
				this.UpdateListeners();
			}
		}
		
		protected virtual void UpdateListeners() {
			foreach (KeyValuePair<InputControlType, HashSet<Action<InputDevice>>> pair in _listenersForInputTypes) {
				InputControlType type = pair.Key;
				
				foreach (InputDevice device in InputManager.Devices) {
					if (device.GetControl(type).WasPressed) {
						// invoke callbacks
						foreach (Action<InputDevice> a in pair.Value) {
							a(device);
						}
						break;
					}
				}
			}
		}
		
		protected HashSet<Action<InputDevice>> ListenersForType(InputControlType type) {
			return _listenersForInputTypes.GetAndCreateIfNotFound(type);
		}
		
		protected IEnumerator RemoveCallBackForTypeAtEndOfFrame(InputControlType type, Action<InputDevice> callback) {
			yield return new WaitForEndOfFrame();
			this.ListenersForType(type).Remove(callback);
		}
	}
}
#endif