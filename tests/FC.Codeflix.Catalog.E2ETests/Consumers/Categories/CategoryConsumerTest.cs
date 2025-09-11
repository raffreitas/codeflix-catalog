using FC.Codeflix.Catalog.Infra.Data.ES.Models;

using FluentAssertions;

namespace FC.Codeflix.Catalog.E2ETests.Consumers.Categories;

[Trait("E2E/Consumers", "Category")]
public sealed class CategoryConsumerTest(CategoryConsumerTestFixture fixture)
    : IClassFixture<CategoryConsumerTestFixture>, IDisposable
{
    [Fact(DisplayName = nameof(CategoryEvent_whenOperationIsCreate_SavesCategory))]
    public async Task CategoryEvent_whenOperationIsCreate_SavesCategory()
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

    public void Dispose() => fixture.DeleteAll();
}