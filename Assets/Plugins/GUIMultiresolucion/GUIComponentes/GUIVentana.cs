using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using GUIMultiresolucion.Core;
using GUIMultiresolucion.GUIComponentes.Paneles;

namespace GUIMultiresolucion.GUIComponentes{
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
		
		#endregion
		
		#region metodos sobreescritos
		public override void inicializar (){
			//cambiamos la coordenada Z a la ventana para que se quede detras de los colliders de los items que tenga
			//para que se puedan detectar sin problemas los gestos sobre los items, de forma independiente a los gestos de los items
			transform.localPosition = new Vector3(transform.localPosition.x, transform.localPosition.y, 0.1f);
			
						
			if(imgFondo != null){
				items.Add(imgFondo);
				imgFondo.transform.position = new Vector3(imgFondo.transform.position.x, imgFondo.transform.position.y, 0.1f);
			}
			if(imgCabecera != null){
				items.Add(imgCabecera);
				imgCabecera.transform.position = new Vector3(imgCabecera.transform.position.x, imgCabecera.transform.position.y, 0.1f);
			}
			if(imgPie != null){
				items.Add(imgPie);
				imgPie.transform.position = new Vector3(imgPie.transform.position.x, imgPie.transform.position.y, 0.1f);
			}
			if(botonCerrar != null){
				items.Add(botonCerrar);
			}
	
			//inicializamos los items
			foreach(GUIComponente c in items){
				c.inicializar();
			}
			
			//inicializamos el panel
			if(panelScrollable != null){
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
				
//				Debug.Log("ancho pantalla: "+Screen.width+", altura pantalla: "+Screen.height);
//				Debug.Log("ancho pantalla escalada: "+GUIEscalador.ANCHO_PANTALLA+", altura pantalla escalada: "+GUIEscalador.ALTO_PANTALLA);
//				Debug.Log("anchura: "+panelScrollable.anchura+", altura: "+panelScrollable.altura);
			}
			
			
			base.inicializar ();
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
			foreach(GUIComponente c in items){
				c.dibujar();	
			}
		}
		#endregion
		
		#region eventos ventana
		public void cerrarVentana(){
			Visible = false;	
		}
		#endregion
		
		#region Unity
		public void LateUpdate(){
			if(botonCerrar!= null && botonCerrar.EjecutarAccionEstandar){
				botonCerrar.EjecutarAccionEstandar = false; //actualizar bandera
				cerrarVentana(); //cerramos la ventana
			}
		}
		#endregion
	}
}