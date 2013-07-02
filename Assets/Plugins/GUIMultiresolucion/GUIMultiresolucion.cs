using UnityEngine;
using System.Collections;
using TouchScript.Hit;

/// <summary>
/// Adjuntar a un GameObject vacio y añadir los componentes de la gui al array
/// </summary>
public class GUIMultiresolucion : MonoBehaviour {

	public GUIBoton[] botones;
	public GUIImagen[] imagenes;
	
	private ArrayList botonesAL = new ArrayList();
	private ArrayList imagenesAL = new ArrayList();
	
	public GUIStyle p_buttonStyle;
	public string buttonStyle; //nombre del estilo para el boton
	
	
	
	
	#region Unity
	
	void Start(){
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
	#if UNITY_IPHONE || UNITY_ANDROID
			foreach(Touch toque in Input.touches){
				foreach(GUIBoton b in botones){
					//si se ha tocado el boton
					if(b.seHaTocado(toque.position)){
						//segun la fase del toque
						switch(toque.phase){
							case TouchPhase.Began:
								b.inicioDelToque(); //delegamos en el inicio del toque
							break;
							case TouchPhase.Moved:
								b.moviendoToque(); //delegamos en el movimiento del toque
							break;
							case TouchPhase.Ended:
							case TouchPhase.Canceled:
								b.finDelToque(); //delegamos en el fin del toque
							break;
						}
					}	
				}
				
			}
	#endif
		
	#if UNITY_EDITOR
			if(Input.GetMouseButton(0)){
				foreach(GUIBoton b in botones){
					//si se ha tocado el boton
					if(b.seHaTocado(Input.mousePosition)){
						if(Input.GetMouseButtonDown(0)){
							b.inicioDelToque(); //delegamos en el inicio del toque
						}
						if(Input.GetMouseButtonUp(0)){
							b.finDelToque();  //delegamos en el fin del toque
						}
					}
				}
				
			}
	#endif
			
		
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
			GUI.Button(b.distribucion,b.texturaNormal, p_buttonStyle);
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
