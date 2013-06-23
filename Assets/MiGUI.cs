using UnityEngine;
using System.Collections;

public class MiGUI : MonoBehaviour {

	public GUIButton[] buttons;
	public GUIImagen[] imagenes;
	public GUIStyle p_buttonStyle;
	public string buttonStyle; //nombre del estilo para el boton
	
	void OnGUI(){
        GUIEscalador.InicioGUI();
		
		

        
        if(p_buttonStyle == null)
        {
            p_buttonStyle = GUI.skin.FindStyle(buttonStyle);
        }
      
        foreach (var t in buttons)
        {
            GUI.Button(t.rect, t.content, p_buttonStyle);
            //Pressed(buttons[i].name);
        }
		
		foreach(var i in imagenes){
			GUI.DrawTexture(i.distribucion, i.textura);
		}


        // Restore matrix before returning
        GUIEscalador.FinGUI();
    }
}
