using DT;
using System.Collections;
﻿using UnityEngine;

namespace DT.GameEngine {
  [RequireComponent (typeof(Rigidbody2D))]
  public class DamageOnTrigger : MonoBehaviour {
  	// PRAGMA MARK - Internal
  	[SerializeField]
  	protected int _damage = 1;

  	protected virtual void OnTriggerEnter2D(Collider2D other) {
  		this.DamageOther(other);
  	}

  	protected virtual void OnTriggerStay2D(Collider2D other) {
  		this.DamageOther(other);
  	}

  	protected virtual void DamageOther(Collider2D other) {
  		HealthComponent otherHealthComponent = other.gameObject.GetComponentInParent<HealthComponent>();
  		if (otherHealthComponent) {
  			otherHealthComponent.DealDamage(this._damage);
  		}
  	}
  }
}