using UnityEngine;
using System.Collections;
using GUIMultiresolucion.Core;

namespace GUIMultiresolucion.GUIComponentes{
	
	public class GUILabel : GUIComponente {
	
		public Font fuente;
		public string texto;
		
		/// <summary>
		/// La textura del label que se crea a partir del texto que se quiere mostrar
		/// y de un bitmap font
		/// </summary>
		private Texture2D texturaLabel;
		
		#region metodos sobreescritos
		public override void dibujar ()
		{
			if(texto != null || texto != "" && texturaLabel != null){
				GUI.DrawTexture(distribucion, texturaLabel);
			}
		}
		#endregion
	}
}