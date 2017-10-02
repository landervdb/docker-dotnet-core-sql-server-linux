using System.Collections.Generic;

namespace DienstenCheques.Models.Domain
{
    public interface IGebruikersRepository
        {
            Gebruiker GetBy(int gebruikersNummer);
            Gebruiker GetByEmail(string email);
            IEnumerable<Gebruiker> GetAll();
            void SaveChanges();
        }

    }

