using UnityEngine;
using System.Collections;
using GUIMultiresolucion.Core;
using TouchScript.Gestures;

namespace GUIMultiresolucion.GUIComponentes.Paneles{
	/*
	 * Representa un elemento que forma parte de un panel
	 */ 
	[System.Serializable]
	[ExecuteInEditMode]
	public class GUIItemPanel :  MonoBehaviour{
		[SerializeField] private GUIComponente componente;
		
		#region propiedades
		public GUIComponente Componente{
			get{return componente;}
			set{componente = value;}
		}
		
		public Rect distribucion{
			get{return componente.distribucion;}	
		}
		
		public void inicializar(){
			componente.inicializar();	
		}
		public void dibujar(){
			componente.dibujar();	
		}
		
		public bool Visible{
			get{return componente.Visible;}
			set{
				componente.Visible = value;
				GetComponent<BoxCollider>().enabled = value; //habilitamos o desactivamos el collider
			}
		}
		#endregion
		
//		#region metodos sobreescritos
////		public override void inicializar ()
////		{
////			
////		}
////		
////		public override void dibujar (){
//////			GUI.DrawTexture(distribucion, textura);
////		}
//		#endregion
		
		#region Unity
		void Start () {
			GetComponent<PanGesture>().StateChanged += realizarScroll;;
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
		
	}
}
