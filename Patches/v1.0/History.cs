using UnityEngine;

namespace Active.Core.Details{
[DisallowMultipleComponent] public class History : MonoBehaviour {

	VTrace previous;
	//
	GameObject logroot => GameObject.Find(lrName) ?? new GameObject(lrName);
	string     label   => $"#{Time.time:0.##}";
	string	   lrName  => $"{name} (H)";

	public void Log(string s, float span, float sz=0.025f, float h=0.1f){
		s = TrimLog(s);
		if(previous && s == previous.info) return;
		var x = Let<VTrace>(label, PrimitiveType.Sphere, sz);
		x.span               = span;
		x.info 			     = s;
		x.transform.parent   = logroot.transform;
		x.transform.position = transform.position + Vector3.up*h;
		x.previous 		     = previous;
		previous 		     = x;
	}

	public static string TrimLog(string s) => TrimDetails(s, '[', ']');

	static string TrimDetails(string self, char @in, char @out){
		var x = new System.Text.StringBuilder();
		var depth = 0;
		for(int i = 0; i < self.Length; i++){
			var c = self[i];
			if(c == @in) depth++;
			if(depth <= 0) x.Append(c);
			if(c==@out) depth--;
		} return x.ToString();
	}

	T Let<T>(string name, PrimitiveType t, float s) where T : Component{
		var x = GameObject.CreatePrimitive(t).transform;
		Destroy(x.GetComponent<Collider>());
		x.gameObject.name = name;
		x.localScale	  = Vector3.one * s;
		x.parent		  = transform;
		return x.gameObject.AddComponent<T>();
	}

}}
