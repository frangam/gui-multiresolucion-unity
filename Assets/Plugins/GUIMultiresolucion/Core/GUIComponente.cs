using UnityEngine;
using System.Collections;
using System;
using GUIMultiresolucion.GUIComponentes.Paneles;
using GUIMultiresolucion.Eventos;

namespace GUIMultiresolucion.Core{
	/// <summary>
	/// Autor: Fran Garcia <www.fgarmo.com>
	/// 
	/// Representa un componente de la GUI
	/// </summary>
	[System.Serializable]
	[ExecuteInEditMode]
	public class GUIComponente : MonoBehaviour, IPosicionable, IComparable {	
		public string nombre;	
		public float anchura = 0;
		public float altura = 0;
		
		public bool visible = true;
		
		/// <summary>
		/// La profundid es un numero para ordenar el renderizado de los gui componentes,
		/// es decir, establece un orden de dibujado en pantalla segun este parametro.
		/// </summary>
		public int profundidad = 0;
		
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
		public TipoAnclado relativoA = TipoAnclado.SIN_ANCLADO;
		
		#region atributos privados
		/// <summary>
		/// La posicion que ocupa en la pantalla el tipo de anclado
		/// </summary>
		private Vector2 posicionDelAnclado;
		
		/// <summary>
		/// La dimension de la pantalla, teniendo en cuenta las dimensiones del componente
		/// </summary>
		private Vector2 dimensionPantalla;
		
		/// <summary>
		/// El collider gui para gestionar gestos sobre elementos gui
		/// </summary>
		private GUICollider colliderGUI;
		
		/// <summary>
		/// Panel al que pertezeca el componente. Null si no pertenece a un panel.
		/// Por lo tanto, esto indica que el componente es un GUIItemPanel
		/// </summary>
		private GUIPanel panel = null;
		#endregion
		
		#region propiedades
		
		public virtual bool Visible{
			get{
				return visible;
			}
			set{
				visible = value; 
				GetComponent<BoxCollider>().enabled = value;
			}
		}
		
		/// <summary>
		/// Un rectangulo que representa la posicion y las dimensiones del componente
		/// </summary>
		public Rect distribucion{
			get{			
				Rect dist; //la distribucion
				Vector2 posicionEscalada = Vector2.zero; //posicion escalada para la resolucion de pantalla
				
				if(relativoA != TipoAnclado.SIN_ANCLADO){ //si se quiere posicionar de forma relativa a un anclado concreto
					posicionEscalada = posicionRelativaAlAncla(relativoA); //obtenemos la posicion relativa al ancla indicada
				}
				else if(posicionFija != null){ //si no se quiere anclar pero se quiere posicionar de forma absoluta por pixeles
					posicionEscalada = new Vector2(posicionFija.x, posicionFija.y);				
				}
				
				posicionFija = posicionEscalada;
				
				//construimos el rectangulo que es la distribucion en pantalla del componente gui
				dist = new Rect(posicionEscalada.x, posicionEscalada.y, anchura, altura);
				
				return dist;
			}
		}
		public GUICollider ColliderGUI{
			get{return colliderGUI;}	
		}
		public GUIPanel Panel{
			get{return panel;}	
		}
		#endregion
		
		#region Unity
		public void Start(){
			
		}
		#endregion
		
		#region metodos publicos
		/// <summary>
		/// Inicializa el panel al que pertenece el componente gui
		/// y algunos atributos relativos a la posicion del componente respecto al panel al que pertenece
		/// </summary>
		/// <param name='_panel'>
		/// El panel al que pertenece
		/// </param>
		public void inicializar(GUIPanel _panel){
			panel = _panel;
			
			GetComponent<EventosGUIItemPanelScrollable>().inicializar(panel);
			
			inicializar();
		}
		public virtual void resetear(){
			
		}
		
