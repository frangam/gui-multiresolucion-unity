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
	public class GUIBoton: GUIComponente{
		/// <summary>
		/// El tipo de boton que es
		/// </summary>
	    public TipoBoton tipo; 
		
		public Texture texturaNormal;
		public Texture texturaPulsado;
		
		private Texture texturaDibujar;
		
		/// <summary>
		/// Bandera para saber que se ha pulsado el boton y se tiene que realizar la accion estandar que corresponda
		/// segun el tipo de boton
		/// </summary>
		private bool ejecutarAccionEstandar = false;
	
		#region propiedades publicas
		public bool EjecutarAccionEstandar{
			get{return ejecutarAccionEstandar;}
			set{ejecutarAccionEstandar = value;}
		}
		
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
		
		
		#region metodos sobreescritos de GUIComponente
		public override void inicializar(){				
			//cambiamos las dimensiones si la anchura/altura del componente es 0
			if(this.anchura == 0 && texturaNormal.width != null){
	        	this.anchura =  texturaNormal.width;
			}
			
			if(this.altura == 0 && texturaNormal.height != null){
				this.altura = texturaNormal.height;	
			}
			
			base.inicializar();
		}		
		public override void dibujar (){
			GUI.DrawTexture(distribucion, TexturaDibujar);
		}
		#endregion
		
		#region Unity
		public void Start(){
			texturaDibujar = texturaNormal;
		}
		#endregion
	}
}