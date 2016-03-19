using DT;
using System;
using UnityEngine;

namespace DT.GameEngine {
	public class IdAttribute : PropertyAttribute {
    public Type type;

    public IdAttribute(Type type) {
      this.type = type;
    }
	}
}