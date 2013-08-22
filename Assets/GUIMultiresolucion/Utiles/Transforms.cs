using UnityEngine;
using System.Collections;

public class Transforms : MonoBehaviour {
	
	/// <summary>
	/// Encuentra un Transform hijo, dado un nombre y el transform padre
	/// </summary>
	/// <returns>
	/// El transform.
	/// </returns>
	/// <param name='parent'>
	/// El transform padre.
	/// </param>
	/// <param name='name'>
	/// Nombre del hijo a encontrar
	/// </param>
	public static Transform FindChildTransform(Transform parent, string name){
        if (parent.name.Equals(name)) return parent;

        foreach (Transform child in parent){
            Transform result = FindChildTransform(child, name);

            if (result != null) return result;
        }

        return null;

    }
}
