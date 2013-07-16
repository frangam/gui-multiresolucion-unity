using UnityEngine;
using System;
using System.Collections;
using GUIMultiresolucion.Core;
using TouchScript.Gestures;

namespace GUIMultiresolucion.GUIComponentes.Paneles{
	/*
	 * Representa un elemento que forma parte de un panel
	 */ 
	[System.Serializable]
	[ExecuteInEditMode]
	public class GUIItemPanel :  MonoBehaviour, IComparable{
		/// <summary>
		/// El GUIComponente que representa al item adjuntado al panel
		/// </summary>
		public GUIComponente item;
		
		/// <summary>
		/// El panel al que pertenece el item
		/// </summary>
		private GUIPanel panel;
		
		/// <summary>
		/// La posicion inicial en la que se localiza el item
		/// </summary>
		private Vector2 posicionInicial;
		
		#region propiedades
		public GUIComponente Item{
			get{return item;}	
		}
		#endregion
		
		#region nuevos metodos
		public void inicializar(TipoScroll _scroll, GUIPanel _panel){
			panel = _panel;
			item.inicializar(panel);
			posicionInicial = item.posicionRelativaA;
		}
		
		public void resetearPosiciones(){
//			float distanciaAlDestino = Vector3.Distance (item.posicionRelativaA, posicionInicial);
//			Debug.Log(distanciaAlDestino);
//			
//			while(distanciaAlDestino > 0.01f){
//				item.posicionRelativaA = Vector2.Lerp(item.posicionRelativaA , posicionInicial, 0.5f*Time.deltaTime);
//				item.actualizar();
//				distanciaAlDestino = Vector3.Distance (item.posicionRelativaA, posicionInicial);
//				Debug.Log(item.posicionRelativaA);
//				Debug.Log(distanciaAlDestino);
//			}
			
			item.posicionRelativaA = posicionInicial;
		}
		
		public void actualizar(Vector2 posRelativa){
			item.posicionRelativaA += posRelativa;
			item.actualizar();
		}

		public void dibujar ()
		{
			item.dibujar();
		}
		#endregion
		
		#region implementacion del IComparable
		
		/// <summary>
		/// Ordena los GUIItemPanel segun el tipo de scroll que se aplica y en funcion a esto la componente x o la y mayor o menor
		/// 
		/// </summary>
		/// <returns>
		/// Con scroll Horizontal: 0 si la x de this es igual que la x del otro item, 1 si la x de this es mayor que la del otro y -1 si la x de this es menor que la del otro.
		/// Con scroll Vertical: 0 si la y de this es igual a la y del otro item, 1 si la y de this es mayor que la del otro y .1 si la y de this es menor que la del otro item.
		/// </returns>
		/// <param name='otroItemPanel'>
		/// El otro gui item panel
		/// </param>
		public int CompareTo(System.Object otroItemPanel){
			int res = 0;
			
			if(panel != null){
				GUIItemPanel aux = (GUIItemPanel) otroItemPanel;
				
				switch(panel.Scroll){
					case TipoScroll.HORIZONTAL:
						if(this.item.posicionFija.x > aux.Item.posicionFija.x){
							res = 1;	
						}
						else if(this.item.posicionFija.x< aux.Item.posicionFija.x){
							res = -1;	
						}
					break;
					
					case TipoScroll.VERTICAL:
						if(this.item.posicionFija.y > aux.Item.posicionFija.y){
							res = 1;	
						}
						else if(this.item.posicionFija.y < aux.Item.posicionFija.y){
							res = -1;	
						}
					break;
				}
			}
			
			return res;
		}
		
		#endregion		
	}
}
