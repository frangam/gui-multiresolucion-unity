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
		
		private float velocidadScrollHorizontal = 0.8f;
		private float velocidadScrollVertical = 0.55f;
		
		#region propiedades
		public List<GUIItemPanel> Items{
			get{return items;}	
		}
		public TipoScroll Scroll{
			get{return scroll;}	
		}
		#endregion
		
		#region Unity
		public void Start(){
			GetComponent<PanGesture>().StateChanged += realizarScroll;
		}
		#endregion
		
		
		
		#region gesto scroll		
		public void realizarScroll (object sender, TouchScript.Events.GestureStateChangeEventArgs e){
			var gestoScroll = sender as PanGesture;
			switch(e.State){
				case Gesture.GestureState.Began:
				break;
				
				case Gesture.GestureState.Changed:
					Vector2 pos = Vector2.zero;
					
					switch(scroll){
						case TipoScroll.HORIZONTAL:
							pos = new Vector2(gestoScroll.LocalDeltaPosition.x*velocidadScrollHorizontal, 0f);
						break;
						case TipoScroll.VERTICAL:
							pos = new Vector2(0f, -gestoScroll.LocalDeltaPosition.y*velocidadScrollVertical);
						break;
					}	
				
					foreach(GUIItemPanel i in items){
						i.actualizar(pos);	
					}					
				break;
				
				case Gesture.GestureState.Ended:
				break;
			}
		}
		#endregion
		
		#region metodos sobreescritos
		public override void inicializar(){				
			if(items != null && items.Count > 0){
				//cambiamos la coordenada Z al panel para que se quede detras de los colliders de los items que tenga
				//para que se puedan detectar sin problemas los gestos sobre los items, de forma independiente a los gestos del panel
				transform.localPosition = new Vector3(transform.localPosition.x, transform.localPosition.y, 0.01f);
				
				//primero inicializamos los items
				foreach(GUIItemPanel i in items){
					i.inicializar(scroll, this);
				}
				
				//horizontal: mantenemos anchura de la pantalla
				if(scroll == TipoScroll.HORIZONTAL){
					float alturaMax = float.NegativeInfinity;
					
					foreach(GUIItemPanel i in items){
						//calculamos que item tiene la mayor altura
						if(i.Item.altura > alturaMax){
							alturaMax = i.Item.altura;	
						}
					}
					
					this.anchura = dimensionPantallaEscalada().x;
					this.altura = alturaMax;
				}
				//vertical: mantenemos altura de la pantalla
				else if(scroll == TipoScroll.VERTICAL){
					float anchuraMax = float.NegativeInfinity;
					
					foreach(GUIItemPanel i in items){
						//calculamos que item tiene la mayor altura
						if(i.Item.anchura > anchuraMax){
							anchuraMax = i.Item.anchura;	
						}
					}
					
					this.altura = dimensionPantallaEscalada().y;
				}	
				
				base.inicializar();
			}
		}
		
		public override void actualizar (){
			foreach(GUIItemPanel i in items){
				i.actualizar(posicionRelativaA);
			}
		}
		
		public override void dibujar (){
			foreach(GUIItemPanel i in items){
				if(i.Item.Visible){
					i.dibujar();
				}
			}
		}
		#endregion
		
	}
}