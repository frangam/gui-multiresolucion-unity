using UnityEngine;
using System.Collections;
using GUIMultiresolucion.GUIComponentes;
using TouchScript.Gestures;

public class AbrirVentana : MonoBehaviour {
	
	public GUIMultiVentana ventana; 
		#region Unity
		void Start(){
			
			GetComponent<TapGesture>().StateChanged += tap;
		}
	
	
		#endregion
		
		#region gestion eventos de toques
		void tap (object sender, TouchScript.Events.GestureStateChangeEventArgs e)
		{
			//segun el estado del evento
			switch(e.State){
				case Gesture.GestureState.Began:
					//si tiene asignada una textura para cuando se pulsa el boton, le cambiamos la textura que tiene que dibujarse por esa
					//y si se quiere cambiar
					
				break;
				//si se cancela o finaliza
				case Gesture.GestureState.Failed:
				case  Gesture.GestureState.Cancelled:
				case Gesture.GestureState.Ended:
					ventana.Visible = true;
					ventana.abrirVentana(2);
				break;
			}
		}
		#endregion
}
