using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using GUIMultiresolucion.Core;
using GUIMultiresolucion.GUIComponentes.Paneles;

namespace GUIMultiresolucion.GUIComponentes{
	[System.Serializable]
	public class GUIVentana : GUIComponente {
		#region atributos de configuracion
		/// <summary>
		/// Imagen para el fondo de la ventana.
		/// </summary>
	 	public GUIImagen imgFondo;
		
		/// <summary>
		/// Imagen para la cabecera de la ventana
		/// </summary>
		public GUIImagen imgCabecera;
		
		/// <summary>
		/// Imagen para el pie de la ventana
		/// </summary>
		public GUIImagen imgPie;
		
		/// <summary>
		/// Boton para cerrar ventana
		/// </summary>
		public GUIBoton botonCerrar;
		
		public GUIPanel panelScrollable;		
		
		#endregion
		
		#region atributos privados
		/// <summary>
		/// Componentes gui que estan dentro de la ventana
		/// </summary>
		public List<GUIComponente> items;
		
		public ArrayList itemsOrdenados;
		
		#endregion
		
		
		#region metodos sobreescritos		
		public override void inicializar (){
			//cambiamos la coordenada Z a la ventana para que se quede detras de los colliders de los items que tenga
			//para que se puedan detectar sin problemas los gestos sobre los items, de forma independiente a los gestos de los items
			transform.localPosition = new Vector3(transform.localPosition.x, transform.localPosition.y, -0.1f);
			
			items = new List<GUIComponente>(); //instanciamos la lista de items
			
			if(imgFondo != null){
				imgFondo.profundidad = 5;
				items.Add(imgFondo);
				imgFondo.transform.position = new Vector3(imgFondo.transform.position.x, imgFondo.transform.position.y, 0.1f);
			}
			if(imgCabecera != null){
				imgCabecera.profundidad = 0;
				items.Add(imgCabecera);
				imgCabecera.transform.position = new Vector3(imgCabecera.transform.position.x, imgCabecera.transform.position.y, 0.1f);
			}
			if(botonCerrar != null){
				botonCerrar.profundidad = -5;
				items.Add(botonCerrar);
			}
			if(imgPie != null){
				imgPie.profundidad = 0;
				items.Add(imgPie);
				imgPie.transform.position = new Vector3(imgPie.transform.position.x, imgPie.transform.position.y, 0.1f);
			}
	
			//inicializamos los items
			foreach(GUIComponente c in items){
				c.inicializar();
			}
			
			//inicializamos el panel
			if(panelScrollable != null){
				panelScrollable.profundidad = 3;
				//posicion en pixeles que debe tener el panel
				Vector2 posEnPixeles = new Vector2(0f, imgCabecera.posicionFija.y+imgCabecera.altura); 
				
				//primero lo escalamos
				float alturaPanel = GUIEscalador.ALTO_PANTALLA - panelScrollable.posicionFija.y;
				
				if(imgPie != null){
					alturaPanel = (imgPie.posicionFija.y) - posEnPixeles.y;
				}
				
				panelScrollable.altura = alturaPanel;
				panelScrollable.anchura = GUIEscalador.ANCHO_PANTALLA;
				
				if(imgCabecera != null){
					panelScrollable.posicionRelativaA.y = panelScrollable.posicionRelativaAlAnclaRespectoAPosicionFijaDada(posEnPixeles,TipoAnclado.SUPERIOR_IZQUIERDA).y; //calculamos la pos relativa que le corresponde
				}

				//por ultimo inicializamos el panel
				items.Add(panelScrollable);
				panelScrollable.inicializar();
				
				//ordenamos los items para pintarlos en pantalla segun la profundidad
				itemsOrdenados = new ArrayList(items);
				itemsOrdenados.Sort();
				
//				Debug.Log("ancho pantalla: "+Screen.width+", altura pantalla: "+Screen.height);
//				Debug.Log("ancho pantalla escalada: "+GUIEscalador.ANCHO_PANTALLA+", altura pantalla escalada: "+GUIEscalador.ALTO_PANTALLA);
//				Debug.Log("anchura: "+panelScrollable.anchura+", altura: "+panelScrollable.altura);
			}
			
			
			base.inicializar ();
			
			this.Visible = true;
		}
		public override bool Visible{
			get{			
				return base.Visible;
			}	
			set{				
				foreach(GUIComponente c in items){
					c.Visible = value;	
				}
				
				base.Visible = value;
			}
		}
		public override void dibujar (){
			//por ultimo dibujamos los componentes haciendo uso de los items ordenados, para dibujarlos en el orden correcto
			foreach(GUIComponente c in itemsOrdenados){
				c.dibujar();	
			}
		}
		#endregion
		
		#region eventos ventana
		public void cerrarVentana(){
			resetearVentana(); //primero reseteamos todos los componentes de la ventana
			
			this.Visible = false;	//por ultimo la ocultamos
		}
		
		public void resetearVentana(){
			if(panelScrollable != null){
				panelScrollable.resetearPosicionesItems(); //reseteamos el panel	
			}
		}
		#endregion
		
		#region Unity
		public void LateUpdate(){
			//boton cerrar pulsado
			if(botonCerrar != null && botonCerrar.EjecutarAccionEstandar){
				botonCerrar.EjecutarAccionEstandar = false; //actualizar bandera
				cerrarVentana(); //cerramos la ventana
				Debug.Log(botonCerrar.tipo);
				Debug.Log("cerrando ventana");
			}
		}
		#endregion
	}
}