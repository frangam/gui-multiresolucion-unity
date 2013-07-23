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
			
			if(imgFondo != null){
				imgFondo.anchura = this.anchura;
				imgFondo.altura = this.altura;
				imgFondo.relativoA = this.relativoA;
				imgFondo.ocuparTodoElAlto = false;
				imgFondo.ocuparTodoElAncho = false;
				imgFondo.posicionRelativaA = new Vector2(this.posicionRelativaA.x, imgFondo.posicionRelativaA.y);
			}
			if(imgCabecera != null){
				imgCabecera.anchura = this.anchura;
				imgCabecera.relativoA = this.relativoA;
				imgCabecera.ocuparTodoElAlto = false;
				imgCabecera.ocuparTodoElAncho = false;
				imgCabecera.posicionRelativaA = new Vector2(this.posicionRelativaA.x, imgCabecera.posicionRelativaA.y);
			}
			if(imgPie != null){
				imgPie.anchura = this.anchura;
				imgPie.relativoA = this.relativoA;
				imgPie.ocuparTodoElAlto = false;
				imgPie.ocuparTodoElAncho = false;
				imgPie.posicionRelativaA = new Vector2(this.posicionRelativaA.x, imgPie.posicionRelativaA.y);
			}
			if(panelScrollable != null){
				panelScrollable.anchura = this.anchura;
				panelScrollable.relativoA = this.relativoA;
				panelScrollable.ocuparTodoElAlto = false;
				panelScrollable.ocuparTodoElAncho = false;
				panelScrollable.posicionRelativaA = new Vector2(this.posicionRelativaA.x, panelScrollable.posicionRelativaA.y);
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