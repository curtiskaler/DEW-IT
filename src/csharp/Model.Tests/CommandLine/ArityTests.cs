using DewIt.Model.CommandLine;

namespace DewIt.Model.Tests.CommandLine;

public class ArityTests
{
    [TestCase(Arity.ZERO | Arity.ONE, "0..1")]
    [TestCase(Arity.ZERO | Arity.MANY, "0..*")]
    [TestCase(Arity.ONE | Arity.MANY, "1..*")]
    [TestCase(Arity.ZERO, "0")]
    [TestCase(Arity.ONE, "1")]
    [TestCase(Arity.MANY, "*")]
    public static void GetUMLString_ProducesTheCorrectStringValues(Arity arity, string umlString)
    {
        Assert.That(arity.GetUMLString().Equals(umlString));
    }
}