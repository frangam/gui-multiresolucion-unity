using UnityEngine;
using System.Collections;
using GUIMultiresolucion.GUIComponentes;
using TouchScript.Gestures;

public class AbrirVentana : MonoBehaviour {
	
	public GUIMultiVentana ventana;
	public int indiceVentana = 0;
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
				case Gesture.GestureState.Ended:
					Debug.Log("Pulsado");
					ventana.abrirVentana(indiceVentana);
					
				break;
			}
		}
		#endregion
}
