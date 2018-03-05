using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Runtime.InteropServices;

public class MoveSphereScript : MonoBehaviour {

    [SerializeField]
	private GameObject preFabSphere;

	// Use this for initialization
	void Start () {
		Debug.Log("GlaDOS Init");
		for(int z = 0; z < 10; z++){
			for(int x = 0; x < 10; x++){
				Instantiate (preFabSphere, new Vector3 (x, 0, z), Quaternion.identity);
			}
		}

		Instantiate (preFabSphere, new Vector3 (3, -2, 4), Quaternion.identity);
		Instantiate (preFabSphere, new Vector3 (1, 2, 2), Quaternion.identity);
		Instantiate (preFabSphere, new Vector3 (6, 2, 5), Quaternion.identity);
        Debug.Log("GlaDOS Start");
        Debug.Log(glaDOS());
    }
	
	// Update is called once per frame
	void Update () {
		
	}

    [DllImport("GlaDOS")]
    private static extern int glaDOS();
}
