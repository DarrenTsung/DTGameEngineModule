using System.Collections;
﻿using UnityEngine;

namespace DT {
  public interface IView : IShowDismissEvents<IView> {
    void Show();
    void Dismiss();
	}
}
