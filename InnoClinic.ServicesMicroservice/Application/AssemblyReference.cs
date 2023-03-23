using System.Reflection;

namespace Application;

public class AssemblyReference
{
    public static Assembly Assembly => Assembly.GetExecutingAssembly();
}
