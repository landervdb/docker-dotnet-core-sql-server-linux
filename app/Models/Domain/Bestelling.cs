using System;

namespace DienstenCheques.Models.Domain
{
    public class Bestelling
    {
        #region "Attributes and Properties"

        private int _aantalAangekochteCheques;

        public static decimal Bedragcheque = 9.0M;
        public int BestellingId { get; private set; }
        public DateTime CreatieDatum { get; private set; }
        public DateTime DebiteerDatum { get; private set; }
        
        public int AantalAangekochteCheques
        {
            get { return _aantalAangekochteCheques; }
            set
            {
                if (value <= 0 || value > 50)
                    throw new ArgumentException("U kan maximaal 50 dienstencheques bestellen");
                _aantalAangekochteCheques = value;
            }
        }
        public bool Elektronisch { get; set; }
        public decimal TotaalBedrag => AantalAangekochteCheques * Bedragcheque;

        #endregion

        #region "Constructors"
        public Bestelling()
        {
            StelDatumsIn(DateTime.Today, DateTime.Today);
        }

        public Bestelling(int aantal, bool elektronisch, DateTime debiteerDatum)
            : this()
        {
            AantalAangekochteCheques = aantal;
            Elektronisch = elektronisch;
            StelDatumsIn(DateTime.Today, debiteerDatum);
        }
        #endregion

        #region "methods"

        public void StelDatumsIn(DateTime creatieDatum, DateTime debiteerDatum)
        {
            if ( creatieDatum.Date > debiteerDatum.Date  || debiteerDatum.Date > creatieDatum.AddMonths(1).Date)
                throw new ArgumentException("Debiteerdatum maximaal 1 maand na bestelling");
            CreatieDatum = creatieDatum;
            DebiteerDatum = debiteerDatum;
        }
        #endregion



    }
}
