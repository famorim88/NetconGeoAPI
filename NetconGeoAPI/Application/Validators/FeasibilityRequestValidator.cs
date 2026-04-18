using FluentValidation;
using NetconGeoAPI.Application.Dto;

namespace NetconGeoAPI.Application.Validators
{
    public class FeasibilityRequestValidator : AbstractValidator<FeasibilityRequest>
    {
        public FeasibilityRequestValidator()
        {
            RuleFor(x => x.Latitude).InclusiveBetween(-90, 90).WithMessage("Latitude inválida.");
            RuleFor(x => x.Longitude).InclusiveBetween(-180, 180).WithMessage("Longitude inválida.");
            RuleFor(x => x.Radius).GreaterThan(0).WithMessage("O raio deve ser maior que zero.");
            RuleFor(x => x.Page).GreaterThanOrEqualTo(1).WithMessage("A página deve ser maior ou igual a 1.");
        }
    }
}
