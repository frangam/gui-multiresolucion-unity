using UnityEngine;
using System.Collections;
using TouchScript.Gestures;
using GUIMultiresolucion;
using GUIMultiresolucion.Core;
using GUIMultiresolucion.GUIComponentes;

namespace GUIMultiresolucion.Eventos{
	public class EventosGUIBoton: MonoBehaviour {
		private GUIBoton boton;
		
		/// <summary>
		/// True si se quiere cambiar la textura cuando se pulsa el boton
		/// </summary>
		public bool cambiarTexturaPulsado = true;
		
		#region Unity
		void Start(){
			boton = GetComponent<GUIBoton>();
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
					if(boton.texturaPulsado && cambiarTexturaPulsado){
						boton.TexturaDibujar = boton.texturaPulsado;
					}
				break;
				//si se cancela o finaliza
				case Gesture.GestureState.Failed:
				case  Gesture.GestureState.Cancelled:
				case Gesture.GestureState.Ended:
					if(boton.texturaPulsado && cambiarTexturaPulsado){
						boton.TexturaDibujar = boton.texturaNormal;
					}
				
					//actualizamos bandera
					boton.EjecutarAccionEstandar = true;
				break;
			}
		}
		#endregion
	}
}