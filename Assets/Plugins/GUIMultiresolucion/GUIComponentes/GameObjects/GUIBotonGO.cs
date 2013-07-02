using UnityEngine;
using System.Collections;

public class GUIBotonGO : MonoBehaviour {
	public GUIBoton boton;
	
	// Use this for initialization
	void Start () {

	 	//cambiamos las dimensiones si la anchura/altura del componente es 0
		if(boton.anchura == 0 && boton.texturaNormal.width != null){
        	boton.anchura =  boton.texturaNormal.width;
		}
		
		if(boton.altura == 0 && boton.texturaNormal.height != null){
			boton.altura = boton.texturaNormal.height;	
		}
		
		Vector3 s = transform.localScale;
		transform.localScale = new Vector3(boton.anchura, boton.altura, s.z);
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
