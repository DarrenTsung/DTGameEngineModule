using DT;
using System;
using System.Collections.Generic;
using System.Reflection;
using System.Linq;

namespace DT.GameEngine {
  public static class DTEntityUtil {
    static DTEntityUtil() {
      DTEntityUtil._entitySubclasses =
        (from assembly in AppDomain.CurrentDomain.GetAssemblies()
            from type in assembly.GetTypes()
            where type.IsSubclassOf(typeof(DTEntity))
            select type).ToArray();
    }

    public static Type[] EntitySubclasses {
      get { return DTEntityUtil._entitySubclasses; }
    }

    private static Type[] _entitySubclasses;
  }
}