using UnityEngine;
using System.Collections;
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
		
		private ArrayList itemsOrdenados;
		private GUIItemPanel primerItem;
		private GUIItemPanel ultimoItem;
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
				
				//ordenamos los items
				itemsOrdenados = new ArrayList(items);
				itemsOrdenados.Sort();
				primerItem = (GUIItemPanel) itemsOrdenados[0];
				ultimoItem = (GUIItemPanel) itemsOrdenados[itemsOrdenados.Count-1];
				
				//horizontal: mantenemos anchura de la pantalla
				if(scroll == TipoScroll.HORIZONTAL){
					float alturaMax = float.NegativeInfinity;
					
					foreach(GUIItemPanel i in items){
						//calculamos que item tiene la mayor altura
						if(i.Item.altura > alturaMax){
							alturaMax = i.Item.altura;	
						}
					}
					
					this.anchura = GUIEscalador.ANCHO_PANTALLA;
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
					
					this.altura = GUIEscalador.ALTO_PANTALLA;
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
		
		
		#region Unity
		public void Start(){
			GetComponent<PanGesture>().StateChanged += realizarScroll;
		}
		#endregion
		
		private float xPrevia = 0f;
		bool ultimoItemEnPantalla = false;
		
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
				
					xPrevia = gestoScroll.LocalDeltaPosition.x;
				break;
				
				case Gesture.GestureState.Ended:
					switch(scroll){
						case TipoScroll.HORIZONTAL:
							//comprobamos si estan en pantalla el primer y ultimo item
							bool primerItenEnPantalla = (((primerItem.item.posicionFija.x + primerItem.item.anchura) >= 0f)) && (primerItem.item.posicionFija.x < GUIEscalador.ANCHO_PANTALLA);
							bool ultimoItemEnPantalla = (((ultimoItem.item.posicionFija.x + ultimoItem.item.anchura) >= 0f)) && (ultimoItem.item.posicionFija.x < GUIEscalador.ANCHO_PANTALLA);
							
							//si el movimiento ultimo era hacia la derecha
							if(xPrevia > 0 && !ultimoItemEnPantalla){
								resetearPosicionesItems();
							}
							//movimiento hacia la izquierda
							else if(xPrevia < 0 && ultimoItemEnPantalla){
								actualizarPosicionesItems();
							}
							
						break;
					}
					
				break;
			}
		}
		
		private void resetearPosicionesItems(){
			foreach(GUIItemPanel i in items){
				i.resetearPosiciones();	
			}
		}
		
		private void actualizarPosicionesItems(){
			foreach(GUIItemPanel i in items){
//				i.item.posicionFija += new Vector2(GUIEscalador.ANCHO_PANTALLA - 10, i.item.posicionFija.y);
				i.actualizar(new Vector2(0.1f, i.item.posicionRelativaA.y));
			}
		}
		#endregion
		
		
	}
}