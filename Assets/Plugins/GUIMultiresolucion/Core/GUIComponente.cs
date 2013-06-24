using UnityEngine;
using System.Collections;

/// <summary>
/// Autor: Fran Garcia <www.fgarmo.com>
/// 
/// Representa un componente de la GUI
/// </summary>
[System.Serializable]
public class GUIComponente : IPosicionable {
	public string nombre;	
	public float anchura = 0;
	public float altura = 0;
	
	/// <summary>
	/// Si es true ocupara todo el ancho de a pantalla
	/// </summary>
	public bool ocuparTodoElAncho = false;
	
	/// <summary>
	/// Si es true ocupara todo el alto de a pantalla
	/// </summary>
	public bool ocuparTodoElAlto = false;
	
	/// <summary>
	/// Posicion fija x, y absolutas
	/// </summary>
	public Vector2 posicionFija;
	
	/// <summary>
	/// Posicion relativa al ancla seleccionado en el atributo "relativoA"
	/// </summary>
	public Vector2 posicionRelativaA;
	
	/// <summary>
	/// Posicionar de forma relativa al tipo de anclado especificado
	/// </summary>
	public TipoAnclado _relativoA = TipoAnclado.SIN_ANCLADO;
	
	/// <summary>
	/// El tipo de anclado que tiene el componente a la pantalla. Por defecto, sin anclado.
	/// <example>
	/// TipoAnclado.SUPERIOR_IZQUIERDA para anclar el componente a la esquina superior izquierda
	/// </example>
	/// </summary>
	public TipoAnclado _ancladoA = TipoAnclado.SIN_ANCLADO;
	
	/// <summary>
	/// Un rectangulo que representa la posicion y las dimensiones del componente
	/// </summary>
	public Rect distribucion{
		get{			
			Rect dist; //la distribucion
			Vector2 posicionEscalada = Vector2.zero; //posicion escalada para la resolucion de pantalla
			
			//si se ha indicado un anclado para el componente, lo anclamos a este
			if(_ancladoA != TipoAnclado.SIN_ANCLADO){
			 	posicionEscalada = posicionDelAncladoSeleccionado(_ancladoA);
			}
			else if(_relativoA != TipoAnclado.SIN_ANCLADO){ //si no se quiere anclar el componente pero si se quiere posicionar de forma relativa a un anclado concreto
				posicionEscalada = posicionRelativaAlAncla(_relativoA); //obtenemos la posicion relativa al ancla indicada
			}
			else if(posicionFija != null){ //si no se quiere anclar pero se quiere posicionar de forma absoluta por pixeles
				posicionEscalada = new Vector2(posicionFija.x, posicionFija.y);				
			}
			
			posicionFija = posicionEscalada;

//			if(Screen.height < GUIEscalador.ALTO_NATIVO){
//				
//			}
			
			//si se quiere que el componente ocupe toda la anchura de la pantalla
			//teniendo en cuenta que la anchura de la pantalla es la anchura nativa a la que se ha modelado la gui
			if(ocuparTodoElAncho){
				anchura = GUIEscalador.ANCHO_NATIVO;	
			}
			//si se quiere que el componente ocupe toda la altura de la pantalla
			//teniendo en cuenta que la altura de la pantalla es la altura nativa a la que se ha modelado la gui
			if(ocuparTodoElAlto){
				altura = GUIEscalador.ALTO_NATIVO;
			}
			
			//construimos el rectangulo que es la distribucion en pantalla del componente gui
			dist = new Rect(posicionEscalada.x, posicionEscalada.y, anchura, altura);
			
			return dist;
		}
	}

	
	#region implementacion de IPosicionable
	
