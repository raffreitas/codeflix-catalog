using Elastic.Clients.Elasticsearch;

using FC.Codeflix.Catalog.Infra.Data.ES.Models;

using FluentAssertions;

namespace FC.Codeflix.Catalog.E2ETests.Consumers.Categories;

[Trait("E2E/Consumers", "Category")]
public sealed class KafkaConsumerTest(CategoryConsumerTestFixture fixture)
    : IClassFixture<CategoryConsumerTestFixture>, IDisposable
{
    [Fact(DisplayName = nameof(CategoryEvent_WhenOperationIsCreate_SavesCategory))]
    public async Task CategoryEvent_WhenOperationIsCreate_SavesCategory()
    {
        var message = fixture.BuildValidMessage("c");
        var category = message.Payload.After;

        await fixture.PublishMessageAsync(message);
        await Task.Delay(2_000);

        var persisted = await fixture.ElasticClient.GetAsync<CategoryModel>(category!.Id);
        persisted.Found.Should().BeTrue();
        var document = persisted.Source;
        document.Should().NotBeNull();
        document.Id.Should().Be(category.Id);
        document.Name.Should().Be(category.Name);
        document.Description.Should().Be(category.Description);
        document.IsActive.Should().Be(category.IsActive);
        document.CreatedAt.Date.Should().Be(category.CreatedAt.Date);
    }

    [Fact(DisplayName = nameof(CategoryEvent_WhenOperationIsUpdate_SavesCategory))]
    public async Task CategoryEvent_WhenOperationIsUpdate_SavesCategory()
    {
        var examplesList = fixture.GetCategoryModelList();
        await fixture.ElasticClient.IndexManyAsync(examplesList);
        var example = examplesList[2];
        var message = fixture.BuildValidMessage("u", example);
        var category = message.Payload.After;
        category!.Name = fixture.DataGenerator.GetValidCategoryName();

        await fixture.PublishMessageAsync(message);
        await Task.Delay(2_000);

        var persisted = await fixture.ElasticClient.GetAsync<CategoryModel>(category.Id);
        persisted.Found.Should().BeTrue();
        var document = persisted.Source;
        document.Should().NotBeNull();
        document.Id.Should().Be(category.Id);
        document.Name.Should().Be(category.Name);
        document.Description.Should().Be(category.Description);
        document.IsActive.Should().Be(category.IsActive);
        document.CreatedAt.Date.Should().Be(category.CreatedAt.Date);
    }

    [Fact(DisplayName = nameof(CategoryEvent_WhenOperationIsDelete_DeletesCategory))]
    public async Task CategoryEvent_WhenOperationIsDelete_DeletesCategory()
    {
        var examplesList = fixture.GetCategoryModelList();
        await fixture.ElasticClient.IndexManyAsync(examplesList);
        var example = examplesList[2];
        var message = fixture.BuildValidMessage("d", example);
        var category = message.Payload.Before;

        await fixture.PublishMessageAsync(message);
        await Task.Delay(2_000);

        var persisted = await fixture.ElasticClient.GetAsync<CategoryModel>(category!.Id);
        persisted.Found.Should().BeFalse();
    }

    [Fact(DisplayName = nameof(CategoryEvent_WhenOperationIsRead_SavesCategory))]
    public async Task CategoryEvent_WhenOperationIsRead_SavesCategory()
    {
        var message = fixture.BuildValidMessage("r");
        var category = message.Payload.After;

        await fixture.PublishMessageAsync(message);
        await Task.Delay(2_000);

        var persisted = await fixture.ElasticClient.GetAsync<CategoryModel>(category!.Id);
        persisted.Found.Should().BeTrue();
        var document = persisted.Source;
        document.Should().NotBeNull();
        document.Id.Should().Be(category.Id);
        document.Name.Should().Be(category.Name);
        document.Description.Should().Be(category.Description);
        document.IsActive.Should().Be(category.IsActive);
        document.CreatedAt.Date.Should().Be(category.CreatedAt.Date);
    }

    public void Dispose() => fixture.DeleteAll();
}