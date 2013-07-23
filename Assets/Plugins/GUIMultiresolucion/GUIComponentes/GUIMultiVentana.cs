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
		public override void inicializar (){	
			ventanaActiva = null;
			
			//cambiamos la coordenada Z a la multiventana para que se quede detras de los colliders de los items que tenga
			//para que se puedan detectar sin problemas los gestos sobre los items, de forma independiente a los gestos de los items
			transform.localPosition = new Vector3(transform.localPosition.x, transform.localPosition.y, -0.1f);		
			
			ventanasOrdenadas = new ArrayList(ventanas);
			ventanasOrdenadas.Sort(); //ordenamos las ventanas
			
			ventanaActiva = (GUIVentanaJerarquizada) ventanasOrdenadas[0]; //obtenemos la primera ventana de la jerarquia
			
			//inicializar todas las ventanas que no son la primera a mostrar
			for(int i=1; i<totalVentanas(); i++){
				((GUIVentanaJerarquizada) ventanasOrdenadas[i]).inicializar(this);
				((GUIVentanaJerarquizada) ventanasOrdenadas[i]).Visible = false; //ocultamos la ventana
			}
			
			ventanaActiva.inicializar(this); //inicializamos la ventana activa
			ventanaActiva.abrirVentana(); //abrimos la ventana activa
			inicializarBotonesNavegacion(); //inicializamos los botones de navegacion entre ventanas	
			
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
			
			if(ventanaActiva.ordenEnMultiventana == 0 && ventanaActiva.ordenEnMultiventana == totalVentanas()-1){
				botonAtras.Visible = false;
				botonDelante.Visible = false;
			}
			else if(ventanaActiva.ordenEnMultiventana == 0 && ventanaActiva.ordenEnMultiventana < totalVentanas()-1){
				botonAtras.Visible = false;
				
				if(!botonDelante.Visible){
					botonDelante.Visible = true;
					botonDelante.inicializar();	
				}
			}	
			else if(ventanaActiva.ordenEnMultiventana > 0 && ventanaActiva.ordenEnMultiventana == totalVentanas()-1){
				if(!botonAtras.Visible){
					botonAtras.Visible = true;
					botonAtras.inicializar();	
				}
				botonDelante.Visible = false;
			}
			else if(ventanaActiva.ordenEnMultiventana > 0 && ventanaActiva.ordenEnMultiventana < totalVentanas()-1){
				if(!botonAtras.Visible){
					botonAtras.Visible = true;
					botonAtras.inicializar();	
				}
				
				if(!botonDelante.Visible){
					botonDelante.Visible = true;
					botonDelante.inicializar();	
				}
			}	
		}
		
		private void abrirVentana(GUIVentanaJerarquizada ventana){
			if(ventanaActiva != ventana){
				ventanaActiva.cerrarVentana();
				ventanaActiva = ventana; //cambiamos la ventana activa
				ventanaActiva.inicializar(this); //inicializamos la ventana activa
				ventanaActiva.abrirVentana(); //abrimos la ventana
				inicializarBotonesNavegacion(); //inicializamos los botones de navegacion
			}
		}
		#endregion
		
		#region Unity
		public void LateUpdate(){
			base.LateUpdate();
			
			if(ventanaActiva != null){
				//boton atras pulsado
				if(botonAtras != null && botonAtras.gameObject.activeSelf && botonAtras.EjecutarAccionEstandar){
					Debug.Log(botonAtras.tipo);
					Debug.Log("ir a ventana anterior");
					
					botonAtras.EjecutarAccionEstandar = false; //actualizar bandera
					int indiceVentanaAnterior = ventanaActiva.ordenEnMultiventana-1;
					
					//si la ventana anterior es una ventana valida de la jerarquia
					if(indiceVentanaAnterior >=0 && indiceVentanaAnterior <= totalVentanas()){
						GUIVentanaJerarquizada ventanaAnterior = (GUIVentanaJerarquizada) ventanasOrdenadas[indiceVentanaAnterior]; //obtenemos el objeto ventana anterior
						abrirVentana(ventanaAnterior); //abrimos la ventana
					}
					else{
						Debug.Log("Ventana anterior fuera de rango");	
					}
					

				}
				
				//boton delante pulsado
				if(botonDelante != null && botonDelante.gameObject.activeSelf && botonDelante.EjecutarAccionEstandar){
					Debug.Log(botonDelante.tipo);
					Debug.Log("ir a ventana siguiente");
					
					botonDelante.EjecutarAccionEstandar = false; //actualizar bandera
					int indiceVentanaSiguiente = ventanaActiva.ordenEnMultiventana+1;
					
					//si la ventana anterior es una ventana valida de la jerarquia
					if(indiceVentanaSiguiente >=0 && indiceVentanaSiguiente < totalVentanas()){
						GUIVentanaJerarquizada vetanaSiguiente = (GUIVentanaJerarquizada) ventanasOrdenadas[indiceVentanaSiguiente]; //obtenemos el objeto ventana siguiente
						abrirVentana(vetanaSiguiente); //abrimos la ventana
					}
					else{
						Debug.Log("Ventana siguiente fuera de rango");	
					}
					
					
				}
			}
		}
		#endregion
	}
}
