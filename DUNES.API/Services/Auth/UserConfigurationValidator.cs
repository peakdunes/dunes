using DUNES.Shared.DTOs.Auth;
using FluentValidation;

namespace DUNES.API.Services.Auth
{
    /// <summary>
    /// User configuration validator (single validator with RuleSets: Create / Update)
    /// Validates only "shape" rules (no DB checks).
    /// </summary>
    public class UserConfigurationValidator : AbstractValidator<UserConfigurationUpdateDto>
    {

        /// <summary>
        /// constructor
        /// </summary>
        public UserConfigurationValidator()
        {
            // Shared rules (apply to both Create & Update)
            RuleFor(x => x.Enviromentname)
                .NotEmpty().WithMessage("Enviromentname is required.")
                .MaximumLength(100).WithMessage("Enviromentname max length is 100.");

            RuleFor(x => x.Companydefault)
                .GreaterThan(0).WithMessage("Companydefault is required.");

            RuleFor(x => x.Companyclientdefault)
                .GreaterThan(0).WithMessage("Company clientdefault is required.");


            RuleFor(x => x.companiesContractId)
               .GreaterThan(0).WithMessage("Company contract is required.");

            RuleFor(x => x.Binesdistribution)
                .MaximumLength(1000).WithMessage("Binesdistribution max length is 1000.")
                .When(x => x.Binesdistribution != null);

            RuleFor(x => x.Roleid)
                .MaximumLength(450).WithMessage("RoleId max length is 450.")
                .When(x => !string.IsNullOrWhiteSpace(x.Roleid));

            RuleFor(x => x.Userid)
                .MaximumLength(450).WithMessage("UserId max length is 450.")
                .When(x => !string.IsNullOrWhiteSpace(x.Userid));

            // Create rules
            RuleSet("Create", () =>
            {
                // En Create, Id debe venir null (o 0 si algún cliente lo manda)
                RuleFor(x => x.Id)
                    .Must(id => id == null || id == 0)
                    .WithMessage("Id must be empty for Create.");
            });

            // Update rules
            RuleSet("Update", () =>
            {
                RuleFor(x => x.Id)
                    .NotNull().WithMessage("Id is required.")
                    .GreaterThan(0).WithMessage("Id is required.");
            });
        }
    }
}
