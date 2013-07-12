using UnityEngine;
using System.Collections;
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
		
		public GUIBoton[] botones;
		public GUIImagen[] imagenes;
		
		private ArrayList botonesAL = new ArrayList();
		private ArrayList imagenesAL = new ArrayList();
		
		public GUIStyle p_buttonStyle;
		public string buttonStyle; //nombre del estilo para el boton
		
		
		/// <summary>
		/// la camara de la gui, que es ortografica.
		/// No crear mas camaras ortograficas, solo con proyeccion perspectiva. 
		/// Si se quiere una camara ortografica, usar esta que se crea sola con el prefab GUIMultiresolucion.
		/// </summary>
		private Camera camGUI;
		#endregion
		
		#region atributos privados
		private DeviceOrientation orientacionPrevia;
		#endregion
		
		#region Unity
		
		void Awake(){
			orientacionPrevia = Input.deviceOrientation;
			camGUI = GameObject.Find("GUIMultiresolucion").GetComponent<Camera>(); 
			
			GUIEscalador.inicializar(camGUI, anchoNativo, altoNativo);
			
			//primero ordenamos los gui componentes segun su profundidad para que se muestren en orden
			//primero los menos profundos, y debajo de estos los mas profundos
			ordenarComponentesADibujar();
			
			inicializarComponentes();
			
			//TEST
	//		GameObject cubo =  GameObject.CreatePrimitive(PrimitiveType.Cube);
	//		cubo.name = "cubo";
	//		cubo.transform.localScale = new Vector3(botones[0].anchura, botones[0].altura, 1);
		}
		
		void Update(){
			if (Input.deviceOrientation != orientacionPrevia && (Input.deviceOrientation == DeviceOrientation.Portrait || Input.deviceOrientation == DeviceOrientation.PortraitUpsideDown)){
				Debug.Log("actualizando guiescalador para Portrait");	
			}
			else if(Input.deviceOrientation != orientacionPrevia && (Input.deviceOrientation == DeviceOrientation.LandscapeLeft || Input.deviceOrientation == DeviceOrientation.LandscapeRight)){
				Debug.Log("actualizando guiescalador para Apaisado");	
			}
		}
		
		void OnGUI(){
			//aqui iniciamos a escalar la GUI para cualquier tipo de resolucion
	        GUIEscalador.InicioGUI(); 
			
			
		  		//asignamos un skin a los botones      
		        if(p_buttonStyle == null)
		        {
		            p_buttonStyle = GUI.skin.FindStyle(buttonStyle);
		        }
		      
		    	//dibuja todos los componentes GUI adjuntados
				dibujarComponentes();
			
				
			
	
	        //finalizamos el escalado de la GUI restaurando la GUI.matrix
	        GUIEscalador.FinGUI();
	    }
		
		#endregion
		
		
		#region metodos privados
		
		private void inicializarComponentes(){
			//inicializamos los botones
			foreach (GUIBoton t in botones){
				t.inicializar();
	        }
		}
		
		private void ordenarComponentesADibujar(){
			//ordenamos los botones
			foreach (GUIBoton t in botones){
				botonesAL.Add(t);
				
	            //Pressed(buttons[i].name);
	        }
	
			botonesAL.Sort(); //ordenar los botones por la profundidad (ver implementacion de CompareTo() en GUIComponente)
			
			//ordenamos la imagenes
			foreach(GUIImagen i in imagenes){
				imagenesAL.Add(i);
			}
			
			imagenesAL.Sort();//ordenar las imagenes por la profundidad (ver implementacion de CompareTo() en GUIComponente)
		}
		
		/// <summary>
		/// Dibuja todos los componentes de la GUI que se han adjuntado
		/// </summary>
		private void dibujarComponentes(){
			//dibuja los botones
			foreach(GUIBoton b in botonesAL){
				//si se pulsa un boton
				GUI.Button(b.distribucion,b.TexturaDibujar, p_buttonStyle);
	//			DibujarRectangulo(b.distribucion, Color.black); //solo para TEST
				
				
			}	
			
			
			//dibuja las imagenes		
			foreach(GUIImagen i in imagenesAL){
				GUI.DrawTexture(i.distribucion, i.textura);
			}
			
		}
		
		/// <summary>
		/// Dibuja un rectangulo
		/// </summary>
		/// <param name='distribucion'>
		/// La distribucion del rectangulo
		/// </param>
		/// <param name='color'>
		/// El color del rectangulo
		/// </param>
		void DibujarRectangulo(Rect distribucion, Color color) {
			Texture2D rgb_texture = new Texture2D((int) distribucion.width, (int) distribucion.height);
		    Color rgb_color = color;
		    int i, j;
		    for(i = 0;i<distribucion.width;i++)
		    {
		        for(j = 0;j<distribucion.height;j++)
		        {
		            rgb_texture.SetPixel(i, j, rgb_color);
		        }
		    }
		    rgb_texture.Apply();
		    GUIStyle generic_style = new GUIStyle();
		    GUI.skin.box = generic_style;
		    GUI.Box (distribucion, rgb_texture);
		}
		
		#endregion
	}
}