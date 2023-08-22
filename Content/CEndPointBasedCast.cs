/* 
Copyright (C) 2023 INF

This Source Code Form is subject to the terms of the Mozilla Public
License, v. 2.0. If a copy of the MPL was not distributed with this
file, You can obtain one at http://mozilla.org/MPL/2.0/.
*/

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using INFAttributes;

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