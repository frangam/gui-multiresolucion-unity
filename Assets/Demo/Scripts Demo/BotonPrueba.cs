using UnityEngine;
using System.Collections;
using TouchScript.Gestures;

public class BotonPrueba : MonoBehaviour {
	Color colorInicial;
	bool colorCambiado = false;
	
	// Use this for initialization
	void Start () {
		colorInicial = GameObject.Find("Sphere").renderer.material.color;
		GetComponent<TapGesture>().StateChanged += HandleStateChanged;
	}

	void HandleStateChanged (object sender, TouchScript.Events.GestureStateChangeEventArgs e)
	{
		switch(e.State){
			case Gesture.GestureState.Ended:
				if(!colorCambiado){
					GameObject.Find("Sphere").renderer.material.color = Color.yellow;
					colorCambiado = true;
				}
				else{
					GameObject.Find("Sphere").renderer.material.color = colorInicial;
					colorCambiado = false;
				}
			break;
		}
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
