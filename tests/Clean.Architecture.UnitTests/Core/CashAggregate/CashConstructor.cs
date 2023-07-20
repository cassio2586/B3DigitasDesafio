using CashFlow.Architecture.Core.CashAggregate;
using CashFlow.Architecture.Core.CashAggregate.Enums;
using Clean.Architecture.Core.ContributorAggregate;
using Xunit;

namespace Clean.Architecture.UnitTests.Core.ContributorAggregate;

public class CashConstructor
{
  private readonly string _testName = "test name";
  private Cash? _testCash;

  private Cash CreateCash()
  {
    return new Cash("test name",100,TransactionTypeEnum.Credit,DateTime.Now);
  }

  [Fact]
  public void InitializesName()
  {
    _testCash = CreateCash();

    Assert.Equal(_testName, _testCash.Description);
  }
}
