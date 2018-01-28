using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SplashScreen : MonoBehaviour {

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
        if (Input.GetKeyDown(KeyCode.Space)
         || Input.GetKeyDown(KeyCode.Return)
         || Input.GetKeyDown(KeyCode.KeypadEnter)
         || GamepadInput.GamePad.GetButton(GamepadInput.GamePad.Button.Start, GamepadInput.GamePad.Index.Any)
         || GamepadInput.GamePad.GetButton(GamepadInput.GamePad.Button.A, GamepadInput.GamePad.Index.Any))
        {
            Application.LoadLevel("Game");
        }
    }
}
