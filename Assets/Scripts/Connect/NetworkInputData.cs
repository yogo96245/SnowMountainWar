using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Fusion;

public enum InputButtons {
    FIRE
}
public struct NetworkInputData : INetworkInput {
    public Vector3 movementInput;
    public Vector2 rotateInput;
    public NetworkButtons buttons;
}
