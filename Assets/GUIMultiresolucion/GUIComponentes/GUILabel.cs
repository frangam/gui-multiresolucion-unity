using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using GUIMultiresolucion.Core;
using GUIMultiresolucion.Core.Fuentes;

namespace GUIMultiresolucion.GUIComponentes{
	/*
	 * El label representa un texto que se dibuja en la GUI.
	 * --------------------------------------------------------------------------------------------------------------------------
	 * 
	 * La construccion de dicho texto se realiza a partir de una cadena texto, que sera lo que se quiera escribir en pantalla,
	 * y de una textura, en concreto, un Bitmap Font, creado con software externos al juego de generacion de Bitmaps Fonts.
	 * Generador Bitmaps fonts pueden ser Hiero, Shoebox.
	 * Shoebox: http://renderhjs.net/shoebox/ (Requisito Adobe air: http://get.adobe.com/es/air/ )
	 * Tutorial Shoebox: https://www.youtube.com/watch?v=Arzk8h4lc1I 
	 * Info adicional: http://www.angelcode.com/products/bmfont/
	 * 
	 * Algoritmo:
	 * - Se transformar una cadena de texto a una textura, es decir, como tenemos un Bitmap Font con sus efectos y estilo gráfico aplicado a cada
	 * caracter, podemos a partir de una cadena de texto obtener cada trozo del Bitmap (no olvidemos que esto es una textura) y crear una textura final
	 * con cada trozo.
	 * 
	 * - Esquema: Tenemos que extraer cada trozo del Bitmap Font para crear la palabra Hola.
	 *  ----  ----  ----  ---- 
	 * | H | | o | | l | | a |  Para ello hacemos uso de la Clase Fuente en combinacion con SimboloLetra, para obtener las propiedades de cada simbolo (letra)
	 * ----  ----  ----  ----
	 * - Una vez que se tienen todos los simbolos, se obtienen los pixeles que le corresponden a este copiando los pixeles del trozo de la textura Bitmap Font
	 * que le corresponda, segun las propiedades que tenga el simbolo, que deben ser obtenidas desde un archivo .fnt generado por el software generador de Bitmap Font.
	 * 
	 * - Obtenemos los pixeles de la textura de la fuente (bitmap) con el metodo GetPixels(posicion X del simbolo, altura bitmap - posicion Y del simbolo - altura del simbolo, anchura del simbolo, altura del simbolo);
	 * 
	 * - Despues se van obteniendo los pixeles de cada simbolo que forma parte del texto final desde la fila 0, que es la fila más cercana al suelo y vamos rellenando 
	 * un array de Color[] que seran los pixeles del resultado final.
	 * 
	 * - Creando un array de Color[] para albergar los pixeles de la textura final. 
	 * 
	 * Unity interpreta este array, cuando se lo asignamos a una Texture2D, empezando su primer elemento a partir de la esquina inferior izquierda de la textura final
	 * y siendo su ultimo elemento (pixel) la esquina superior derecha como intentamos mostrar en el esquema siquiente:
	 *     ----------------------   ----------------------     
	 *    |-|-|-|-|-|-|-|-|-|-|-|  |   ultimo pixel << |-|
	 *   |-H-|-|-o-|-|-l-|-|-a-|  |                     |  
	 *  |-|-|-|-|-|-|-|-|-|-|-|  |-| >> primer pixel   | 
	 *  ----------------------   ----------------------
	 * 
	 * Sin olvidar que es una representacion grafica para comprenderlo, internamente es un array unidimensional Color[] pixeles:
	 * 
	 * primer elemento                                                                                                          ultimo elemento
	 *  ^                                                                                                                                 ^
	 * |-|-|-|-|-|-|-|-|-|-|-|-|-|-|-|-|-|-|-|-|-|-|-|-|-|-|-|-|-|-|-|-|-|-|-|-|-|-|-|-|-|-|-|-|-|-|-|-|-|-|-|-|-|-|-|-|-|-|-|-|-|-|-|-|-|-|
	 */ 
	public class GUILabel : GUIComponente {
	
		#region atributos de configuracion
		/// <summary>
		/// El texto que muestra el label
		/// </summary>
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
		public float PorcentajeSize = 1;
		
		#endregion
		
		#region atributos privados
		private string nombreTipografia;
		
		/// <summary>
		/// La textura que tendra finalmente el label y la que sera dibujada
		/// </summary>
		private Texture2D texturaFinalLabel;
		private Color pixelTransparente;
		int alturaLinea = 79; //altura, en pixeles, de la linea
		#endregion
		
		#region propiedades publicas
		public Rect distribucion{
			get{
				//cambiamos las dimensiones si la anchura/altura del componente es 0
				if(this.anchura == 0 || (texturaFinalLabel != null && texturaFinalLabel.GetPixels().Length > 0 && texturaFinalLabel.width != null)){
		        	this.anchura =  texturaFinalLabel.width*PorcentajeSize;
				}
				
				if(this.altura == 0 || (texturaFinalLabel != null && texturaFinalLabel.GetPixels().Length > 0 && texturaFinalLabel.height != null)){
					this.altura = texturaFinalLabel.height*PorcentajeSize;	
				}
				
				return base.distribucion;
			}
		}
		#endregion
		
