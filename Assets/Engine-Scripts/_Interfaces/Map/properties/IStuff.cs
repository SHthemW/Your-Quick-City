using Game.General.Properties;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace Game.General.Interfaces
{
    public interface IStuff
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
            if (!DensityIsMatch(density))
                return 0;

            var diff_left = density - MinGenerateDensity;
            var diff_right = MaxGenerateDensity - density;

            return diff_left < diff_right ? diff_left : diff_right;
        }
        Quaternion GetGenerateDirection(IMapTerrainDetector data, Vector3 origRotation)
        {
            Vector3 targetRotationVector = DirectionalGenerate switch
            {
                DirectionalGenerateType.Part => data.AttachDirectionIsNSWE ? data.AttachDirection : Vector3.zero,
                DirectionalGenerateType.Full => data.AttachDirection,
                _ => Vector3.zero
            };

            Vector3 targetAngle = new(origRotation.x, GetRotationOffset(), origRotation.z);

            if (targetRotationVector != Vector3.zero)
                targetAngle += Quaternion.LookRotation(targetRotationVector, Vector3.up).eulerAngles;

            return Quaternion.Euler(targetAngle);
        }
    }
}
