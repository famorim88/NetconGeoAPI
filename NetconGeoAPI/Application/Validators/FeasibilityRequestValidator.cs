using FluentValidation;
using NetconGeoAPI.Application.Dto;
using System.Globalization;

namespace NetconGeoAPI.Application.Validators
{
    public class FeasibilityRequestValidator : AbstractValidator<FeasibilityRequest>
    {
        public FeasibilityRequestValidator()
        {
            RuleFor(x => x.Latitude).InclusiveBetween(-90, 90).WithMessage("Latitude inválida.")
            .Must(HasMinimumPrecision).WithMessage("A Latitude deve ter no mínimo 5 casas decimais para precisão técnica.");

            RuleFor(x => x.Longitude).InclusiveBetween(-180, 180).WithMessage("Longitude inválida.")
                .Must(HasMinimumPrecision).WithMessage("A Longitude deve ter no mínimo 5 casas decimais para precisão técnica.");

            RuleFor(x => x.Radius).GreaterThan(10).WithMessage("O raio deve ser maior que 10m.")
                                   .LessThanOrEqualTo(1000).WithMessage("O raio deve ser até 1Km.");
            
            RuleFor(x => x.Page).GreaterThanOrEqualTo(1).WithMessage("A página deve ser maior ou igual a 1.");

            RuleFor(x => x.Size).LessThanOrEqualTo(20).WithMessage("Deve filtrar menos de 20 itens por página");
        }
        private bool HasMinimumPrecision(double value)
        {
            // Converte para string usando cultura invariante (ponto como separador)
            string text = value.ToString(CultureInfo.InvariantCulture);
            int decimaPlace = text.IndexOf('.');

            if (decimaPlace == -1) return false; // Não tem casas decimais

            // Conta quantos dígitos existem após o ponto
            int count = text.Length - decimaPlace - 1;
            return count >= 5;
        }
    }
}
