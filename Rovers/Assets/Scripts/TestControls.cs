using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GamepadInput;

public class TestControls : MonoBehaviour {

	public GameObject roverGameObject;
	private Rover rover;

	void Awake()
	{
        rover = roverGameObject.GetComponent<Rover>();
	}
	
	void Update () {
        
		float axisY = GamePad.GetAxis(GamePad.Axis.LeftStick, GamePad.Index.One).y;
        float axisX = GamePad.GetAxis(GamePad.Axis.LeftStick, GamePad.Index.One).x;
        if(axisY > 0)
		{
            rover.MoveForward2();
		}
		if(axisY < 0)
		{
            rover.MoveBackward2();
		}
        if(axisX < 0)
        {
            rover.RotateLeft2();
        }
        if(axisX > 0)
        {
            rover.RotateRight2();
        }
    }
}
