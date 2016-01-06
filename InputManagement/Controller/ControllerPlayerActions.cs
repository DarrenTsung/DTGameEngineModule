using DT;
using System.Collections;
﻿using UnityEngine;

#if IN_CONTROL
using InControl;

namespace DT.GameEngine {
	public class ControllerPlayerActions : PlayerActionSet {
		public PlayerTwoAxisAction PrimaryDirection;
		public PlayerTwoAxisAction SecondaryDirection;
		
		public ControllerPlayerActions() {
			_primaryLeft = this.CreatePlayerAction("Primary Left");
			_primaryRight = this.CreatePlayerAction("Primary Right");
			_primaryUp = this.CreatePlayerAction("Primary Up");
			_primaryDown = this.CreatePlayerAction("Primary Down");
			this.PrimaryDirection = this.CreateTwoAxisPlayerAction(_primaryLeft, _primaryRight, _primaryDown, _primaryUp);
			
			_secondaryLeft = this.CreatePlayerAction("Secondary Left");
			_secondaryRight = this.CreatePlayerAction("Secondary Right");
			_secondaryUp = this.CreatePlayerAction("Secondary Up");
			_secondaryDown = this.CreatePlayerAction("Secondary Down");
			this.SecondaryDirection = this.CreateTwoAxisPlayerAction(_secondaryLeft, _secondaryRight, _secondaryDown, _secondaryUp);
		}

		public void BindWithInputType(ControllerPlayerInputType type) {
			switch (type) {
				case ControllerPlayerInputType.MOUSE_AND_KEYBOARD:
					this.BindMouseAndKeyboardType(type);
					break;
				
				case ControllerPlayerInputType.CONTROLLER:
					this.BindControllerType(type);
					break;
			}
		}
		
		protected virtual void BindMouseAndKeyboardType(ControllerPlayerInputType type) {
			_primaryLeft.AddDefaultBinding(Key.A);
			_primaryRight.AddDefaultBinding(Key.D);
			_primaryUp.AddDefaultBinding(Key.W);
			_primaryDown.AddDefaultBinding(Key.S);
		}
		
		protected virtual void BindControllerType(ControllerPlayerInputType type) {
			_primaryLeft.AddDefaultBinding(InputControlType.LeftStickLeft);
			_primaryRight.AddDefaultBinding(InputControlType.LeftStickRight);
			_primaryUp.AddDefaultBinding(InputControlType.LeftStickUp);
			_primaryDown.AddDefaultBinding(InputControlType.LeftStickDown);
			
			_secondaryLeft.AddDefaultBinding(InputControlType.RightStickLeft);
			_secondaryRight.AddDefaultBinding(InputControlType.RightStickRight);
			_secondaryUp.AddDefaultBinding(InputControlType.RightStickUp);
			_secondaryDown.AddDefaultBinding(InputControlType.RightStickDown);
		}
		
		// PRAGMA MARK - Internal
		protected PlayerAction _primaryLeft;
		protected PlayerAction _primaryRight;
		protected PlayerAction _primaryUp;
		protected PlayerAction _primaryDown;
		
		protected PlayerAction _secondaryLeft;
		protected PlayerAction _secondaryRight;
		protected PlayerAction _secondaryUp;
		protected PlayerAction _secondaryDown;
	}
}
#endif