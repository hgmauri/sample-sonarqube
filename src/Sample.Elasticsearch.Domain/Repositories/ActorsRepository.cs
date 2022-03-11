using Nest;
using Sample.Elasticsearch.Domain.Interfaces;
using Sample.Elasticsearch.Infrastructure.Elastic;
using Sample.Elasticsearch.Infrastructure.Indices;

namespace Sample.Elasticsearch.Domain.Repositories
{
    public class ActorsRepository : ElasticBaseRepository<IndexActors>, IActorsRepository
    {
        /// <summary>
        /// //TODO: verificar
        /// //TODO: verificar
        /// //TODO: verificar
        /// //TODO: verificar
        /// </summary>
        /// <param name="elasticClient"></param>
        public ActorsRepository(IElasticClient elasticClient) 
            : base(elasticClient)
        {
        }
        //TODO: verificar
        //TODO: verificar
        public override string IndexName { get; } = "indexactors";
    }
}
