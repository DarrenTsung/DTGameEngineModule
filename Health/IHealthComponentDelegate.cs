using DT;
using System.Collections;
﻿using UnityEngine;

namespace DT.GameEngine {
  public interface IHealthComponentDelegate {
  	void HandleDamageDealt(int damage);
  	void HandleHealthGiven(int health);
  	void HandleNoHealth();
  }
}
