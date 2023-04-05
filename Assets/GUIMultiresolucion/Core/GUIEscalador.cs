using UnityEngine;
using System.Collections;
using System.Collections.Generic;

/// <summary>
/// Autor: Fran Garcia <www.fgarmo.com>
/// 
/// Se encarga de reescalar la GUI segun la resolucion de pantalla del dispositivo donde se ejecute el juego.
/// Para ello se tiene que especificar en sus constantes ANCHO_NATIVO y ALTO_NATIVO los valores en los que 
/// se han diseñado los elementos de la gui por el departamento de arte, es decir, la resolucion nativa 
/// a la que se ha diseñado. Por defecto, esta pensado para ser diseñado a una resolucion de 800x1280 pixeles.
/// 
/// Como usarla:
/// 
/// Se utiliza en el metodo de Unity void OnGUI().
/// Para ello, antes de crear los elementos con los metodos de la clase GUI de Unity en OnGUI() tenemos que
/// llamar a InicioGUI() y al final de la creacion terminar llamando a FinGUI();
/// </summary>
public class GUIEscalador {
	#region atributos estaticos
	/// <summary>
	/// La anchura nativa es a la que se han diseñado todos los elementos de la GUI
	/// </summary>
	public static float ANCHO_NATIVO {get; private set;}
	
	/// <summary>
	/// La altura nativa es a la que se han diseñado todos los elementos de la GUI
	/// </summary>
	public static float ALTO_NATIVO {get; private set;}  
	
	/// <summary>
	/// El ancho de la pantalla ya escalada
	/// </summary>
	public static float ANCHO_PANTALLA {get; private set;}
	
	/// <summary>
	/// La altura de la pantalla ya escalada
	/// </summary>
	public static float ALTO_PANTALLA {get; private set;}
	
	/// <summary>
	/// Es el factor que aplicamos al escalado en la coordenada X del elemento GUI.
	/// Esta relacionado con la altura del dispositivo en el que se ejecuta el juego 
	/// y el alto nativo usado para crear las texturas
	/// </summary>
	public static float factorEscaladoX {get; private set;} 
	
	/// <summary>
	/// The factor escalado y.
	/// </summary>
	public static float factorEscaladoY {get; private set;} 
	
	/// <summary>
	/// relacion entre pixeles y unidades de unity en camara ortografica inicializado para orientacion de pantalla portrait
	/// </summary>
	public static float pixelRatio {get; private set;} 
	
	#endregion
	
	#region atributos privados	
	/// <summary>
	/// Lista con las matrices 4x4 que se van usando para escalar la gui
	/// </summary>
	private static List<Matrix4x4>  matricesGUI = new List<Matrix4x4>();
	#endregion
	
	#region metodos estaticos
	/// <summary>
	/// Inicializa el GUIEscalador
	/// </summary>
	/// <param name='camGUI'>
	/// La camara de la gui
	/// </param>
	/// <param name='anchoNativo'>
	/// Ancho nativo de las texturas
	/// </param>
	/// <param name='altoNativo'>
	/// Alto nativo de las texturas
	/// </param>
	public static void inicializar(Camera camGUI, float anchoNativo, float altoNativo){
		//inicializamos las propiedades
		ANCHO_NATIVO = anchoNativo;
		ALTO_NATIVO = altoNativo;
		pixelRatio = (camGUI.orthographicSize * 2f) / camGUI.pixelHeight;
		
		bool esPortrait = Screen.height >= Screen.width;
		
		//segun la orientacion de pantalla calculamos los factores de escalado en la X y la Y de los elementos de la GUI
		calculaFactoresEscalado(esPortrait);
		
		//por ultimo, cuando ya se conocen todos los valores necesarios
		//calculamos el ancho y alto de la pantalla ya escalada
		Vector2 dimensionPantalla = dimensionPantallaEscalada();
		ANCHO_PANTALLA = dimensionPantalla.x;
		ALTO_PANTALLA = dimensionPantalla.y;
	}
	
