using UnityEngine;
using System.Collections;

/// <summary>
/// Autor: Fran Garcia <www.fgarmo.com>
/// 
/// Representa un componente de la GUI
/// </summary>
[System.Serializable]
public class GUIComponente {
	public string nombre;
	
	/// <summary>
	/// Posicion fija x, y absolutas
	/// </summary>
	public Vector2 posicionFija;
	
	/// <summary>
	/// Posicion relativa al ancla que se especifique.
	/// 
	/// Componente X del Vector2 es la distancia en % desde el primer Ancla indicado.
	/// Componente Y del Vector2 es la distancia en % desde el segundo Ancla indicado.
	/// 
	/// <example> posicionRelativaAlAncla = new Vector2(0.5f, 0f);
	/// posicionarRespectoAlAnclado = TipoAnclado.SUPERIOR_IZQUIERDA;
	/// 
	/// Por tanto, se posicionara el elemento a 0.05f %, es decir, al 50% del ancla superior y a 0f % del ancla izquierda. </example>
	/// </summary>
	public PosicionRelativa posicionRelativaAlAncla;
	
	
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
	/// El tipo de anclado que tiene el componente a la pantalla. Por defecto, sin anclado.
	/// <example>
	/// TipoAnclado.SUPERIOR_IZQUIERDA para anclar el componente a la esquina superior izquierda
	/// </example>
	/// </summary>
	public TipoAnclado anclado = TipoAnclado.SIN_ANCLADO;
	
	/// <summary>
	/// Un rectangulo que representa la posicion y las dimensiones del componente
	/// </summary>
	public Rect distribucion{
		get{			
			Rect dist; //la distribucion
			Vector2 posicionEscalada = Vector2.zero; //posicion escalada para la resolucion de pantalla
			
			//si se ha indicado un anclado para el componente, lo anclamos a este
			if(anclado != TipoAnclado.SIN_ANCLADO){
			 	posicionEscalada = getPosicionEscaladaDelAnclado(anclado);
			}
			else if(posicionRelativaAlAncla.anclaRelativa != TipoAnclado.SIN_ANCLADO){ //si no se quiere anclar el componente pero si se quiere posicionar de forma relativa a un anclado concreto
				posicionEscalada = getPosicionRelativaAlAncla(); //obtenemos la posicion relativa al ancla indicada
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
	
	public Vector2 getPosicionRelativaAlAncla(){
		float anchuraDispositivo = Screen.width; //la anchura del dispositivo donde se esta ejecutando el juego
		float alturaDispositivo = Screen.height; //la anchura del dispositivo donde se esta ejecutando el juego
		float factorEscaladoAnchura = GUIEscalador.factorEscaladoX; //factor para escalar la anchura del componenete gui
		float factorEscaladoAltura = GUIEscalador.factorEscaladoY; //factor para escalar la altura del componenete gui
		float xEscalada = 0f; 
		float yEscalada = 0f;
		Vector2 posicionEscalada;// la posicion del componente gui escalada y relativa al anclado indicado
		
		//segun el tipo de anclado desde el que se toma como referencia
		switch(posicionRelativaAlAncla.anclaRelativa){
			case TipoAnclado.SUPERIOR_IZQUIERDA:
				float porcentajeAncla1 = Mathf.Clamp(posicionRelativaAlAncla.porcentajeDesdeAncla1, 0f, 1f);
				float porcentajeAncla2 = Mathf.Clamp(posicionRelativaAlAncla.porcentajeDesdeAncla2, 0f, 1f);
			
				xEscalada = 0f + (GUIEscalador.ANCHO_NATIVO*porcentajeAncla2); //ancla2: IZQUIERDA
				yEscalada = 0f + (GUIEscalador.ALTO_NATIVO*porcentajeAncla1); //ancla1: SUPERIOR
			break;
			case TipoAnclado.SUPERIOR_CENTRO:
				xEscalada = ((anchuraDispositivo/2)/factorEscaladoAnchura) - anchura/2;
				xEscalada += (alturaDispositivo*posicionRelativaAlAncla.porcentajeDesdeAncla1)/factorEscaladoAltura;
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
		
		posicionEscalada = new Vector2(xEscalada, yEscalada);
		
		return posicionEscalada;
	}
	
	private Vector2 getPosicionEscaladaDelAnclado(TipoAnclado _anclado){
		float anchuraDispositivo = Screen.width; //la anchura del dispositivo donde se esta ejecutando el juego
		float alturaDispositivo = Screen.height; //la anchura del dispositivo donde se esta ejecutando el juego
		float factorEscaladoAnchura = GUIEscalador.factorEscaladoX; //factor para escalar la anchura del componenete gui
		float factorEscaladoAltura = GUIEscalador.factorEscaladoY; //factor para escalar la altura del componenete gui
		float xEscalada = 0f;
		float yEscalada = 0f;
		Vector2 res = Vector2.zero;
		
		//segun el tipo de anclado del componente
		switch(_anclado){
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
	
}
