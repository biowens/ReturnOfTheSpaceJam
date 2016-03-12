using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class ColorizeWorld : MonoBehaviour {
    public List<Material> materialsToColor;
    public float hueSpeed;
    float hue;
	
	void Update () {
        hue = (hue + Time.deltaTime * hueSpeed)%1;
        Color c = Color.HSVToRGB(hue, 1, 1);
        for (int index = 0; index < materialsToColor.Count; index++)
        {
            materialsToColor[index].SetColor("_Color", c);
        }
	
	}
}
