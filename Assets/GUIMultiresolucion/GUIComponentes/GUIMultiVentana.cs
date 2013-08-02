using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using GUIMultiresolucion.Core;

namespace GUIMultiresolucion.GUIComponentes{
	/*
	 * Conjunto de ventanas
	 */ 
	[System.Serializable]
	public class GUIMultiVentana : GUIVentana {
		#region atributos de configuracion
		/// <summary>
		/// Las ventanas que forman el conjunto
		/// </summary>
		public List<GUIVentanaJerarquizada> ventanas;
		
		/// <summary>
		/// Boton para navegar hacia atras en la jerarquia de ventanas de una multiventana
		/// </summary>
		public GUIBoton botonAtras;
		
		/// <summary>
		/// Boton para navegar hacia delante en la jerarquia de ventanas de una multiventana
		/// </summary>
		public GUIBoton botonDelante;
		
		/// <summary>
		/// La ventana que esta activa, que se esta mostrando en el momento actual
		/// </summary>
		public GUIVentanaJerarquizada ventanaActiva;
			
		#endregion
		
		#region atributos privados
		/// <summary>
		/// Las ventanas ordenadas segun el orden que ocupan en la jerarquia
		/// </summary>
		private ArrayList ventanasOrdenadas;
		#endregion
		
		#region propiedades publicas
		public int totalVentanas(){
			return ventanas.Count;	
		}
		#endregion
		
		#region metodos sobreescritos
		public override bool Visible{
			get{			
				return base.Visible;
			}	
			set{	
				foreach(GUIVentanaJerarquizada vj in ventanas){
					if(vj != null && value == false){
						vj.Visible = false;
					}
				}
				
				if(value){
					ventanaActiva.abrirVentana(); //abrimos la ventana activa
					inicializarBotonesNavegacion(); //inicializamos los botones de navegacion entre ventanas	
				}
				
				base.Visible = value;
			}
		}		
		
		public override void inicializar (){	
			ventanaActiva = null;
			
			//cambiamos la coordenada Z a la multiventana para que se quede detras de los colliders de los items que tenga
			//para que se puedan detectar sin problemas los gestos sobre los items, de forma independiente a los gestos de los items
			transform.localPosition = new Vector3(transform.localPosition.x, transform.localPosition.y, -0.1f);		
			
			//si no se han adjuntado las ventanas de forma manual
			if(ventanas == null || ventanas.Count == 0){
				GUIVentanaJerarquizada[] ventanasHijos = transform.parent.GetComponentsInChildren<GUIVentanaJerarquizada>(); //obtenemos los hijos de la multiventana que deben ser GUIVentanaJerarquizada
				
				//adjuntamos esos hijos a los items
				foreach(GUIVentanaJerarquizada v in ventanasHijos){
					ventanas.Add(v);	
				}
			}
			
			ventanasOrdenadas = new ArrayList(ventanas);
			ventanasOrdenadas.Sort(); //ordenamos las ventanas
			
			ventanaActiva = (GUIVentanaJerarquizada) ventanasOrdenadas[0]; //obtenemos la primera ventana de la jerarquia
			
			//inicializar todas las ventanas que no son la primera a mostrar
			for(int i=1; i<totalVentanas(); i++){
				((GUIVentanaJerarquizada) ventanasOrdenadas[i]).inicializar(this, false);
				((GUIVentanaJerarquizada) ventanasOrdenadas[i]).Visible = false; //ocultamos la ventana
			}
			
			ventanaActiva.inicializar(this, true); //inicializamos la ventana activa
			
			
			
			botonAtras.Visible = false;
			botonDelante.Visible = false;
			
			
			
			
			base.inicializar();
		}
		
		public override void dibujar (){	
			if(imgFondo.Visible){
				imgFondo.dibujar();
			}	
			
			//por ultimo, dibujamos la ventana activa
			if(ventanaActiva != null){
				ventanaActiva.dibujar(); 
			}
			
			
			if(imgCabecera.Visible){
				imgCabecera.dibujar();	
			}
			
			if(botonCerrar.Visible){
				botonCerrar.dibujar();	
			}
			
			if(imgPie.Visible){
				imgPie.dibujar();
			}
			
			//--
			//dibujamos botones de navegacion
			//--
			
			if(botonAtras.Visible){
				botonAtras.dibujar();	
			}
			
			if(botonDelante.Visible){
				botonDelante.dibujar();	
			}
		}
		#endregion
		
		#region metodos privados
		/// <summary>
		/// Inicializa los botones de navegacion entre las ventanas
		/// </summary>
		private void inicializarBotonesNavegacion(){
			//---
			//condiciones para que la ventana posea botones atras y hacia delante
			//---
			
//			if(ventanaActiva.ordenEnMultiventana == 0 && ventanaActiva.ordenEnMultiventana == totalVentanas()-1){
//				botonAtras.Visible = false;
//				botonDelante.Visible = false;
//			}
//			else if(ventanaActiva.ordenEnMultiventana == 0 && ventanaActiva.ordenEnMultiventana < totalVentanas()-1){
			if(ventanaActiva.ordenEnMultiventana == 0 && ventanaActiva.ordenEnMultiventana < totalVentanas()-1){
				botonAtras.Visible = false;
				
				if(!botonDelante.Visible){
					botonDelante.Visible = true;
					botonDelante.inicializar(this);
				}
			}	
			else if(ventanaActiva.ordenEnMultiventana > 0 && ventanaActiva.ordenEnMultiventana == totalVentanas()-1){
				if(!botonAtras.Visible){
					botonAtras.Visible = true;
					botonAtras.inicializar(this);
				}
				botonDelante.Visible = false;
			}
			else if(ventanaActiva.ordenEnMultiventana > 0 && ventanaActiva.ordenEnMultiventana < totalVentanas()-1){
				if(!botonAtras.Visible){
					botonAtras.Visible = true;
					botonAtras.inicializar(this);	
				}
				
				if(!botonDelante.Visible){
					botonDelante.Visible = true;
					botonDelante.inicializar(this);
				}
			}	
		}
		
		public void abrirVentana(int indiceVentana){
			//si la ventana anterior es una ventana valida de la jerarquia
			if(indiceVentana >=0 && indiceVentana < totalVentanas()){
				GUIVentanaJerarquizada ventana = (GUIVentanaJerarquizada) ventanasOrdenadas[indiceVentana]; //obtenemos el objeto ventana siguiente
				
				if(ventanaActiva != ventana){
					ventanaActiva.cerrarVentana(); //primero, cerramos la ventana activa
					ventanaActiva = ventana; //cambiamos la ventana activa por la ventana que queremos abrir
					ventanaActiva.inicializar(this, true); //inicializamos la ventana activa
					ventanaActiva.abrirVentana(); //abrimos la ventana activa
					inicializarBotonesNavegacion(); //inicializamos los botones de navegacion
				}
			}
			else{
				Debug.Log("Ventana con indice "+ indiceVentana+" fuera de rango");	
			}
			
			
		}
		#endregion
		
		#region metodos publicos
		public override void cerrarVentana ()
		{
			ventanaActiva.cerrarVentana(); //cerrar ventana activa
			
			//cerrar botones de navegacion
			botonAtras.Visible = false;
			botonDelante.Visible = false;
			
			base.cerrarVentana ();
		}
		
		public void abrirVentanaSiguiente(){
			abrirVentana(ventanaActiva.ordenEnMultiventana + 1);	
		}
		
		public void abrirVentanaAnterior(){
			abrirVentana(ventanaActiva.ordenEnMultiventana - 1);
		}
		#endregion
	}
}
