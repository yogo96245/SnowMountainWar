using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Fusion;

public enum InputButtons {
    FIRE
}
public struct NetworkInputData : INetworkInput {
    public Vector2 movementInput;
    public Vector3 aimForwardVector;
    public NetworkButtons buttons;
}
