using DUNES.Shared.DTOs.WMS;
using FluentValidation;

namespace DUNES.API.ServicesWMS.Masters.CompaniesContract
{
    /// <summary>
    /// contract data validator
    /// </summary>
    public class CompaniesContractWMSAPIValidator : AbstractValidator<WMSCompaniesContractDTO>
    {

        /// <summary>
        /// validation rules
        /// </summary>
        public CompaniesContractWMSAPIValidator()
        {
            RuleFor(x => x.CompanyId).NotEmpty().WithMessage("Company is required");

            RuleFor(x => x.CompanyClientId).NotEmpty().WithMessage("Company Client is required");

            RuleFor(x => x.StartDate).NotEmpty().WithMessage("Start Date is required");

            RuleFor(x => x.ContractCode).NotEmpty().WithMessage("Contract Number is required");

            RuleFor(x => x.ContactName).NotEmpty().WithMessage("Contact Name is required");

            RuleFor(x => x.ContactEmail).NotEmpty().WithMessage("Contact Mail is required");

            RuleFor(x => x.ContactPhone).NotEmpty().WithMessage("Contact Phone is required");

            RuleFor(x => x.ItemCatalogMode).NotEmpty().WithMessage("Item Catalog Mode is required");


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


                RuleFor(x => x.EndDate)
                   .NotEmpty()
                   .WithMessage("End Date is required when closing the contract.")
                   .When(x => x.IsActive == false);


                RuleFor(x => x.EndDate)
                .GreaterThanOrEqualTo(x => x.StartDate)
                .WithMessage("End Date cannot be earlier than Start Date.")
                .When(x => !x.IsActive && x.EndDate.HasValue);
            });


        }
    }
}
