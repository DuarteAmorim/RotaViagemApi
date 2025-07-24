using FluentValidation;
using RotaViagem.domain.Entities;

namespace RotaViagem.application.Validations
{
    public class RotaValidation : AbstractValidator<Rota>
    {
        public RotaValidation()
        {
            RuleFor(r => r.Origem)
                .NotEmpty().WithMessage("A origem não pode ser vazia.")
                .Length(3).WithMessage("A origem deve ter exatamente 3 caracteres.")
                .Matches("^[A-Z]{3}$").WithMessage("A origem deve conter apenas letras maiúsculas.");
            RuleFor(r => r.Destino)
                .NotEmpty().WithMessage("O destino não pode ser vazio.")
                .Length(3).WithMessage("O destino deve ter exatamente 3 caracteres.")
                .Matches("^[A-Z]{3}$").WithMessage("O destino deve conter apenas letras maiúsculas.");

            RuleFor(r => r.Preco)
                .GreaterThan(0).WithMessage("O preço deve ser maior que zero.");


        }
    }
}
