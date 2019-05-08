namespace OctopusDeployVariableCopy.BL_Layer
{
    public class CopyRules
    {
        public string VariableSetNameToCopy { get; set; }
        public string NewVariableSetName { get; set; }
        public int NumberOfCopies { get; set; }
        public bool KeepValues { get; set; }
        public bool OverwriteEnvironmentScope { get; set; }
        public bool OverwriteRoleScope { get; set; }
        public string NewEnvironmentScope { get; set; }
        public string NewRoleScope { get; set; }

        public CopyRules()
        {
            NumberOfCopies = 1;
            KeepValues = true;
        }
    }
}
