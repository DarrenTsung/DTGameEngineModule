using DT;
using System.Collections;
using UnityEngine;

#if IN_CONTROL
using InControl;
#endif

public interface IPlayerInputManager {
#if IN_CONTROL
	bool IsDeviceBoundToAPlayerIndex(InputDevice device);
#endif

	bool InputDisabled {
		get;
		set;
	}

	void SetInputDisabledForPlayer(int playerIndex, bool inputDisabled);
	bool IsInputDisabledForPlayer(int playerIndex);
}