	public Vector2 posicionDelAncladoSeleccionado(TipoAnclado anclado){
		float anchuraDispositivo = Screen.width; //la anchura del dispositivo donde se esta ejecutando el juego
		float alturaDispositivo = Screen.height; //la anchura del dispositivo donde se esta ejecutando el juego
		float factorEscaladoAnchura = GUIEscalador.factorEscaladoX; //factor para escalar la anchura del componenete gui
		float factorEscaladoAltura = GUIEscalador.factorEscaladoY; //factor para escalar la altura del componenete gui
		float xEscalada = 0f;
		float yEscalada = 0f;
		Vector2 res = Vector2.zero;
		
		//segun el tipo de anclado del componente
		switch(anclado){
			case TipoAnclado.SUPERIOR_IZQUIERDA:
				xEscalada = 0f;
				yEscalada = 0f;
			break;
			case TipoAnclado.SUPERIOR_CENTRO:
				xEscalada = ((anchuraDispositivo/2)/factorEscaladoAnchura) - anchura/2;
				yEscalada = 0f;
			break;
			case TipoAnclado.SUPERIOR_DERECHA:
				xEscalada = (anchuraDispositivo/factorEscaladoAnchura) - anchura;
				yEscalada = 0f;
			break;
			case TipoAnclado.CENTRO:
				xEscalada = ((anchuraDispositivo/2) / factorEscaladoAnchura) - anchura/2;
				yEscalada = ((alturaDispositivo/2) / factorEscaladoAltura) - altura/2;
			break;
			case TipoAnclado.CENTRO_IZQUIERDA:
				xEscalada = 0f;
				yEscalada = ((alturaDispositivo/2)/factorEscaladoAltura) - altura/2;
			break;
			case TipoAnclado.CENTRO_DERECHA:
				xEscalada = (anchuraDispositivo/factorEscaladoAnchura) - anchura;
				yEscalada = ((alturaDispositivo/2)/factorEscaladoAltura) - altura/2;
			break;
			case TipoAnclado.INFERIOR_IZQUIERDA:
				xEscalada = 0f;
				yEscalada =  (alturaDispositivo/factorEscaladoAltura) - altura;
			break;
			case TipoAnclado.INFERIOR_CENTRO:
				xEscalada = ((anchuraDispositivo/2)/factorEscaladoAnchura) - anchura/2;
				yEscalada = (alturaDispositivo/factorEscaladoAltura) - altura;
			break;
			case TipoAnclado.INFERIOR_DERECHA:
				xEscalada = (anchuraDispositivo/factorEscaladoAnchura) - anchura;
				yEscalada = (alturaDispositivo/factorEscaladoAltura) - altura;
			break;
		}	
		
		res = new Vector2(xEscalada, yEscalada);
		
		return res;
	}
	
	public Vector2 posicionRelativaAlAncla(TipoAnclado relativoA){
		Vector2 posicionRelativa = Vector2.zero;// la posicion del componente gui escalada y relativa al anclado indicado
		
		//segun el tipo de anclado desde el que se toma como referencia
		switch(relativoA){
			case TipoAnclado.SUPERIOR_IZQUIERDA:
				posicionRelativa = posicionDesdeSuperiorIzquierda(posicionRelativaA.x, posicionRelativaA.y);
			break;
			case TipoAnclado.SUPERIOR_CENTRO:
				posicionRelativa = posicionDesdeSuperiorCentro(posicionRelativaA.x, posicionRelativaA.y);
			break;
			case TipoAnclado.SUPERIOR_DERECHA:
				posicionRelativa = posicionDesdeSuperiorDerecha(posicionRelativaA.x, posicionRelativaA.y);
			break;
			case TipoAnclado.CENTRO:
				posicionRelativa = posicionDesdeCentro(posicionRelativaA.x, posicionRelativaA.y);
			break;
			case TipoAnclado.CENTRO_IZQUIERDA:
				posicionRelativa = posicionDesdeCentroIzquierda(posicionRelativaA.x, posicionRelativaA.y);
			break;
			case TipoAnclado.CENTRO_DERECHA:
				posicionRelativa = posicionDesdeCentroDerecha(posicionRelativaA.x, posicionRelativaA.y);
			break;
			case TipoAnclado.INFERIOR_IZQUIERDA:
				posicionRelativa = posicionDesdeInferiorIzquierda(posicionRelativaA.x, posicionRelativaA.y);
			break;
			case TipoAnclado.INFERIOR_CENTRO:
				posicionRelativa = posicionDesdeInferiorCentro(posicionRelativaA.x, posicionRelativaA.y);
			break;
			case TipoAnclado.INFERIOR_DERECHA:
				posicionRelativa = posicionDesdeInferiorDerecha(posicionRelativaA.x, posicionRelativaA.y);
			break;
		}	
		
		return posicionRelativa;
	}
	