	/// <summary>
	/// Actualiza el escalador segun sea la orientacion de la pantalla portrait o landscape (apaisado)
	/// </summary>
	/// <param name='esPortrait'>
	/// True si es portrait la orientacion de la pantalla, false si es landscape (apaisado)
	/// </param>
	public static void actualizar(bool esPortrait){
		calculaFactoresEscalado(esPortrait);
	}
	
	/// <summary>
	/// Calcula los factores de escalado segun sea la orientacion de la pantalla portrait o landscape (apaisado)
	/// </summary>
	/// <param name='esPortrait'>
	/// True si es portrait la orientacion de la pantalla, false si es landscape (apaisado)
	/// </param>
	private static void calculaFactoresEscalado(bool esPortrait){
		if(esPortrait){
			factorEscaladoX = Screen.height / ALTO_NATIVO; 
			factorEscaladoY = Screen.width / ANCHO_NATIVO;
		}
		else{
//			float aux = ALTO_NATIVO;
//			ALTO_NATIVO = ANCHO_NATIVO;
//			ANCHO_NATIVO = aux;
				
			factorEscaladoX = Screen.height / ANCHO_NATIVO; 
			factorEscaladoY = Screen.width / ALTO_NATIVO;
		}
	}
	
	/// <summary>
	/// La dimension de la pantalla escalada para todo tipo de resoluciones de pantalla
	/// Componente x: anchura, y:altura
	/// </summary>
	/// <returns>
	/// La componenete x del Vector2 es la anchura escalada, y la componente y la altura escalada
	/// </returns>
	private static Vector2 dimensionPantallaEscalada(){
		float anchuraDispositivo = Screen.width; //la anchura del dispositivo donde se esta ejecutando el juego
		float alturaDispositivo = Screen.height; //la anchura del dispositivo donde se esta ejecutando el juego
		float factorEscaladoAnchura = GUIEscalador.factorEscaladoX; //factor para escalar la anchura del componenete gui
		float factorEscaladoAltura = GUIEscalador.factorEscaladoY; //factor para escalar la altura del componenete gui
		
		float anchuraEscalada = (anchuraDispositivo/factorEscaladoAnchura);
		float alturaEscalada = (alturaDispositivo/factorEscaladoAltura);
		
		return new Vector2(anchuraEscalada, alturaEscalada);	
	}
	
	/// <summary>
	/// Este metodo debe ser llamado antes de empezar a crear los componentes de la gui, al principio del metodo OnGUI.
	/// Modifica la GUI.matrix con una matriz que la reescala segun la altura y anchura del dispositivo y la altura/anchura nativa
	/// </summary>
	public static void InicioGUI(){                
	    matricesGUI.Add(UnityEngine.GUI.matrix); 	
	    var m = new Matrix4x4();             
	    var escaladoEnX = 1f;           
	    var escaladoEnY = 1f;          
	   
	    escaladoEnX = factorEscaladoX;
	    escaladoEnY = factorEscaladoY;
	  
		if(factorEscaladoX == 0f){
			factorEscaladoX = 1f;	
		}
		if(factorEscaladoY == 0f){
			factorEscaladoY = 1f;	
		}
		
	    m.SetTRS(Vector3.zero, Quaternion.identity, new Vector3(factorEscaladoX, factorEscaladoY, 1));
	    UnityEngine.GUI.matrix *= m;
	}
	 
	/// <summary>
	/// Este metodo debe ser llamado al final de la creacion de los componenetes de la gui, al final del metodo ONGUI.
	/// Elimina la matriz creada para el reescalado y resetea la GUI.matrix con la anterior
	/// </summary>
	public static void FinGUI(){            
		UnityEngine.GUI.matrix = matricesGUI[matricesGUI.Count - 1];
      	matricesGUI.RemoveAt(matricesGUI.Count-1);
	}
	#endregion
}
