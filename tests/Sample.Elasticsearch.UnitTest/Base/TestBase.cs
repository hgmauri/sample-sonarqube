using AutoFixture;
using AutoFixture.AutoMoq;

namespace Sample.Elasticsearch.UnitTest.Base;

public abstract class TestBase
{
    protected IFixture Fixture { get; }

    public TestBase()
    {
        Fixture = new Fixture().Customize(new AutoMoqCustomization { ConfigureMembers = true });
    }
}
