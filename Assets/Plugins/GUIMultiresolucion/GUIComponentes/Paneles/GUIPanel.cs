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
		
		private bool primerItenEnPantalla;
		private bool ultimoItemEnPantalla;
		private float xPrevia = 0f;
		private bool resetearPrincipio = false; //true si se tienen que resetear las posiciones de los items del inicio del panel
		private bool resetearFinal = false; //true si se tienen que resetear las posiciones de los items del final del panel
		
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
		
		

		
		#region gesto scroll		
		public void realizarScroll (object sender, TouchScript.Events.GestureStateChangeEventArgs e){
			var gestoScroll = sender as PanGesture;
			switch(e.State){
				case Gesture.GestureState.Began:
				break;
				
				case Gesture.GestureState.Changed:
					Vector2 desplazamiento = Vector2.zero;
					
					xPrevia = gestoScroll.LocalDeltaPosition.x;
				
					switch(scroll){
						case TipoScroll.HORIZONTAL:
							resetearPrincipio = primerItem.Item.posicionRelativaA.x > 0.75f;
							resetearFinal = ultimoItem.Item.posicionRelativaA.x < 0.25f;
					
							//caso en que se tenga que reiniciar el principio pero se ha cambiado la direccion del movimiento
							//por tanto no hay que resetearlo
							if(resetearPrincipio && xPrevia < 0){
								resetearPrincipio = false;
							}	
					
							//caso en que se tenga que reiniciar el final pero se ha cambiado la direccion del movimiento
							//por tanto no hay que resetearlo
							else if(resetearFinal && xPrevia > 0){
								resetearFinal = false;
							}	
							
							//si no hay que resetear nada se calcula el desplazamiento de los items
							if(!resetearPrincipio && !resetearFinal){
								desplazamiento = new Vector2(gestoScroll.LocalDeltaPosition.x*velocidadScrollHorizontal, 0f);
							}
						break;
						case TipoScroll.VERTICAL:
							
						break;
					}	
				
					//si no hay que resetear posiciones ni del principio del panel ni del final
					//actualizamos las posiciones de todos los items
					if(!resetearPrincipio && !resetearFinal){
						actualizarPosiciones(desplazamiento);
					}

				break;
				
				case Gesture.GestureState.Ended:
					//si no hay que resetear el principio pero si el final
					if(!resetearPrincipio && resetearFinal){
						Vector2 posRel = ultimoItem.item.posicionRelativaAlAnclaRespectoAPosicionFijaDada(new Vector2(GUIEscalador.ANCHO_PANTALLA-ultimoItem.item.anchura-20f, ultimoItem.Item.posicionFija.y));
						float desplazamientoFinal = posRel.x - ultimoItem.item.posicionRelativaA.x;
						actualizarPosiciones(new Vector2(desplazamientoFinal, 0f));
					}
					//si hay que resetear el principio y no el final
					else if(resetearPrincipio && !resetearFinal){
						resetearPosicionesItems();
					}
					//si no hay que resetear posiciones ni del principio del panel ni del final
					else if(!resetearPrincipio && !resetearFinal){
						switch(scroll){
							case TipoScroll.HORIZONTAL:
								//comprobamos si estan en pantalla el primer y ultimo item
								bool primerItenEnPantalla = ((primerItem.item.posicionFija.x >= 0f)) && (primerItem.item.posicionFija.x + primerItem.item.anchura < GUIEscalador.ANCHO_PANTALLA);
								bool ultimoItemEnPantalla = ((ultimoItem.item.posicionFija.x >= 0f)) && (ultimoItem.item.posicionFija.x + ultimoItem.item.anchura < GUIEscalador.ANCHO_PANTALLA);
								
	//							Debug.Log(GUIEscalador.ANCHO_PANTALLA-ultimoItem.item.anchura);
								Vector2 posRel = ultimoItem.item.posicionRelativaAlAnclaRespectoAPosicionFijaDada(new Vector2(GUIEscalador.ANCHO_PANTALLA-ultimoItem.item.anchura-20f, ultimoItem.Item.posicionFija.y));
	//							Debug.Log(posRel.x+", "+posRel.y);
								Debug.Log("1o en pantalla? " +primerItenEnPantalla +", ultimo en pantalla? "+ultimoItemEnPantalla);
	//							Debug.Log(xPrevia);
						
								//si el movimiento ultimo del dedo era hacia la derecha
								if((xPrevia > 0 && primerItem.item.posicionRelativaA.x > 0.75f)
								|| (xPrevia > 0 && primerItenEnPantalla && !ultimoItemEnPantalla)
								|| (xPrevia <= 0 && primerItenEnPantalla && !ultimoItemEnPantalla)) //condicion: cambiar movimiento a la izquierda despues de haber hecho uno a la derecha previamente
								{
									resetearPosicionesItems();
								}
								//movimiento del dedo hacia la izquierda
								else if(xPrevia < 0 && ultimoItem.item.posicionRelativaA.x < 0.25f
									|| (xPrevia < 0 && ultimoItemEnPantalla && !primerItenEnPantalla)
									|| (xPrevia >= 0 && ultimoItemEnPantalla && !primerItenEnPantalla)) //condicion: cambiar movimiento a la derecha despues de haber hecho uno a la izquierda previamente
								{
									float desplazamientoFinal = posRel.x - ultimoItem.item.posicionRelativaA.x;
									actualizarPosiciones(new Vector2(desplazamientoFinal, 0f));
								}
								
							break;
						}
					}
					
				break;
			}
		}
		
		private void resetearPosicionesItems(){
			foreach(GUIItemPanel i in items){
				i.resetearPosiciones();	
			}
		}
		
		private void actualizarPosiciones(Vector2 posicion){
			foreach(GUIItemPanel i in items){	
				i.actualizar(posicion);
			}
		}
		
		#endregion
		
		
	}
}