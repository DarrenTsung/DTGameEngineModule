using DT;
using System.Collections;
using UnityEngine;

#if IN_CONTROL
using InControl;
#endif

public interface IControllerPlayerInputManager {
#if IN_CONTROL
	void BindDeviceToUnusedPlayerIndex(InputDevice device);
	bool IsDeviceBoundToAPlayerIndex(InputDevice device);
#endif
	
	UnityEvents.V2 GetPrimaryDirectionEvent(int playerIndex);
	UnityEvents.V2 GetSecondaryDirectionEvent(int playerIndex);
  
  int[] AllPlayerIndexesWithDevices();
}
