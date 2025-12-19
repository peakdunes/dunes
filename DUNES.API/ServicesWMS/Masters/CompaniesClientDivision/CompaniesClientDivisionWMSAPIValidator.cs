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


        }
    }
}
