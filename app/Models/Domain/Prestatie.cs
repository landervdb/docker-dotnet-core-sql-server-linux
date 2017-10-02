using System;

namespace DienstenCheques.Models.Domain
{
   public class Prestatie
    {
       #region "Properties"

       public int PrestatieId { get; set; }
       public int AantalUren { get; set; }
       public PrestatieType PrestatieType { get; set; }
       public Onderneming Onderneming { get; set; }
       public bool Betaald { get; set; }
       public DateTime DatumPrestatie { get; set; }

       #endregion

       }
}
