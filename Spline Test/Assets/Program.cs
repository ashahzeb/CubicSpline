using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Scripts
{

    public class Program : MonoBehaviour
    {
        public LineRenderer Line;
        // Use this for initialization
        void Start()
        {
            TestFitParametric();
        }

        void Update()
        {
            if (Input.anyKeyDown)
            {
                TestFitParametric();
            }
        }

        private void TestTrajectory()
        {

        }

        private void TestFitParametric()
        {
            float[] Coordinate = new[] { -13.0f, -1.4f, 16.2f };

            GameObject baseobject = GameObject.CreatePrimitive(PrimitiveType.Sphere);
            baseobject.transform.position = new Vector3(Coordinate[0], Coordinate[1], Coordinate[2]);

            GameObject firstObject = GameObject.CreatePrimitive(PrimitiveType.Sphere);
            firstObject.name = " firstPoint";

            int upperLimit = 380;

            List<float[]> _points = new List<float[]>();
            List<float[]> _returningPoints = new List<float[]>(6);

            _points.Add(Coordinate);

            var radius = Random.Range(10, 35);

            var multiplier = Random.Range(0, 1) * 2 - 1;

            for (int i = 0; i < 6; i++)
            {
                var divisor = 180;

                int angleVariation;

                if (i == 0)
                {
                    angleVariation = Random.Range(20, 360);
                }
                else
                {
                    var quad = Random.Range(0, 2);

                    if (quad == 0)
                    {
                        angleVariation = Random.Range(-45, 75);
                    }

                    else
                    {
                        angleVariation = Random.Range(105, 180);
                    }

                }

                if (angleVariation == 0)
                {
                    angleVariation = 45;
                }

                var x = multiplier * radius * (float)Math.Cos(angleVariation * Math.PI / divisor);
                var y = multiplier * radius * (float)Math.Sin(angleVariation * Math.PI / divisor);
                var z = multiplier * radius * (float)Math.Cos(angleVariation / 2 * 20 * Math.PI / divisor);

                var previousPoint = _points[_points.Count - 1];

                if (z < 6f)
                {
                    z = 6f + (float)Random.Range(0f, 1f);
                }

                var point = new[] { previousPoint[0] + x, previousPoint[1] + y, previousPoint[2] + z };

                if (i == 0)
                {
                    firstObject.transform.position = new Vector3(point[0], point[1], point[2]);
                }

                _returningPoints.Insert(0, new[] { -1 * point[0], -1 * point[1], point[2] });

                _points.Add(point);
            }

            _points.Add(Coordinate);

            //// Create the data to be fitted
            float[] _xCoordinates = new float[_points.Count];
            float[] _yCoordinates = new float[_points.Count];
            float[] _zCoordinates = new float[_points.Count];

            for (int i = 0; i < _points.Count; i++)
            {
                var value = _points[i];
                _xCoordinates[i] = value[0];
                _yCoordinates[i] = value[1];
                _zCoordinates[i] = value[2];
            }

            float[] xs, ys, zs;
            CubicSpline.FitParametric(_xCoordinates, _yCoordinates, _zCoordinates, 100, out xs, out ys, out zs);

            Debug.Log(xs.Length);

            Vector3[] points = new Vector3[100];

            for (int i = 0; i < 100; i++)
            {
                points[i] = new Vector3(xs[i], ys[i], zs[i]);
            }
            Line.positionCount = 100;
            Line.SetPositions(points);
        }
    }


}