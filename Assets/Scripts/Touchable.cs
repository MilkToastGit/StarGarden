using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface Touchable
{
    public bool Passthrough { get; }
    public void OnStartTouch();
    public void OnEndTouch();
}
