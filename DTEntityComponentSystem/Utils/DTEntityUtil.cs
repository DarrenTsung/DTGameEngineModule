using DT;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace DT.GameEngine {
  public static class DTEntityUtil {
    static DTEntityUtil() {
      DTEntityUtil.EntitySubclasses =
        (from assembly in AppDomain.CurrentDomain.GetAssemblies()
            from type in assembly.GetTypes()
            where type != typeof(DTEntity) && typeof(DTEntity).IsAssignableFrom(type) && !type.IsAbstract
            select type).ToArray();

      DTEntityUtil.EntitySubclassesByName = DTEntityUtil.EntitySubclasses.ToMapWithKeys(t => t.Name);
    }

    public static Type[] EntitySubclasses {
      get; private set;
    }

    public static Dictionary<string, Type> EntitySubclassesByName {
      get; private set;
    }
  }
}