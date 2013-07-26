using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using GUIMultiresolucion.Core.Fuentes;

public class Texto : MonoBehaviour {
	#region atributos de configuracion
	public string texto;
	
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
	private Color pixelTransparente;
	#endregion
	
	// Use this for initialization
	void Start () {
		pixelTransparente = new Color(1, 1, 1, 1); //inicializamos el pixel transparente
		int alturaLinea = 79; //altura, en pixeles, de la linea
		Color[] pixelesResultado = null;
		int anchuraTotal = 0;
		
		nombreTipografia = texturaFuente.name;
		
		Fuente fuente = seleccionarFuente();
		
		if(fuente != null){		
			Fuente.CustomChar[] simbolos = fuente.GetCharsOfString(texto); //obtenemos los simbolos del texto
			Dictionary<int, Color[]> pixelesLetras = new Dictionary<int, Color[]>(); //diccionario que relaciona el codigo ascii de la letra con los pixeles que le corresponden a esa letra en la textura original de la tipografia
			Dictionary<Fuente.CustomChar, int> pixelPartidaRellenarFila = new Dictionary<Fuente.CustomChar, int>(); //diccionario que relaciona el simbolo de la letra con el pixel del partida, a partir del cual se rellena la fila
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
				for(int j=0; j<alturaLinea; j++){ //recorremos los simbolos tantas veces como numero de simbolos haya * la altura de linea (en pixeles)
//					Fuente.CustomChar simbolo = simbolos[j]; //obtenemos el simbolo a recorrer
					foreach(Fuente.CustomChar simbolo in simbolos){
						Color[] pixelesSimbolo = pixelesLetras[simbolo.charID]; //obtener los pixeles que le corresponden al caracter del texto que vamos recorriendo
						
	//					//---------
	//					//actualizar pixel de partida para rellenar el resultado final
	//					//---------
	//					if(!pixelPartidaRellenarFila.ContainsKey(simbolo)){ //si el simbolo no esta aun en el diccionario lo introducimos con su valor inicial del pixel de partida
	//						pixelPartidaRellenarFila.Add(simbolo, 0); //inicializacion del diccionario con el codigo ascii del simbolo y el pixel de partida para rellenar la fila el 0
	//					}
	//					else{
	//						pixelPartidaRellenarFila[simbolo] += simbolo.w; //actualizamos el pixel de partida para rellenar la fila 
	//					}
						
						//---------
						//obtenemos la fila de pixeles de cada simbolo
						//---------
	//					Color[] filaPixeles = pixelesFila(pixelesSimbolo, pixelPartidaRellenarFila[simbolo], simbolo.w);
						Color[] filaPixeles = pixelesFila(pixelesSimbolo, simbolo.w*j, simbolo.w);
						
						//---------
						//asignamos la fila al resultado final
						//---------
						foreach(Color p in filaPixeles){
							pixelesResultado[i] = p;
							i++;
						}
						
//						//---------
//						//actualizar inidice j de recorrido de los simbolos si no se ha terminado
//						//---------
//						if(j == simbolos.Length-1){ //recorriendo ultimo simbolo
//							j = 0; //empezamos en el primer simbolo de nuevo
//						}
//						else{
//							j++; //continuamos con el proximo simbolo	
//						}
					}
					
				}
				
				
				
//				foreach(Fuente.CustomChar c in simbolos){ //recorremos el texto para ir adjuntando la zona de la textura que le corresponde a cada caracter que lo compone
//			
//					Color[] pixelesCaracter = null;
//					bool pixelesObtenidos = pixelesLetras.TryGetValue(c.charID, out pixelesCaracter); //intentamos obtener los pixeles que le corresponden al caracter del texto que vamos recorriendo
//					
//					if(pixelesObtenidos){ //si se obtienen los pixeles del diccionario creado previamente
//						foreach(Color pixel in pixelesCaracter){ //recorremos los pixeles del caracter
//							pixelesResultado[i] = pixel; //vamos adjuntando cada pixel del caracte al resultado final
//							i++;
//						}
//						
//						int areaCaracter = c.h * c.w; //area que ocupan los pixeles del caracter
//						int areaRellenar = alturaLinea * c.w; //el area que se debe rellenar con el caracter y con pixeles transparentes si no se ha rellenado con el caracter el area por completo
//						
//						if(areaCaracter < areaRellenar){ //si el area del caracter es menor al area que se tiene que rellenar, adjuntamos pixeles transparentes para completar el relleno
//							int pixelesRestantes = areaRellenar - areaCaracter; //los pixeles que quedan para rellenar el area completa
//							
//							for(int k=0; k<pixelesRestantes; k++){ //rellenamos de pixeles transparentes el array con los pixeles del resultado final
//								Color pixelTransparente = Color.white; //un pixel blanco
//								
//								pixelesResultado[i] = pixelTransparente; //adjuntamos el pixel transparente
//								i++;
//							}
//						}
//					}
//				}
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
		Color[] fila = new Color[anchuraFila];
		
		//si el pixel de partida esta fuera de los limites del array de todos los pixeles
		if(pixelPartida >= todosPixeles.Length){
			for(int i=0; i<anchuraFila; i++){
				fila[i] = pixelTransparente; //adjuntamos el pixeles transparentes a la fila
			}
		}
		else{
			for(int i=0; i<anchuraFila; i++, pixelPartida++){
				fila[i] = todosPixeles[pixelPartida];	
			}
		}
		
		return fila;
	}	
}
