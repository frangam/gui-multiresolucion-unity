using UnityEngine;
using System;
using System.Collections;
using GUIMultiresolucion.Core;
using TouchScript.Gestures;

namespace GUIMultiresolucion.GUIComponentes.Paneles{
	/*
	 * Representa un elemento que forma parte de un panel
	 */ 
	[ExecuteInEditMode]
	public class GUIItemPanel :  MonoBehaviour, IComparable{		
		/// <summary>
		/// El panel al que pertenece el item
		/// </summary>
		private GUIPanel panel;
		
		/// <summary>
		/// La posicion relativa al anchlado inicial en la que se localiza el item
		/// </summary>
		private Vector2 posicionRelativaInicial;
		
		/// <summary>
		/// La posicion fija inicial
		/// </summary>
		private Vector2 posicionFijaInicial;
		
		private Vector2 posActualizar;
		
		private bool iniciarAnimacionScroll;
		
		/// <summary>
		/// El GUIComponente que representa al item adjuntado al panel
		/// </summary>
		private GUIComponente item;
		
		private float progresoAnimacionScroll = 0f;
		private float duracionAnimacionScroll = 2f; //los segundos que tarda el lerp
		
		#region propiedades
		public GUIComponente Item{
			get{return item;}	
		}
		#endregion
		
		#region nuevos metodos
		public void inicializar(TipoScroll _scroll, GUIPanel _panel){
			panel = _panel;
			
			if(item != null){
				item.Visible = panel.Visible;
				item.inicializar(panel);
				posicionRelativaInicial = item.posicionRelativaA;
				posicionFijaInicial = item.posicionFija;
			}
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
			
			if(item != null){
				item.posicionRelativaA = posicionRelativaInicial;
				item.actualizar();
			}
		}
		
		public void actualizar(Vector2 posRelativa){
			posActualizar = posRelativa;
			
			if(item != null){
				item.posicionRelativaA += posActualizar;
				item.actualizar();
			}
			
			
//			iniciarAnimacionScroll = true;
			
//			Debug.Log("posActualizar: "+(posRelativa.x+item.posicionRelativaA.x));
		}

		public void dibujar ()
		{
			if(item != null){
				item.dibujar();
			}
		}
		#endregion
		
		#region Unity
		public void Update(){
//			if(iniciarAnimacionScroll){
//				progresoAnimacionScroll += Time.deltaTime / duracionAnimacionScroll;
//				item.posicionRelativaA = Vector2.Lerp(item.posicionRelativaA, item.posicionRelativaA+posActualizar, progresoAnimacionScroll);
//				item.actualizar();
//				
//				Vector2 aux = (item.posicionRelativaA+posActualizar);
//				float distanciaDestino = Vector2.Distance(item.posicionRelativaA, aux);
//				
//				Debug.Log("distancia destino: "+distanciaDestino);
//				Debug.Log(item.posicionRelativaA.x+", "+ (posActualizar.x+item.posicionRelativaA.x));
//				Debug.Log("distancia: "+ Mathf.Abs((item.posicionRelativaA.x-(posActualizar.x+item.posicionRelativaA.x))));
//				
//				float distancia = (item.posicionRelativaA.x-(posActualizar.x+item.posicionRelativaA.x));
//				
////				if(progresoAnimacionScroll >= 1){
////					iniciarAnimacionScroll = false;	
////					progresoAnimacionScroll = 0f;
////				}		
//				
//				if(distanciaDestino >= 0.00001f){
//					iniciarAnimacionScroll = false;	
//					progresoAnimacionScroll = 0f;
//				}		
//			}
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
			
			if(panel != null && this.item != null){
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
