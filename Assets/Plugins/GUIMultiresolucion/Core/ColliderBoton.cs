using UnityEngine;
using System.Collections;
using GUIMultiresolucion.GUIComponentes;
using GUIMultiresolucion.Core;

public class ColliderBoton : MonoBehaviour {
	private GUIBoton boton;
	
	private float anchuraCollider; 
	private float alturaCollider;
	
	private Vector2 origenCoordsColliders;
	private Vector3 posicion;
	
	
	// Use this for initialization
	void Start () {
		//obtenemos el boton
		boton = GetComponent<GUIBoton>();
		actualizar();
	}
	
	// Update is called once per frame
	void Update () {
		
	}
	
	public void actualizar(){
//		bool esPortrait = Screen.height >= Screen.width;
//		
//		if(esPortrait){
//			anchuraCollider = GUIEscalador.pixelRatio*GUIEscalador.factorEscaladoX*boton.distribucion.width;
//			alturaCollider = GUIEscalador.pixelRatio*GUIEscalador.factorEscaladoY*boton.distribucion.height;
//		}
//		else{
//			anchuraCollider = GUIEscalador.pixelRatio*GUIEscalador.factorEscaladoY*boton.distribucion.width;
//			alturaCollider = GUIEscalador.pixelRatio*GUIEscalador.factorEscaladoX*boton.distribucion.height;
//		}
//		
		
		//-------------------------
		//escala del collider
		//-------------------------
		
		anchuraCollider = GUIEscalador.pixelRatio*GUIEscalador.factorEscaladoX*boton.distribucion.width;
		alturaCollider = GUIEscalador.pixelRatio*GUIEscalador.factorEscaladoY*boton.distribucion.height;
		transform.localScale = new Vector3(anchuraCollider, alturaCollider, transform.localScale.z);
		
		
		//-------------------------
		//posicion del collider
		//-------------------------
		
		//El origen de coordenadas de los colliders es diferente al origen de coordenadas de las texturas
		//El origen de coordenadas de las texturas es la esquina superior izquierda.
		//El origen de coordenadas de los colliders es el centro de la pantalla.
		origenCoordsColliders = boton.posicionDelAncladoSeleccionado(TipoAnclado.CENTRO);
		
		float posX = GUIEscalador.pixelRatio*GUIEscalador.factorEscaladoX*(boton.posicionFija.x-origenCoordsColliders.x);
		float posY = GUIEscalador.pixelRatio*GUIEscalador.factorEscaladoY*(origenCoordsColliders.y-boton.posicionFija.y);

		posicion = new Vector3(posX, posY, transform.position.z);
		
		//asignamos finalmente la posicion
		transform.position = posicion;	
	}
}
