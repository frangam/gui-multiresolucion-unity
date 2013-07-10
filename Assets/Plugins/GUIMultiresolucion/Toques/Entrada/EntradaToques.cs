using System;
using UnityEngine;

namespace GUIMultiresolucion.Toques.Entrada{
	public abstract class EntradaToques : MonoBehaviour {
	
		#region Private variables
	
	    /// <summary>
	    /// Referencia global al controlador de toques
	    /// </summary>
	    protected ControladorToques controladorToques;
	
	    #endregion
	
	    #region Unity
	
	    protected virtual void Start(){
	        controladorToques = ControladorToques.Instancia;
	        
			if (controladorToques == null){
				throw new InvalidOperationException("Se requiere una instancia de ControlToques!");
			}
	    }
	
	    protected virtual void OnDestroy(){
	        controladorToques = null;
	    }
	
	    protected virtual void Update(){
			
		}
	
	    #endregion
	
	    #region Callbacks
	
	    /// <summary>
	    /// Inicia un toque en las coordenadas de pantalla indicadas
	    /// </summary>
	    /// <param name="posicion">La posicion de la pantalla</param>
	    /// <returns>ID interno para el toque.</returns>
	    protected int iniciarToque(Vector2 posicion)
	    {
	        return  controladorToques.iniciarToque(posicion);
	    }
	
	    /// <summary>
	    /// Finaliza el toque con el id indicado del toque a finalizar
	    /// </summary>
	    /// <param name="id">ID del toque a finalizar</param>
	    protected void finalizarToque(int id)
	    {
	        controladorToques.finalizarToque(id);
	    }
	
	    /// <summary>
	    /// Mueve el toque con el id indicado del toque a mover
	    /// </summary>
	    /// <param name="id">ID del toque a mover.</param>
	    /// <param name="posicion">Nueva posicion de pantalla</param>
	    protected void moverToque(int id, Vector2 posicion)
	    {
	        controladorToques.moverToque(id, posicion);
	    }
	
	    /// <summary>
	    /// Cancela el toque con el id indicado del toque a cancelar
	    /// </summary>
	    /// <param name="id">ID del toque a cancelar</param>
	    protected void cancelarToque(int id)
	    {
	       controladorToques.cancelarToque(id);
	    }
	
	    #endregion
	}
}