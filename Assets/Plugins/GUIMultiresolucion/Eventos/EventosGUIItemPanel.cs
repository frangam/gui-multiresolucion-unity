using UnityEngine;
using System.Collections;
using TouchScript.Gestures;
using GUIMultiresolucion.Core;
using GUIMultiresolucion.GUIComponentes.Paneles;

namespace GUIMultiresolucion.Eventos{
	public class EventosGUIItemPanel : MonoBehaviour {
		private GUIItemPanel itemPanel;
		
		#region Unity
		void Start () {
			itemPanel = GetComponent<GUIItemPanel>();
			GetComponent<PanGesture>().StateChanged += scroll;
		}
		#endregion
		
		#region gestion eventos
		void scroll (object sender, TouchScript.Events.GestureStateChangeEventArgs e){
			var gestoScroll = sender as PanGesture;
			
			Debug.Log(e.State);
			
			switch(e.State){
				case Gesture.GestureState.Began:
				
				break;
				case Gesture.GestureState.Changed:
					Vector2 pos = Vector2.zero;
				
					switch(itemPanel.Scroll){
						case TipoScroll.HORIZONTAL:
							pos = new Vector2(gestoScroll.LocalDeltaPosition.x, 0f);
						break;
						case TipoScroll.VERTICAL:
							pos = new Vector2(0f, -gestoScroll.LocalDeltaPosition.y);
						break;
					}	
					
					itemPanel.actualizar(pos);
					
				break;
				case Gesture.GestureState.Ended:
//					componente.posicionRelativaA = posicionInicial;
				break;
			}
		}
		#endregion

	}
}