	public Vector2 dimensionEscaladaPantalla(){
		float anchuraDispositivo = Screen.width; //la anchura del dispositivo donde se esta ejecutando el juego
		float alturaDispositivo = Screen.height; //la anchura del dispositivo donde se esta ejecutando el juego
		float factorEscaladoAnchura = GUIEscalador.factorEscaladoX; //factor para escalar la anchura del componenete gui
		float factorEscaladoAltura = GUIEscalador.factorEscaladoY; //factor para escalar la altura del componenete gui
		
		float anchuraEscalada = (anchuraDispositivo/factorEscaladoAnchura) - anchura;
		float alturaEscalada = (alturaDispositivo/factorEscaladoAltura) - altura;
		
		return new Vector2(anchuraEscalada, alturaEscalada);
	}
	
	public Vector2 posicionDesdeSuperiorIzquierda(float xPorcentaje, float yPorcentaje){
		Vector2 posicionAncla = posicionDelAncladoSeleccionado(TipoAnclado.SUPERIOR_IZQUIERDA); //obtenemos la posicion del anclado
		Vector2 dimensionPantalla = dimensionEscaladaPantalla(); //x: anchura, y:altura
		float xEscalada = posicionAncla.x + (dimensionPantalla.x*Mathf.Clamp(xPorcentaje, 0f, 1f)); 
		float yEscalada = posicionAncla.y + (dimensionPantalla.y*Mathf.Clamp(yPorcentaje, 0f, 1f)); 
		
		return new Vector2(xEscalada, yEscalada);
	}
	
	public Vector2 posicionDesdeSuperiorCentro(float xPorcentaje, float yPorcentaje){
		Vector2 posicionAncla = posicionDelAncladoSeleccionado(TipoAnclado.SUPERIOR_CENTRO); //obtenemos la posicion del anclado
		Vector2 dimensionPantalla = dimensionEscaladaPantalla(); //x: anchura, y:altura
		float xEscalada = posicionAncla.x + (dimensionPantalla.x/2*Mathf.Clamp(xPorcentaje, -1f, 1f)); //Dividir entre 2 dimensionPantalla.x para no salirnos de los bordes. -1f de minimo (-100%) para distanciarlo hacia la izq
		float yEscalada = posicionAncla.y + (dimensionPantalla.y*Mathf.Clamp(yPorcentaje, 0f, 1f)); 
		
		return new Vector2(xEscalada, yEscalada);
	}
	
	public Vector2 posicionDesdeSuperiorDerecha(float xPorcentaje, float yPorcentaje){
		Vector2 posicionAncla = posicionDelAncladoSeleccionado(TipoAnclado.SUPERIOR_DERECHA); //obtenemos la posicion del anclado
		Vector2 dimensionPantalla = dimensionEscaladaPantalla(); //x: anchura, y:altura
		float xEscalada = posicionAncla.x - (dimensionPantalla.x*Mathf.Clamp(xPorcentaje, 0f, 1f)); //restamos porque distanciamos el componente gui hacia la izquierda
		float yEscalada = posicionAncla.y + (dimensionPantalla.y*Mathf.Clamp(yPorcentaje, 0f, 1f)); 
		
		return new Vector2(xEscalada, yEscalada);
	}
	
	public Vector2 posicionDesdeCentroIzquierda(float xPorcentaje, float yPorcentaje){
		Vector2 posicionAncla = posicionDelAncladoSeleccionado(TipoAnclado.CENTRO_IZQUIERDA); //obtenemos la posicion del anclado
		Vector2 dimensionPantalla = dimensionEscaladaPantalla(); //x: anchura, y:altura
		float xEscalada = posicionAncla.x + (dimensionPantalla.x*Mathf.Clamp(xPorcentaje, 0f, 1f)); 
		float yEscalada = posicionAncla.y + (dimensionPantalla.y/2*Mathf.Clamp(yPorcentaje, -1f, 1f)); //Dividir entre 2 dimensionPantalla.x para no salirnos de los bordes. -1f de minimo (-100%) para distanciarlo hacia abajo
		
		return new Vector2(xEscalada, yEscalada);
	}
	
