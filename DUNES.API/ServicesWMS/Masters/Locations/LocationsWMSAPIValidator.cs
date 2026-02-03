using DUNES.Shared.DTOs.WMS;
using FluentValidation;

namespace DUNES.API.ServicesWMS.Masters.Locations
{
    /// <summary>
    /// locations validator
    /// </summary>

    public class LocationsWMSAPIValidator :AbstractValidator<WMSLocationsUpdateDTO>
    {
        /// <summary>
        /// validation rules
        /// </summary>
        public LocationsWMSAPIValidator()
        {
           
            RuleFor(x => x.Name).NotEmpty().WithMessage("Location name is required");
            RuleFor(x => x.Idcountry).NotEmpty().WithMessage("Country is required");
            RuleFor(x => x.Idstate).NotEmpty().WithMessage("State is required");
            RuleFor(x => x.Idcity).NotEmpty().WithMessage("City is required");
            RuleFor(x => x.Address).NotEmpty().WithMessage("Address is required");
            RuleFor(x => x.Phone).NotEmpty().WithMessage("Phone is required");

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
