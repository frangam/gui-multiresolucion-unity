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
	public class GUIPanel : GUIComponente {
		#region atributos de configuracion
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
		
		/// <summary>
		/// La velocidad con la que se deben de mover los items del panel con un scroll de tipo horizontal
		/// </summary>
		[SerializeField] private float velocidadScrollHorizontal = 0.8f;
		
		/// <summary>
		/// La velocidad con la que se deben de mover los items del panel con un scroll de tipo vertical
		/// </summary>
		[SerializeField] private float velocidadScrollVertical = 0.55f;
		
		/// <summary>
		/// Coordenada X maxima relativa hasta donde se puede realizar el scroll en horizontal
		/// </summary>
		[SerializeField] private float xMaximaRelativaScrollable = 0.75f;
		
		/// <summary>
		/// Coordenada Y maxima relativa hasta donde se puede realizar el scroll en vertical
		/// </summary>
		[SerializeField] private float yMaximaRelativaScrollable = 0.75f;
		#endregion
		
		#region atributos privados
		/// <summary>
		/// Los items que estan en el panel ordenados por el criterio de IComparable implementado en GUIItemPanel
		/// </summary>
		private ArrayList itemsOrdenados;
		
		/// <summary>
		/// El primer item del panel
		/// </summary>
		private GUIItemPanel primerItem;
		
		/// <summary>
		/// El ultimo item del panel
		/// </summary>
		private GUIItemPanel ultimoItem;
		
		/// <summary>
		/// True si el primer item del panel esta en pantalla.
		/// Condicion para que este en pantalla es que su posicion este dentro de los margenes de la pantalla
		/// </summary>
		private bool primerItenEnPantalla;
		
		/// <summary>
		/// True si el ultimo item del panel esta en pantalla.
		/// Condicion para que este en pantalla es que su posicion este dentro de los margenes de la pantalla
		/// </summary>
		private bool ultimoItemEnPantalla;
		
		/// <summary>
		/// Coordenada X del scroll previo
		/// </summary>
		private float xPreviaScroll = 0f;
		
		/// <summary>
		/// Coordenada Y del scroll previo
		/// </summary>
		private float yPreviaScroll = 0f;
				
		/// <summary>
		/// Coordenada X minima relativa hasta donde se puede realizar el scroll en horizontal
		/// </summary>
		private float xMinimaRelativaScrollable;
		
		/// <summary>
		/// Coordenada Y minima relativa hasta donde se puede realizar el scroll en vertical
		/// </summary>
		private float yMinimaRelativaScrollable;
		
		/// <summary>
		/// El margen entre el final del panel y el ultimo item
		/// </summary>
		private float margenFinal = 20f;
		
		/// <summary>
		/// True si se tienen que resetear las posiciones de los items del inicio del panel
		/// </summary>
		private bool resetearPrincipio = false; 
		
		/// <summary>
		/// True si se tienen que resetear las posiciones de los items del final del panel
		/// </summary>
		private bool resetearFinal = false;
		#endregion
		
		#region propiedades
		public override bool Visible{
			get{				
				return base.Visible;
			}
			set{				
				foreach(GUIItemPanel i in items){
					if(i.item != null){
						i.Item.Visible = value;
					}
				}
				
				base.Visible = value;
			}
		}
		public List<GUIItemPanel> Items{
			get{return items;}	
		}
		public TipoScroll Scroll{
			get{return scroll;}	
		}
		#endregion
		
		#region metodos sobreescritos
		public override void inicializar(){						
			xMinimaRelativaScrollable = 1f - xMaximaRelativaScrollable;
			yMinimaRelativaScrollable = 1f - yMaximaRelativaScrollable;
			
			//si no se han adjuntado items al panel de forma manual
			if(items == null || items.Count == 0){
				GUIItemPanel[] itemsHijos = GetComponentsInChildren<GUIItemPanel>(); //obtenemos los hijos del panel que deben ser GUIItemPanel
				
				//adjuntamos esos hijos a los items
				foreach(GUIItemPanel i in itemsHijos){
					items.Add(i);	
				}
			}
			
			//primero inicializamos los items
			foreach(GUIItemPanel i in items){
				i.inicializar(scroll, this);
			}
			
			//ordenamos los items
			itemsOrdenados = new ArrayList(items);
			itemsOrdenados.Sort();
			primerItem = (GUIItemPanel) itemsOrdenados[0];
			ultimoItem = (GUIItemPanel) itemsOrdenados[itemsOrdenados.Count-1];
			
//				//horizontal: mantenemos anchura de la pantalla
//				if(scroll == TipoScroll.HORIZONTAL){
//					float alturaMax = float.NegativeInfinity;
//					
//					foreach(GUIItemPanel i in items){
//						//calculamos que item tiene la mayor altura
//						if(i.Item.altura > alturaMax){
//							alturaMax = i.Item.altura;	
//						}
//					}
//					
//					this.anchura = GUIEscalador.ANCHO_PANTALLA;
//					this.altura = alturaMax;
//				}
//				//vertical: mantenemos altura de la pantalla
//				else if(scroll == TipoScroll.VERTICAL){
//					float anchuraMax = float.NegativeInfinity;
//					
//					foreach(GUIItemPanel i in items){
//						//calculamos que item tiene la mayor altura
//						if(i.Item.anchura > anchuraMax){
//							anchuraMax = i.Item.anchura;	
//						}
//					}
//					
//					this.altura = GUIEscalador.ALTO_PANTALLA;
//				}	
			
			base.inicializar();
			
//				//cambiamos la coordenada Z al panel para que se quede detras de los colliders de los items que tenga
//				//para que se puedan detectar sin problemas los gestos sobre los items, de forma independiente a los gestos del panel
//				transform.position = new Vector3(transform.position.x, transform.position.y, 0.01f);
	
		}
		
		public override void actualizar (){
			foreach(GUIItemPanel i in items){
				i.actualizar(posicionRelativaA);
			}
		}
		
		public override void dibujar (){
			foreach(GUIItemPanel i in items){
				if(i.Item != null && i.Item.Visible){
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
			var gestoScroll = sender as PanGesture; //el gesto
			
			switch(e.State){
				//haciendo scroll
				case Gesture.GestureState.Changed:
					Vector2 desplazamiento = Vector2.zero; //el desplazamiento que realizara cada uno de los items
					xPreviaScroll = gestoScroll.LocalDeltaPosition.x; //vamos registrando la componente x del gesto de scroll
					yPreviaScroll = -gestoScroll.LocalDeltaPosition.y; //vamos registrando la componente y del gesto de scroll
				
					switch(scroll){
						case TipoScroll.HORIZONTAL:
							desplazamiento = desplazamientoHorizontal(); //calculamos el desplazamiento que tiene que realizar cada item en horizontal
						break;
						case TipoScroll.VERTICAL:
							desplazamiento = desplazamientoVertical(); //calculamos el desplazamiento que tiene que realizar cada item en vertical
						break;
					}	
				
					//si no hay que resetear posiciones ni del principio del panel ni del final
					//actualizamos las posiciones de todos los items
					if(!resetearPrincipio && !resetearFinal){
						actualizarPosiciones(desplazamiento);
					}

				break;
				
				//fin del scroll
				case Gesture.GestureState.Ended:
					//si no hay que resetear el principio pero si el final
					if(!resetearPrincipio && resetearFinal){
						resetearItemsDesdeElFinalPanel();
					}
					//si hay que resetear el principio y no el final
					else if(resetearPrincipio && !resetearFinal){
						resetearPosicionesItems();
					}
					//si no hay que resetear posiciones ni del principio del panel ni del final
					else if(!resetearPrincipio && !resetearFinal){
						gestionarFinScroll();
					}
				break;
			}
		}
		
		/// <summary>
		/// Calcula el desplazamiento que deben realizar los items del panel en horizontal
		/// </summary>
		/// <returns>
		/// El desplazamiento horizontal
		/// </returns>
		private Vector2 desplazamientoHorizontal(){
			Vector2 desplazamiento = Vector2.zero; //el desplazamiento que realizara cada uno de los items
			
			//condiciones basicas de reseteo de posiciones de los items del panel
			resetearPrincipio = primerItem.Item.posicionRelativaA.x > xMaximaRelativaScrollable;
			resetearFinal = ultimoItem.Item.posicionRelativaA.x < xMinimaRelativaScrollable;
			
			//caso en que se tenga que reiniciar el principio pero se ha cambiado la direccion del movimiento
			//por tanto no hay que resetearlo
			if(resetearPrincipio && xPreviaScroll < 0){
				resetearPrincipio = false;
			}	
	
			//caso en que se tenga que reiniciar el final pero se ha cambiado la direccion del movimiento
			//por tanto no hay que resetearlo
			else if(resetearFinal && xPreviaScroll > 0){
				resetearFinal = false;
			}	
			
			//si no hay que resetear nada se calcula el desplazamiento de los items
			if(!resetearPrincipio && !resetearFinal){
				desplazamiento = new Vector2(xPreviaScroll*velocidadScrollHorizontal, 0f);
			}
			
			return desplazamiento;
		}
		
		/// <summary>
		/// Calcula el desplazamiento que deben realizar los items del panel en vertical
		/// </summary>
		/// <returns>
		/// El desplazamiento vertical
		/// </returns>
		private Vector2 desplazamientoVertical(){
			Vector2 desplazamiento = Vector2.zero; //el desplazamiento que realizara cada uno de los items
			
			//condiciones basicas de reseteo de posiciones de los items del panel
			resetearPrincipio = primerItem.Item.posicionRelativaA.y > yMaximaRelativaScrollable;
			resetearFinal = ultimoItem.Item.posicionRelativaA.y < yMinimaRelativaScrollable;
			
			//caso en que se tenga que reiniciar el principio pero se ha cambiado la direccion del movimiento
			//por tanto no hay que resetearlo
			if(resetearPrincipio && yPreviaScroll < 0){
				resetearPrincipio = false;
			}	
	
			//caso en que se tenga que reiniciar el final pero se ha cambiado la direccion del movimiento
			//por tanto no hay que resetearlo
			else if(resetearFinal && yPreviaScroll > 0){
				resetearFinal = false;
			}	
			
			//si no hay que resetear nada se calcula el desplazamiento de los items
			if(!resetearPrincipio && !resetearFinal){
				desplazamiento = new Vector2(0f, yPreviaScroll*velocidadScrollVertical);
			}
			
			return desplazamiento;
		}
		
		/// <summary>
		/// Resetea las posiciones de los items desde el final del panel
		/// </summary>
		private void resetearItemsDesdeElFinalPanel(){
			Vector2 posFija = Vector2.zero; //posicion fija del margen donde deben juntarse los items
			Vector2 posRelativaAFija = Vector2.zero; //posicion relativa a esa posicion fija
			float desplazamientoFinal = 0f; //el desplazamiento que deben realizar los items para alinearse al margen correspondiente
			
			switch(scroll){
				case TipoScroll.HORIZONTAL: //se tienen que posicionar los elementos en la misma distribucion pero junto al margen derecho de la pantalla
					float xAnchuraPanel = this.posicionFija.x+this.anchura;
					posFija = new Vector2(xAnchuraPanel-ultimoItem.item.anchura-margenFinal, ultimoItem.Item.posicionFija.y); //calculamos la posicion del ultimo elemento
					posRelativaAFija = ultimoItem.item.posicionRelativaAlAnclaRespectoAPosicionFijaDada(posFija); //en funcion a esa posicion fija del ultimo elemento calculamos una relativa
					desplazamientoFinal = posRelativaAFija.x - ultimoItem.item.posicionRelativaA.x; //calculamos el desplazamiento que deberan realizar cada uno de los items
					actualizarPosiciones(new Vector2(desplazamientoFinal, 0f)); //realizamos el desplazamiento en horizontal
				break;
				
				case TipoScroll.VERTICAL:  //se tienen que posicionar los elementos en la misma distribucion pero junto al margen inferior de la pantalla
					float yAlturaPanel = this.posicionFija.y+this.altura;
					posFija = new Vector2(ultimoItem.Item.posicionFija.x, yAlturaPanel-ultimoItem.item.altura-margenFinal); //calculamos la posicion del ultimo elemento
					posRelativaAFija = ultimoItem.item.posicionRelativaAlAnclaRespectoAPosicionFijaDada(posFija); //en funcion a esa posicion fija del ultimo elemento calculamos una relativa
					desplazamientoFinal = posRelativaAFija.y - ultimoItem.item.posicionRelativaA.y; //calculamos el desplazamiento que deberan realizar cada uno de los items
					actualizarPosiciones(new Vector2(0f, desplazamientoFinal)); //realizamos el desplazamiento en vertical
				break;
			}
			
//			Debug.Log(desplazamientoFinal);
		}
		
		private void gestionarFinScroll(){
			switch(scroll){
				case TipoScroll.HORIZONTAL:
					float xAnchuraPanel = this.posicionFija.x+this.anchura;
					//comprobamos si estan en pantalla el primer y ultimo item
					primerItenEnPantalla = ((primerItem.item.posicionFija.x >= this.posicionFija.x)) && (primerItem.item.posicionFija.x + primerItem.item.anchura < xAnchuraPanel);
					ultimoItemEnPantalla = ((ultimoItem.item.posicionFija.x >= this.posicionFija.x)) && (ultimoItem.item.posicionFija.x + ultimoItem.item.anchura < xAnchuraPanel);
					
					
					resetearPrincipio = (xPreviaScroll > 0 && primerItem.item.posicionRelativaA.x > xMaximaRelativaScrollable)
					|| (xPreviaScroll > 0 && primerItenEnPantalla && !ultimoItemEnPantalla)
					|| (xPreviaScroll <= 0 && primerItenEnPantalla && !ultimoItemEnPantalla); //condicion: cambiar movimiento a la izquierda despues de haber hecho uno a la derecha previamente
				
					resetearFinal = (xPreviaScroll < 0 && ultimoItem.item.posicionRelativaA.x < xMinimaRelativaScrollable)
						|| (xPreviaScroll < 0 && ultimoItemEnPantalla && !primerItenEnPantalla)
						|| (xPreviaScroll >= 0 && ultimoItemEnPantalla && !primerItenEnPantalla);//condicion: cambiar movimiento a la derecha despues de haber hecho uno a la izquierda previamente
				
				
					//----
				 	// reseteamos posiciones de los items en el principio del panel
					// si el movimiento ultimo del dedo era hacia la derecha
					//---
					if(resetearPrincipio) {
						resetearPosicionesItems();
					}
					//----
				 	// reseteamos posiciones de los items en el final del panel
					// si el movimiento del dedo hacia la izquierda
					//---
					else if(resetearFinal) {
						resetearItemsDesdeElFinalPanel();
					}
					//----
					// no hay que resetear nada
					//---
					else{
//						Vector2 desplazamiento = new Vector2(xPreviaScroll*5f, 0f);
//						actualizarPosiciones(desplazamiento);
//						if(){
//						}
					}
				break;
				
				case TipoScroll.VERTICAL:
					float yAlturaPanel = this.posicionFija.y+this.altura;
				
					//comprobamos si estan en pantalla el primer y ultimo item
					primerItenEnPantalla = ((primerItem.item.posicionFija.y >= this.posicionFija.y)) && (primerItem.item.posicionFija.y + primerItem.item.altura < yAlturaPanel);
					ultimoItemEnPantalla = ((ultimoItem.item.posicionFija.y >= this.posicionFija.y)) && (ultimoItem.item.posicionFija.y + ultimoItem.item.altura < yAlturaPanel);
					
			
					//----
				 	// reseteamos posiciones de los items en el principio del panel
					// si el movimiento ultimo del dedo era hacia abajo
					//---
					if((yPreviaScroll > 0 && primerItem.item.posicionRelativaA.y > yMaximaRelativaScrollable)
					|| (yPreviaScroll > 0 && primerItenEnPantalla && !ultimoItemEnPantalla)
					|| (yPreviaScroll <= 0 && primerItenEnPantalla && !ultimoItemEnPantalla)) //condicion: cambiar movimiento hacia arriba despues de haber hecho uno a la derecha previamente
					{
						resetearPosicionesItems();
					}
					
					//----
				 	// reseteamos posiciones de los items en el final del panel
					// si el movimiento del dedo hacia arriba
					else if((yPreviaScroll < 0 && ultimoItem.item.posicionRelativaA.y < yMinimaRelativaScrollable)
						|| (yPreviaScroll < 0 && ultimoItemEnPantalla && !primerItenEnPantalla)
						|| (yPreviaScroll >= 0 && ultimoItemEnPantalla && !primerItenEnPantalla)) //condicion: cambiar movimiento hacia abajo despues de haber hecho uno a la izquierda previamente
					{
						resetearItemsDesdeElFinalPanel();
					}
				break;
			}
		}
		
		public void resetearPosicionesItems(){
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