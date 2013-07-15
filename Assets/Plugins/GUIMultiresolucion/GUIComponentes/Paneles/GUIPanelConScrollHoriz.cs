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
	public class GUIPanelConScrollHoriz : GUIComponente {
		/// <summary>
		/// Los elementos que estan dentro del panel
		/// </summary>
		[SerializeField] private List<GUIItemPanel> items;
		
		public List<GUIItemPanel> Items{
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
			foreach(GUIItemPanel i in items){
				i.inicializar();
			}
			
			//calculamos la anchura del panel en funcion a la anchura de cada item
			if(items != null && items.Count > 0){
				this.anchura = 0f;
				
				foreach(GUIItemPanel i in items){
					this.anchura += i.Componente.anchura;
					
					//calculamos que item tiene la mayor altura
					if(i.Componente.altura > alturaMax){
						alturaMax = i.Componente.altura;	
					}
				}	
			}
			
			this.altura = alturaMax;
		}		
		
		public override void dibujar (){
			foreach(GUIItemPanel i in items){
				if(i.Visible){
					i.dibujar();
				}
			}
		}
		#endregion
		
	}
}