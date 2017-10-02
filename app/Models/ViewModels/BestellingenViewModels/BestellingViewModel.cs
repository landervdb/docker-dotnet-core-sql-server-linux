using System;
using System.ComponentModel.DataAnnotations;
using DienstenCheques.Models.Domain;

namespace DienstenCheques.Models.ViewModels.BestellingenViewModels
{
    public class BestellingViewModel
    {
        [Display(Name = "Referentie")]
        public int BestellingId { get; set; }
        [Display(Name = "Datum bestelling")]
        public DateTime CreatieDatum { get; set; }
        public bool Elektronisch { get; set; }
        [Display(Name = "Hoeveelheid")]
        public int AantalCheques { get; set; }
        [Display(Name = "Zichtwaarde")]
        public decimal Zichtwaarde { get; set; }
        [Display(Name = "Totaal bedrag")]
        public decimal TotaalBedrag { get; set; }

        public BestellingViewModel(Bestelling bestelling)
        {
            BestellingId = bestelling.BestellingId;
            CreatieDatum = bestelling.CreatieDatum;
            Elektronisch = bestelling.Elektronisch;
            AantalCheques = bestelling.AantalAangekochteCheques;
            Zichtwaarde = Bestelling.Bedragcheque;
            TotaalBedrag = bestelling.TotaalBedrag;
        }
    }
}
