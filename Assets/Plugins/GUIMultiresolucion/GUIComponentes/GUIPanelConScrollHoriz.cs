using UnityEngine;
using System.Collections.Generic;
using GUIMultiresolucion.Core;
using TouchScript.Gestures;

namespace GUIMultiresolucion.GUIComponentes{
	/*
	 * Representa un panel que incorpora la funcionalidad de un scroll horizontal
	 * mediante gestos con los dedos. 
	 */ 
	[System.Serializable]
	[ExecuteInEditMode]
	public class GUIPanelConScrollHoriz : GUIComponente {
		/// <summary>
		/// Los elementos que estan dentro del panel
		/// </summary>
		[SerializeField] private List<GUIComponente> items;
		
		public List<GUIComponente> Items{
			get{return items;}	
		}
		
		#region propiedades
		public bool Visible{
			get{return base.visible;}
			set{
				base.Visible = value;
				GetComponent<BoxCollider>().enabled = value; //habilitamos o desactivamos el collider
			}
		}
//		public Rect distribucion{
//	        get{
//	          
//	            inicializar();
//				
//				return base.distribucion;
//	        }
//	    }
		#endregion
		
		#region Unity
		public void Start(){
			GetComponent<PanGesture>().StateChanged += realizarScroll;
			GetComponent<BoxCollider>().enabled = visible;
		}
		#endregion
		
		#region gesto scroll
		void realizarScroll (object sender, TouchScript.Events.GestureStateChangeEventArgs e){
			var gestoScroll = sender as PanGesture;
			
			switch(e.State){
				case Gesture.GestureState.Began:
				break;
				case Gesture.GestureState.Changed:
				break;
				case Gesture.GestureState.Ended:
				break;
			}
		}
		#endregion
		
		#region metodos sobreescritos
		public override void inicializar(){	
			float alturaMax = float.NegativeInfinity;
			
			//primero inicializamos los items
			foreach(GUIComponente c in items){
				c.inicializar();
			}
			
			//calculamos la anchura del panel en funcion a la anchura de cada item
			if(items != null && items.Count > 0){
				this.anchura = 0f;
				
				foreach(GUIComponente c in items){
					this.anchura += c.anchura;
					
					//calculamos que item tiene la mayor altura
					if(c.altura > alturaMax){
						alturaMax = c.altura;	
					}
				}	
			}
			
			this.altura = alturaMax;
		}		
		
		public override void dibujar (){
			foreach(GUIComponente c in items){
				if(c.visible){
					c.dibujar();
				}
			}
		}
		#endregion
		
	}
}