using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using GUIMultiresolucion.Core;

namespace GUIMultiresolucion.GUIComponentes{
	/*
	 * Conjunto de ventanas
	 */ 
	[System.Serializable]
	public class GUIMultiVentana : GUIComponente {
		#region atributos de configuracion
		/// <summary>
		/// Las ventanas que forman el conjunto
		/// </summary>
		public List<GUIVentanaJerarquizada> ventanas;
		
		/// <summary>
		/// Ventana comun para todas las demas
		/// </summary>
		public GUIVentanaJerarquizada ventanaComun;
		
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
			//cambiamos la coordenada Z a la multiventana para que se quede detras de los colliders de los items que tenga
			//para que se puedan detectar sin problemas los gestos sobre los items, de forma independiente a los gestos de los items
			transform.localPosition = new Vector3(transform.localPosition.x, transform.localPosition.y, -0.1f);
			
			ventanaComun.transform.localScale = new Vector3(0, 0f, 0f);
			ventanaComun.botonAtras.inicializar();
			ventanaComun.botonDelante.inicializar();
			
			
			ventanasOrdenadas = new ArrayList(ventanas);
			ventanasOrdenadas.Sort(); //ordenamos las ventanas
			ventanaActiva = (GUIVentanaJerarquizada) ventanasOrdenadas[0]; //obtenemos la primera ventana de la jerarquia
			ventanaActiva.inicializar(this); //inicializamos la ventana activa
			
			base.inicializar ();
		}
		
		public override void dibujar (){
			if(ventanaActiva != null){
				ventanaActiva.dibujar(); //solo dibujamos la ventana activa
			}
		}
		#endregion
		
		#region metodos privados
		private void abrirVentana(GUIVentanaJerarquizada ventana){
			if(ventanaActiva != ventana){
				ventanaActiva = ventana;	
			}
		}
		#endregion
		
		#region Unity
		public void LateUpdate(){
			if(ventanaActiva != null){
				//boton atras pulsado
				if(ventanaActiva.botonAtras != null && ventanaActiva.botonAtras.EjecutarAccionEstandar){
					ventanaActiva.botonAtras.EjecutarAccionEstandar = false; //actualizar bandera
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
				if(ventanaActiva.botonDelante != null && ventanaActiva.botonDelante.EjecutarAccionEstandar){
					ventanaActiva.botonDelante.EjecutarAccionEstandar = false; //actualizar bandera
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
