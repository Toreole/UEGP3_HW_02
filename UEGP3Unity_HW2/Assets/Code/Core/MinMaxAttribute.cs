using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

namespace UEGP3.Core
{
    [AttributeUsage(AttributeTargets.Field)]
    public class MinMaxAttribute : PropertyAttribute
    {
        public float min, max;

        public MinMaxAttribute(float min, float max)
        {
            this.min = min;
            this.max = max;
        }
    }
}