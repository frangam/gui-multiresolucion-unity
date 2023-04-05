using UnityEngine;
using System.Collections;

/// <summary>
/// Autor: Fran Garcia <www.fgarmo.com>
/// 
/// Enumerado con los diferentes tipos de anclados a la pantalla.
/// 
/// Incorpora un valor para indicar tambien que no se ancla
/// </summary>
public enum TipoAnclado{
	SUPERIOR_IZQUIERDA
	,SUPERIOR_CENTRO
	,SUPERIOR_DERECHA
	,CENTRO_IZQUIERDA
	,CENTRO
	,CENTRO_DERECHA
	,INFERIOR_IZQUIERDA
	,INFERIOR_CENTRO
	,INFERIOR_DERECHA
	
	/// <summary>
	/// Indica que no se va a anclar, no tiene anclado a la pantalla
	/// </summary>
	,SIN_ANCLADO
}

