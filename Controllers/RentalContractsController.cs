using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using RentalCar.Models;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace RentalCar.Controllers
{
    public class RentalContractsController : Controller
    {
        private readonly AppDbContext _context;

        public RentalContractsController(AppDbContext context)
        {
            _context = context;
        }

        // GET: RentalContracts
        public async Task<IActionResult> Index()
        {
            var contracts = _context.RentalContracts
                .Include(c => c.Client)
                .Include(c => c.Vehicle);

            return View(await contracts.ToListAsync());
        }

        // GET: RentalContracts/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null) return NotFound();

            var contract = await _context.RentalContracts
                .Include(c => c.Client)
                .Include(c => c.Vehicle)
                .FirstOrDefaultAsync(m => m.Id == id);

            if (contract == null) return NotFound();

            return View(contract);
        }

        // GET: RentalContracts/Create
        public IActionResult Create()
        {
            ViewBag.Clients = new SelectList(_context.Clients, "Id", "NomeCompleto");
            ViewBag.Vehicles = new SelectList(_context.Vehicles.Where(v => !v.EstaAlugado), "Id", "Matricula");

            return View();
        }

        // POST: RentalContracts/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(RentalContract contract)
        {
            // Validações de datas
            if (contract.DataInicio < DateTime.Today)
                ModelState.AddModelError("DataInicio", "A data de início não pode ser anterior à data actual.");

            if (contract.DataFim <= contract.DataInicio)
                ModelState.AddModelError("DataFim", "A data de fim deve ser posterior à data de início.");

            // Validar veículo
            var vehicle = _context.Vehicles.FirstOrDefault(v => v.Id == contract.VehicleId);
            if (vehicle == null || vehicle.EstaAlugado)
                ModelState.AddModelError("VehicleId", "O veículo seleccionado não está disponível.");

            // Validar se o cliente tem carta de condução
            var client = _context.Clients.FirstOrDefault(c => c.Id == contract.ClientId);
            if (client != null && !client.CartaConducao)
            {
                ModelState.AddModelError("ClientId", "O cliente selecionado não tem carta de condução.");
            }

            // Se houver erros, recarrega dropdowns e volta à view
            if (!ModelState.IsValid)
            {
                ViewBag.Clients = new SelectList(_context.Clients, "Id", "NomeCompleto");
                ViewBag.Vehicles = new SelectList(_context.Vehicles.Where(v => !v.EstaAlugado), "Id", "Matricula");
                return View(contract);
            }
            else
            {
                // Marcar veículo como alugado
                vehicle.EstaAlugado = true;

                _context.Add(contract);
                await _context.SaveChangesAsync();

                return RedirectToAction(nameof(Index));
            }
        }

        // GET: RentalContracts/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null) return NotFound();

            var contract = await _context.RentalContracts.FindAsync(id);
            if (contract == null) return NotFound();

            ViewBag.Clients = new SelectList(_context.Clients, "Id", "NomeCompleto", contract.ClientId);

            // Veículo actual deve ser incluído na lista mesmo se estiver alugado
            ViewBag.Vehicles = new SelectList(_context.Vehicles, "Id", "Matricula", contract.VehicleId);

            return View(contract);
        }

        // POST: RentalContracts/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, RentalContract contract)
        {
            if (id != contract.Id) return NotFound();

            if (contract.DataFim <= contract.DataInicio)
                ModelState.AddModelError("DataFim", "A data de fim deve ser posterior à data de início.");

            if (!ModelState.IsValid)
            {
                ViewBag.Clients = new SelectList(_context.Clients, "Id", "NomeCompleto", contract.ClientId);
                ViewBag.Vehicles = new SelectList(_context.Vehicles, "Id", "Matricula", contract.VehicleId);
                return View(contract);
            }

            try
            {
                _context.Update(contract);
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!_context.RentalContracts.Any(e => e.Id == contract.Id))
                    return NotFound();

                throw;
            }

            return RedirectToAction(nameof(Index));
        }

        // GET: RentalContracts/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null) return NotFound();

            var contract = await _context.RentalContracts
                .Include(c => c.Client)
                .Include(c => c.Vehicle)
                .FirstOrDefaultAsync(m => m.Id == id);

            if (contract == null) return NotFound();

            return View(contract);
        }

        // POST: RentalContracts/Delete
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var contract = await _context.RentalContracts
                .Include(c => c.Vehicle)
                .FirstOrDefaultAsync(m => m.Id == id);

            if (contract != null)
            {
                // Veículo fica livre outra vez
                contract.Vehicle.EstaAlugado = false;

                _context.RentalContracts.Remove(contract);
                await _context.SaveChangesAsync();
            }

            return RedirectToAction(nameof(Index));
        }
    }
}
