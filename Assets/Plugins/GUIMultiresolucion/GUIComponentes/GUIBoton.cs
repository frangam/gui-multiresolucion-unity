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
		
		private Texture texturaDibujar;
	
		#region propiedades publicas
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
		
		public Texture TexturaDibujar{
			get{return texturaDibujar;}
			set{texturaDibujar = value;}
		}
		
		#endregion
		
		
		
		public void inicializar(){	
			//cambiamos las dimensiones si la anchura/altura del componente es 0
			if(this.anchura == 0 && texturaNormal.width != null){
	        	this.anchura =  texturaNormal.width;
			}
			
			if(this.altura == 0 && texturaNormal.height != null){
				this.altura = texturaNormal.height;	
			}
			
		}		
		
		
		#region Unity
		public void Start(){
//			base.Start();
			texturaDibujar = texturaNormal;
		}
		#endregion
	}
}