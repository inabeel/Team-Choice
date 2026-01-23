using TeamChoice.WebApis.Infrastructure.Repositories;
using TeamChoice.WebApis.Models.DTOs;
using TeamChoice.WebApis.Utils;

namespace TeamChoice.WebApis.Application.Services
{
    public interface ILocServicesService
    {
        Task<IEnumerable<LocServiceRes>> GetByLocIdAsync(string locId);
    }
    public class LocServicesService : ILocServicesService
    {
        private readonly ILocServiceRepository _locServiceRepo;

        public LocServicesService(ILocServiceRepository locServiceRepo)
        {
            _locServiceRepo = locServiceRepo;
        }

        public async Task<IEnumerable<LocServiceRes>> GetByLocIdAsync(string locId)
        {
            var services = await _locServiceRepo.FindByLocIdAsync(locId);

            if (services == null || !services.Any())
            {
                throw new KeyNotFoundException($"No services found for location: {locId}");
            }

            // Mapping entities to DTOs
            return services.Select(LocServiceMapper.ToResponse);
        }
    }
}
