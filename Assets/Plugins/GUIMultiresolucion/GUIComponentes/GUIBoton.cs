using UnityEngine;
using System.Collections;
using System;

/// <summary>
/// Autor: Fran Garcia <www.fgarmo.com>
/// 
/// Representa un boton de la gui
/// </summary>
[System.Serializable]
public class GUIBoton: GUIComponente, ITocable{
        
	public Texture texturaNormal;
	public Texture texturaPulsado;
	
	public Texture texturaDibujar;
	
	

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
	
	public BoxCollider collider;
	
	
	
	public void inicializar(){
		//cambiamos las dimensiones si la anchura/altura del componente es 0
		if(this.anchura == 0 && texturaNormal.width != null){
        	this.anchura =  texturaNormal.width;
		}
		
		if(this.altura == 0 && texturaNormal.height != null){
			this.altura = texturaNormal.height;	
		}
		
	}
	
	
	#region implementacion de ITocable
	/// <summary>
	/// Indica true si se ha tocado el boton y false en caso contrario
	/// </summary>
	/// <returns>
	/// True si se toca el boton
	/// </returns>
	/// <param name='posicionToque'>
	/// La posicion del toque en pantalla
	/// </param>
	public bool seHaTocado(Vector2 posicionToque){
		//corregimos el toque en pantalla a las coordenadas de pantalla escaladas
		Vector2 toqueEnPantallaEscalada = new Vector2(posicionToque.x / GUIEscalador.factorEscaladoX, posicionToque.y / GUIEscalador.factorEscaladoY);
		
		//condicion de tocado, que el toque ya escaladao esté dentro del rectangulo distribucion del boton
		bool tocado = (toqueEnPantallaEscalada.x >= distribucion.x && toqueEnPantallaEscalada.x <= (distribucion.x + anchura))
					&& (toqueEnPantallaEscalada.y >= distribucion.y && toqueEnPantallaEscalada.y <= (distribucion.y + altura));
		
		return tocado;
	}
	
	public void inicioDelToque(){
		Debug.Log("Inicio del toque");
	}
	
	public void finDelToque(){
		Debug.Log("Fin del toque");
	}
	
	public void moviendoToque(){
		Debug.Log("Moviendo toque");
	}
	#endregion
}
