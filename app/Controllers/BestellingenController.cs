using System;
using System.Linq;
using DienstenCheques.Filters;
using DienstenCheques.Models.Domain;
using DienstenCheques.Models.ViewModels.BestellingenViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace DienstenCheques.Controllers
{
    [ServiceFilter(typeof(GebruikerFilter))]
    [Authorize(Policy = "customer")]
    public class BestellingenController : Controller
    {
        private readonly IGebruikersRepository _gebruikersRepository;

        public BestellingenController(IGebruikersRepository gebruikersRepository)
        {
            _gebruikersRepository = gebruikersRepository;
        }


        public ActionResult Index(Gebruiker gebruiker, int aantalMaanden=6)
        {
            IndexViewModel vm = new IndexViewModel()
            {
                Bestellingen = gebruiker.GetBestellingen(aantalMaanden)
                             .Select(b => new BestellingViewModel(b)),
                AantalBeschikbareCheques = gebruiker.AantalBeschikbareElektronischeCheques,
                AantalOpenstaandePrestatieUren = gebruiker.AantalOpenstaandePrestatieUren,
                AantalMaanden = aantalMaanden
            };
            if (Request != null && Request.Headers["X-Requested-With"] == "XMLHttpRequest")
                return PartialView("_Bestelling", vm.Bestellingen);
            return View(vm);
        }

        public ActionResult Nieuw(Gebruiker gebruiker)
        {
            NieuwViewModel vm = new NieuwViewModel(Bestelling.Bedragcheque);
            return View(vm);
        }

        [HttpPost]
        public ActionResult Nieuw(Gebruiker gebruiker, NieuwViewModel model)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    Bestelling b = gebruiker.AddBestelling(model.AantalCheques, model.Elektronisch, model.DebiteerDatum);
                    _gebruikersRepository.SaveChanges();
                    TempData["message"] = $"Uw bestelling voor een totaalbedrag van {b.TotaalBedrag:N0} € werd gecreëerd";
                    return RedirectToAction(nameof(Index));
                }
                catch (Exception ex)
                {
                    ModelState.AddModelError("", ex.Message);
                }
            }
            model.Zichtwaarde = Bestelling.Bedragcheque;
            return View(model);


        }
    }
}
