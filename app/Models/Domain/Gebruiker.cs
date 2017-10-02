using System;
using System.Collections.Generic;
using System.Linq;

namespace DienstenCheques.Models.Domain
{
    public class Gebruiker
    {
        #region Properties
        public int GebruikersNummer { get; set; }
        public string Naam { get; set; }
        public string Voornaam { get; set; }
       public string Email { get; set; }

        public ICollection<Bestelling> Bestellingen { get; set; }
        public ICollection<DienstenCheque> Portefeuille { get; set; }
        public ICollection<Prestatie> Prestaties { get; set; }
        public int AantalOpenstaandePrestatieUren
        {
            get { return GetOpenstaandePrestaties().Sum(p=>p.AantalUren); }
        }

        public int AantalBeschikbareElektronischeCheques
        {
            get { return GetBeschikbareElektronischeDienstenCheques().Count(); }
        }

        #endregion

        #region Constructors
        public Gebruiker()
        {
            Bestellingen = new List<Bestelling>();
            Prestaties = new List<Prestatie>();
            Portefeuille = new List<DienstenCheque>();
        }
        #endregion

        #region Methods
        private IEnumerable<Prestatie> GetOpenstaandePrestaties()
        {
            return Prestaties.Where(p => !p.Betaald);
        }
        private IEnumerable<DienstenCheque> GetBeschikbareElektronischeDienstenCheques()
        {
            return Portefeuille.Where(c => c.Elektronisch && c.Prestatie==null).OrderBy(c => c.CreatieDatum);
        }

        private int GetAantalBesteldeCheques(int jaar)
        {
            return Bestellingen.Where(b => b.CreatieDatum.Year == jaar).Sum(b => b.AantalAangekochteCheques);
        }


        public IEnumerable<Bestelling> GetBestellingen(int aantalMaanden)
        {
            return Bestellingen.Where(b => (DateTime.Today.AddMonths(-aantalMaanden) <= b.CreatieDatum)).OrderByDescending(b=>b.CreatieDatum);
        }

        private void BetaalPrestatie(Prestatie p)
        {
            IList<DienstenCheque> openstaandeCheques = GetBeschikbareElektronischeDienstenCheques().ToList();
            if (openstaandeCheques.Count() >= p.AantalUren)
            {
                for (int i = 0; i < p.AantalUren; i++)
                {
                    openstaandeCheques[i].Prestatie = p;
                    openstaandeCheques[i].GebruiksDatum = DateTime.Today;
                }
                p.Betaald = true;
            }
        }

        public Bestelling AddBestelling(int aantalCheques, bool elektronisch, DateTime debiteerDatum)
        {

            Bestelling b = new Bestelling(aantalCheques, elektronisch, debiteerDatum);
            if (GetAantalBesteldeCheques(b.CreatieDatum.Year) + aantalCheques > 500)
                throw new ArgumentException("Je hebt de grens van 500 checques bereikt");
            Bestellingen.Add(b);
            if (elektronisch)
            {
                for (int i = 0; i < aantalCheques; i++)
                    Portefeuille.Add(new DienstenCheque(elektronisch));
                IEnumerable<Prestatie> nietBetaaldePrestaties = GetOpenstaandePrestaties();
                foreach (Prestatie p in nietBetaaldePrestaties)
                {
                    BetaalPrestatie(p);
                    if (!p.Betaald) break;
                }
            }
            return b;
        }


        public Prestatie GetPrestatie(int index)
        {
            return Prestaties.ToList()[index];
        }
        #endregion
    }
}