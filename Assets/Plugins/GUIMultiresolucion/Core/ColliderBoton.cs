using UnityEngine;
using System.Collections;
using GUIMultiresolucion.GUIComponentes;
using GUIMultiresolucion.Core;

public class ColliderBoton : MonoBehaviour {
	private GUIBoton boton;
	
	private float anchuraCollider; 
	private float alturaCollider;
	
	public Vector2 origenCoordsColliders;
	public Vector3 posicion;
	
	
// Use this for initialization
	void Start () {
//		Camera camGUI = GameObject.Find("GUIMultiresolucion").GetComponent<Camera>(); //la camara de la gui
//		float pixelRatio = (camGUI.orthographicSize * 2f) / camGUI.pixelHeight; //relacion entre pixeles y unidades de unity en camara ortogradica (el orthographicSize establecido a 1)
//		float pixelRatioAnchura = (camGUI.orthographicSize * 2f) / camGUI.pixelWidth;
		boton = GetComponent<GUIBoton>();
		
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
		
		Debug.Log("x: "+(boton.posicionFija.x-origenCoordsColliders.x));
		Debug.Log(boton.posicionFija.y);
		Debug.Log(origenCoordsColliders.y);
		

		posicion = new Vector3(posX, posY, transform.position.z);
		
		transform.position = posicion;
				
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
