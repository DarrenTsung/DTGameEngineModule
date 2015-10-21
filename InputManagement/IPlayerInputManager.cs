using DT;
using System.Collections;
using UnityEngine;

public interface IPlayerInputManager {
	UnityEvents.V2 GetPrimaryDirectionEvent(int playerIndex);
	UnityEvents.V2 GetSecondaryDirectionEvent(int playerIndex);
}
