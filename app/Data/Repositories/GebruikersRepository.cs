using System.Collections.Generic;
using System.Linq;
using DienstenCheques.Models.Domain;
using Microsoft.EntityFrameworkCore;

namespace DienstenCheques.Data.Repositories
{
    public class GebruikersRepository : IGebruikersRepository
    {
        private readonly ApplicationDbContext _context;
        private readonly DbSet<Gebruiker> _gebruikers;

        public GebruikersRepository(ApplicationDbContext context)
        {
            _context = context;
            _gebruikers = context.Gebruikers;
        }
        public Gebruiker GetBy(int gebruikersNummer)
        {
            return _gebruikers
                .Include(g => g.Bestellingen)
                .Include(g => g.Portefeuille)
                .Include(g => g.Prestaties)
                .SingleOrDefault(g => g.GebruikersNummer == gebruikersNummer);
        }
        public Gebruiker GetByEmail(string email)
        {
            return _gebruikers
                .Include(g => g.Bestellingen)
                .Include(g => g.Portefeuille)
                .Include(g => g.Prestaties)
                .FirstOrDefault(g => g.Email == email);
        }

        public IEnumerable<Gebruiker> GetAll()
        {
            return _gebruikers.ToList();
        }

        public void SaveChanges()
        {
            _context.SaveChanges();
        }
    }

}
