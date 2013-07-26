using UnityEngine;
using System.Collections.Generic;
using GUIMultiresolucion.Core.Fuentes;

public class Texto : MonoBehaviour {
	#region atributos de configuracion
	/// <summary>
	/// Textura de la tipografia
	/// </summary>
	public Texture2D texturaFuente;
	
	/// <summary>
	/// El espacio, en pixeles, que existe entre cada linea del texto
	/// </summary>
	public int espacioLineas = 30;
	
	/// <summary>
	/// El espacio, en pixeles, que existe entre cada letra
	/// </summary>
	public int espacioLetras = 10;
	#endregion
	
	#region atributos privados
	private string nombreTipografia;
	private Texture2D resultado;
	#endregion
	
	// Use this for initialization
	void Start () {
		int alturaLinea = 79;
		string texto = "hola";
		Color[] pixelesResultado = null;
		int anchuraTotal = 0;
		
		nombreTipografia = texturaFuente.name;
		
		Fuente fuente = seleccionarFuente();
		
		if(fuente != null){		
			Fuente.CustomChar[] simbolos = fuente.GetCharsOfString(texto); //obtenemos los simbolos del texto
			Dictionary<int, Color[]> pixelesLetras = new Dictionary<int, Color[]>(); //diccionario que relaciona el codigo ascii de la letra con los pixeles que le corresponden a esa letra en la textura original de la tipografia
			Dictionary<int, int> filaPixelesRellenando = new Dictionary<int, int>(); //diccionario que relaciona el codigo ascii de la letra con la fila de pixeles que se esta rellenando
			Color[] pixelesDeLaLetra = null;
			
			//obtenemos la anchura total de la textura resultante para el texto a dibujar
			foreach(Fuente.CustomChar s in simbolos){
				anchuraTotal += s.w;
				
				//vamos rellenando el diccionario con los pixeles correspondientes a cada letra, sin repetirlos
				if(!pixelesLetras.ContainsKey(s.charID)){
					pixelesDeLaLetra = texturaFuente.GetPixels(s.posX, texturaFuente.height - s.posY - s.h, s.w, s.h); //obtenemos los pixeles que le corresponden a la letra de la textura tipografia
					pixelesLetras.Add (s.charID, pixelesDeLaLetra); //adjuntamos el codigo ascii de la letra y los pixeles de la misma al diccionario
				}
			}
			
			pixelesResultado = new Color[anchuraTotal*alturaLinea]; //instanciamos el array de pixeles del resultado final
			
			//vamos a rellenar los pixeles del resultado final
			for(int i=0; i<pixelesResultado.Length;){
				foreach(Fuente.CustomChar c in simbolos){ //recorremos el texto para ir adjuntando la zona de la textura que le corresponde a cada caracter que lo compone
			
					Color[] pixelesCaracter = null;
					bool pixelesObtenidos = pixelesLetras.TryGetValue(c.charID, out pixelesCaracter); //intentamos obtener los pixeles que le corresponden al caracter del texto que vamos recorriendo
					
					if(pixelesObtenidos){ //si se obtienen los pixeles del diccionario creado previamente
						foreach(Color pixel in pixelesCaracter){ //recorremos los pixeles del caracter
							pixelesResultado[i] = pixel; //vamos adjuntando cada pixel del caracte al resultado final
							i++;
						}
						
						int areaCaracter = c.h * c.w; //area que ocupan los pixeles del caracter
						int areaRellenar = alturaLinea * c.w; //el area que se debe rellenar con el caracter y con pixeles transparentes si no se ha rellenado con el caracter el area por completo
						
						if(areaCaracter < areaRellenar){ //si el area del caracter es menor al area que se tiene que rellenar, adjuntamos pixeles transparentes para completar el relleno
							int pixelesRestantes = areaRellenar - areaCaracter; //los pixeles que quedan para rellenar el area completa
							
							for(int k=0; k<pixelesRestantes; k++){ //rellenamos de pixeles transparentes el array con los pixeles del resultado final
								Color pixelTransparente = Color.white; //un pixel blanco
								
								pixelesResultado[i] = pixelTransparente; //adjuntamos el pixel transparente
								i++;
							}
						}
					}
				}
			}
			
			resultado = new Texture2D(anchuraTotal, alturaLinea, TextureFormat.ARGB32, false);
			resultado.SetPixels(pixelesResultado);	
			resultado.Apply();
		}
		else{
			Debug.LogError("Fuente no seleccionada, revisar el nombre de la fuente");	
		}
	}
	
	void OnGUI(){
		if(resultado != null && resultado.GetPixels().Length > 0){
			GUI.DrawTexture(new Rect(100, 200, resultado.width, resultado.height), resultado);
		}
	}
	
	/// <summary>
	/// Selecciona la fuente que se quiere usar segun el nombre de la tipografia
	/// </summary>
	/// <returns>
	/// La fuente.
	/// </returns>
	private Fuente seleccionarFuente(){
		bool encontrada = false;
		Fuente fuente = null;
		
		foreach(Fuente f in Fuentes.fuentes){
			if(f.NombreFichero == nombreTipografia){
				fuente = f;
				break;
			}
		}	
		
		return fuente;
	}
	
	/// <summary>
	/// Obtiene los pixeles de una fila concreta
	/// </summary>
	/// <returns>
	/// La fila de pixeles
	/// </returns>
	/// <param name='todosPixeles'>
	/// Todos pixeles.
	/// </param>
	/// <param name='pixelPartida'>
	/// El pixel del conjunto total de pixeles a partir del cual se empieza a crear la fila
	/// </param>
	/// <param name='anchuraFila'>
	/// Anchura fila.
	/// </param>
	private Color[] pixelesFila(Color[] todosPixeles, int pixelPartida, int anchuraFila){
		Color[] res = null;
		
		for(int i=0; i<anchuraFila; i++, pixelPartida++){
			res[i] = todosPixeles[pixelPartida];	
		}
		
		return res;
	}
	
}
