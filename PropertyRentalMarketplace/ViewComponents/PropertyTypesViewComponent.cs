using Microsoft.AspNetCore.Mvc;
using PropertyRentalBL.Interfaces;
using PropertyRentalBL.Repositories;

namespace PropertyRentalMarketplace.ViewComponents
{
    public class PropertyTypesViewComponent : ViewComponent
    {
        IPropertyTypeRepository _PropertyTypeRepository;
        public PropertyTypesViewComponent(IPropertyTypeRepository PropertyTypeRepository)
        {
            _PropertyTypeRepository = PropertyTypeRepository;
        }
        public async Task<IViewComponentResult> InvokeAsync()
        {
            var propertyTypes = await _PropertyTypeRepository.GetAll();
            return View(propertyTypes);
        }
    }
}
