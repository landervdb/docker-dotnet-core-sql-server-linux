
using System;

namespace DienstenCheques.Models.Domain
{
    public class DienstenCheque
    {
        #region "Properties"
        public int DienstenChequeNummer { get; set; }
        public virtual Prestatie Prestatie { get; set; }
        public DateTime? GebruiksDatum { get; set; }
        public DateTime CreatieDatum { get; private set; }
        public bool Elektronisch { get; private set; }

        #endregion


        #region "Constructors"
        public DienstenCheque()
        {

        }

        public DienstenCheque(bool elektronisch, DateTime creatieDatum)
            : this()
        {
            Elektronisch = elektronisch;
            CreatieDatum = creatieDatum;
        }

        public DienstenCheque(bool elektronisch) : this(elektronisch, DateTime.Today)
        {
        }
        #endregion

        }
    }
