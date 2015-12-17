using DT;
﻿using UnityEngine;
﻿using UnityEngine.Events;

namespace DT.GameEngine {
	public class ButtonEventGroup {
    public UnityEvent StartPressed = new UnityEvent();
    public UnityEvent BeingPressed = new UnityEvent();
    public UnityEvent WasReleased = new UnityEvent();
	}
}
