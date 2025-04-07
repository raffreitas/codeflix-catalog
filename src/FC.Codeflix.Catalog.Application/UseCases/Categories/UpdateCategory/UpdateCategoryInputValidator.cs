﻿using FluentValidation;

namespace FC.Codeflix.Catalog.Application.UseCases.Categories.UpdateCategory;
public class UpdateCategoryInputValidator : AbstractValidator<UpdateCategoryInput>
{
    public UpdateCategoryInputValidator()
    {
        RuleFor(x => x.Id)
            .NotEmpty();
    }
}
