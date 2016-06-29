using DT;
using System;
using UnityEngine;

namespace DT.GameEngine {
	public class IdAttribute : PropertyAttribute {
    public Type type;
    public bool hidePrefixLabel;

    public IdAttribute(Type type, bool hidePrefixLabel = false) : this(hidePrefixLabel) {
      this.type = type;
    }

    public IdAttribute(bool hidePrefixLabel = false) {
      this.hidePrefixLabel = hidePrefixLabel;

      // NOTE: if IdAttribute is created without type argument, then IdListDrawer will attempt to
      // infer the DTEntity type from the generic type arguments from the enclosing class

      // This is to solve trying to note something as an [Id] in a generic class like IdQuantity
    }
	}
}