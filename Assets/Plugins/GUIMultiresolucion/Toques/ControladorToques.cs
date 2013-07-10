using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GUIMultiresolucion;
using GUIMultiresolucion.Core;
using GUIMultiresolucion.GUIComponentes;
using GUIMultiresolucion.Toques.Entrada;

namespace GUIMultiresolucion.Toques{
	[AddComponentMenu("GUIMultiresolucion/Controlador Toques")]
	public class ControladorToques: MonoBehaviour{
		/// <summary>
	    /// Ratio of cm to inch
	    /// </summary>
	    public const float CM_TO_INCH = 0.393700787f;
	
	    /// <summary>
	    /// Ratio of inch to cm
	    /// </summary>
	    public const float INCH_TO_CM = 1/CM_TO_INCH;
		
		#region private Variables
		/// <summary>
		/// Instancia unica del control de toques
		/// </summary>
	    private static ControladorToques instancia;
		private List<Toque> toquesActivos = new List<Toque>();
		private Dictionary<int, Toque> idParaElToque = new Dictionary<int, Toque>();
		
		// toques que empezaran a gestionarse
	    private List<Toque> toquesIniciados = new List<Toque>();
	    private List<Toque> toquesFinalizados = new List<Toque>();
	    private List<Toque> toquesCancelados = new List<Toque>();
	    private Dictionary<int, Vector2> toquesMovidos = new Dictionary<int, Vector2>();
		
		private int siguienteIDToque = 0;
		
		// Locks
	    private readonly object sync = new object();
		#endregion
	 	
		#region atributos publicos
	
	    /// <summary>
	    /// La instancia unica del control de toques
	    /// </summary>
	    public static ControladorToques Instancia
	    {
	        get
	        {
	            if (instancia == null)
	            {
	                instancia = FindObjectOfType(typeof(ControladorToques)) as ControladorToques;
	                
					if (instancia == null && Application.isPlaying)
	                {
	                    var go = GameObject.Find("GUIMultiresolucion");
	                    if (go == null) go = new GameObject("GUIMultiresolucion");
	                    instancia = go.AddComponent<ControladorToques>();
	                }
	            }
	            return instancia;
	        }
	    }
		
		/// <summary>
	    /// Number of active touches.
	    /// </summary>
	    public int NumToquesActivos
	    {
	       get{return toquesActivos.Count;}
	    }
	
	    /// <summary>
	    /// lista de toques activos
	    /// </summary>
	    public List<Toque> ToquesActivos
	    {
	        get { return new List<Toque>(toquesActivos); }
	    }
	
	    #endregion
		
		
		#region Eventos
	
	    /// <summary>
	    /// Ocurre cuando comienzan a aparecer toques
	    /// </summary>
	    /// 
	    public event EventHandler<ArgsEventosToques> ToquesIniciados
	    {
	        add { invocadorToquesIniciados += value; }
	        remove { invocadorToquesIniciados -= value; }
	    }
	
	    /// <summary>
	    /// Ocurre cuando los toques cambian
	    /// </summary>
	    public event EventHandler<ArgsEventosToques> ToquesMovidos
	    {
	        add { invocadorToquesMovidos += value; }
	        remove { invocadorToquesMovidos -= value; }
	    }
	
	    /// <summary>
	    /// Ocurre cuando los toques son eliminados
	    /// </summary>
	    public event EventHandler<ArgsEventosToques> ToquesFinalizados
	    {
	        add { invocadorToquesFinalizados += value; }
	        remove { invocadorToquesFinalizados -= value; }
	    }
	
	    /// <summary>
	    /// Ocurre cuando los toques son cancelados
	    /// </summary>
	    public event EventHandler<ArgsEventosToques> ToquesCandelados
	    {
	        add { invocadorToquesCancelados += value; }
	        remove { invocadorToquesCancelados -= value; }
	    }
	
	    // iOS Events AOT hack
	    private EventHandler<ArgsEventosToques> invocadorToquesIniciados, invocadorToquesMovidos,
	                                         invocadorToquesFinalizados, invocadorToquesCancelados;
	
	    #endregion
		
		
		#region metodos publicos
	
