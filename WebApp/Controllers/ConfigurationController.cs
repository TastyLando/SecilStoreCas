using Microsoft.AspNetCore.Mvc;
using ConfigLib.Interfaces;
using DataAccess.Models;
using MongoDB.Bson;
using DataAccess.Interfaces;

namespace WebApp.Controllers
{
	public class ConfigurationController : Controller
	{
		private readonly IConfigurationRepository _configurationRepository;
		private readonly IConfigurationService _configurationService;
		private readonly IConfiguration _configuration;

		public ConfigurationController(IConfigurationRepository configurationRepository, IConfiguration configuration, IConfigurationService configurationService)
		{
			_configurationRepository = configurationRepository;
			_configuration = configuration;
			_configurationService = configurationService;
		}

		public async Task<IActionResult> Index()
		{
			var applicationName = _configuration["AppSettings:ApplicationName"];
			var configurations = await _configurationRepository.GetActiveConfigurationsAsync(applicationName);

			return View(configurations);
		}

		public async Task<IActionResult> Details(ObjectId id)
		{
			var configuration = await _configurationRepository.GetConfigurationAsync(id);

			if (configuration == null)
			{
				return NotFound();
			}

			return View(configuration);
		}

		public IActionResult Create()
		{
			return View();
		}

		[HttpPost]
		public async Task<IActionResult> Create(ConfigurationRecord configurationRecord)
		{
			if (ModelState.IsValid)
			{
                if (configurationRecord.Id == ObjectId.Empty)
                {
                    configurationRecord.Id = ObjectId.GenerateNewId();
                }

                await _configurationRepository.AddOrUpdateConfigurationAsync(configurationRecord);

				return RedirectToAction(nameof(Index));
			}

			return View(configurationRecord);
		}

		public async Task<IActionResult> Edit(ObjectId id)
		{
			var configuration = await _configurationRepository.GetConfigurationAsync(id);

			if (configuration == null)
			{
				return NotFound();
			}

			return View(configuration);
		}

		[HttpPost]
		public async Task<IActionResult> Edit(ObjectId id, ConfigurationRecord configurationRecord)
		{
			if (id != configurationRecord.Id)
			{
				return BadRequest();
			}

			if (ModelState.IsValid)
			{
				await _configurationRepository.AddOrUpdateConfigurationAsync(configurationRecord);

				return RedirectToAction(nameof(Index));
			}

			return View(configurationRecord);
		}

		public async Task<IActionResult> Delete(ObjectId id)
		{
			var configuration = await _configurationRepository.GetConfigurationAsync(id);

			if (configuration == null)
			{
				return NotFound();
			}

			return View(configuration);
		}

		[HttpPost, ActionName("DeleteConfirmed")]
		public async Task<IActionResult> DeleteConfirmed(ObjectId id)
		{
			await _configurationRepository.DeleteConfigurationAsync(id);

			return RedirectToAction(nameof(Index));
		}
	}
}
