using Octopus.Client.Model;
using OctopusDeployVariableCopy.OctopusAccess;
using System;
using System.Collections.Generic;

namespace OctopusDeployVariableCopy.BL_Layer
{
    class OctopusHandler
    {
        private OctopusAccessor _octoDal;

        public OctopusHandler(string server, string apiKey)
        {
            _octoDal = new OctopusAccessor(server, apiKey);
        }

        public void CopyVariableSet(CopyRules copyRules)
        {
            try
            {
                var originalLibVarSet = _octoDal.GetLibraryVariableSetByName(copyRules.VariableSetNameToCopy);
                var originalVariables = _octoDal.GetVariableSet(originalLibVarSet.VariableSetId).Variables;                                                   
                
                string copiedNumber = null;
                for (int i = 0; i < copyRules.NumberOfCopies; i++)
                {
                    if (i > 0)
                    {
                        copiedNumber = i.ToString();
                    }
                    var newVarSet = _octoDal.CreateNewLibraryVariableSetWithVariableSet(string.Concat(copyRules.NewVariableSetName, copiedNumber), originalLibVarSet.Description);
                    foreach (var variable in originalVariables)
                    {
                        var value = copyRules.KeepValues ? variable.Value : "";
                        if(copyRules.OverwriteEnvironmentScope)
                        {
                            variable.Scope.Remove(ScopeField.Environment);
                            if (!string.IsNullOrEmpty(copyRules.NewEnvironmentScope))
                            {
                                variable.Scope.Add(ScopeField.Environment, copyRules.NewEnvironmentScope);
                            }                            
                        }

                        if (copyRules.OverwriteRoleScope)
                        {
                            variable.Scope.Remove(ScopeField.Role);
                            if (!string.IsNullOrEmpty(copyRules.NewRoleScope))
                            {
                                variable.Scope.Add(ScopeField.Role, copyRules.NewRoleScope);
                            }
                        }                        

                        _octoDal.AddVariableToVariableSet(newVarSet.VariableSetId, variable.Name, value ?? "", variable.Scope, variable.IsEditable, variable.IsSensitive, variable.Prompt);
                    }

                }
            }
            catch(Exception e)
            {
                throw new Exception("Failed to copy the variable set", e);
            }
        }

        public Dictionary<string, string> GetAllLibraryVariableSetsAvaialable()
        {
            try
            {
                var result = new Dictionary<string, string>();
                var libVarSets = _octoDal.GetLibraryVariableSets();
                foreach (var libVarSet in libVarSets)
                {
                    result.Add(libVarSet.Name, libVarSet.Id);
                }
                return result;
            }
            catch(Exception e)
            {
                throw new Exception("Failed to get all of the library variable sets", e);
            }

        }

        public Dictionary<string, string> GetAllEnvironmentsAvaialable()
        {
            try
            {
                var result = new Dictionary<string, string>();
                var environments = _octoDal.GetEnvironments();
                foreach (var environment in environments)
                {
                    result.Add(environment.Name, environment.Id);
                }
                return result;
            }
            catch (Exception e)
            {
                throw new Exception("Failed to get all of the environment resources", e);
            }

        }
    }
}
