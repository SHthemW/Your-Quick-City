using Yours.QuickCity.Internal;
using System;
using UnityEngine;

namespace Yours.QuickCity.Internal
{
    internal interface IStuff
    {
        GameObject Obj { get; }

        int MaxGenerateNum { get; }        
        float MinGenerateDensity { get; }
        float MaxGenerateDensity { get; }

        DirectionalGenerateType DirectionalGenerate { get; }

        float GetRotationOffset();
        float GetGenerateSpacing();

        // default functions

        bool DensityIsMatch(float density)
        {
            return (density > MinGenerateDensity) && (density < MaxGenerateDensity);
        }
        float GetDensityMatchingValue(float density)
        {
            if (density < 0)
                throw new ArgumentOutOfRangeException(density.ToString());

            if (!DensityIsMatch(density))
                return 0;

            float middle     = (MinGenerateDensity + MaxGenerateDensity) / 2;
            float x_halfLen  = (MaxGenerateDensity - MinGenerateDensity) / 2;
            float x_distance = Mathf.Abs(density - middle);

            return Mathf.Abs(x_distance - x_halfLen) / x_halfLen;
        }
        Quaternion GetGenerateDirection(Vector3 attachDirection, Vector3 origRotation)
        {
            attachDirection = Quaternion.Euler(0, 90, 0) * attachDirection;

            Vector3 targetRotationVector = DirectionalGenerate switch
            {
                DirectionalGenerateType.Part => attachDirection.IsNSWE() ? attachDirection : Vector3.zero,
                DirectionalGenerateType.Full => attachDirection,
                _ => Vector3.zero
            };

            Vector3 targetAngle = new(origRotation.x, GetRotationOffset(), origRotation.z);

            if (targetRotationVector != Vector3.zero)
                targetAngle += Quaternion.LookRotation(targetRotationVector, Vector3.up).eulerAngles;

            return Quaternion.Euler(targetAngle);
        }
    }
}
