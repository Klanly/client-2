using UnityEngine;
using System.Collections;

public class UIDepth : MonoBehaviour {
	public int order;
	public bool isUI = true;
	void Start () 
	{
		if(isUI){
			Canvas canvas = GetComponent<Canvas>();
			if( canvas == null){
				canvas = gameObject.AddComponent<Canvas>();
			}
			canvas.overrideSorting = true;
			canvas.sortingOrder = order;
		}
		else
		{
			Renderer []renders  =  GetComponentsInChildren<Renderer>();
			
			foreach(Renderer render in renders){
				render.sortingOrder = order;
			}
		}
	}

	[ContextMenu("Excute")]
	public void Excute() {
		if(isUI){
			Canvas canvas = GetComponent<Canvas>();
			if( canvas == null){
				canvas = gameObject.AddComponent<Canvas>();
			}
			canvas.overrideSorting = true;
			canvas.sortingOrder = order;
		}
		else
		{
			Renderer []renders  =  GetComponentsInChildren<Renderer>();
			
			foreach(Renderer render in renders){
				render.sortingOrder = order;
			}
		}
	}

	public void SetDepht(int depht) {
		if(isUI){
			Canvas canvas = GetComponent<Canvas>();
			if( canvas == null){
				canvas = gameObject.AddComponent<Canvas>();
			}
			canvas.overrideSorting = true;
			canvas.sortingOrder = depht;
		}
		else
		{
			Renderer []renders  =  GetComponentsInChildren<Renderer>();
			
			foreach(Renderer render in renders){
				render.sortingOrder = depht;
			}
		}
	}
}
