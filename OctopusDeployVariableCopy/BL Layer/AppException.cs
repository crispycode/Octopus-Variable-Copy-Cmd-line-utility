using System;

namespace OctopusDeployVariableCopy.BL_Layer
{
    public class AppException : Exception
    {
        public bool ShouldRetry { get; set; }
    }
}
