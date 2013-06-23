using UnityEngine;
using System.Collections;

[System.Serializable]
public class GUIButton{
        
	    public string name;
	    public GUIContent content;
		public float width=0;
		public float height=0;
	    public Vector2 position;

	    private Rect r;
	    public Rect rect
	    {
	        get
	        {
	           
	           if(width == 0 && content.image.width != null)
				{

                    width =  content.image.width;
                    
				}
				if(height == 0 && content.image.height != null)
				{
				    height = content.image.height;
				}

                r = new Rect(position.x , position.y , width , height );
//                Debug.Log(string.Format("Position: ({0},{1}), Size: ({2},{3})", position.x, position.y, width, height));
	            return r;
	        }
	    }
	}