		#region metodos sobreescritos
		public override void inicializar ()
		{
			generarTexturaLabel(); //generamos la textura del label
			
			//cambiamos las dimensiones si la anchura/altura del componente es 0
			if(this.anchura == 0 || (texturaFinalLabel != null && texturaFinalLabel.GetPixels().Length > 0 && texturaFinalLabel.width != null)){
	        	this.anchura =  texturaFinalLabel.width;
			}
			
			if(this.altura == 0 || (texturaFinalLabel != null && texturaFinalLabel.GetPixels().Length > 0 &&  texturaFinalLabel.height != null)){
				this.altura = texturaFinalLabel.height;	
			}

			base.inicializar ();
		}
		
		public override void dibujar ()
		{
			if(texto != null || texto != "" && texturaFinalLabel != null && texturaFinalLabel.GetPixels().Length > 0){
//				Debug.Log(distribucion);
//				Debug.Log(new Rect(100, 200, texturaFinalLabel.width, texturaFinalLabel.height));
//				Rect _dist = new Rect(distribucion.x, distribucion.y, distribucion.width * PorcentajeSize, distribucion.height * PorcentajeSize); 
				GUI.DrawTexture(distribucion, texturaFinalLabel);
			}
		}
		#endregion
		
		/// <summary>
		/// Crea la textura final para el label, es decir, el texto que se quiere mostrar como una imagen formada por cada una de las letras
		/// </summary>
		private void generarTexturaLabel () {
			

			pixelTransparente = new Color(1, 1, 1, 0); //inicializamos el pixel transparente
			Color[] pixelesResultado = null;
			int anchuraTotal = 0; //anchura total que debe tener la textura final del label
			int alturaTotal = 1; //altura total que debe tener la textura final del label
			
			nombreTipografia = texturaFuente.name;
			Fuente fuente = seleccionarFuente();
			
			
			
			if(fuente != null){		
			//	Fuente.SimboloLetra[] simbolos = fuente.GetCharsOfString(texto); //obtenemos los simbolos del texto
				List<List<Fuente.SimboloLetra>> simbolosPorLineas = fuente.TextoATrozos(texto, 2000); //conjunto de simbolos (letras) que tiene cada linea del texto a mostrar
				alturaLinea = fuente.CommonLineHeigth;
			
				
				//config alto y ancho textura.
				Dictionary<int, Color[]> pixelesLetras = new Dictionary<int, Color[]>(); //diccionario que relaciona el codigo ascii de la letra con los pixeles que le corresponden a esa letra en la textura original de la tipografia
				Color[] pixelesDeLaLetra = null;
				int anchuraMax = -1;
				int anchuraPalabra = 0;
				
				foreach(List<Fuente.SimboloLetra> simbolosDeLaLinea in simbolosPorLineas){ //recorrer cada palabra
					//obtenemos la anchura total de la textura resultante para el texto a dibujar
					foreach(Fuente.SimboloLetra s in simbolosDeLaLinea){ //recorre cada simbolo de la palabra recorrida
						anchuraPalabra += (s.w + s.offsetx);
						
						//vamos rellenando el diccionario con los pixeles correspondientes a cada letra, sin repetirlos
						if(!pixelesLetras.ContainsKey(s.charID)){
							pixelesDeLaLetra = texturaFuente.GetPixels(s.posX, texturaFuente.height - s.posY - s.h, s.w, s.h); //obtenemos los pixeles que le corresponden a la letra de la textura tipografia
							pixelesLetras.Add (s.charID, pixelesDeLaLetra); //adjuntamos el codigo ascii de la letra y los pixeles de la misma al diccionario
						}
					}
					
					//calculo de la maxima anchura de palabras
					if(anchuraPalabra > anchuraMax){
						anchuraMax = anchuraPalabra;
					}
					anchuraPalabra = 0;
				}
				
				anchuraTotal = anchuraMax; //asignar anchura de la maxima palabra a la anchura total de la textura final del label
				alturaTotal = fuente.CommonLineHeigth*simbolosPorLineas.Count; //altura de la textura final del label (numero de lineas de texto * altura comun de linea)
//				pixelesResultado = new Color[anchuraTotal*alturaLinea]; //instanciamos el array de pixeles del resultado final
				pixelesResultado = new Color[anchuraTotal*alturaTotal];
				
				//vamos a rellenar los pixeles del resultado final
				for(int i=0; i<pixelesResultado.Length;){		
					for(int k=simbolosPorLineas.Count-1; k>=0;k--){//recorrer cada linea del texto, empezando por la ultima
						for(int j=0; j<fuente.CommonLineHeigth; j++){ //recorremos los simbolos tantas veces como numero de simbolos haya * la altura de linea (en pixeles)
							int anchoActual = 0;
							foreach(Fuente.SimboloLetra simbolo in simbolosPorLineas[k]){ //recorrer los simbolos que componen la palabra recorrida
								anchoActual += simbolo.w+simbolo.offsetx;
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
							//si ancho menor que anchototal rellenar con transparente
							if(anchoActual<anchuraTotal){
								for(int p=0; p<anchuraTotal-anchoActual; p++, i++){
									pixelesResultado[i] = pixelTransparente;
								}
							}
						}
					}
					
				}
				
//				texturaFinalLabel = new Texture2D(anchuraTotal, alturaLinea, TextureFormat.ARGB32, false);
				texturaFinalLabel = new Texture2D(anchuraTotal, alturaTotal, TextureFormat.ARGB32, false);
				texturaFinalLabel.SetPixels(pixelesResultado);	
				texturaFinalLabel.Apply();
			}
			else{
				Debug.LogError("Fuente no seleccionada, revisar el nombre de la fuente");	
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
}