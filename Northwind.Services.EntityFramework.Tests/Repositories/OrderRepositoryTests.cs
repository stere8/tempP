using System.Reflection;
using Northwind.Services.EntityFramework.Entities;
using Northwind.Services.EntityFramework.Repositories;
using NUnit.Framework;
using RepositoryOrder = Northwind.Services.Repositories.Order;

namespace Northwind.Services.EntityFramework.Tests.Repositories;

[TestFixture]
public sealed class OrderRepositoryTests : RepositoryTestsBase
{
    private static readonly object[][] ConstructorData =
    [
        [
            new[] { typeof(NorthwindContext) }
        ],
    ];

    [SetUp]
    public void SetUp()
    {
        this.ClassType = typeof(OrderRepository);
    }

    [Test]
    public void IsPublicClass()
    {
        this.AssertThatClassIsPublic(true);
    }

    [Test]
    public void InheritsObject()
    {
        this.AssertThatClassInheritsObject();
    }

    [TestCaseSource(nameof(ConstructorData))]
    public void HasPublicInstanceConstructor(Type[] parameterTypes)
    {
        this.AssertThatClassHasPublicConstructor(parameterTypes);
    }

    [Test]
    public void HasRequiredMembers()
    {
        Assert.That(this.ClassType.GetFields(BindingFlags.Instance | BindingFlags.Public).Length, Is.EqualTo(0));
        Assert.That(this.ClassType.GetConstructors(BindingFlags.Static | BindingFlags.Public).Length, Is.EqualTo(0));
        Assert.That(this.ClassType.GetConstructors(BindingFlags.Instance | BindingFlags.Public).Length, Is.EqualTo(1));
        Assert.That(this.ClassType.GetProperties(BindingFlags.Static | BindingFlags.Public).Length, Is.EqualTo(0));
        Assert.That(this.ClassType.GetProperties(BindingFlags.Instance | BindingFlags.Public).Length, Is.EqualTo(0));
        Assert.That(this.ClassType.GetMethods(BindingFlags.Static | BindingFlags.Public | BindingFlags.DeclaredOnly).Length, Is.EqualTo(0));
        Assert.That(this.ClassType.GetMethods(BindingFlags.Instance | BindingFlags.Public | BindingFlags.DeclaredOnly).Length, Is.EqualTo(5));
        Assert.That(this.ClassType.GetEvents(BindingFlags.Static | BindingFlags.Instance | BindingFlags.Public).Length, Is.EqualTo(0));
    }

    [TestCase("GetOrdersAsync", false, true, true, typeof(Task<IList<RepositoryOrder>>))]
    [TestCase("GetOrderAsync", false, true, true, typeof(Task<RepositoryOrder>))]
    [TestCase("AddOrderAsync", false, true, true, typeof(Task<long>))]
    [TestCase("RemoveOrderAsync", false, true, true, typeof(Task))]
    public void HasMethod(string methodName, bool isStatic, bool isPublic, bool isVirtual, Type returnType)
    {
        _ = this.AssertThatClassHasMethod(methodName, isStatic, isPublic, isVirtual, returnType);
    }
}
