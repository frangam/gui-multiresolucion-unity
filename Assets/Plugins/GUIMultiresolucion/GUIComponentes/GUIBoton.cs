using UnityEngine;
using System.Collections;
using System;
using GUIMultiresolucion.Core;

namespace GUIMultiresolucion.GUIComponentes{
	/// <summary>
	/// Autor: Fran Garcia <www.fgarmo.com>
	/// 
	/// Representa un boton de la gui
	/// </summary>
	[System.Serializable]
	public class GUIBoton: GUIComponente{
	        
		public Texture texturaNormal;
		public Texture texturaPulsado;
		
		public Texture texturaDibujar;
	
	    public Rect distribucion{
	        get{
	           
	           	//cambiamos las dimensiones si la anchura/altura del componente es 0
				if(this.anchura == 0 && texturaNormal.width != null){
		        	this.anchura =  texturaNormal.width;
				}
				
				if(this.altura == 0 && texturaNormal.height != null){
					this.altura = texturaNormal.height;	
				}
	            
				
				return base.distribucion;
	        }
	    }
		
		public BoxCollider collider;
		
		
		
		public void inicializar(){	
			//cambiamos las dimensiones si la anchura/altura del componente es 0
			if(this.anchura == 0 && texturaNormal.width != null){
	        	this.anchura =  texturaNormal.width;
			}
			
			if(this.altura == 0 && texturaNormal.height != null){
				this.altura = texturaNormal.height;	
			}
			
		}		
		
		

	}
}