	    /// <summary>
	    /// Registra un toque
	    /// </summary>
	    /// <param name="posicion">Posicion del toque</param>
	    /// <returns>ID interno para el nuevo toque</returns>
	    public int iniciarToque(Vector2 posicion)
	    {
	        Toque toque;
			
	        lock (sync)
	        {
	            toque = new Toque(siguienteIDToque++, posicion);
	            toquesIniciados.Add(toque);
	        }
	        return toque.Id;
	    }
	
	    /// <summary>
	    /// Finaliza un toque con su id
	    /// </summary>
	    /// <param name="id">ID del toque a finalizar</param>
	    public void finalizarToque(int id)
	    {
	        lock (sync)
	        {
	            Toque toque;
	            if (!idParaElToque.TryGetValue(id, out toque))
	            {
	                foreach (var toqueAdjuntado in toquesIniciados)
	                {
	                    if (toqueAdjuntado.Id == id)
	                    {
	                        toque = toqueAdjuntado;
	                        break;
	                    }
	                }
	                if (toque == null) return;
	            }
	            toquesFinalizados.Add(toque);
	        }
	    }
	
	    /// <summary>
	    /// Cancela un toque
	    /// </summary>
	    /// <param name="id">IOD del toque a cancelar</param>
	    public void cancelarToque(int id)
	    {
	        lock (sync)
	        {
	            Toque toque;
	            if (!idParaElToque.TryGetValue(id, out toque))
	            {
	                foreach (var toqueAdjuntado in toquesIniciados)
	                {
	                    if (toqueAdjuntado.Id == id)
	                    {
	                        toque = toqueAdjuntado;
	                        break;
	                    }
	                }
	                if (toque == null) return;
	            }
	            toquesCancelados.Add(toque);
	        }
	    }
	
	    /// <summary>
	    /// Mueve un toque
	    /// </summary>
	    /// <param name="id">Id del toque a mover</param>
	    /// <param name="posicion">Nueva posicion para el toque</param>
	    public void moverToque(int id, Vector2 posicion)
	    {
	        lock (sync)
	        {
	            Vector2 actualizacion;
				
	            if (toquesMovidos.TryGetValue(id, out actualizacion))
	            {
	                toquesMovidos[id] = posicion;
	            } else
	            {
	                toquesMovidos.Add(id, posicion);
	            }
	        }
	    }
		
		#endregion
		
		#region metodos privados
		
		private void crearInputsParaToques(){
	        var inputs = FindObjectsOfType(typeof(EntradaToques));
			
	        if (inputs.Length == 0){
				//si la plataforma es para moviles
	            if (Application.platform == RuntimePlatform.IPhonePlayer || Application.platform == RuntimePlatform.Android){
	                gameObject.AddComponent<EntradaToquesMoviles>();
	            } else{
	                gameObject.AddComponent<EntradaToquesRaton>();
	            }
	        }
	    }
		
		private bool seHaTocadoGUIComponente(GUIComponente elementoGUI, Vector2 posicionToque){
			//corregimos el toque en pantalla a las coordenadas de pantalla escaladas
			Vector2 toqueEnPantallaEscalada = new Vector2(posicionToque.x / GUIEscalador.factorEscaladoX, posicionToque.y / GUIEscalador.factorEscaladoY);
			
			//condicion de tocado, que el toque ya escaladao esté dentro del rectangulo distribucion del objeto tocable
			bool tocado = (toqueEnPantallaEscalada.x >= elementoGUI.distribucion.x && toqueEnPantallaEscalada.x <= (elementoGUI.distribucion.x + elementoGUI.anchura))
						&& (toqueEnPantallaEscalada.y >= elementoGUI.distribucion.y && toqueEnPantallaEscalada.y <= (elementoGUI.distribucion.y + elementoGUI.altura));
			
			return tocado;
		}
		
