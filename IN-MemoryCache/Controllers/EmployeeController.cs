using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.OpenApi.Writers;

namespace IN_MemoryCache.Controllers
{
    [ApiController]
    [Route("api/employee")]
    public class EmployeeController : ControllerBase
    {
        private readonly IGetEmployeeService _employeeService;
        private readonly IMemoryCache _memoryCache;
        private const string CacheKey = "GetEmployeeKey";
        private ILogger<EmployeeController> _logger;
        private static readonly SemaphoreSlim _semaphoreSlim = new SemaphoreSlim(1, 1);
        public EmployeeController(IGetEmployeeService getEmployee, IMemoryCache memoryCache, ILogger<EmployeeController> logger)
        {
            _employeeService = getEmployee;
            _memoryCache = memoryCache;
            _logger = logger;
        }
        [HttpGet("GetEmployee")]
        public async Task<IActionResult> GetEmployee()
        {
            if (_memoryCache.TryGetValue(CacheKey, out List<EmployeeModel>? employee))
            {
                _logger.LogInformation("Employee Found in Cache");
            }
            else
            {
                try
                {
                    await _semaphoreSlim.WaitAsync();
                    if (_memoryCache.TryGetValue(CacheKey, out employee))
                    {
                        _logger.LogInformation("Employee Found in Cache");
                    }
                    else
                    {
                        _logger.LogInformation("Employee not found in cache");

                        employee = _employeeService.GetEmployee();
                        //var cacheentryoptions = new MemoryCacheEntryOptions()
                        //    .SetSlidingExpiration(TimeSpan.FromSeconds(30))
                        //    .SetAbsoluteExpiration(TimeSpan.FromMinutes(1))
                        //    .SetPriority(CacheItemPriority.Normal);

                        var cacheSetting = CacheEntryOptionFactory.GetCacheConfiguration();
                        cacheSetting.SetSize(1);
                        //cacheSetting.SetPriority(CacheItemPriority.High);

                        //_memoryCache.Set(CacheKey, employee,cacheentryoptions);
                        _memoryCache.Set(CacheKey, employee, cacheSetting);
                    }
                    
                }
                finally
                {
                    _semaphoreSlim.Release();
                }

            }
            return Ok(employee);
        }

        [HttpPost("AddEmployee")]
        public IActionResult AddEmployee(EmployeeModel employee)
        {
            var data = _employeeService.AddEmployee(employee);
            _memoryCache.Remove(CacheKey);
                return RedirectToAction("GetEmployee");
        }
    }
}

