using UnityEngine;
using System.Collections;
using GUIMultiresolucion.Core;

namespace GUIMultiresolucion.GUIComponentes
{
	public class GUIBarraProgreso : GUIComponente
	{	
		/// <summary>
		/// La textura de progreso que se proyecta sobre el fondo.
		/// </summary>
		public Texture texturaProgreso;
		/// <summary>
		/// Fondo de la barra de progreso, unos pixeles mas grande que la barra para que dibuje un marco.
		/// </summary>
		public Texture fondo;
		/// <summary>
		/// The porcentaje.
		/// </summary>
		public float porcentaje;
		
		public Texture TexturaProgreso
		{
			get { return texturaProgreso; }
			set { texturaProgreso = value; }
		}
		public Texture Fondo
		{
			get { return fondo; }
			set { fondo = value; }
		}
		
	    public Rect distribucion{
	        get{
				//cambiamos las dimensiones si la anchura/altura del componente es 0
				if(this.anchura == 0 && fondo.width != null){
		        	this.anchura =  fondo.width;
				}
				if(this.altura == 0 && fondo.height != null){
					this.altura = fondo.height;	
				}
				
				return base.distribucion;
	        }
	    }
		public Rect Progreso(float porcentaje)
		{
			//creamos un Rect donde se dibujara la barra de progreso
			Rect res = new Rect(0, 0, altura, anchura);
			//configuramos el Rect para que la barra de progreso deje un marco y se pueda ver el fondo.
			res.yMax = altura-5;
			res.yMin = 5;
			res.xMax = anchura-10;
			res.xMin = 5;
			//posicionamos el Rect en la misma posicion que el GUIBarraProgreso
			res.x = this.posicionFija.x+5;
			res.y = this.posicionFija.y+5;
			//recalculamos el ancho de la barra en funcion del porcetaje que queremos que se rellene.
			res.width = Mathf.Clamp(((porcentaje*res.xMax)/100),res.xMin,res.xMax);
			
			return res;
		}
		#region metodos sobreescritos de GUIComponente
		public override void inicializar(){				
			//cambiamos las dimensiones si la anchura/altura del componente es 0
			if(this.anchura == 0 && fondo.width != null){
	        	this.anchura =  fondo.width;
			}
			
			if(this.altura == 0 && fondo.height != null){
				this.altura = fondo.height;	
			}
			
			base.inicializar();
		}		
		public override void dibujar (){
			
			GUI.DrawTexture(distribucion,Fondo);
			GUI.DrawTexture(Progreso(porcentaje),TexturaProgreso, ScaleMode.ScaleAndCrop, false, anchura/altura);
		}
		#endregion
	}
}
