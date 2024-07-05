using ENTITIES.ENTITY;
using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BLL.Validator
{
    public class ValidacionInventario : AbstractValidator<INVENTARIO>
    {
        public ValidacionInventario()
        {
            RuleFor(x => x.Descrip).NotEmpty().WithMessage("Debe escribir una descripcion")
                .MinimumLength(2).WithMessage("La descripcion no es valida");

            RuleFor(x => x.Existencias).GreaterThan(0).WithMessage("La existencia no puede ser 0");
            RuleFor(x => x.IdCategoria).GreaterThan(0).WithMessage("Debe seleccionar una categoria");
            RuleFor(x => x.IVA).GreaterThan(0).WithMessage("Ingrese un IVA mayor a 0");
            RuleFor(x => x.PVenta).GreaterThan(0).WithMessage("Ingrese un precio de venta mayor a 0");

        }
    }
}
