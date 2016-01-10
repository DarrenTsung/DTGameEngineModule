using System.Collections;
﻿using UnityEngine;

namespace DT {
  public interface IViewController : IShowDismissEvents<IViewController> {
    void Show();
    void Dismiss();
	}
}
