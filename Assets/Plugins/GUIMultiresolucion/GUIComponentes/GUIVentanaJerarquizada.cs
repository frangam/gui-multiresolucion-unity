using UnityEngine;
using System;
using System.Collections;
using GUIMultiresolucion.Core;

namespace GUIMultiresolucion.GUIComponentes{
	public class GUIVentanaJerarquizada : GUIVentana, IComparable {
		#region atributos de configuracion
		/// <summary>
		/// Boton para navegar hacia atras en la jerarquia de ventanas de una multiventana
		/// </summary>
		public GUIBoton botonAtras;
		
		/// <summary>
		/// Boton para navegar hacia delante en la jerarquia de ventanas de una multiventana
		/// </summary>
		public GUIBoton botonDelante;
		
		/// <summary>
		/// Diferente de 0 si la ventana pertecene a una multiventana, sirve para ordenar las ventanas de una multiventana
		/// </summary>
		public int ordenEnMultiventana = 0;
		#endregion
		
		#region metodos sobreescritos
		public override void resetear(){
			botonCerrar = null;
			imgCabecera = null;
			imgPie = null;
			imgFondo = null;
			botonAtras = null;
			botonDelante = null;
		}
		
		public void inicializar (GUIMultiVentana multiventana)
		{			
			//---
			//condiciones para que la ventana posea botones atras y hacia delante
			//---
			
			if(ordenEnMultiventana == 0 && ordenEnMultiventana == multiventana.totalVentanas()-1){
				botonAtras = null;
				botonDelante = null;
			}
			else if(ordenEnMultiventana == 0 && ordenEnMultiventana < multiventana.totalVentanas()-1){
				botonAtras = null;
				botonDelante = multiventana.ventanaComun.botonDelante;
			}	
			else if(ordenEnMultiventana > 0 && ordenEnMultiventana == multiventana.totalVentanas()-1){
				botonAtras = multiventana.ventanaComun.botonAtras;
				botonDelante = null;
			}
			else if(ordenEnMultiventana > 0 && ordenEnMultiventana < multiventana.totalVentanas()-1){
				botonAtras = multiventana.ventanaComun.botonAtras;
				botonDelante = multiventana.ventanaComun.botonDelante;
			}
			
			//adjuntamos los elementos basicos de la ventana, tomando los que tiene la ventana comun
			this.imgCabecera = multiventana.ventanaComun.imgCabecera;
			this.botonCerrar = multiventana.ventanaComun.botonCerrar;
			this.imgFondo = multiventana.ventanaComun.imgFondo;
			this.imgPie = multiventana.ventanaComun.imgPie;			
			
			base.inicializar ();
		}
		
		public override void dibujar ()
		{
			//primero dibujamos la ventana	
			base.dibujar ();
			
			//despues botones
			if(botonAtras != null){
				botonAtras.dibujar();	
			}
			
			if(botonDelante != null){
				botonDelante.dibujar();
			}
		}
		#endregion
		
		#region implementacion del IComparable
		
		/// <summary>
		/// Ordena las ventanas de una jerarquia de ventanas segun el atributo ordenEnMultiventana
		/// </summary>
		/// <returns>
		/// -1 si this ocupa un orden inferior  que otraVentana en la jerarquia de ventanas de la multiventana, +1 si this ocupa un orden mayor que otraVentana en la jerarquia de ventanas, 0 si ocupan el mismo orden
		/// componente es mayor.
		/// </returns>
		/// <param name='otraVentana'>
		/// La otra ventana de la jerarquia
		/// </param>
		public int CompareTo(System.Object otraVentana){
			GUIVentanaJerarquizada aux = (GUIVentanaJerarquizada) otraVentana;
			
			return this.ordenEnMultiventana.CompareTo(aux.ordenEnMultiventana);
		}
		
		#endregion		
	}
}