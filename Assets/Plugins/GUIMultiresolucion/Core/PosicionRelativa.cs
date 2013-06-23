using UnityEngine;
using System.Collections;

[System.Serializable]
public class PosicionRelativa {
	/// <summary>
	/// Los valores de la coordenada son valores entre 0 y 1 de tipo float.
	/// Representan porcentajes: 1 es el 100%.
	/// </summary>
	public float porcentajeDesdeAncla1;
	
	/// <summary>
	/// Los valores de la coordenada son valores entre 0 y 1 de tipo float.
	/// Representan porcentajes: 1 es el 100%.
	/// </summary>
	public float porcentajeDesdeAncla2;
	
	/// <summary>
	/// El ancla que se toma como referencia para a partir de este posiciona un componente gui.
	/// Si el tipo de ancla es SIN_ANCLADO, se posiciona de forma absoluta.
	/// </summary>
	public TipoAnclado anclaRelativa;
	
}
