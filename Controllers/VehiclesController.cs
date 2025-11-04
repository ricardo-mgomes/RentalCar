using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using RentalCar.Models;

namespace RentalCar.Controllers
{
    public class VehiclesController : Controller
    {
        private readonly AppDbContext _context;

        public VehiclesController(AppDbContext context)
        {
            _context = context;
        }

        // LISTAGEM / INDEX
        public async Task<IActionResult> Index()
        {
            return View(await _context.Vehicles.ToListAsync());
        }

        // DETALHES
        public async Task<IActionResult> Details(int id)
        {
            var vehicle = await _context.Vehicles.FindAsync(id);
            if (vehicle == null) return NotFound();

            return View(vehicle);
        }

        // GET - CREATE
        public IActionResult Create()
        {
            return View();
        }

        // POST - CREATE
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Vehicle vehicle)
        {
            // REGRAS DE NEGÓCIO
            if (vehicle.AnoFabricacao > DateTime.Now.Year)
            {
                ModelState.AddModelError("AnoFabricacao", "O ano de fabrico não pode ser superior ao ano atual.");
            }

            if (_context.Vehicles.Any(v => v.Matricula == vehicle.Matricula))
            {
                ModelState.AddModelError("Matricula", "Já existe um veículo com esta matrícula.");
            }

            if (!ModelState.IsValid)
                return View(vehicle);

            vehicle.EstaAlugado = false;

            _context.Add(vehicle);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        // GET - EDIT
        public async Task<IActionResult> Edit(int id)
        {
            var vehicle = await _context.Vehicles.FindAsync(id);
            if (vehicle == null) return NotFound();

            return View(vehicle);
        }

        // POST - EDIT
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, Vehicle vehicle)
        {
            var exists = await _context.Vehicles.FindAsync(id);
            if (exists == null) return NotFound();

            // Validar ano
            if (vehicle.AnoFabricacao > DateTime.Now.Year)
            {
                ModelState.AddModelError("AnoFabricacao", "O ano de fabrico não pode ser superior ao ano atual.");
            }

            // Validar matrícula única (exceto este veículo)
            if (_context.Vehicles.Any(v => v.Matricula == vehicle.Matricula && v.Id != id))
            {
                ModelState.AddModelError("Matricula", "Já existe outro veículo com esta matrícula.");
            }

            if (!ModelState.IsValid)
                return View(vehicle);

            exists.Marca = vehicle.Marca;
            exists.Modelo = vehicle.Modelo;
            exists.Matricula = vehicle.Matricula;
            exists.AnoFabricacao = vehicle.AnoFabricacao;
            exists.Combustivel = vehicle.Combustivel;

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        // GET - DELETE
        public async Task<IActionResult> Delete(int id)
        {
            var vehicle = await _context.Vehicles.FindAsync(id);
            if (vehicle == null) return NotFound();

            return View(vehicle);
        }

        // POST - DELETE
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var vehicle = await _context.Vehicles.FindAsync(id);
            if (vehicle == null) return NotFound();

            // Não pode apagar se estiver alugado
            if (vehicle.EstaAlugado)
            {
                TempData["Error"] = "Não é possível apagar um veículo que está alugado.";
                return RedirectToAction(nameof(Index));
            }

            _context.Vehicles.Remove(vehicle);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }
    }
}
