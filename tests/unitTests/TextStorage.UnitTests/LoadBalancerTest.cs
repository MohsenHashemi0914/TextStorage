using FluentAssertions;
using TextStorage.Web;

namespace TextStorage.UnitTests;

public class LoadBalancerTest
{
    private readonly LoadBalancer _sut;

    public LoadBalancerTest()
    {
        _sut = new(["Master1", "Master2", "Master3"]);
    }

    [Fact]
    public void Get_Tenant_Test()
    {
        var tenant = _sut.GetTenant();
        tenant.Id.Should().Be(1);
        tenant.ConnectionString.Should().Be("Master1");
        tenant.ConnectionPrefix.ToString().Should().Be("a");

        tenant = _sut.GetTenant();
        tenant.Id.Should().Be(2);
        tenant.ConnectionString.Should().Be("Master2");
        tenant.ConnectionPrefix.ToString().Should().Be("b");

        tenant = _sut.GetTenant();
        tenant.Id.Should().Be(3);
        tenant.ConnectionString.Should().Be("Master3");
        tenant.ConnectionPrefix.ToString().Should().Be("c");

        tenant = _sut.GetTenant();
        tenant.Id.Should().Be(1);
        tenant.ConnectionString.Should().Be("Master1");
        tenant.ConnectionPrefix.ToString().Should().Be("a");

        tenant = _sut.GetTenant();
        tenant.Id.Should().Be(2);
        tenant.ConnectionString.Should().Be("Master2");
        tenant.ConnectionPrefix.ToString().Should().Be("b");

        tenant = _sut.GetTenant();
        tenant.Id.Should().Be(3);
        tenant.ConnectionString.Should().Be("Master3");
        tenant.ConnectionPrefix.ToString().Should().Be("c");

        tenant = _sut.GetTenant();
        tenant.Id.Should().Be(1);
        tenant.ConnectionString.Should().Be("Master1");
        tenant.ConnectionPrefix.ToString().Should().Be("a");
    }

    [Fact]
    public void Get_All_Prefixes_Test()
    {
        var prefixes = _sut.GetAllPrefixes();
        prefixes.Should().HaveCount(3);
        prefixes[0].ToString().Should().Be("a");
        prefixes[1].ToString().Should().Be("b");
        prefixes[2].ToString().Should().Be("c");
    }
}