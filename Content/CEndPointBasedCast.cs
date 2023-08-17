/* 
Copyright (C) 2023 INF

This code is free software: you can redistribute it and/or modify
it under the terms of the GNU Affero General Public License as published
by the Free Software Foundation, either version 3 of the License, or
(at your option) any later version.

This program is distributed in the hope that it will be useful,
but WITHOUT ANY WARRANTY; without even the implied warranty of
MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
GNU Affero General Public License for more details.

You should have received a copy of the GNU Affero General Public License
along with this program.  If not, see <https://www.gnu.org/licenses/
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