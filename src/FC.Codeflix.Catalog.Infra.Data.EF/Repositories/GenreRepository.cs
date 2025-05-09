﻿using FC.Codeflix.Catalog.Application.Exceptions;
using FC.Codeflix.Catalog.Domain.Entities;
using FC.Codeflix.Catalog.Domain.Repositories;
using FC.Codeflix.Catalog.Domain.SeedWork.SearcheableRepository;
using FC.Codeflix.Catalog.Infra.Data.EF.Models;

using Microsoft.EntityFrameworkCore;

namespace FC.Codeflix.Catalog.Infra.Data.EF.Repositories;
public class GenreRepository(CodeflixCatalogDbContext context) : IGenreRepository
{
    private DbSet<Genre> _genres => context.Set<Genre>();
    private DbSet<GenresCategories> _genresCategories => context.Set<GenresCategories>();

    public async Task Insert(Genre genre, CancellationToken cancellationToken = default)
    {
        await _genres.AddAsync(genre, cancellationToken);
        if (genre.Categories.Count > 0)
        {
            var relations = genre.Categories
                .Select(categoryId => new GenresCategories(genre.Id, categoryId));
            await _genresCategories.AddRangeAsync(relations, cancellationToken);
        }
    }

    public async Task<Genre> Get(Guid id, CancellationToken cancellationToken = default)
    {
        var genre = await _genres
            .AsNoTracking()
            .FirstOrDefaultAsync((x) => x.Id == id, cancellationToken)
                ?? throw new NotFoundException($"Genre '{id}' not found.");

        var categoryIds = await _genresCategories
            .Where(x => x.GenreId == id)
            .Select(x => x.CategoryId)
            .ToListAsync(cancellationToken);
        categoryIds.ForEach(genre.AddCategory);
        return genre;
    }

    public Task Delete(Genre aggregate, CancellationToken cancellationToken = default)
    {
        throw new NotImplementedException();
    }

    public Task<SearchOutput<Genre>> Search(SearchInput input, CancellationToken cancellationToken = default)
    {
        throw new NotImplementedException();
    }

    public Task Update(Genre aggregate, CancellationToken cancellationToken = default)
    {
        throw new NotImplementedException();
    }
}
