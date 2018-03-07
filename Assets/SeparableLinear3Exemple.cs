using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using UnityEngine;

public class SeparableLinear3Exemple : MonoBehaviour {

    [SerializeField]
    GameObject prefab;

    [SerializeField]
    GameObject[] trainingSpheres;

    private System.IntPtr _model;

    // Use this for initialization
    void Start () {

        _model = CreateModel(9, 3);

        var outputs = new int[] { -1, 1, -1 };

        var allInputsPosition = GetPositionOfSpheres(trainingSpheres);

        _model = TrainClassif(_model, allInputsPosition, outputs, 3, 3);

        var testSpheres = new List<GameObject>();

        for(var i = 0; i < 10; i++)
        {
            for (var j = 0; j < 10; j++)
            {
                testSpheres.Add(Instantiate(prefab, new Vector3(i - 5f, 0, j - 5f), Quaternion.identity));
            }
        }

        for (var i = 0; i < testSpheres.Count; i++)
        {
            var allPosition = GetPositionOfSphere(testSpheres[i]);
            testSpheres[i].transform.position += Vector3.up * perceptronLinearClassify(_model, allPosition, 3);
        }
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    private double[] GetPositionOfSpheres(IEnumerable<GameObject> gameObject)
    {
        List<double> list = new List<double>();

        foreach (var sphere in gameObject)
        {
            list.AddRange(GetPositionOfSphere(sphere));
        }

        return list.ToArray();
    }

    private double[] GetPositionOfSphere(GameObject gameObject)
    {
        List<double> list = new List<double>
        {
            gameObject.transform.position.x,
            gameObject.transform.position.y,
            gameObject.transform.position.z
        };

        return list.ToArray();
    }

    private System.IntPtr CreateModel(int height, int width)
    {
        return perceptronLinearInit(height, width);
    }

    private System.IntPtr TrainClassif(System.IntPtr model, double[] inputs, int[] outputs, int height, int width)
    {
        return perceptronLinearTraining(model, inputs, outputs, height, width);
    }

    private int Classify(System.IntPtr model, double[] inputs, int width)
    {
        return perceptronLinearClassify(model, inputs, width);
    }

    private void DeleteModel(System.IntPtr model)
    {
        freePtr(model);
    }

    [DllImport("GlaDOS")]
    private static extern System.IntPtr perceptronLinearInit(int height, int width);
    [DllImport("GlaDOS")]
    private static extern System.IntPtr perceptronLinearTraining(System.IntPtr model, double[] inputs, int[] outputs, int height, int width);
    [DllImport("GlaDOS")]
    private static extern int perceptronLinearClassify(System.IntPtr model, double[] inputs, int width);
    [DllImport("GlaDOS")]
    private static extern void freePtr(System.IntPtr model);
}
