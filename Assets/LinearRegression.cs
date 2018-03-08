using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Runtime.InteropServices;
using System.Linq;
using System;

public class LinearRegression : MonoBehaviour {

    [SerializeField]
    GameObject prefab;

    [SerializeField]
    GameObject[] trainingSpheres;

    private System.IntPtr _model;

    // Use this for initialization
    void Start () {

        var inputs = GetAllPosition(trainingSpheres);
        var outputs = GetAllPositionY(trainingSpheres);
        int width = 2;

        _model = perceptronLinearRegressionCreate(width);

        perceptronLinearRegressionTrain(_model, inputs, width, inputs.Length, outputs, outputs.Length);

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
            double[] inputsTestSpheres = new double[] {
                testSpheres[i].transform.position.x,
                testSpheres[i].transform.position.z
            };
            float Y = (float)perceptronLinearRegressionPredict(_model, inputsTestSpheres);
            testSpheres[i].transform.position += new Vector3(0, Y, 0);

            if (Y > 0)
            {
                AddColorToSphere(testSpheres[i], Color.red);
            }
            else
            {
                AddColorToSphere(testSpheres[i], Color.blue);
            }
        }

        freePtr(_model);
    }
	
	// Update is called once per frame
	void Update () {
		
	}

    private void AddColorToSphere(GameObject gameObject, Color color)
    {
        gameObject.transform.GetComponent<Renderer>().material.color = color;
    }

    private double[] GetAllPosition(GameObject[] gameObject)
    {
        var positions = new List<double>();

        for(int i = 0; i < gameObject.Length; i++)
        {
            positions.Add(gameObject[i].transform.position.x);
            positions.Add(gameObject[i].transform.position.z);
        }

        return positions.ToArray();
    }

    private double[] GetAllPositionY(GameObject[] gameObject)
    {
        var positions = new List<double>();

        for (int i = 0; i < gameObject.Length; i++)
        {
            positions.Add(gameObject[i].transform.position.y);
        }

        return positions.ToArray();
    }

    [DllImport("GlaDOS")]
    private static extern System.IntPtr perceptronLinearRegressionCreate(int width);
    [DllImport("GlaDOS")]
    private static extern void perceptronLinearRegressionTrain(System.IntPtr weights, double[] inputs, int width, int inputsLength, double[] outputs, int outputsLength);
    [DllImport("GlaDOS")]
    private static extern double perceptronLinearRegressionPredict(System.IntPtr weights, double[] inputs);
    [DllImport("GlaDOS")]
    private static extern void freePtr(System.IntPtr model);
}
