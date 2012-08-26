using System;
using System.Reflection;
using NUnit.Framework;

[TestFixture]
public class WeaverTests
{
    Assembly assembly;

    [TestFixtureSetUp]
    public void Setup()
    {
        assembly = WeaverHelper.WeaveAssembly();
    }

    [Test]
    public void ValidateHelloWorldIsInjected()
    {
        var type = assembly.GetType("Hello");
        var instance = (dynamic)Activator.CreateInstance(type);
        instance.World();
    }

#if(DEBUG)
    [Test]
    public void PeVerify()
    {
        Verifier.Verify(assembly.CodeBase.Remove(0, 8));
    }
#endif

}