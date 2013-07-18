using UnityEngine;
using System.Collections;
using GUIMultiresolucion.Core;
using GUIMultiresolucion.GUIComponentes;


namespace GUIMultiresolucion{
	[ExecuteInEditMode]
	/// <summary>
	/// Adjuntar a un GameObject vacio y añadir los componentes de la gui al array
	/// </summary>
	public class GUIMultiresolucion : MonoBehaviour {
		#region atributos configurables
		/// <summary>
		/// Ancho usado para crear las texturas
		/// </summary>
		public float anchoNativo = 800; 
		
		/// <summary>
		/// Alto usado para crear las texturas
		/// </summary>
		public float altoNativo = 1280;
		
		/// <summary>
		/// Los diferentes componentes de la GUI
		/// </summary>
		public GUIComponente[] componentesGUI;
		
//		public GUIStyle p_buttonStyle;
//		public string buttonStyle; //nombre del estilo para el boton
		
		
		/// <summary>
		/// la camara de la gui, que es ortografica.
		/// No crear mas camaras ortograficas, solo con proyeccion perspectiva. 
		/// Si se quiere una camara ortografica, usar esta que se crea sola con el prefab GUIMultiresolucion.
		/// </summary>
		private Camera camGUI;
		#endregion
		
		#region atributos privados
		/// <summary>
		/// True si la orientacion previa es en portrait, false si es en landscape (apaisado)
		/// </summary>
		private bool orientacionPreviaEraPortrait;
		
		/// <summary>
		/// los componentes de la gui ordenados por la profundidad, para que sean dibujados segun esta
		/// </summary>
		private ArrayList componentesGUIOrdenados = new ArrayList();
		#endregion
		
		#region Unity
		
		void Awake(){
			//condicion de pantalla en portrait.
			orientacionPreviaEraPortrait = Screen.height >= Screen.width;
			
			//obtenemos la camara de la gui
			camGUI = GameObject.Find("GUIMultiresolucion").GetComponent<Camera>(); 
			
			//inicializamos el escalador de la gui
			GUIEscalador.inicializar(camGUI, anchoNativo, altoNativo);
			
			//primero ordenamos los gui componentes segun su profundidad para que se muestren en orden
			//primero los menos profundos, y debajo de estos los mas profundos
			ordenarComponentesADibujar();
			
			//inicializamos los componentes de la gui
			inicializarComponentes();
		}
		
		void LateUpdate(){
//			//comprobamos si esta orientada en portrait o no la pantalla
//			bool esPortraitAhora = Screen.height >= Screen.width;
//			
//			//cambiando orientacion a Portrait
//			if (!orientacionPreviaEraPortrait && esPortraitAhora){
//				orientacionPreviaEraPortrait = true;
////				GUIEscalador.actualizar(orientacionPreviaEraPortrait);
////				actualizarComponentes();	
//			}
//			//cambiando orientacion a Landscape (apaisado)
//			else if(orientacionPreviaEraPortrait && !esPortraitAhora){
//				orientacionPreviaEraPortrait = false;
////				GUIEscalador.actualizar(orientacionPreviaEraPortrait);
////				actualizarComponentes();	
//			}
		}
		
		void OnGUI(){
			//aqui iniciamos a escalar la GUI para cualquier tipo de resolucion
	        GUIEscalador.InicioGUI(); 
			
			
//		  		//asignamos un skin a los botones      
//		        if(p_buttonStyle == null)
//		        {
//		            p_buttonStyle = GUI.skin.FindStyle(buttonStyle);
//		        }
		      
		    	//dibuja todos los componentes GUI adjuntados
				dibujarComponentes();
			
				
			
	
	        //finalizamos el escalado de la GUI restaurando la GUI.matrix
	        GUIEscalador.FinGUI();
	    }
		
		#endregion
		
		
		#region metodos privados
		/// <summary>
		/// inicializamos los componentes
		/// </summary>
		public void inicializarComponentes(){
			foreach (GUIComponente c in componentesGUI){
				c.inicializar();
	        }
		}
		
		public void actualizarComponentes(){
			foreach (GUIComponente c in componentesGUI){
				c.actualizar();
	        }
		}
		
		/// <summary>
		/// Ordena los componentes de la gui segun la profundidad que tengan
		/// </summary>
		private void ordenarComponentesADibujar(){
			//adjuntamos al array list todos los componentes de la gui
			foreach(GUIComponente c in componentesGUI){
				componentesGUIOrdenados.Add(c);
			}
			//y los ordenamos
			componentesGUIOrdenados.Sort();
		}
		
		/// <summary>
		/// Dibuja todos los componentes de la GUI que se han adjuntado
		/// </summary>
		private void dibujarComponentes(){
			//para dibujar los componentes recorremos el arraylist que los contiene ya ordenados por la profundidad
			foreach(GUIComponente c in componentesGUIOrdenados){
				//si se puede ver lo dibujamos
				if(c.Visible){
					c.dibujar();
				}
			}
		}
		
//		/// <summary>
//		/// Dibuja un rectangulo
//		/// </summary>
//		/// <param name='distribucion'>
//		/// La distribucion del rectangulo
//		/// </param>
//		/// <param name='color'>
//		/// El color del rectangulo
//		/// </param>
//		void DibujarRectangulo(Rect distribucion, Color color) {
//			Texture2D rgb_texture = new Texture2D((int) distribucion.width, (int) distribucion.height);
//		    Color rgb_color = color;
//		    int i, j;
//		    for(i = 0;i<distribucion.width;i++)
//		    {
//		        for(j = 0;j<distribucion.height;j++)
//		        {
//		            rgb_texture.SetPixel(i, j, rgb_color);
//		        }
//		    }
//		    rgb_texture.Apply();
//		    GUIStyle generic_style = new GUIStyle();
//		    GUI.skin.box = generic_style;
//		    GUI.Box (distribucion, rgb_texture);
//		}
		
		#endregion
	}
}