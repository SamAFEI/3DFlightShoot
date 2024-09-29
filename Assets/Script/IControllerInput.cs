using UnityEngine;

public delegate void InputEvent();
public delegate void InputEventFloat(float vlaue);
public delegate void InputEventInt(int vlaue);
public delegate void InputEventVector3(Vector3 vlaue);

public interface IControllerInput
{
    event InputEventFloat ForwardEvent;
    event InputEventFloat HorizontalStrafeEvent;
    event InputEventFloat VerticalStrafeEvent;
    event InputEventFloat YawEvent;
    event InputEventFloat PitchEvent;
    event InputEventFloat RollEvent;
    event InputEventVector3 TurnEvent;
    event InputEventVector3 Fire01Event;
    event InputEventVector3 Fire02Event;
}
