using DT;
using System.Collections;
﻿using UnityEngine;
﻿using UnityEngine.Events;

namespace DT.GameEngine {
	public static class DTGameEngineNotifications {
		// NOTE (darren): notification snippet is useful here [notif, notifArgs]

		// PRAGMA MARK - Player
		public static UnityEvents.IG PlayerChanged = new UnityEvents.IG();

		// PRAGMA MARK - Input
		public static UnityEvents.V2 HandleMouseScreenPosition = new UnityEvents.V2();
		public static UnityEvents.IV2 HandlePrimaryDirection = new UnityEvents.IV2();
		public static UnityEvents.IV2 HandleSecondaryDirection = new UnityEvents.IV2();

    // PRAGMA MARK - Game Manager
    public static UnityEvent PlayerRegistrationFinished = new UnityEvent();

		public static UnityEvent OnLevelSimulationSectionSetup = new UnityEvent();
		public static UnityEvent OnLevelSimulationSectionTeardown = new UnityEvent();

    // PRAGMA MARK - Game Sessions
    public static UnityEvents.O GameSessionStarted = new UnityEvents.O();
    public static UnityEvents.O GameSessionFinished = new UnityEvents.O();
	}
}