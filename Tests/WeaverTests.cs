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

        Assert.AreEqual("Hello World", instance.World());
    }

    [Test]
    public void PeVerify()
    {
        Verifier.Verify(assembly.CodeBase.Remove(0, 8));
    }

}