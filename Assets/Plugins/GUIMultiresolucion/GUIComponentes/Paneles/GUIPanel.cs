using UnityEngine;
using System.Collections.Generic;
using GUIMultiresolucion.Core;
using TouchScript.Gestures;

namespace GUIMultiresolucion.GUIComponentes.Paneles{
	/*
	 * Representa un panel que incorpora la funcionalidad de un scroll horizontal
	 * mediante gestos con los dedos. 
	 */ 
	[System.Serializable]
	[ExecuteInEditMode]
	public class GUIPanel : GUIComponente {
		/// <summary>
		/// Los elementos que estan dentro del panel
		/// </summary>
		[SerializeField] private List<GUIItemPanel> items;
		
		/// <summary>
		/// Si se quiere hacer que el panel tenga scroll para desplazar sus items,
		/// se indica el tipo de scroll que se quiere hacer, o indicar el tipo NINGUNO, para que
		/// el panel no tenga scroll
		/// </summary>
		[SerializeField] private TipoScroll scroll;
		
		public List<GUIItemPanel> Items{
			get{return items;}	
		}
		
		#region Unity
		public void Start(){
			GetComponent<PanGesture>().StateChanged += realizarScroll;
		}
		#endregion
		
		#region gesto scroll
		void realizarScroll (object sender, TouchScript.Events.GestureStateChangeEventArgs e){
			var gestoScroll = sender as PanGesture;
			switch(e.State){
				case Gesture.GestureState.Began:
				break;
				case Gesture.GestureState.Changed:
					Vector2 pos = Vector2.zero;
					
					switch(scroll){
						case TipoScroll.HORIZONTAL:
							pos = new Vector2(gestoScroll.LocalDeltaPosition.x, 0f);
						break;
						case TipoScroll.VERTICAL:
							pos = new Vector2(0f, -gestoScroll.LocalDeltaPosition.y);
						break;
					}	
				
//					foreach(GUIItemPanel i in items){
//						i.actualizar(pos);	
//					}
				
				Debug.Log("scroll en panel");
					
				break;
				case Gesture.GestureState.Ended:
				break;
			}
		}
		#endregion
		
		#region metodos sobreescritos
		public override void inicializar(){	
			transform.localPosition = new Vector3(0f, 0f, -0.01f);
			
			float alturaMax = float.NegativeInfinity;
			
			//primero inicializamos los items
			foreach(GUIItemPanel i in items){
				i.inicializar(scroll);
			}
			
			//calculamos la anchura del panel en funcion a la anchura de cada item
			if(items != null && items.Count > 0){
				this.anchura = 0f;
				
				foreach(GUIItemPanel i in items){
					this.anchura += i.anchura;
					
					//calculamos que item tiene la mayor altura
					if(i.altura > alturaMax){
						alturaMax = i.altura;	
					}
				}	
			}
			
			this.altura = alturaMax;
			
			base.inicializar();
		}
		
		public override void actualizar (){
			foreach(GUIItemPanel i in items){
				i.actualizar(posicionRelativaA);
			}
		}
		
		public override void dibujar (){
			foreach(GUIItemPanel i in items){
				if(i.Visible && i.Item.Visible){
					i.dibujar();
				}
			}
		}
		#endregion
		
	}
}