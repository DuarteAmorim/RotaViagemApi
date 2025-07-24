using FluentValidation.TestHelper;
using RotaViagem.application.Validations;
using RotaViagem.domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RotaViagem.Test.Validators
{
    public class RotaValidatorTests
    {
        private readonly RotaValidation _rotaValidation;

        public RotaValidatorTests()
        {
            _rotaValidation = new RotaValidation();
        }

        [Fact]
        public void Deve_Validar_Rota_Valida()
        {
            var rota = new Rota("GRU", "JFK", 1500);

            var resultado = _rotaValidation.TestValidate(rota);

            resultado.ShouldNotHaveAnyValidationErrors();
        }

        [Theory]
        [InlineData(null)]
        [InlineData("")]
        [InlineData("GR")]
        [InlineData("GRUU")]
        public void Deve_Validar_Origem_Invalida(string origem)
        {
            var rota = new Rota(origem ?? "", "JFK", 1500);

            var resultado = _rotaValidation.TestValidate(rota);

            resultado.ShouldHaveValidationErrorFor(r => r.Origem);
        }

        [Theory]
        [InlineData(null)]
        [InlineData("")]
        [InlineData("JK")]
        [InlineData("JKFF")]
        public void Deve_Validar_Destino_Invalido(string destino)
        {
            var rota = new Rota("GRU", destino ?? "", 1500);

            var resultado = _rotaValidation.TestValidate(rota);

            resultado.ShouldHaveValidationErrorFor(r => r.Destino);
        }

        [Theory]
        [InlineData(0)]
        [InlineData(-10)]
        public void Deve_Validar_Preco_Invalido(double preco)
        {
            var rota = new Rota("GRU", "JFK", preco);

            var resultado = _rotaValidation.TestValidate(rota);

            resultado.ShouldHaveValidationErrorFor(r => r.Preco);
        }
    }
}
