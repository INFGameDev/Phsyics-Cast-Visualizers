using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace PhysCastVisualier
{
    [AddComponentMenu("Physics Cast Visualizer/Shape Casts/Cast Sphere")]
    public class CastSphere : ShapeCastVisualizer
    {
        protected override bool Cast()
        {
            return true;
        }


    }

}

