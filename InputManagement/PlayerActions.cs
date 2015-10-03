using DT;
using InControl;
using System.Collections;
﻿using UnityEngine;

#if IN_CONTROL
namespace DT {
	public class PlayerActions : PlayerActionSet {
		public PlayerTwoAxisAction PrimaryDirection;
		public PlayerTwoAxisAction SecondaryDirection;
		
		public PlayerActions() {
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

		public virtual void BindWithActions(PlayerInputType type) {
			switch (type) {
				case PlayerInputType.MOUSE_AND_KEYBOARD:
					_primaryLeft.AddDefaultBinding(Key.A);
					_primaryRight.AddDefaultBinding(Key.D);
					_primaryUp.AddDefaultBinding(Key.W);
					_primaryDown.AddDefaultBinding(Key.S);
					break;
				
				case PlayerInputType.CONTROLLER:
					_primaryLeft.AddDefaultBinding(InputControlType.LeftStickLeft);
					_primaryRight.AddDefaultBinding(InputControlType.LeftStickRight);
					_primaryUp.AddDefaultBinding(InputControlType.LeftStickUp);
					_primaryDown.AddDefaultBinding(InputControlType.LeftStickDown);
					
					_secondaryLeft.AddDefaultBinding(InputControlType.RightStickLeft);
					_secondaryRight.AddDefaultBinding(InputControlType.RightStickRight);
					_secondaryUp.AddDefaultBinding(InputControlType.RightStickUp);
					_secondaryDown.AddDefaultBinding(InputControlType.RightStickDown);
					break;
			}
		}
		
		// PRAGMA MARK - INTERNAL
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