using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Reflection;
using System.Text.RegularExpressions;
using AutoFixture;
using AutoFixture.Kernel;

namespace Sample.Elasticsearch.UnitTest.Base;

//Code inspired by: https://stackoverflow.com/questions/47391406/autofixture-and-read-only-properties
//This allow us to configure values to readonly properties which the only way to change it is generally by the class constructor
public class FixtureCustomization<T>
{
    public IFixture Fixture { get; }

    public FixtureCustomization(IFixture fixture)
    {
        Fixture = fixture;
    }

    public FixtureCustomization<T> With<TProp>(Expression<Func<T, TProp>> expr, TProp value)
    {
        Fixture.Customizations.Add(new OverridePropertyBuilder<T, TProp>(expr, value));
        return this;
    }

    public T Create() => Fixture.Create<T>();

    public IEnumerable<T> CreateMany() => Fixture.CreateMany<T>();

    public IEnumerable<T> CreateMany(int count) => Fixture.CreateMany<T>(count);
}

public static class CompositionExt
{
    public static FixtureCustomization<T> For<T>(this IFixture fixture)
        => new(fixture);
}

public class OverridePropertyBuilder<T, TProp> : ISpecimenBuilder
{
    private readonly PropertyInfo _propertyInfo;
    private readonly TProp _value;

    public OverridePropertyBuilder(Expression<Func<T, TProp>> expr, TProp value)
    {
        _propertyInfo = (expr.Body as MemberExpression)?.Member as PropertyInfo ??
                        throw new InvalidOperationException("invalid property expression");
        _value = value;
    }

    public object Create(object request, ISpecimenContext context)
    {
        var pi = request as ParameterInfo;
        if (pi == null)
            return new NoSpecimen();

        var camelCase = Regex.Replace(_propertyInfo.Name, @"(\w)(.*)",
            m => m.Groups[1].Value.ToLower() + m.Groups[2]);

        if (pi.ParameterType != typeof(TProp) || pi.Name != camelCase)
            return new NoSpecimen();

        return _value;
    }
}
