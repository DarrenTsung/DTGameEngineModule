using DT;
using System.Collections;
﻿using UnityEngine;

#if IN_CONTROL
using InControl;

namespace DT.GameEngine {
	public class PlayerRegistrationGameSection : GameSection {
		// PRAGMA MARK - Public Interface
		public PlayerRegistrationGameSection(InputControlType registerControlType, 
																				 InputControlType finishRegistrationControlType) {
			_registerControlType = registerControlType;
			_finishRegistrationControlType = finishRegistrationControlType;
		}
		
		public override void Update() {
			base.Update();
		}
		
		
		// PRAGMA MARK - Internal
		protected InputControlType _registerControlType = InputControlType.Action1;
		protected InputControlType _finishRegistrationControlType = InputControlType.Command;
		
		protected override void InternalSetup() {
			this.SetupInputListeners();
		}
		
		protected override void InternalTeardown() {
			this.CleanupInputListeners();
		}
		
		protected void SetupInputListeners() {
			Toolbox.GetInstance<InputListenerManager>().ListenForType(_registerControlType, this.HandleDeviceRegisterPressed);
			Toolbox.GetInstance<InputListenerManager>().ListenForType(_finishRegistrationControlType, this.HandleFinishRegistrationPressed);
		}
		
		protected void CleanupInputListeners() {
			Toolbox.GetInstance<InputListenerManager>().RemoveCallbackForType(_registerControlType, this.HandleDeviceRegisterPressed);
			Toolbox.GetInstance<InputListenerManager>().RemoveCallbackForType(_finishRegistrationControlType, this.HandleFinishRegistrationPressed);
		}
		
		protected void HandleDeviceRegisterPressed(InputDevice device) {
			Toolbox.GetInstance<IControllerPlayerInputManager>().BindDeviceToUnusedPlayerIndex(device);
		}
		
		protected void HandleFinishRegistrationPressed(InputDevice device) {
			Debug.Log("Finish registration!");
			DTGameEngineNotifications.PlayerRegistrationFinished.Invoke();
		}
	}
}

#endif
