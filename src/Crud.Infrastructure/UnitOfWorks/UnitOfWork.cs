using Crud.Core.Enums;
using Crud.Core.UnitOfWorks;
using Crud.Infrastructure.DbContext;
using Microsoft.Extensions.Logging;

namespace Crud.Infrastructure.UnitOfWorks
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly CrudContext _context;
        private readonly ILogger<UnitOfWork> _logger;

        public UnitOfWork(CrudContext context, ILogger<UnitOfWork> logger)
        {
            _context = context;
            _logger = logger;
        }

        public async Task<int> SaveChangesAsync()
        {
            try
            {
                return await _context.SaveChangesAsync();
            }
            catch (Exception e)
            {
                _logger.LogError("{Message}", e.Message);
                return (int)DatabaseResponseCodes.SaveChangesFailed;
            }
        }
    }
}
