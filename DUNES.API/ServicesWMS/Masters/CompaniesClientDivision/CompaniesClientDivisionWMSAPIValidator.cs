using DUNES.Shared.DTOs.WMS;
using FluentValidation;

namespace DUNES.API.ServicesWMS.Masters.CompaniesClientDivision
{

    /// <summary>
    /// CompaniesClientDivision Validator
    /// </summary>
    public class CompaniesClientDivisionWMSAPIValidator : AbstractValidator<WMSCompanyClientDivisionDTO>
    {

        /// <summary>
        /// contructor
        /// </summary>
        public CompaniesClientDivisionWMSAPIValidator()
        {
            RuleFor(x => x.DivisionName).NotEmpty().WithMessage("Division name is required");

            RuleFor(x => x.Idcompanyclient).NotEmpty().WithMessage("Company Client is required");


            // Reglas específicas para INSERT
            RuleSet("Create", () =>
            {
                RuleFor(x => x.Id)
                    .Equal(0).WithMessage("Id must be 0 when creating.");


            });

            // Reglas específicas para UPDATE
            RuleSet("Update", () =>
            {
                RuleFor(x => x.Id)
                    .GreaterThan(0).WithMessage("Id is required for update.");
            });

        }
    }
}
