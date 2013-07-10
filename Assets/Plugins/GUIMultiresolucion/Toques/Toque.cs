using UnityEngine;

namespace GUIMultiresolucion.Toques{
	public class Toque {
	
		/// <summary>
	    /// Inicializa una nueva instancia de la clase <see cref="Toque"/>.
	    /// </summary>
	    /// <param name="id">ID para el toque</param>
	    /// <param name="position">Posicion de la pantalla tocada</param>
	    public Toque(int id, Vector2 position)
	    {
	        Id = id;
	        Posicion = position;
	        PosicionPrevia = position;
	    }
	
	    /// <summary>
	    /// ID interno unico para el toque
	    /// </summary>
	    public int Id { get; private set; }
	
	    private Vector2 posicion = Vector2.zero;
	
	    /// <summary>
	    /// Posicion actual del toque
	    /// </summary>
	    public Vector2 Posicion{
	        get { return posicion; }
			
	        set{
	            PosicionPrevia = posicion;
	            posicion = value;
	        }
	    }
	
	    /// <summary>
	    /// Posicion previa
	    /// </summary>
	    public Vector2 PosicionPrevia { get; private set; }
	
	    /// <summary>
	    /// Objetivo original
	    /// </summary>
	    public Transform Target { get; internal set; }
	
	    /// <summary>
	    /// Informacion original del hit
	    /// </summary>
	    public RaycastHit Hit { get; internal set; }
	}
}