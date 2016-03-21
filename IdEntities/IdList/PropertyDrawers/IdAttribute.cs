using DT;
using System;
using UnityEngine;

namespace DT.GameEngine {
	public class IdAttribute : PropertyAttribute {
    public Type type;

    public IdAttribute(Type type) {
      this.type = type;
    }

    public IdAttribute() {
      // NOTE: if IdAttribute is created without type argument, then IdListDrawer will attempt to
      // infer the DTEntity type from the generic type arguments from the enclosing class

      // This is to solve trying to note something as an [Id] in a generic class like IdQuantity
    }
	}
}