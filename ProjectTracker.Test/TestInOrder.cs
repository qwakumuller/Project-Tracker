using Xunit.Abstractions;
using Xunit.Sdk;

namespace ProjectTracker.Test;

/// <summary>
/// This class helps us in running the unit tests in a spicific order by the method name. Sometimes, it is important to run the tests in a specific order.
/// </summary>
public class TestInOrder : ITestCaseOrderer
{
    public IEnumerable<TTestCase> OrderTestCases<TTestCase>(IEnumerable<TTestCase> testCases) where TTestCase : ITestCase
    {
        return testCases.OrderBy(testCase => testCase.TestMethod.Method.Name);
    }
}
