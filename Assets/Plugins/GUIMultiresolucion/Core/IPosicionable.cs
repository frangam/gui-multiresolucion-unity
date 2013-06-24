using UnityEngine;
using System.Collections;

/// <summary>
/// Autor: Fran Garcia <www.fgarmo.com>
/// 
/// Ofrece un conjunto de metodos utiles para posicionar de forma relativa en la pantalla
/// elementos de la GUI. De forma relativa a una serie de anclas predefinidos.
/// </summary>
public interface IPosicionable {
	/// <summary>
	/// Devuelve la posicion en pixeles exactos relativa al anclado Superior-Izquierda
	/// indicandole el porcentaje de distancia que queremos aplicar en la coordenada X e Y.
	/// </summary>
	/// <returns>
	/// Posicion desde el anclado Superior-Izquierda
	/// </returns>
	/// <param name='x'>
	/// Porcentaje de distancia en la coordenada X
	/// </param>
	/// <param name='y'>
	/// Porcentaje de distancia en la coordenada Y
	/// </param>
	Vector2 posicionDesdeSuperiorIzquierda(float xPorcentaje, float yPorcentaje);
	
	/// <summary>
	/// Devuelve la posicion en pixeles exactos relativa al anclado Superior-Centro
	/// indicandole el porcentaje de distancia que queremos aplicar en la coordenada X e Y.
	/// </summary>
	/// <returns>
	/// Posicion desde el anclado Superior-Centro
	/// </returns>
	/// <param name='x'>
	/// Porcentaje de distancia en la coordenada X
	/// </param>
	/// <param name='y'>
	/// Porcentaje de distancia en la coordenada Y
	/// </param>
	Vector2 posicionDesdeSuperiorCentro(float xPorcentaje, float yPorcentaje);
	
	/// <summary>
	/// Devuelve la posicion en pixeles exactos relativa al anclado Superior-Derecha
	/// indicandole el porcentaje de distancia que queremos aplicar en la coordenada X e Y.
	/// </summary>
	/// <returns>
	/// Posicion desde el anclado Superior-Derecha
	/// </returns>
	/// <param name='x'>
	/// Porcentaje de distancia en la coordenada X
	/// </param>
	/// <param name='y'>
	/// Porcentaje de distancia en la coordenada Y
	/// </param>
	Vector2 posicionDesdeSuperiorDerecha(float xPorcentaje, float yPorcentaje);
	
	/// <summary>
	/// Devuelve la posicion en pixeles exactos relativa al anclado Centro-Izquierda
	/// indicandole el porcentaje de distancia que queremos aplicar en la coordenada X e Y.
	/// </summary>
	/// <returns>
	/// Posicion desde el anclado Centro-Izquierda
	/// </returns>
	/// <param name='x'>
	/// Porcentaje de distancia en la coordenada X
	/// </param>
	/// <param name='y'>
	/// Porcentaje de distancia en la coordenada Y
	/// </param>
	Vector2 posicionDesdeCentroIzquierda(float xPorcentaje, float yPorcentaje);
	
	/// <summary>
	/// Devuelve la posicion en pixeles exactos relativa al anclado Centro
	/// indicandole el porcentaje de distancia que queremos aplicar en la coordenada X e Y.
	/// </summary>
	/// <returns>
	/// Posicion desde el anclado Centro
	/// </returns>
	/// <param name='x'>
	/// Porcentaje de distancia en la coordenada X
	/// </param>
	/// <param name='y'>
	/// Porcentaje de distancia en la coordenada Y
	/// </param>
	Vector2 posicionDesdeCentro(float xPorcentaje, float yPorcentaje);
	
	/// <summary>
	/// Devuelve la posicion en pixeles exactos relativa al anclado Centro-Derecha
	/// indicandole el porcentaje de distancia que queremos aplicar en la coordenada X e Y.
	/// </summary>
	/// <returns>
	/// Posicion desde el anclado Centro-Derecha
	/// </returns>
	/// <param name='x'>
	/// Porcentaje de distancia en la coordenada X
	/// </param>
	/// <param name='y'>
	/// Porcentaje de distancia en la coordenada Y
	/// </param>
	Vector2 posicionDesdeCentroDerecha(float xPorcentaje, float yPorcentaje);
	
	/// <summary>
	/// Devuelve la posicion en pixeles exactos relativa al anclado Inferior-Izquierda
	/// indicandole el porcentaje de distancia que queremos aplicar en la coordenada X e Y.
	/// </summary>
	/// <returns>
	/// Posicion desde el anclado Inferior-Izquierda
	/// </returns>
	/// <param name='x'>
	/// Porcentaje de distancia en la coordenada X
	/// </param>
	/// <param name='y'>
	/// Porcentaje de distancia en la coordenada Y
	/// </param>
	Vector2 posicionDesdeInferiorIzquierda(float xPorcentaje, float yPorcentaje);
	
	/// <summary>
	/// Devuelve la posicion en pixeles exactos relativa al anclado Inferior-Centro
	/// indicandole el porcentaje de distancia que queremos aplicar en la coordenada X e Y.
	/// </summary>
	/// <returns>
	/// Posicion desde el anclado Inferior-Centro
	/// </returns>
	/// <param name='x'>
	/// Porcentaje de distancia en la coordenada X
	/// </param>
	/// <param name='y'>
	/// Porcentaje de distancia en la coordenada Y
	/// </param>
	Vector2 posicionDesdeInferiorCentro(float xPorcentaje, float yPorcentaje);
	
	/// <summary>
	/// Devuelve la posicion en pixeles exactos relativa al anclado Inferior-Derecha
	/// indicandole el porcentaje de distancia que queremos aplicar en la coordenada X e Y.
	/// </summary>
	/// <returns>
	/// Posicion desde el anclado Inferior-Derecha
	/// </returns>
	/// <param name='x'>
	/// Porcentaje de distancia en la coordenada X
	/// </param>
	/// <param name='y'>
	/// Porcentaje de distancia en la coordenada Y
	/// </param>
	Vector2 posicionDesdeInferiorDerecha(float xPorcentaje, float yPorcentaje);
	
	/// <summary>
	/// Posicion del ancla indicado como parametro
	/// </summary>
	/// <returns>
	/// Posicion del anclado
	/// </returns>
	/// <param name='anclado'>
	/// El anclado.
	/// </param>
	Vector2 posicionDelAncladoSeleccionado(TipoAnclado anclado);
	
	/// <summary>
	/// Posicion relativa al ancla seleccionado
	/// </summary>
	/// <returns>
	/// Posicion relativa al anclado
	/// </returns>
	/// <param name='relativoA'>
	/// El anclado desde el que se obtiene la posicion relativa
	/// </param>
	Vector2 posicionRelativaAlAncla(TipoAnclado relativoA);
	
	/// <summary>
	/// La dimension de la pantalla escalada para todo tipo de resoluciones de pantalla
	/// </summary>
	/// <returns>
	/// La componenete x del Vector2 es la anchura escalada, y la componente y la altura escalada
	/// </returns>
	Vector2 dimensionEscaladaPantalla();
}
