using DT;
using System.Collections;
using UnityEngine;

#if IN_CONTROL
using InControl;

public interface IPlayerInputManager {
	bool InputDisabled {
		get;
		set;
	}
	
	void BindDeviceToUnusedPlayerIndex(InputDevice device);
	bool IsDeviceBoundToAPlayerIndex(InputDevice device);
		
	UnityEvents.V2 GetPrimaryDirectionEvent(int playerIndex);
	UnityEvents.V2 GetSecondaryDirectionEvent(int playerIndex);
	
	void SetInputDisabledForPlayer(int playerIndex, bool inputDisabled);
	bool IsInputDisabledForPlayer(int playerIndex);
}
#endif