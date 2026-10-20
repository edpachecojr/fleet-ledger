using FleetLedger.Core.Models;
using FleetLedger.Core.Tests.Fakes;

namespace FleetLedger.Core.Tests.Models;

public class BaseModelTests
{
    [Fact]
    public void Id_WhenEntityIsCreated_IsVersion7Guid()
        => new Organization().Id.Version.Should().Be(7);

    [Fact]
    public void Id_WhenTwoEntitiesAreCreated_AreDistinct()
        => new Organization().Id.Should().NotBe(new Organization().Id);

    [Fact]
    public void Equals_WhenSameTypeAndId_ReturnsTrue()
    {
        Guid id = Guid.CreateVersion7();

        new Organization { Id = id, Name = "A" }.Equals(new Organization { Id = id, Name = "B" }).Should().BeTrue();
    }

    [Fact]
    public void Equals_WhenDifferentIds_ReturnsFalse()
        => new Organization().Equals(new Organization()).Should().BeFalse();

    [Fact]
    public void Equals_WhenDifferentTypesShareId_ReturnsFalse()
    {
        Guid id = Guid.CreateVersion7();

        new Organization { Id = id }.Equals(new SampleModel { Id = id }).Should().BeFalse();
    }

    [Fact]
    public void Equals_WhenOtherIsNull_ReturnsFalse()
        => new Organization().Equals(null).Should().BeFalse();

    [Fact]
    public void GetHashCode_WhenSameId_IsEqual()
    {
        Guid id = Guid.CreateVersion7();

        new Organization { Id = id }.GetHashCode().Should().Be(new Organization { Id = id }.GetHashCode());
    }
}
