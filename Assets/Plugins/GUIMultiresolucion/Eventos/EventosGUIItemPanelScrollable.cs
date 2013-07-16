using UnityEngine;
using System.Collections;
using TouchScript.Gestures;
using GUIMultiresolucion.Core;
using GUIMultiresolucion.GUIComponentes;
using GUIMultiresolucion.GUIComponentes.Paneles;

namespace GUIMultiresolucion.Eventos{
	[RequireComponent(typeof(Transform))]
	[RequireComponent(typeof(PanGesture))]
	public class EventosGUIItemPanelScrollable : MonoBehaviour {
		/// <summary>
		/// El panel al que pertenece el boton
		/// </summary>
		private GUIPanel panel;
			
		#region metodos publicos
		public void inicializar(GUIPanel _panel){
			panel = _panel;
			
			if(panel != null){
				GetComponent<PanGesture>().StateChanged += scroll;
			}
			else{
				Debug.Log("Falta el GUIPanel al que pertenece el gui componente");	
			}
		}
		#endregion
		
		#region gestos
		void scroll (object sender, TouchScript.Events.GestureStateChangeEventArgs e){
			Debug.Log("scroll en item");
			panel.realizarScroll(sender, e);
		}
		#endregion
	}
}