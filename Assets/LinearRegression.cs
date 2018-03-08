using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Runtime.InteropServices;
using System.Linq;

public class LinearRegression : MonoBehaviour {

    [SerializeField]
    GameObject prefab;

    [SerializeField]
    GameObject[] trainingSpheres;

    private System.IntPtr _equation;

    // Use this for initialization
    void Start () {

        var inputsX = GetAllPositionOfAxeX(trainingSpheres);
        var inputsZ = GetAllPositionOfAxeZ(trainingSpheres);


        _equation = perceptronLinearRegressionTrain(inputsX, inputsZ, trainingSpheres.Length);

        var testSpheres = new List<GameObject>();

        for (var i = 0; i < 10; i++)
        {
            for (var j = 0; j < 10; j++)
            {
                testSpheres.Add(Instantiate(prefab, new Vector3(i - 5f, 0, j - 5f), Quaternion.identity));
            }
        }

        for (var i = 0; i < testSpheres.Count; i++)
        {
            int Y = perceptronLinearRegressionPredict(_equation, testSpheres[i].transform.position.x, testSpheres[i].transform.position.z);
            testSpheres[i].transform.position += Vector3.up * Y;

            if (Y > 0)
            {
                AddColorToSphere(testSpheres[i], Color.red);
            }
            else
            {
                AddColorToSphere(testSpheres[i], Color.blue);
            }
        }
    }
	
	// Update is called once per frame
	void Update () {
		
	}

    private void AddColorToSphere(GameObject gameObject, Color color)
    {
        gameObject.transform.GetComponent<Renderer>().material.color = color;
    }

    private double[] GetAllPositionOfAxeX(GameObject[] gameObject)
    {
        var positions = new List<double>();

        for(int i = 0; i < gameObject.Length; i++)
        {
            positions.Add(gameObject[i].transform.position.x);
        }

        return positions.ToArray();
    }

    private double[] GetAllPositionOfAxeZ(GameObject[] gameObject)
    {
        var positions = new List<double>();

        for (int i = 0; i < gameObject.Length; i++)
        {
            positions.Add(gameObject[i].transform.position.z);
        }

        return positions.ToArray();
    }

    [DllImport("GlaDOS")]
    private static extern System.IntPtr perceptronLinearRegressionTrain(double[] inputsX, double[] inputsY, int sizeInputs);
    [DllImport("GlaDOS")]
    private static extern int perceptronLinearRegressionPredict(System.IntPtr equation, double x, double y);
}
