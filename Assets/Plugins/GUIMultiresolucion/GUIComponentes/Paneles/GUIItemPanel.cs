using UnityEngine;
using System.Collections;
using GUIMultiresolucion.Core;
using TouchScript.Gestures;

namespace GUIMultiresolucion.GUIComponentes.Paneles{
	/*
	 * Representa un elemento que forma parte de un panel
	 */ 
	[System.Serializable]
	[ExecuteInEditMode]
	public class GUIItemPanel :  MonoBehaviour{
		/// <summary>
		/// El GUIComponente que representa al item adjuntado al panel
		/// </summary>
		public GUIComponente item;
		
		private Vector2 posicionInicial;
		
		#region propiedades
		public GUIComponente Item{
			get{return item;}	
		}
		#endregion
		
		#region nuevos metodos
		public void inicializar(TipoScroll _scroll, GUIPanel panel){
			item.inicializar(panel);
		}
		
		public void actualizar(Vector2 posRelativa){
			item.posicionRelativaA += posRelativa;
			item.actualizar();
		}

		public void dibujar ()
		{
			item.dibujar();
		}
		#endregion
	}
}
