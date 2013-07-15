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
	public class GUIItemPanel :  GUIComponente{
		/// <summary>
		/// El GUIComponente que representa al item adjuntado al panel
		/// </summary>
		[SerializeField] private GUIComponente item;
		
		/// <summary>
		/// Si se quiere hacer que el item tenga scroll para desplazar sus items,
		/// se indica el tipo de scroll que se quiere hacer, o indicar el tipo NINGUNO, para que
		/// el item no tenga scroll
		/// </summary>
		private TipoScroll scroll;
		
		private Vector2 posicionInicial;
		
		#region propiedades
		public GUIComponente Item{
			get{return item;}	
		}
		public TipoScroll Scroll{
			get{return scroll;}	
		}
		#endregion
		
		#region nuevos metodos
		public void inicializar(TipoScroll _scroll){	
			transform.localPosition = new Vector3(0f, 0f, 0.01f);
			
			scroll = _scroll;
			
			item.inicializar();
			
			base.inicializar(item);
		}
		
		public void actualizar(Vector2 posRelativa){
			item.posicionRelativaA += posRelativa;
			item.actualizar();
			actualizar();
		}
		#endregion
		
		#region metodos sobreescritos de la clase base
		public override void dibujar ()
		{
			item.dibujar();
		}
		#endregion
		
		#region Unity
		void Start () {
			posicionInicial = item.posicionRelativaA;
			GetComponent<PanGesture>().StateChanged += realizarScroll;	
		}
		#endregion
		
		#region gesto scroll
		void realizarScroll (object sender, TouchScript.Events.GestureStateChangeEventArgs e){
			Debug.Log(scroll);
			if(scroll != TipoScroll.NINGUNO){
				var gestoScroll = sender as PanGesture;
				
				switch(e.State){
					case Gesture.GestureState.Began:
					
					break;
					case Gesture.GestureState.Changed:
						
						Debug.Log("scroll en item");
						
					break;
					case Gesture.GestureState.Ended:
	//					componente.posicionRelativaA = posicionInicial;
					break;
				}
			}
		}
		#endregion
		
	}
}
