using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using GUIMultiresolucion.Core;

namespace GUIMultiresolucion.GUIComponentes{
	[System.Serializable]
	public class GUIVentana : GUIComponente {
		#region atributos de configuracion
		/// <summary>
		/// Textura para el fondo de la ventana.
		/// </summary>
	 	public Texture texturaFondo;
		
		/// <summary>
		/// Textura para la cabecera de la ventana
		/// </summary>
		public Texture texturaCabecera;
		
		/// <summary>
		/// Textura para el pie de la ventana
		/// </summary>
		public Texture texturaPie;
		
		/// <summary>
		/// Componentes gui que estan dentro de la ventana
		/// </summary>
		public List<GUIComponente> items;
		
		#endregion
		
		#region atributos privados
		/// <summary>
		/// Los GUIComponetes ordenados segun el criterio implementado en GUIComponente
		/// </summary>
		private ArrayList itemsOrdenados;
		#endregion
		
		#region metodos sobreescritos
		public override void inicializar ()
		{
			itemsOrdenados = new ArrayList(items);
			itemsOrdenados.Sort(); //ordenamos los items por el criterio indicado en el compareTo de GUIComponente
			
			//inicializamos los items
			foreach(GUIComponente c in items){
				c.inicializar();
			}
			
			base.inicializar ();
		}
		public override bool Visible{
			get{
				foreach(GUIComponente c in items){
					c.Visible = visible;	
				}
				
				return base.Visible;
			}	
			set{
				foreach(GUIComponente c in items){
					c.Visible = visible;	
				}
				
				base.Visible = value;
			}
		}
		public override void dibujar (){
			if(texturaCabecera){
				GUI.DrawTexture(distribucion, texturaCabecera);
			}
			
			//por ultimo dibujamos los componentes haciendo uso de los items ordenados, para dibujarlos en el orden correcto
			foreach(GUIComponente c in itemsOrdenados){
				c.dibujar();	
			}
		}
		#endregion
	}
}