		private bool actualizarToquesIniciados()
	    {
	        bool actualizado = false;
			
			if (toquesIniciados.Count > 0){
	            // get touches per target
	            var toquesObjetivo = new Dictionary<Transform, List<Toque>>();
	            
				Debug.Log("toques iniciados: " + toquesIniciados.Count);
				
				foreach (var toque in toquesIniciados)
	            {
	                toquesActivos.Add(toque);
	                idParaElToque.Add(toque.Id, toque);
					
					//si se ha tocado un componente GUI
					GUIBoton[] botones = GameObject.Find("GUIMultiresolucion").GetComponent<GUIMultiresolucion>().botones; //los botones de la gui
					bool tocado = false;
					
					foreach(GUIBoton boton in botones){
						tocado = seHaTocadoGUIComponente(boton, toque.Posicion);
						
						if(tocado){
							toque.Target = boton.transform;
							break;	
						}
					}
					
					if(tocado){
						
		               	if (toque.Target != null)
	                    {
							Debug.Log("target del toque: " + toque.Target.name);
							
	                        List<Toque> list;
							
	                        if (!toquesObjetivo.TryGetValue(toque.Target, out list))
	                        {
	                            list = new List<Toque>();
	                            toquesObjetivo.Add(toque.Target, list);
	                        }
	                        list.Add(toque);
	                    }
					}
	            }
				
				Debug.Log("toques objetivo: " + toquesObjetivo.Count);
	
	            // get touches per gesture
	            // touches can come to a gesture from multiple targets in hierarchy
//	            var gestureTouches = new Dictionary<Gesture, List<TouchPoint>>();
//	            var activeGestures = new List<Gesture>(); // no order in dictionary
	            
				
				foreach (var toque in toquesObjetivo.Keys)
	            {
					Debug.Log("toques objetivo: " + toque);
					
					
//	                var mightBeActiveGestures = getHierarchyContaining(target);
//	                var possibleGestures = getHierarchyEndingWith(target);
	                
//					foreach (var gesture in possibleGestures)
//	                {
//	                    if (!gestureIsActive(gesture)) continue;
//	
//	                    var canReceiveTouches = true;
//	                    foreach (var activeGesture in mightBeActiveGestures)
//	                    {
//	                        if (gesture == activeGesture) continue;
//	                        if ((activeGesture.State == Gesture.GestureState.Began || activeGesture.State == Gesture.GestureState.Changed) && (activeGesture.CanPreventGesture(gesture)))
//	                        {
//	                            canReceiveTouches = false;
//	                            break;
//	                        }
//	                    }
//	                    if (canReceiveTouches)
//	                    {
//	                        var touchesToReceive =
//	                            toquesObjetivo[target].FindAll((TouchPoint touch) => gesture.ShouldReceiveTouch(touch));
//	                        if (touchesToReceive.Count > 0)
//	                        {
//	                            if (gestureTouches.ContainsKey(gesture))
//	                            {
//	                                gestureTouches[gesture].AddRange(touchesToReceive);
//	                            } else
//	                            {
//	                                activeGestures.Add(gesture);
//	                                gestureTouches.Add(gesture, touchesToReceive);
//	                            }
//	                        }
//	                    }
//	                }
	            }
	
//	            foreach (var gesture in activeGestures)
//	            {
//	                if (gestureIsActive(gesture)) gesture.TouchesBegan(gestureTouches[gesture]);
//	            }
	
				Debug.Log("invocador toques iniciados: " + invocadorToquesIniciados); 
				
	            if (invocadorToquesIniciados != null){
					
					invocadorToquesIniciados(this, new ArgsEventosToques(new List<Toque>(toquesIniciados)));
				}
				
	            toquesIniciados.Clear();
	
	            actualizado = true;
	        }
			
	        return actualizado;
	    }
		
		#endregion
		
		#region Unity
		private void Awake(){
			crearInputsParaToques();	
		}
		
		private void Update()
	    {
	        actualizarToques();
			
			
	    }
		
		
		
		private void actualizarToques()
	    {
	        bool updated;
	        lock (sync)
	        {
	            updated = actualizarToquesIniciados();
//	            updated = updateMoved() || updated;
//	            updated = updateEnded() || updated;
//	            updated = updateCancelled() || updated;
	        }
	
//	        if (updated) resetGestures();
	    }
		#endregion
	}

}
