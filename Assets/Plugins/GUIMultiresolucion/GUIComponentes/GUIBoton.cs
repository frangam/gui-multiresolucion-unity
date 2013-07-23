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
		
		/// <summary>
		/// Textura del boton en estado normal, sin estar pulsado
		/// </summary>
		public Texture texturaNormal;
		
		/// <summary>
		/// Textura del boton en estado pulsado
		/// </summary>
		public Texture texturaPulsado;
		
		#region atributos privados
		/// <summary>
		/// La textura que se dibujara para el boton
		/// </summary>
		private Texture texturaDibujar;
		
		/// <summary>
		/// (Opcional) El componente gui al que pertenece el boton. Null si no pertecene a ninguno
		/// </summary>
		private GUIComponente componenteAlQuePertenece = null;
		
		#endregion
	
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
		
		public GUIComponente ComponenteAlQuePertenece{
			get{return componenteAlQuePertenece;}	
		}
		#endregion
		
		#region metodos publicos
		/// <summary>
		/// Inicializa el componente gui al que pertenece el boton
		/// </summary>
		/// <param name='componente'>
		/// Componente gui al que pertenece el boton
		/// </param>
		public void inicializar(GUIComponente componente){
			componenteAlQuePertenece = componente;
			
			inicializar();
		}
		
		public void ejecutarAccion(){
			if(componenteAlQuePertenece != null){
				//GUIVentana
				if(componenteAlQuePertenece.GetType() == typeof(GUIVentana) && tipo == TipoBoton.CERRAR){
					GUIVentana ventana = (GUIVentana) componenteAlQuePertenece; //la ventana a la que pertenece el boton
					ventana.cerrarVentana(); //cerramos la ventana
				}
				//GUIMultiventana
				else if(componenteAlQuePertenece.GetType() == typeof(GUIMultiVentana)){
					GUIMultiVentana multiventana = (GUIMultiVentana) componenteAlQuePertenece; //la multiventana a la que pertenece el boton
					
					switch(tipo){
						case TipoBoton.CERRAR:
							multiventana.cerrarVentana(); //cerrar la multiventana
						break;
					
						case TipoBoton.DELANTE:
							multiventana.abrirVentanaSiguiente(); //abrir ventana siguiente
						break;
						
						case TipoBoton.ATRAS:
							multiventana.abrirVentanaAnterior(); //abrir ventana anterior
						break;
					}
				}
			}
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