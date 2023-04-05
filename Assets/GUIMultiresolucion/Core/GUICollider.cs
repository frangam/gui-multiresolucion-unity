using UnityEngine;
using System.Collections;
using GUIMultiresolucion.GUIComponentes;
using GUIMultiresolucion.Core;

namespace GUIMultiresolucion.Core{
	/*
	 * Representa un collider para un elemento de la gui.
	 * Se realizan calculos segun el escalado de la gui, los pixeles de la dimension del componente gui
	 * y las unidades de unity para escalar el transform
	 */ 
	[RequireComponent(typeof(Transform))]

	public class GUICollider : MonoBehaviour {
		/// <summary>
		/// El componente gui al que se le adjunta el collider
		/// </summary>
		private GUIComponente componenteGUI;
		
		private float anchuraCollider; 
		private float alturaCollider;
		
		private Vector2 origenCoordsColliders;
		private Vector3 posicion;
		
		
		public void inicializar (GUIComponente c) {
			//obtenemos el componente gui
			componenteGUI = c;
			actualizar();
		}
	
		public void actualizar(){

			//---------------------------------------------
			//escala del transform que tiene el collider
			//---------------------------------------------

			anchuraCollider = GUIEscalador.pixelRatio*GUIEscalador.factorEscaladoX*componenteGUI.distribucion.width;
			alturaCollider = GUIEscalador.pixelRatio*GUIEscalador.factorEscaladoY*componenteGUI.distribucion.height;
			
			transform.localScale = new Vector3(anchuraCollider, alturaCollider, transform.localScale.z);
			
			//---------------------------------------------
			//posicion del transform que tiene el collider
			//---------------------------------------------
			
			//El origen de coordenadas de los colliders es diferente al origen de coordenadas de las texturas
			//El origen de coordenadas de las texturas es la esquina superior izquierda.
			//El origen de coordenadas de los colliders es el centro de la pantalla.
			origenCoordsColliders = componenteGUI.posicionDelAncladoSeleccionado(TipoAnclado.CENTRO);
			
			float posX = GUIEscalador.pixelRatio*GUIEscalador.factorEscaladoX*(componenteGUI.posicionFija.x-origenCoordsColliders.x);
			float posY = GUIEscalador.pixelRatio*GUIEscalador.factorEscaladoY*(origenCoordsColliders.y-componenteGUI.posicionFija.y);
	
			posicion = new Vector3(posX, posY, transform.position.z);
			
			//asignamos finalmente la posicion
			transform.position = posicion;	
		}
	}
}