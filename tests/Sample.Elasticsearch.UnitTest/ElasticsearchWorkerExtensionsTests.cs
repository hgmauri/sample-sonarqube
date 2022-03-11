using System;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.Json;
using FluentAssertions;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Sample.Elasticsearch.UnitTest.Base;
using Sample.Elasticsearch.WebApi.Core.Extensions;
using Xunit;

namespace Sample.Elasticsearch.UnitTest;
public class ElasticsearchWorkerExtensionsTests : TestBase
{
    [Fact]
    public void Should_AddIndicesElasticsearch_To_ServiceCollection()
    {

        var services = new ServiceCollection();

        var setting = new
        {
            uri = "http://localhost:9200",
            defaultIndex = "indexteste",
            username = "teste",
            password = "teste"
        };

        var conf = new { ElasticsearchSettings = setting };

        var jsonString = JsonSerializer.Serialize(conf);

        byte[] bytes = Encoding.ASCII.GetBytes(jsonString);

        using (var stream = new MemoryStream(bytes))
        {
            var config = new ConfigurationBuilder().AddJsonStream(stream).Build();


            services.AddElasticsearch(config);
        }

        var IElasticClient = services.First(x => x.ServiceType.Name == "IElasticClient");


        IElasticClient.Should().NotBeNull();
        IElasticClient.Lifetime.Should().Be(ServiceLifetime.Singleton);
    }

    [Fact]
    public void Should_Throw_NullReferenceException_When_AddIndicesElasticsearch_Was_Not_Registered()
    {

        var services = new ServiceCollection();

        Action action = () => services.AddElasticsearch(null);

        //assert
        action.Should().Throw<NullReferenceException>();
    }
}

