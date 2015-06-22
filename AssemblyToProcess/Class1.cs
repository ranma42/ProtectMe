
namespace AssemblyToProcess
{
    using ProtectionAttributes;

    public class Class1
    {
	[Protected]
	public void TestProtectedMethod()
	{
	}
    }

    public class Class2 : Class1
    {
	public void TestPublicMethod()
	{
	    TestProtectedMethod();
	}
    }
}
