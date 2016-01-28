using DT;
using System.Collections;
﻿using UnityEngine;

namespace DT.GameEngine {
	public class HealthComponent : MonoBehaviour {
		// PRAGMA MARK - Public Interface
    public UnityEvents.B OnInvulnerableStateChange = new UnityEvents.B();

		public int Health {
			get {
				return this._health;
			}
		}

		public bool IsFullHealth {
			get {
				return this._health == this._baseHealth;
			}
		}

		public virtual void DealDamage(int damage) {
      if (this._invulnerable) {
        return;
      }

			this._health -= damage;
			if (this._health <= 0) {
				this._delegate.HandleNoHealth();
			} else {
				this._delegate.HandleDamageDealt(damage);
			}
		}

		public virtual void GiveHealth(int health) {
			if (this._health <= 0) {
				return;
			}

			if (this._health >= this._baseHealth) {
				return;
			}

			this._health += health;
			if (this._health > this._baseHealth) {
				this._health = this._baseHealth;
			}
			this._delegate.HandleHealthGiven(health);
		}

    public int BaseHealth {
      set {
        this._baseHealth = value;
        this._health = this._baseHealth;
      }
    }

    public IHealthComponentDelegate Delegate {
      set {
        this._delegate = value;
      }
    }

    public void SetInvulnerableForTime(float time) {
      this.Invulnerable = true;

      // stop the setting false coroutine if it exists
      if (this._invulnerableCoroutine != null) {
        this.StopCoroutine(this._invulnerableCoroutine);
      }

      this._invulnerableCoroutine = this.DoAfterDelay(time, () => {
        this.Invulnerable = false;
        this._invulnerableCoroutine = null;
      });
    }


		// PRAGMA MARK - Interface
		[SerializeField]
		protected int _baseHealth = 5;
		[SerializeField, ReadOnly]
		protected int _health;

    protected bool Invulnerable {
      get {
        return this._invulnerable;
      }
      set {
        this._invulnerable = value;
        this.OnInvulnerableStateChange.Invoke(this._invulnerable);
      }
    }

    [SerializeField, ReadOnly]
    private bool _invulnerable;

		protected IHealthComponentDelegate _delegate;

    private IEnumerator _invulnerableCoroutine;

		protected virtual void Awake() {
      if (this._delegate == null) {
  			this._delegate = this.GetRequiredComponentInParent<IHealthComponentDelegate>();
      }
			this._health = this._baseHealth;
		}
	}
}