		public virtual void inicializar(){
			Vector2 dimensionesPantalla = dimensionPantallaEscalada(); //obtenemos la dimension de la pantalla ya aplicado el escalado
			
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
			
//			GetComponent<BoxCollider>().enabled = visible;
			colliderGUI = GetComponent<GUICollider>();	
			colliderGUI.inicializar(this);
			
		}
		public virtual void actualizar(){
			colliderGUI.actualizar();
		}
		public virtual void dibujar(){
			
		}
		#endregion
		
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
		/// Obtiene una posicion relativa al tipo de ancla del componente que se le indique y respecto a unas coordenadas fijas
		/// de la pantalla en pixeles. Es decir, dada una posicion fija en pixeles y un tipo de ancla, devuelve la posicion
		/// relativa que le corresponde en esos pixeles y con ese tipo de ancla.
		/// </summary>
		/// <returns>
		/// La posicion relativa
		/// </returns>
		/// <param name='posFija'>
		/// La posicion fija
		/// </param>
		public Vector2 posicionRelativaAlAnclaRespectoAPosicionFijaDada(Vector2 posFija){
			return posicionRelativaAlAnclaRespectoAPosicionFijaDada(posFija, relativoA);
		}
		
		/// <summary>
		/// Obtiene una posicion relativa al tipo de ancla que se le indique y respecto a unas coordenadas fijas
		/// de la pantalla en pixeles. Es decir, dada una posicion fija en pixeles y un tipo de ancla, devuelve la posicion
		/// relativa que le corresponde en esos pixeles y con ese tipo de ancla.
		/// </summary>
		/// <returns>
		/// La posicion relativa
		/// </returns>
		/// <param name='anclado'>
		/// El tipo de anclado
		/// </param>
		/// <param name='posFija'>
		/// La posicion fija
		/// </param>
		public Vector2 posicionRelativaAlAnclaRespectoAPosicionFijaDada(Vector2 posFija, TipoAnclado anclado){
			Vector2 posicionRelativa = Vector2.zero;// la posicion del componente gui escalada y relativa al anclado indicado
			
			this.dimensionPantalla = dimensionPantallaEscaladaSegunDimensionesDelComponente(); //x: anchura, y:altura
			this.posicionDelAnclado = posicionDelAncladoSeleccionado(relativoA); //obtenemos la posicion del anclado seleccionado
			
			//segun el tipo de anclado desde el que se toma como referencia
			switch(anclado){
				case TipoAnclado.SUPERIOR_IZQUIERDA:
				case TipoAnclado.SUPERIOR_CENTRO:
				case TipoAnclado.CENTRO:
				case TipoAnclado.CENTRO_IZQUIERDA:
					posicionRelativa = new Vector2(posFija.x / dimensionPantalla.x, posFija.y / dimensionPantalla.y);
				break;
				
				case TipoAnclado.SUPERIOR_DERECHA:
				case TipoAnclado.CENTRO_DERECHA:
					posicionRelativa = new Vector2(1f - (posFija.x / dimensionPantalla.x), posFija.y / dimensionPantalla.y);
				break;

				case TipoAnclado.INFERIOR_IZQUIERDA:
				case TipoAnclado.INFERIOR_CENTRO:
					posicionRelativa = new Vector2(posFija.x / dimensionPantalla.x, 1f - (posFija.y / dimensionPantalla.y));
				break;
				
				case TipoAnclado.INFERIOR_DERECHA:
					posicionRelativa = new Vector2(1f - (posFija.x / dimensionPantalla.x), 1f - (posFija.y / dimensionPantalla.y));
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
			float xEscalada = this.posicionDelAnclado.x + (dimensionPantalla.x*xPorcentaje);//Mathf.Clamp(xPorcentaje, 0f, 1f)); 
			float yEscalada = this.posicionDelAnclado.y + (dimensionPantalla.y*yPorcentaje);//Mathf.Clamp(yPorcentaje, 0f, 1f)); 
			
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
			float xEscalada = this.posicionDelAnclado.x + (dimensionPantalla.x/2*xPorcentaje);//Mathf.Clamp(xPorcentaje, -1f, 1f)); //Dividir entre 2 dimensionPantalla.x para no salirnos de los bordes. -1f de minimo (-100%) para distanciarlo hacia la izq
			float yEscalada = this.posicionDelAnclado.y + (dimensionPantalla.y*yPorcentaje);//Mathf.Clamp(yPorcentaje, 0f, 1f)); 
			
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
			float xEscalada = this.posicionDelAnclado.x - (dimensionPantalla.x*xPorcentaje);//Mathf.Clamp(xPorcentaje, 0f, 1f)); //restamos porque distanciamos el componente gui hacia la izquierda
			float yEscalada = this.posicionDelAnclado.y + (dimensionPantalla.y*yPorcentaje);//Mathf.Clamp(yPorcentaje, 0f, 1f)); 
			
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
			float xEscalada = this.posicionDelAnclado.x + (dimensionPantalla.x*xPorcentaje);//Mathf.Clamp(xPorcentaje, 0f, 1f)); 
			float yEscalada = this.posicionDelAnclado.y + (dimensionPantalla.y/2*yPorcentaje);//Mathf.Clamp(yPorcentaje, -1f, 1f)); //Dividir entre 2 dimensionPantalla.x para no salirnos de los bordes. -1f de minimo (-100%) para distanciarlo hacia abajo
			
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
			float xEscalada = this.posicionDelAnclado.x + (dimensionPantalla.x/2*xPorcentaje);//Mathf.Clamp(xPorcentaje, -1f, 1f)); //Dividir entre 2 dimensionPantalla.x para no salirnos de los bordes. -1f de minimo (-100%) para distanciarlo hacia la izq
			float yEscalada = this.posicionDelAnclado.y + (dimensionPantalla.y/2*yPorcentaje);//Mathf.Clamp(yPorcentaje, -1f, 1f)); //Dividir entre 2 dimensionPantalla.x para no salirnos de los bordes. -1f de minimo (-100%) para distanciarlo hacia abajo
			
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
			float xEscalada = this.posicionDelAnclado.x - (dimensionPantalla.x*xPorcentaje);//Mathf.Clamp(xPorcentaje, 0f, 1f)); //restamos porque distanciamos el componente gui hacia la izquierda 
			float yEscalada = this.posicionDelAnclado.y + (dimensionPantalla.y/2*yPorcentaje);//Mathf.Clamp(yPorcentaje, -1f, 1f)); //Dividir entre 2 dimensionPantalla.x para no salirnos de los bordes. -1f de minimo (-100%) para distanciarlo hacia abajo
			
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
			float xEscalada = this.posicionDelAnclado.x + (dimensionPantalla.x*xPorcentaje);//Mathf.Clamp(xPorcentaje, 0f, 1f)); 
			float yEscalada = this.posicionDelAnclado.y - (dimensionPantalla.y*yPorcentaje);//Mathf.Clamp(yPorcentaje, 0f, 1f)); //restamos porque distanciamos el componente gui hacia arriba
				
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
			float xEscalada = this.posicionDelAnclado.x + (dimensionPantalla.x/2*xPorcentaje);//Mathf.Clamp(xPorcentaje, -1f, 1f)); //Dividir entre 2 dimensionPantalla.x para no salirnos de los bordes. -1f de minimo (-100%) para distanciarlo hacia la izq
			float yEscalada = this.posicionDelAnclado.y - (dimensionPantalla.y*yPorcentaje);//Mathf.Clamp(yPorcentaje, 0f, 1f)); //restamos porque distanciamos el componente gui hacia arriba
				
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
			float xEscalada = this.posicionDelAnclado.x - (dimensionPantalla.x*xPorcentaje);//Mathf.Clamp(xPorcentaje, 0f, 1f)); //restamos porque distanciamos el componente gui hacia la izquierda
			float yEscalada = this.posicionDelAnclado.y - (dimensionPantalla.y*yPorcentaje);//Mathf.Clamp(yPorcentaje, 0f, 1f)); //restamos porque distanciamos el componente gui hacia arriba
				
			return new Vector2(xEscalada, yEscalada);
		}
		
		#endregion
		
		
		#region implementacion del IComparable
		
		/// <summary>
		/// Ordena los GUI Componentes segun su valor de profundidad. Una profundidad mayor indica que el componente
		/// se dibujara mas abajo. Una profundidad menor, indica que el componente se dibujara mas al frente (arriba).
		/// 
		/// Por lo que, tenemos que modificar el CompareTo para que funcione al revés.
		/// </summary>
		/// <returns>
		/// -1 si la profundidad del otro componente es menor. 0 si tienen la misma profundidad. +1 si la profundidad del otro 
		/// componente es mayor.
		/// </returns>
		/// <param name='otroComponente'>
		/// El otro gui componente
		/// </param>
		public int CompareTo(System.Object otroComponente){
			int res = 0;
			GUIComponente aux = (GUIComponente) otroComponente;
			
			if(aux.profundidad > this.profundidad){
				res = 1;	
			}
			else if(aux.profundidad < this.profundidad){
				res = -1;	
			}
			
			return res;
		}
		
		#endregion		
	}
}