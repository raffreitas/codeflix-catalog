using Elastic.Clients.Elasticsearch;

using FC.Codeflix.Catalog.Application.UseCases.Categories.DeleteCategory;
using FC.Codeflix.Catalog.Domain.Exceptions;
using FC.Codeflix.Catalog.Infra.Data.ES.Models;
using FC.Codeflix.Catalog.IntegrationTests.Categories.Common;

using FluentAssertions;

using MediatR;

using Microsoft.Extensions.DependencyInjection;

namespace FC.Codeflix.Catalog.IntegrationTests.Categories.DeleteCategory;

[Trait("Integration", "[UseCase] DeleteCategory")]
public sealed class DeleteCategoryTest(CategoryTestFixture fixture) : IClassFixture<CategoryTestFixture>, IDisposable
{
    [Fact(DisplayName = nameof(DeleteCategory_WhenReceivesAnExistingId_DeleteCategory))]
    public async Task DeleteCategory_WhenReceivesAnExistingId_DeleteCategory()
    {
        var serviceProvider = fixture.ServiceProvider;
        var mediatr = serviceProvider.GetRequiredService<IMediator>();
        var elasticClient = serviceProvider.GetRequiredService<ElasticsearchClient>();
        var categoriesExample = fixture.GetCategoryModelList();
        await elasticClient.IndexManyAsync(categoriesExample);
        var input = new DeleteCategoryInput(categoriesExample[3].Id);

        await mediatr.Send(input);

        var deletedCategory = await elasticClient.GetAsync<CategoryModel>(categoriesExample[3].Id);
        deletedCategory.Found.Should().BeFalse();
    }

    [Fact(DisplayName = nameof(DeleteCategory_WhenReceivesANonExistingId_ThrowsException))]
    public async Task DeleteCategory_WhenReceivesANonExistingId_ThrowsException()
    {
        var serviceProvider = fixture.ServiceProvider;
        var mediatr = serviceProvider.GetRequiredService<IMediator>();
        var elasticClient = serviceProvider.GetRequiredService<ElasticsearchClient>();
        var categoriesExample = fixture.GetCategoryModelList();
        await elasticClient.IndexManyAsync(categoriesExample);
        var input = new DeleteCategoryInput(Guid.NewGuid());

        var action = async () => await mediatr.Send(input);

        await action.Should().ThrowAsync<NotFoundException>()
            .WithMessage($"Category '{input.Id}' not found.");
    }

    public void Dispose()
    {
        fixture.DeleteAll();
    }
}