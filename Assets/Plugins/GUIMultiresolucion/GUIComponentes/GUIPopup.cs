using UnityEngine;
using System.Collections;
using GUIMultiresolucion.Core;
using TouchScript.Gestures;

namespace GUIMultiresolucion.GUIComponentes{
	public class GUIPopup : GUIVentana {
		/// <summary>
		/// El fondo exterior del popup que ocupa toda la pantalla
		/// </summary>
		public GUIImagen fondoExterior;
		
		
		#region metodos sobreescritos
		public override void inicializar ()
		{
			fondoExterior.inicializar();
			base.inicializarSoloVentana();
			
			if(imgFondo != null){
				imgFondo.anchura = this.anchura;
				imgFondo.altura = this.altura;
				imgFondo.relativoA = this.relativoA;
				imgFondo.ocuparTodoElAlto = false;
				imgFondo.ocuparTodoElAncho = false;
				imgFondo.posicionRelativaA = this.posicionRelativaA;
			}
			if(imgCabecera != null){
				imgCabecera.anchura = this.anchura;
				imgCabecera.relativoA = this.relativoA;
				imgCabecera.ocuparTodoElAlto = false;
				imgCabecera.ocuparTodoElAncho = false;
				
				Vector2 posEnPixeles = imgCabecera.posicionRelativaAlAnclaRespectoAPosicionFijaDada(new Vector2(0f, this.posicionFija.y), this.relativoA);
				imgCabecera.posicionRelativaA = new Vector2(this.posicionRelativaA.x, posEnPixeles.y);
			}
			if(botonCerrar != null){
				botonCerrar.relativoA = this.relativoA;
				
				Vector2 posEnPixeles = botonCerrar.posicionRelativaAlAnclaRespectoAPosicionFijaDada(new Vector2(((this.posicionFija.x+this.anchura)-(botonCerrar.anchura+5f)), this.posicionFija.y+5f), this.relativoA);
				botonCerrar.posicionRelativaA = posEnPixeles;
			}
			if(imgPie != null){
				imgPie.anchura = this.anchura;
				imgPie.relativoA = this.relativoA;
				imgPie.ocuparTodoElAlto = false;
				imgPie.ocuparTodoElAncho = false;
				
				Vector2 posEnPixeles = imgPie.posicionRelativaAlAnclaRespectoAPosicionFijaDada(new Vector2(0f, (this.altura + this.posicionFija.y)-imgPie.altura), this.relativoA);
				float y = posEnPixeles.y == 0 ? 1f : posEnPixeles.y;
				
				imgPie.posicionRelativaA = new Vector2(this.posicionRelativaA.x, y);
			}
			if(panelScrollable != null){	
				panelScrollable.anchura = this.anchura;
				panelScrollable.relativoA = this.relativoA;
				panelScrollable.ocuparTodoElAlto = false;
				panelScrollable.ocuparTodoElAncho = false;
			}
			
			
			
			
			
			base.inicializar ();
			
		}
		
		public override void dibujar ()
		{
			fondoExterior.dibujar();
			
			base.dibujar ();
		}
		#endregion
		
		#region Unity
		void Start(){
			fondoExterior.GetComponent<TapGesture>().StateChanged += tapExteriorPopup;
		}
		#endregion
		
		#region evento tap exterior del popup
		
		void tapExteriorPopup (object sender, TouchScript.Events.GestureStateChangeEventArgs e){
			switch(e.State){
				case Gesture.GestureState.Ended:
					cerrarVentana();
				break;
			}
		}
		#endregion
	}
}