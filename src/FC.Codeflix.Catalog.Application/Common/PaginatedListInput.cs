﻿using FC.Codeflix.Catalog.Domain.SeedWork.SearcheableRepository;

namespace FC.Codeflix.Catalog.Application.Common;
public abstract record PaginatedListInput
{
    public int Page { get; set; }
    public int PerPage { get; set; }
    public string Search { get; set; }
    public string Sort { get; set; }
    public SearchOrder Dir { get; set; }

    protected PaginatedListInput(
        int page,
        int perPage,
        string search,
        string sort,
        SearchOrder dir)
    {
        Page = page;
        PerPage = perPage;
        Search = search;
        Sort = sort;
        Dir = dir;
    }

    public SearchInput ToSearchInput()
        => new(
            page: Page,
            perPage: PerPage,
            search: Search,
            orderBy: Sort,
            order: Dir
        );
}
