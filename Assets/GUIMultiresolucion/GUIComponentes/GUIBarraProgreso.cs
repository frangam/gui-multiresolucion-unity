using UnityEngine;
using System.Collections;
using GUIMultiresolucion.Core;

namespace GUIMultiresolucion.GUIComponentes
{
	public enum Relleno{DerechaIzquierda, IzquierdaDerecha}
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
		public int Offsetizquierdo = 5;
		public int Offsetderecho = 5;
		public int Offsetarriba = 5;
		public int Offsetabajo = 5;
		public Relleno Rellenobarra = Relleno.DerechaIzquierda;
		
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
			Rect barraProgreso = new Rect(0, 0, altura, anchura);
			
			if(Rellenobarra == Relleno.IzquierdaDerecha)
			{
				//configuramos el Rect para que la barra de progreso deje un marco y se pueda ver el fondo.
				barraProgreso.yMax = altura-Offsetarriba;
				barraProgreso.yMin = Offsetabajo;
				barraProgreso.xMax = anchura-(Offsetderecho+Offsetizquierdo);
				barraProgreso.xMin = Offsetderecho;
				//posicionamos el Rect en la misma posicion que el GUIBarraProgreso
				barraProgreso.x = this.posicionFija.x+Offsetizquierdo;
				barraProgreso.y = this.posicionFija.y+Offsetarriba;
				//recalculamos el ancho de la barra en funcion del porcetaje que queremos que se rellene.
				barraProgreso.width = Mathf.Clamp(((porcentaje*barraProgreso.xMax)/100),0,barraProgreso.xMax);
			}
			else
			{
				//configuramos el Rect para que la barra de progreso deje un marco y se pueda ver el fondo.
				barraProgreso.yMax = altura-Offsetarriba;
				barraProgreso.yMin = Offsetabajo;
				
				barraProgreso.xMax = Offsetizquierdo;
				barraProgreso.xMin = anchura-(Offsetderecho+Offsetizquierdo*2);
				//posicionamos el Rect en la misma posicion que el GUIBarraProgreso
				barraProgreso.x = this.posicionFija.x+barraProgreso.xMin+Offsetderecho;
				barraProgreso.y = this.posicionFija.y+Offsetarriba;
				//recalculamos el ancho de la barra en funcion del porcetaje que queremos que se rellene.
				barraProgreso.width = barraProgreso.xMax - Mathf.Clamp(((porcentaje*barraProgreso.xMin)/100),barraProgreso.xMax,barraProgreso.xMin);
			}
			return barraProgreso;
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
