using UnityEngine;
using System.Collections;
using GUIMultiresolucion.Core;

namespace GUIMultiresolucion.GUIComponentes{
	/// <summary>
	/// Autor: Fran Garcia <www.fgarmo.com>
	/// 
	/// Representa una imagen en la gui que tiene una textura
	/// </summary>
	[System.Serializable]
	public class GUIImagen:GUIComponente {
	    public Texture textura;
		
	    public Rect distribucion{
	        get{
				//cambiamos las dimensiones si la anchura/altura del componente es 0
				if(this.anchura == 0 && textura.width != null){
		        	this.anchura =  textura.width;
				}
				
				if(this.altura == 0 && textura.height != null){
					this.altura = textura.height;	
				}
				
				return base.distribucion;
	        }
	    }
	}
}