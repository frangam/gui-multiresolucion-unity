using UnityEngine;
using System;
using System.Collections;
using GUIMultiresolucion.Core;

namespace GUIMultiresolucion.GUIComponentes{
	public class GUIVentanaJerarquizada : GUIVentana, IComparable {
		#region atributos de configuracion		
		/// <summary>
		/// Diferente de 0 si la ventana pertecene a una multiventana, sirve para ordenar las ventanas de una multiventana
		/// </summary>
		public int ordenEnMultiventana = 0;
		#endregion
		
		#region privados
		public void inicializar(GUIMultiVentana multiventana, bool ventanaActiva = false){
			base.inicializar(multiventana.imgCabecera.posicionFija.y, multiventana.imgCabecera.altura, multiventana.imgPie.posicionFija.y);
			
			if(ventanaActiva){
				base.inicializar();	
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