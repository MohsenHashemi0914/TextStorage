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
        tenant.GetConnectionPrefix().Should().Be("1M");

        tenant = _sut.GetTenant();
        tenant.Id.Should().Be(2);
        tenant.ConnectionString.Should().Be("Master2");
        tenant.GetConnectionPrefix().Should().Be("2M");

        tenant = _sut.GetTenant();
        tenant.Id.Should().Be(3);
        tenant.ConnectionString.Should().Be("Master3");
        tenant.GetConnectionPrefix().Should().Be("3M");

        tenant = _sut.GetTenant();
        tenant.Id.Should().Be(1);
        tenant.ConnectionString.Should().Be("Master1");
        tenant.GetConnectionPrefix().Should().Be("1M");

        tenant = _sut.GetTenant();
        tenant.Id.Should().Be(2);
        tenant.ConnectionString.Should().Be("Master2");
        tenant.GetConnectionPrefix().Should().Be("2M");

        tenant = _sut.GetTenant();
        tenant.Id.Should().Be(3);
        tenant.ConnectionString.Should().Be("Master3");
        tenant.GetConnectionPrefix().Should().Be("3M");

        tenant = _sut.GetTenant();
        tenant.Id.Should().Be(1);
        tenant.ConnectionString.Should().Be("Master1");
        tenant.GetConnectionPrefix().Should().Be("1M");
    }
}