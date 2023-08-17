/* 
Copyright 2023 INF

Licensed under the Apache License, Version 2.0 (the "License");
you may not use this file except in compliance with the License.
You may obtain a copy of the License at

    http://www.apache.org/licenses/LICENSE-2.0

Unless required by applicable law or agreed to in writing, software
distributed under the License is distributed on an "AS IS" BASIS,
WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
See the License for the specific language governing permissions and
limitations under the License.
*/

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

namespace PhysCastVisualier
{
    public enum EndPointsSource { Transform, Vector }

    [Serializable]
    public class CEndPointBasedCast
    {
        public EndPointsSource endPointsSource;
        public Transform startPointTransform;
        public Transform endPointTransform;
        [DisplayOnly] public Vector3 startPointVector;
        [DisplayOnly] public Vector3 endPointVector;

        [Header("Debug")]
        [DisplayOnly] public Vector3 startPoint;
        [DisplayOnly] public Vector3 endPoint;

        public void MoveStartPointTransform(Vector3 pos) => startPointTransform.position = pos;
        public void MoveEndPointTransform(Vector3 pos) => endPointTransform.position = pos;
        public void SetStartPointTransform(Transform t) => startPointTransform = t;
        public void SetEndPointTransform(Transform t) => endPointTransform = t;
        public void SetStartPointVector(Vector3 pos) => startPointVector = pos;
        public void SetEndPointVector(Vector3 pos) => endPointVector = pos;
        public void SetEndPointsSource(EndPointsSource source) => endPointsSource = source;


        /// <summary>
        /// Assign what position is used in the start and end points based on what EndPointsSource option is chosen
        /// </summary>
        public void DetermineEndPoints()
        {
            if (startPointTransform == null) {
                Debug.LogError("Determining end point source (Transform) option set but there is no [ Start Point Transform ] assigned!");
                return;
            }
                
            if (endPointTransform == null) {
                Debug.LogError("Determining end point source (Transform) option set but there is no [ End Point Transform ] assigned!");
                return;
            }
                

            startPoint = endPointsSource == EndPointsSource.Transform ? startPointTransform.position : startPointVector;
            endPoint = endPointsSource == EndPointsSource.Transform ? endPointTransform.position : endPointVector;
        }

        public (Quaternion rot, Vector3 pos, float length) CalculateCapsuleTransformProperties(float radius)
        {
            Quaternion rot = Quaternion.LookRotation(startPoint - endPoint) * Quaternion.Euler(90, 0, 0); // orient the wire mesh properly
            Vector3 pos = Vector3.Lerp(startPoint, endPoint, 0.5f); // position it between the 2 points
            float lenght = (Vector3.Distance(startPoint, endPoint) / 2) - radius; // calculate lenght based on distance between the points

            return (rot, pos, lenght);
        }
    }
}