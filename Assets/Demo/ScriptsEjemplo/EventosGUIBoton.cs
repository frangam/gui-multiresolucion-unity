using UnityEngine;
using System.Collections;
using TouchScript.Gestures;
using GUIMultiresolucion;
using GUIMultiresolucion.GUIComponentes;

public class EventosGUIBoton: MonoBehaviour {
	private GUIBoton boton;
	
	#region Unity
	void Start(){
		boton = GetComponent<GUIBoton>();
		GetComponent<TapGesture>().StateChanged += HandleStateChanged;
	}


	#endregion
	
	#region gestion eventos de toques
	void HandleStateChanged (object sender, TouchScript.Events.GestureStateChangeEventArgs e)
	{
		//segun el estado del evento
		switch(e.State){
			case Gesture.GestureState.Began:
				boton.TexturaDibujar = boton.texturaPulsado;
			break;
			//si se cancela o finaliza
			case Gesture.GestureState.Failed:
			case  Gesture.GestureState.Cancelled:
			case Gesture.GestureState.Ended:
				boton.TexturaDibujar = boton.texturaNormal;
			
				if(e.State == Gesture.GestureState.Failed){
					Debug.Log("fallo");
				}
			break;
		}
	}
	#endregion
}
