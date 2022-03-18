using Autofac;
using FluentValidation;
using System;

namespace ATS.Web.Infrastructure.APIClone.Validation
{
    public class AutofacValidatorFactory : ValidatorFactoryBase
    {
        private readonly IComponentContext _context;

        public AutofacValidatorFactory(IComponentContext context)
        {
            _context = context;
        }

        public override IValidator CreateInstance(Type validatorType)
        {
            return _context.Resolve(validatorType) as IValidator;
        }
    }
}