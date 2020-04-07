using System.Collections.Generic;

namespace Artemis.Web.Server.Users
{
    public static class ArtemisPolicies
    {
        public static Policy EmployeeOnly =
            new Policy {Name = "EmployeeOnly", Claim = "EmployeeNumber"};

        public static IEnumerable<Policy> GetPolicies()
        {
            yield return EmployeeOnly;
        }
    }



    public class Policy
    {
        public string Name { get; set; }
        public string Claim { get; set; }
    }
}