	public Vector2 posicionDesdeCentro(float xPorcentaje, float yPorcentaje){
		Vector2 posicionAncla = posicionDelAncladoSeleccionado(TipoAnclado.CENTRO); //obtenemos la posicion del anclado
		Vector2 dimensionPantalla = dimensionEscaladaPantalla(); //x: anchura, y:altura
		float xEscalada = posicionAncla.x + (dimensionPantalla.x/2*Mathf.Clamp(xPorcentaje, -1f, 1f)); //Dividir entre 2 dimensionPantalla.x para no salirnos de los bordes. -1f de minimo (-100%) para distanciarlo hacia la izq
		float yEscalada = posicionAncla.y + (dimensionPantalla.y/2*Mathf.Clamp(yPorcentaje, -1f, 1f)); //Dividir entre 2 dimensionPantalla.x para no salirnos de los bordes. -1f de minimo (-100%) para distanciarlo hacia abajo
		
		return new Vector2(xEscalada, yEscalada);
	}
	
	public Vector2 posicionDesdeCentroDerecha(float xPorcentaje, float yPorcentaje){
		Vector2 posicionAncla = posicionDelAncladoSeleccionado(TipoAnclado.CENTRO_DERECHA); //obtenemos la posicion del anclado
		Vector2 dimensionPantalla = dimensionEscaladaPantalla(); //x: anchura, y:altura
		float xEscalada = posicionAncla.x - (dimensionPantalla.x*Mathf.Clamp(xPorcentaje, 0f, 1f)); //restamos porque distanciamos el componente gui hacia la izquierda 
		float yEscalada = posicionAncla.y + (dimensionPantalla.y/2*Mathf.Clamp(yPorcentaje, -1f, 1f)); //Dividir entre 2 dimensionPantalla.x para no salirnos de los bordes. -1f de minimo (-100%) para distanciarlo hacia abajo
		
		return new Vector2(xEscalada, yEscalada);
	}
	
	public Vector2 posicionDesdeInferiorIzquierda(float xPorcentaje, float yPorcentaje){
		Vector2 posicionAncla = posicionDelAncladoSeleccionado(TipoAnclado.INFERIOR_IZQUIERDA); //obtenemos la posicion del anclado	
		Vector2 dimensionPantalla = dimensionEscaladaPantalla(); //x: anchura, y:altura
		float xEscalada = posicionAncla.x + (dimensionPantalla.x*Mathf.Clamp(xPorcentaje, 0f, 1f)); 
		float yEscalada = posicionAncla.y - (dimensionPantalla.y*Mathf.Clamp(yPorcentaje, 0f, 1f)); //restamos porque distanciamos el componente gui hacia arriba
			
		return new Vector2(xEscalada, yEscalada);
	}
	
	public Vector2 posicionDesdeInferiorCentro(float xPorcentaje, float yPorcentaje){
		Vector2 posicionAncla = posicionDelAncladoSeleccionado(TipoAnclado.INFERIOR_CENTRO); //obtenemos la posicion del anclado	
		Vector2 dimensionPantalla = dimensionEscaladaPantalla(); //x: anchura, y:altura
		float xEscalada = posicionAncla.x + (dimensionPantalla.x/2*Mathf.Clamp(xPorcentaje, -1f, 1f)); //Dividir entre 2 dimensionPantalla.x para no salirnos de los bordes. -1f de minimo (-100%) para distanciarlo hacia la izq
		float yEscalada = posicionAncla.y - (dimensionPantalla.y*Mathf.Clamp(yPorcentaje, 0f, 1f)); //restamos porque distanciamos el componente gui hacia arriba
			
		return new Vector2(xEscalada, yEscalada);
	}
	
	public Vector2 posicionDesdeInferiorDerecha(float xPorcentaje, float yPorcentaje){
		Vector2 posicionAncla = posicionDelAncladoSeleccionado(TipoAnclado.INFERIOR_DERECHA); //obtenemos la posicion del anclado	
		Vector2 dimensionPantalla = dimensionEscaladaPantalla(); //x: anchura, y:altura
		float xEscalada = posicionAncla.x - (dimensionPantalla.x*Mathf.Clamp(xPorcentaje, 0f, 1f)); //restamos porque distanciamos el componente gui hacia la izquierda
		float yEscalada = posicionAncla.y - (dimensionPantalla.y*Mathf.Clamp(yPorcentaje, 0f, 1f)); //restamos porque distanciamos el componente gui hacia arriba
			
		return new Vector2(xEscalada, yEscalada);
	}
	
	#endregion
	
}
