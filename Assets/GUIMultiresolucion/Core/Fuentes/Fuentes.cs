using UnityEngine;
using System.Collections;

namespace GUIMultiresolucion.Core.Fuentes{
	public class Fuentes : MonoBehaviour {
		
		public static Fuente[] fuentes;
		public string[] Nombrefuentes;
		
		void Awake()
		{
			fuentes = new Fuente[Nombrefuentes.Length];
			for(int i = 0; i<Nombrefuentes.Length; i++)
			{
				fuentes[i] = new Fuente(Nombrefuentes[i]);
			}
			
//			foreach(Fuente f in fuentes)
//			{
//				foreach(Fuente.CustomChar c in f.GetCharsOfString("hola mundo"))
//				{
//					Debug.Log (c.charID);
//				}
//	
//			}
		}
	}
}