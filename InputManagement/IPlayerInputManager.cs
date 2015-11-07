﻿using DT;
using System.Collections;
using UnityEngine;

public interface IPlayerInputManager {
	bool InputDisabled {
		get;
		set;
	}
		
	UnityEvents.V2 GetPrimaryDirectionEvent(int playerIndex);
	UnityEvents.V2 GetSecondaryDirectionEvent(int playerIndex);
	
	void SetInputDisabledForPlayer(int playerIndex, bool inputDisabled);
	bool IsInputDisabledForPlayer(int playerIndex);
}
