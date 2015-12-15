using DT;
using System.Collections;
﻿using UnityEngine;

namespace DT.GameEngine {
	public class HealthComponent : MonoBehaviour {
		// PRAGMA MARK - Public Interface
		public int Health {
			get {
				return _health;
			}
		}
		
		public bool IsFullHealth {
			get { 
				return _health == _baseHealth; 
			}
		}
		
		public virtual void DealDamage(int damage) {
			_health -= damage;
			if (_health <= 0) {
				_delegate.HandleNoHealth();
			} else {
				_delegate.HandleDamageDealt(damage);
			}
		}
		
		public virtual void GiveHealth(int health) {
			if (_health <= 0) {
				return;
			}
			
			if (_health >= _baseHealth) {
				return;
			}
			
			_health += health;
			if (_health > _baseHealth) {
				_health = _baseHealth;
			}
			_delegate.HandleHealthGiven(health);
		}
		
		// PRAGMA MARK - Interface 
		[SerializeField]
		protected int _baseHealth = 5;
		[SerializeField, ReadOnly]
		protected int _health;
		
		protected IHealthComponentDelegate _delegate;	
		
		protected virtual void Awake() {
			_delegate = this.GetRequiredComponentInParent<IHealthComponentDelegate>();
			_health = _baseHealth;
		}
	}
}