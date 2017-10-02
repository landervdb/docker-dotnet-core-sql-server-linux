using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace DienstenCheques.Models.ViewModels.BestellingenViewModels
{
    public class IndexViewModel
    {
       public IEnumerable<BestellingViewModel> Bestellingen { get;  set; }
        [Display(Name="Aantal beschikbare cheques")]
        public int AantalBeschikbareCheques { get; set; }
       [Display(Name = "Aantal niet betaalde prestaties (uren)")]
        public int AantalOpenstaandePrestatieUren { get; set; }
        public SelectList AantalMaandenKeuzeList { get; private set; }
        [Display(Name="Periode overzicht bestellingen")]
        public int AantalMaanden { get; set; }
        public IndexViewModel()
        {
            int[] maanden = new int[] {1, 2, 3, 6, 12, 18, 24};
            List<SelectListItem> items  = new List<SelectListItem>();
            foreach (int maand in maanden)
                items.Add(new SelectListItem(){Text = maand + " " + ((maand == 1)? "maand":"maanden"), Value=maand.ToString()});
            AantalMaandenKeuzeList = new SelectList(items, "Value", "Text");
        }
    }

    

   
}