using UnityEngine;
using System.Collections;
using TouchScript.Gestures;
using GUIMultiresolucion;
using GUIMultiresolucion.GUIComponentes;

namespace GUIMultiresolucion.Eventos{
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
//					transform.position = new Vector3(transform.position.x, transform.position.y, 0f);
				
					//si tiene asignada una textura para cuando se pulsa el boton, le cambiamos la textura que tiene que dibujarse por esa
					if(boton.texturaPulsado){
						boton.TexturaDibujar = boton.texturaPulsado;
					}
				break;
				case Gesture.GestureState.Changed:
					transform.position = new Vector3(transform.position.x, transform.position.y, 0.01f);
				break;
				//si se cancela o finaliza
				case Gesture.GestureState.Failed:
				case  Gesture.GestureState.Cancelled:
				case Gesture.GestureState.Ended:
					boton.TexturaDibujar = boton.texturaNormal;
				
					if(e.State == Gesture.GestureState.Failed){
	//					Debug.Log("fallo");
					}
				break;
			}
		}
		#endregion
	}
}