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
	int alturaLinea = 79; //altura, en pixeles, de la linea
	#endregion
	
	// Use this for initialization
	void Start () {
		pixelTransparente = new Color(1, 1, 1, 1); //inicializamos el pixel transparente
		Color[] pixelesResultado = null;
		int anchuraTotal = 0;
		
		nombreTipografia = texturaFuente.name;
		
		Fuente fuente = seleccionarFuente();
		
		if(fuente != null){		
			Fuente.SimboloLetra[] simbolos = fuente.GetCharsOfString(texto); //obtenemos los simbolos del texto
			Dictionary<int, Color[]> pixelesLetras = new Dictionary<int, Color[]>(); //diccionario que relaciona el codigo ascii de la letra con los pixeles que le corresponden a esa letra en la textura original de la tipografia
			Dictionary<Fuente.SimboloLetra, int> pixelPartidaRellenarFila = new Dictionary<Fuente.SimboloLetra, int>(); //diccionario que relaciona el simbolo de la letra con el pixel del partida, a partir del cual se rellena la fila
			Color[] pixelesDeLaLetra = null;
			
			//obtenemos la anchura total de la textura resultante para el texto a dibujar
			foreach(Fuente.SimboloLetra s in simbolos){
				anchuraTotal += (s.w + s.offsetx);
				
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
					foreach(Fuente.SimboloLetra simbolo in simbolos){
						Color[] pixelesSimbolo = pixelesLetras[simbolo.charID]; //obtener los pixeles que le corresponden al caracter del texto que vamos recorriendo
						
						//---------
						//obtenemos la fila de pixeles de cada simbolo
						//---------
						Color[] filaPixeles = obtenerPixelesFila(pixelesSimbolo, simbolo);
						
						//---------
						//asignamos la fila al resultado final
						//---------
						foreach(Color p in filaPixeles){
							pixelesResultado[i] = p;
							i++;
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
			GUI.DrawTexture(new Rect(100, 200, resultado.width*0.5f, resultado.height*0.5f), resultado);
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
	/// <param name='simbolo'>
	/// El simbolo de la letra
	/// </param>
	private Color[] obtenerPixelesFila(Color[] todosPixeles, Fuente.SimboloLetra simbolo){
		Color[] fila = new Color[simbolo.w+simbolo.offsetx];
		int pixelPartida = simbolo.w * simbolo.filaPartidaDibujar;
		
		//por debajo de la letra, hay espacios vacios
		if(simbolo.filaSuelo < (alturaLinea - simbolo.h - simbolo.offsety)){
			for(int i=0; i<(simbolo.offsetx+simbolo.w); i++){
				fila[i] = pixelTransparente; //adjuntamos los pixeles transparentes a la fila
			}
			
			simbolo.filaSuelo++;
		}
		//pixeles de la letra
		else if(simbolo.filaPartidaDibujar < alturaLinea-simbolo.offsety-(alturaLinea - simbolo.h - simbolo.offsety)){
			//si tiene offset en x rellenamos con transparentes
			for(int i=0; i<simbolo.offsetx; i++){
				fila[i] = pixelTransparente; //adjuntamos los pixeles transparentes a la fila
			}
			
			if(simbolo.filaPartidaDibujar < simbolo.h){					
				//rellenamos con los pixeles de la letra
				for(int i=simbolo.offsetx; i<simbolo.w+simbolo.offsetx; i++, pixelPartida++){
					fila[i] = todosPixeles[pixelPartida];	
				}	
				
				simbolo.filaPartidaDibujar++;
			}
		}
		//por encima de la letra hay espacios vacios
		else{		
			for(int i=0; i<simbolo.w+simbolo.offsetx; i++){
				fila[i] = pixelTransparente; //adjuntamos los pixeles transparentes a la fila
			}
		}
		
		return fila;
	}	
}
