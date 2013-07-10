using UnityEngine;
using System;
using System.Collections.Generic;
using GUIMultiresolucion.Core;

namespace GUIMultiresolucion.Toques{
	public class EventosToquesGUIComponente: MonoBehaviour{
		
		#region Private variables
	
	    private float limiteTiempo = float.PositiveInfinity;
	    private float tiempoInicial;
	
	    #endregion
		
		#region atributos publicos
	
	    private EstadosToque estadoDelToque = EstadosToque.POSIBLE;
		
		/// <summary>
	    /// Estado previo del toque
	    /// </summary>
	    public EstadosToque EstadoPrevioDelToque { get; private set; }
	
	    /// <summary>
	    /// Estado del toque actual
	    /// </summary>
	    public EstadosToque EstadoDelToque
	    {
	        get { return estadoDelToque; }
	        private set
	        {
	            EstadoPrevioDelToque = estadoDelToque;
	            estadoDelToque = value;
	
	            switch (value)
	            {
				case EstadosToque.INICIADO:
	                    inicioDelToque();
	                    break;
	                case EstadosToque.CAMBIADO:
	                    cambiandoToque();
	                    break;
	                case EstadosToque.CANCELADO:
	                    toqueCancelado();
	                    break;
	            }
	
	            if (invocadorEstadoCambiado != null){ 
					invocadorEstadoCambiado(this, new ArgsEventosToquesGUIComponentes(estadoDelToque, EstadoPrevioDelToque));
				}
	        }
	    }
	
		/// <summary>
	    /// los toques que se gestionan
	    /// </summary>
	    protected List<Toque> toquesActivos = new List<Toque>();
	
	    /// <summary>
	    /// lista de los toques activos
	    /// </summary>
	    public List<Toque> ToquesActivos
	    {
	        get { return new List<Toque>(toquesActivos); }
	    }
		
		#endregion
		
		#region Events
	
	    /// <summary>
	    /// Ocurre cuando un toque cambia de estado
	    /// </summary>
	    public event EventHandler<ArgsEventosToquesGUIComponentes> estadoCambiado
	    {
	        add { invocadorEstadoCambiado += value; }
	        remove { invocadorEstadoCambiado -= value; }
	    }
	
	    // necesario para superar las limitaciones del iOS AOT
	    private EventHandler<ArgsEventosToquesGUIComponentes> invocadorEstadoCambiado;
	
	    #endregion
		
		#region funciones internal
		 internal void ToquesIniciados(IList<Toque> toques)
	    {
	        toquesActivos.AddRange(toques);
	        toquesIniciados(toques);
	    }
	
	    internal void ToquesMovidos(IList<Toque> toques)
	    {
	        toquesMovidos(toques);
	    }
	
	    internal void ToquesFinalizados(IList<Toque> toques)
	    {
	        toquesActivos.RemoveAll(toques.Contains);
	        toquesFinalizados(toques);
	    }
	
	    internal void ToquesCancelados(IList<Toque> toques)
	    {
	        toquesActivos.RemoveAll(toques.Contains);
	        toquesCancelados(toques);
	    }
		#endregion
		
		#region otros metodos
		/// <summary>
	    /// Ignorar un toque
	    /// </summary>
	    /// <param name="touch">Toque a ignorar</param>
	    protected void ignorarToque(Toque toque)
	    {
	        toquesActivos.Remove(toque);
	    }
		
		/// <summary>
	    /// Intenta cambiar el estado del toque
	    /// </summary>
	    /// <param name="value">Nuevo estado</param>
	    /// <returns><c>true</c> si el estado se cambio, en otro caso <c>false</c>.</returns>
	    protected bool setEstadoToque(EstadosToque estadoNuevo)
	    {
	        bool estadoCambiado = true;
			
			if (estadoNuevo == estadoDelToque && estadoDelToque != EstadosToque.CAMBIADO){
				estadoCambiado = false;
			}
			else{
				estadoDelToque = estadoNuevo;
			}
			
	        return estadoCambiado;
	    }
		#endregion
		
		
		#region Callbacks
	
	    /// <summary>
	    /// Called when new touches appear.
	    /// </summary>
	    /// <param name="touches">The touches.</param>
	    protected virtual void toquesIniciados(IList<Toque> toques)
	    {
			if(toquesActivos.Count == toquesActivos.Count){
				tiempoInicial = Time.time; //obtenemos el tiempo inicial
				setEstadoToque (EstadosToque.INICIADO);
			}
		}
	
	    /// <summary>
	    /// Called for moved touches.
	    /// </summary>
	    /// <param name="touches">The touches.</param>
	    protected virtual void toquesMovidos(IList<Toque> toques)
	    {
			setEstadoToque(EstadosToque.CAMBIADO);
		}
	
	    /// <summary>
	    /// Called if touches are removed.
	    /// </summary>
	    /// <param name="touches">The touches.</param>
	    protected virtual void toquesFinalizados(IList<Toque> toques)
	    {
			if(toquesActivos.Count == 0){
				//si se ha superado el tiempo limite, cancelamos el toque
				if(Time.time - tiempoInicial > limiteTiempo){
					setEstadoToque(EstadosToque.CANCELADO);
				}
				else{
					setEstadoToque(EstadosToque.FINALIZADO);	
				}
			}
		}
	
	    /// <summary>
	    /// Called when touches are cancelled.
	    /// </summary>
	    /// <param name="touches">The touches.</param>
	    protected virtual void toquesCancelados(IList<Toque> toques)
	    {
			setEstadoToque(EstadosToque.CANCELADO);
		}
	
	
	    /// <summary>
	    /// Called when state is changed to Began.
	    /// </summary>
	    protected virtual void inicioDelToque()
	    {
			setEstadoToque(EstadosToque.INICIADO);
		}
	
	    /// <summary>
	    /// Called when state is changed to Changed.
	    /// </summary>
	    protected virtual void cambiandoToque()
	    {
			setEstadoToque(EstadosToque.CAMBIADO);
		}
	
	
	    /// <summary>
	    /// Called when state is changed to Cancelled.
	    /// </summary>
	    protected virtual void toqueCancelado()
	    {
			setEstadoToque(EstadosToque.CANCELADO);
		}
	
	    #endregion
	}
}