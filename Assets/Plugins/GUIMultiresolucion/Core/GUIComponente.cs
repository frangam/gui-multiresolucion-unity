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
	/// Un rectangulo que representa la posicion y las dimensiones del componente
	/// </summary>
	public Rect distribucion{
		get{			
			Rect dist; //la distribucion
			Vector2 posicionEscalada = Vector2.zero; //posicion escalada para la resolucion de pantalla
			Vector2 dimensionesPantalla = dimensionPantallaEscalada(); //obtenemos la dimension de la pantalla ya aplicado el escalado
			
//			Debug.Log(dimensionesPantalla);
			
			if(_relativoA != TipoAnclado.SIN_ANCLADO){ //si se quiere posicionar de forma relativa a un anclado concreto
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
				anchura = dimensionesPantalla.x; //anchura	
			}
			//si se quiere que el componente ocupe toda la altura de la pantalla
			//teniendo en cuenta que la altura de la pantalla es la altura nativa a la que se ha modelado la gui
			if(ocuparTodoElAlto){
				altura = dimensionesPantalla.y; //altura
			}
			
			//construimos el rectangulo que es la distribucion en pantalla del componente gui
			dist = new Rect(posicionEscalada.x, posicionEscalada.y, anchura, altura);
			
			return dist;
		}
	}

	/// <summary>
	/// La posicion que ocupa en la pantalla el tipo de anclado
	/// </summary>
	private Vector2 posicionDelAnclado;
	
	/// <summary>
	/// La dimension de la pantalla, teniendo en cuenta las dimensiones del componente
	/// </summary>
	private Vector2 dimensionPantalla;
	
	#region implementacion de IPosicionable
	
	/// <summary>
	/// Calcula la posicion en cualquier resolucion de pantalla del ancla que se pasa como parametro
	/// </summary>
	/// <returns>
	/// Posicion del tipo de anclado
	/// </returns>
	/// <param name='anclado'>
	/// El tipo de anclado
	/// </param>
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
	
	/// <summary>
	/// Posicion del componente GUI relativa al tipo de ancla que se le indique
	/// </summary>
	/// <returns>
	/// La posicion relativa
	/// </returns>
	/// <param name='relativoA'>
	/// El tipo de anclado
	/// </param>
	public Vector2 posicionRelativaAlAncla(TipoAnclado relativoA){
		Vector2 posicionRelativa = Vector2.zero;// la posicion del componente gui escalada y relativa al anclado indicado
		
		this.dimensionPantalla = dimensionPantallaEscaladaSegunDimensionesDelComponente(); //x: anchura, y:altura
		this.posicionDelAnclado = posicionDelAncladoSeleccionado(relativoA); //obtenemos la posicion del anclado seleccionado
		
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
	
	/// <summary>
	/// La dimension de la pantalla escalada para todo tipo de resoluciones de pantalla
	/// Componente x: anchura, y:altura
	/// </summary>
	/// <returns>
	/// La componenete x del Vector2 es la anchura escalada, y la componente y la altura escalada
	/// </returns>
	public Vector2 dimensionPantallaEscalada(){
		float anchuraDispositivo = Screen.width; //la anchura del dispositivo donde se esta ejecutando el juego
		float alturaDispositivo = Screen.height; //la anchura del dispositivo donde se esta ejecutando el juego
		float factorEscaladoAnchura = GUIEscalador.factorEscaladoX; //factor para escalar la anchura del componenete gui
		float factorEscaladoAltura = GUIEscalador.factorEscaladoY; //factor para escalar la altura del componenete gui
		
		float anchuraEscalada = (anchuraDispositivo/factorEscaladoAnchura);
		float alturaEscalada = (alturaDispositivo/factorEscaladoAltura);
		
		return new Vector2(anchuraEscalada, alturaEscalada);	
	}
	
	/// <summary>
	/// La dimension de la pantalla escalada para todo tipo de resoluciones de pantalla
	/// teniendo en cuenta las dimensiones del componente GUI que se va a posicionar.
	/// </summary>
	/// <returns>
	/// La componenete x del Vector2 es la anchura escalada, y la componente y la altura escalada
	/// </returns>
	public Vector2 dimensionPantallaEscaladaSegunDimensionesDelComponente(){
		Vector2 dimensionPantalla = dimensionPantallaEscalada();
		float anchuraEscalada = dimensionPantalla.x - anchura;
		float alturaEscalada = dimensionPantalla.y - altura;
		
		return new Vector2(anchuraEscalada, alturaEscalada);
	}
	
	/// <summary>
	/// Devuelve la posicion en pixeles exactos relativa al anclado Superior-Izquierda
	/// indicandole el porcentaje de distancia que queremos aplicar en la coordenada X e Y.
	/// </summary>
	/// <returns>
	/// Posicion desde el anclado Superior-Izquierda
	/// </returns>
	/// <param name='x'>
	/// Porcentaje de distancia en la coordenada X
	/// </param>
	/// <param name='y'>
	/// Porcentaje de distancia en la coordenada Y
	/// </param>
	public Vector2 posicionDesdeSuperiorIzquierda(float xPorcentaje, float yPorcentaje){
		float xEscalada = this.posicionDelAnclado.x + (dimensionPantalla.x*Mathf.Clamp(xPorcentaje, 0f, 1f)); 
		float yEscalada = this.posicionDelAnclado.y + (dimensionPantalla.y*Mathf.Clamp(yPorcentaje, 0f, 1f)); 
		
		return new Vector2(xEscalada, yEscalada);
	}
	
	/// <summary>
	/// Devuelve la posicion en pixeles exactos relativa al anclado Superior-Centro
	/// indicandole el porcentaje de distancia que queremos aplicar en la coordenada X e Y.
	/// </summary>
	/// <returns>
	/// Posicion desde el anclado Superior-Centro
	/// </returns>
	/// <param name='x'>
	/// Porcentaje de distancia en la coordenada X
	/// </param>
	/// <param name='y'>
	/// Porcentaje de distancia en la coordenada Y
	/// </param>
	public Vector2 posicionDesdeSuperiorCentro(float xPorcentaje, float yPorcentaje){
		float xEscalada = this.posicionDelAnclado.x + (dimensionPantalla.x/2*Mathf.Clamp(xPorcentaje, -1f, 1f)); //Dividir entre 2 dimensionPantalla.x para no salirnos de los bordes. -1f de minimo (-100%) para distanciarlo hacia la izq
		float yEscalada = this.posicionDelAnclado.y + (dimensionPantalla.y*Mathf.Clamp(yPorcentaje, 0f, 1f)); 
		
		return new Vector2(xEscalada, yEscalada);
	}
	
	/// <summary>
	/// Devuelve la posicion en pixeles exactos relativa al anclado Superior-Derecha
	/// indicandole el porcentaje de distancia que queremos aplicar en la coordenada X e Y.
	/// </summary>
	/// <returns>
	/// Posicion desde el anclado Superior-Derecha
	/// </returns>
	/// <param name='x'>
	/// Porcentaje de distancia en la coordenada X
	/// </param>
	/// <param name='y'>
	/// Porcentaje de distancia en la coordenada Y
	/// </param>
	public Vector2 posicionDesdeSuperiorDerecha(float xPorcentaje, float yPorcentaje){
		float xEscalada = this.posicionDelAnclado.x - (dimensionPantalla.x*Mathf.Clamp(xPorcentaje, 0f, 1f)); //restamos porque distanciamos el componente gui hacia la izquierda
		float yEscalada = this.posicionDelAnclado.y + (dimensionPantalla.y*Mathf.Clamp(yPorcentaje, 0f, 1f)); 
		
		return new Vector2(xEscalada, yEscalada);
	}
	
	/// <summary>
	/// Devuelve la posicion en pixeles exactos relativa al anclado Centro-Izquierda
	/// indicandole el porcentaje de distancia que queremos aplicar en la coordenada X e Y.
	/// </summary>
	/// <returns>
	/// Posicion desde el anclado Centro-Izquierda
	/// </returns>
	/// <param name='x'>
	/// Porcentaje de distancia en la coordenada X
	/// </param>
	/// <param name='y'>
	/// Porcentaje de distancia en la coordenada Y
	/// </param>
	public Vector2 posicionDesdeCentroIzquierda(float xPorcentaje, float yPorcentaje){
		float xEscalada = this.posicionDelAnclado.x + (dimensionPantalla.x*Mathf.Clamp(xPorcentaje, 0f, 1f)); 
		float yEscalada = this.posicionDelAnclado.y + (dimensionPantalla.y/2*Mathf.Clamp(yPorcentaje, -1f, 1f)); //Dividir entre 2 dimensionPantalla.x para no salirnos de los bordes. -1f de minimo (-100%) para distanciarlo hacia abajo
		
		return new Vector2(xEscalada, yEscalada);
	}
	
	/// <summary>
	/// Devuelve la posicion en pixeles exactos relativa al anclado Centro
	/// indicandole el porcentaje de distancia que queremos aplicar en la coordenada X e Y.
	/// </summary>
	/// <returns>
	/// Posicion desde el anclado Centro
	/// </returns>
	/// <param name='x'>
	/// Porcentaje de distancia en la coordenada X
	/// </param>
	/// <param name='y'>
	/// Porcentaje de distancia en la coordenada Y
	/// </param>
	public Vector2 posicionDesdeCentro(float xPorcentaje, float yPorcentaje){
		float xEscalada = this.posicionDelAnclado.x + (dimensionPantalla.x/2*Mathf.Clamp(xPorcentaje, -1f, 1f)); //Dividir entre 2 dimensionPantalla.x para no salirnos de los bordes. -1f de minimo (-100%) para distanciarlo hacia la izq
		float yEscalada = this.posicionDelAnclado.y + (dimensionPantalla.y/2*Mathf.Clamp(yPorcentaje, -1f, 1f)); //Dividir entre 2 dimensionPantalla.x para no salirnos de los bordes. -1f de minimo (-100%) para distanciarlo hacia abajo
		
		return new Vector2(xEscalada, yEscalada);
	}
	
	/// <summary>
	/// Devuelve la posicion en pixeles exactos relativa al anclado Centro-Derecha
	/// indicandole el porcentaje de distancia que queremos aplicar en la coordenada X e Y.
	/// </summary>
	/// <returns>
	/// Posicion desde el anclado Centro-Derecha
	/// </returns>
	/// <param name='x'>
	/// Porcentaje de distancia en la coordenada X
	/// </param>
	/// <param name='y'>
	/// Porcentaje de distancia en la coordenada Y
	/// </param>
	public Vector2 posicionDesdeCentroDerecha(float xPorcentaje, float yPorcentaje){
		float xEscalada = this.posicionDelAnclado.x - (dimensionPantalla.x*Mathf.Clamp(xPorcentaje, 0f, 1f)); //restamos porque distanciamos el componente gui hacia la izquierda 
		float yEscalada = this.posicionDelAnclado.y + (dimensionPantalla.y/2*Mathf.Clamp(yPorcentaje, -1f, 1f)); //Dividir entre 2 dimensionPantalla.x para no salirnos de los bordes. -1f de minimo (-100%) para distanciarlo hacia abajo
		
		return new Vector2(xEscalada, yEscalada);
	}
	
	/// <summary>
	/// Devuelve la posicion en pixeles exactos relativa al anclado Inferior-Izquierda
	/// indicandole el porcentaje de distancia que queremos aplicar en la coordenada X e Y.
	/// </summary>
	/// <returns>
	/// Posicion desde el anclado Inferior-Izquierda
	/// </returns>
	/// <param name='x'>
	/// Porcentaje de distancia en la coordenada X
	/// </param>
	/// <param name='y'>
	/// Porcentaje de distancia en la coordenada Y
	/// </param>
	public Vector2 posicionDesdeInferiorIzquierda(float xPorcentaje, float yPorcentaje){
		float xEscalada = this.posicionDelAnclado.x + (dimensionPantalla.x*Mathf.Clamp(xPorcentaje, 0f, 1f)); 
		float yEscalada = this.posicionDelAnclado.y - (dimensionPantalla.y*Mathf.Clamp(yPorcentaje, 0f, 1f)); //restamos porque distanciamos el componente gui hacia arriba
			
		return new Vector2(xEscalada, yEscalada);
	}
		
	/// <summary>
	/// Devuelve la posicion en pixeles exactos relativa al anclado Inferior-Centro
	/// indicandole el porcentaje de distancia que queremos aplicar en la coordenada X e Y.
	/// </summary>
	/// <returns>
	/// Posicion desde el anclado Inferior-Centro
	/// </returns>
	/// <param name='x'>
	/// Porcentaje de distancia en la coordenada X
	/// </param>
	/// <param name='y'>
	/// Porcentaje de distancia en la coordenada Y
	/// </param>
	public Vector2 posicionDesdeInferiorCentro(float xPorcentaje, float yPorcentaje){
		float xEscalada = this.posicionDelAnclado.x + (dimensionPantalla.x/2*Mathf.Clamp(xPorcentaje, -1f, 1f)); //Dividir entre 2 dimensionPantalla.x para no salirnos de los bordes. -1f de minimo (-100%) para distanciarlo hacia la izq
		float yEscalada = this.posicionDelAnclado.y - (dimensionPantalla.y*Mathf.Clamp(yPorcentaje, 0f, 1f)); //restamos porque distanciamos el componente gui hacia arriba
			
		return new Vector2(xEscalada, yEscalada);
	}
	
	/// <summary>
	/// Devuelve la posicion en pixeles exactos relativa al anclado Inferior-Derecha
	/// indicandole el porcentaje de distancia que queremos aplicar en la coordenada X e Y.
	/// </summary>
	/// <returns>
	/// Posicion desde el anclado Inferior-Derecha
	/// </returns>
	/// <param name='x'>
	/// Porcentaje de distancia en la coordenada X
	/// </param>
	/// <param name='y'>
	/// Porcentaje de distancia en la coordenada Y
	/// </param>
	public Vector2 posicionDesdeInferiorDerecha(float xPorcentaje, float yPorcentaje){
		float xEscalada = this.posicionDelAnclado.x - (dimensionPantalla.x*Mathf.Clamp(xPorcentaje, 0f, 1f)); //restamos porque distanciamos el componente gui hacia la izquierda
		float yEscalada = this.posicionDelAnclado.y - (dimensionPantalla.y*Mathf.Clamp(yPorcentaje, 0f, 1f)); //restamos porque distanciamos el componente gui hacia arriba
			
		return new Vector2(xEscalada, yEscalada);
	}
	
	#endregion
	
}
