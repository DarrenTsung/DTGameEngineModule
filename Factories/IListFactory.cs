using DT;
using System.Collections;
using System.Collections.Generic;
﻿using UnityEngine;

namespace DT.GameEngine {
  public interface IListFactory<TEntity> where TEntity : DTEntity {
    IIdList<TEntity> GetList();
	}
}
