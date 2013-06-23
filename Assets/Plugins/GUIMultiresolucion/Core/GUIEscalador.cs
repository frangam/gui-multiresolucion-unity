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
	#region constantes
	/// <summary>
	/// La anchura nativa es a la que se han diseñado todos los elementos de la GUI
	/// </summary>
	public const float ANCHO_NATIVO = 800; 
	
	/// <summary>
	/// La altura nativa es a la que se han diseñado todos los elementos de la GUI
	/// </summary>
	public const float ALTO_NATIVO = 1280;          
	#endregion
	
	#region atributos estaticos
	/// <summary>
	/// Es el factor que aplicamos al escalado en la coordenada X del elemento GUI.
	/// Esta relacionado con la altura del dispositivo en el que se ejecuta el juego 
	/// y el alto nativo usado para crear las texturas
	/// </summary>
	public static float factorEscaladoX = Screen.height / ALTO_NATIVO; 
	
	/// <summary>
	/// The factor escalado y.
	/// </summary>
	public static float factorEscaladoY = Screen.width / ANCHO_NATIVO; //el factor que aplicamos al escalado en la coordenada Y esta relacionado con la anchura del dispositivo y el ancho usado para crear las texturas
	#endregion
	
	#region atributos privados
	/// <summary>
	/// Lista con las matrices 4x4 que se van usando para escalar la gui
	/// </summary>
	private static List<Matrix4x4>  matricesGUI = new List<Matrix4x4>();
	#endregion
	
	#region metodos estaticos
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
