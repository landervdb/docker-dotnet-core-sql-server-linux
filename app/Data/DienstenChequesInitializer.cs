using System;
using DienstenCheques.Models.Domain;
using Microsoft.AspNetCore.Identity;
using System.Security.Claims;
using System.Threading.Tasks;


namespace DienstenCheques.Data
{
    public  class DienstenChequesInitializer 
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<ApplicationUser> _userManager;

        public DienstenChequesInitializer(ApplicationDbContext context, UserManager<ApplicationUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        public async Task InitializeData()
        {
            _context.Database.EnsureDeleted();
            if (_context.Database.EnsureCreated())
            {
               Onderneming onderneming = new Onderneming() { Naam = "Hogeschool Gent" };
                _context.Ondernemingen.Add(onderneming);

                //onvoldoende cheques beschikbaar, nog 2 openstaande prestatie waarvoor 7 cheques nodig
                Gebruiker jan = new Gebruiker()
                {
                    Naam = "Peeters",
                    Voornaam = "Jan",
                    Email = "jan.peeters@hogent.be"
                };
                for (int i = 12; i >= 0; i--)
                {
                    Prestatie pres = new Prestatie()
                    {
                        AantalUren = 4,
                        DatumPrestatie = DateTime.Today.AddMonths(-i),
                        PrestatieType = PrestatieType.Schoonmaken,
                        Onderneming = onderneming,
                        Betaald = true
                    };
                    jan.Prestaties.Add(pres);
                }
                jan.GetPrestatie(11).Betaald = false;
                jan.GetPrestatie(12).Betaald = false;
                int p = 0;
                int pi = 0;
                for (int i = 3; i > 0; i--)
                {
                    Bestelling b = new Bestelling()
                    {
                        AantalAangekochteCheques = 15,
                        Elektronisch = true
                    };
                    b.StelDatumsIn(DateTime.Today.AddMonths(-4 * i), DateTime.Today.AddMonths(-4 * i));
                    jan.Bestellingen.Add(b);
                    for (int j = 1; j <= 15; j++)
                    {
                        DienstenCheque d = new DienstenCheque(true, DateTime.Today.AddMonths(-4 * i));
                        if (p < 11)
                        {
                            d.Prestatie = jan.GetPrestatie(p);
                            d.GebruiksDatum = d.Prestatie.DatumPrestatie;
                        }
                        jan.Portefeuille.Add(d);
                        if (pi < 3)
                            pi++;
                        else
                        {
                            pi = 0;
                            p++;
                        }
                    }
                }
      
                _context.Gebruikers.Add(jan);
                ApplicationUser user1 = new ApplicationUser { UserName = jan.Email, Email = jan.Email };
                await _userManager.CreateAsync(user1, "P@ssword1");
                await _userManager.AddClaimAsync(user1, new Claim(ClaimTypes.Role, "customer"));


                //alle cheques zijn toegewezen aan prestaties, geen openstaande prestatie meer
                Gebruiker an = new Gebruiker() {  Naam = "Pieters", Voornaam = "An", Email = "an.pieters@hogent.be"};
                Bestelling anBestelling = new Bestelling() { AantalAangekochteCheques = 20, Elektronisch = true };
                anBestelling.StelDatumsIn(DateTime.Today.AddMonths(-1), DateTime.Today.AddMonths(-1));
                an.Bestellingen.Add(anBestelling);
                for (int i = 4; i > 0; i--)
                    an.Prestaties.Add(new Prestatie() { AantalUren = 5, DatumPrestatie = DateTime.Today.AddMonths(-i), PrestatieType = PrestatieType.Schoonmaken, Onderneming = onderneming, Betaald = true });
                for (int j = 0; j <= 19; j++)
                {
                    DienstenCheque d = new DienstenCheque(true, DateTime.Today.AddMonths(-1));
                    d.Prestatie = an.GetPrestatie(j / 5);
                    d.GebruiksDatum = d.Prestatie.DatumPrestatie;
                    an.Portefeuille.Add(d);
                }
              
                _context.Gebruikers.Add(an);
                ApplicationUser user2 = new ApplicationUser { UserName = an.Email, Email = an.Email };
                await _userManager.CreateAsync(user2, "P@ssword1");
                await _userManager.AddClaimAsync(user2, new Claim(ClaimTypes.Role, "customer"));


                //nog 2 cheques niet gebruikt, geen openstaande prestaties
                Gebruiker tine = new Gebruiker() {  Naam = "Pieters", Voornaam = "Tine", Email = "tine.pieters@hogent.be"};
                Bestelling tineBestelling = new Bestelling() { AantalAangekochteCheques = 6, Elektronisch = true };
                tineBestelling.StelDatumsIn(DateTime.Today.AddMonths(-1), DateTime.Today.AddMonths(-1));
                tine.Bestellingen.Add(tineBestelling);
                tine.Prestaties.Add(new Prestatie() { AantalUren = 4, DatumPrestatie = DateTime.Today.AddDays(-10), PrestatieType = PrestatieType.Schoonmaken, Onderneming = onderneming, Betaald = true });
                for (int j = 1; j <= 6; j++)
                {
                    DienstenCheque d = new DienstenCheque(true, DateTime.Today.AddMonths(-1));
                    if (j < 5)
                    {
                        d.Prestatie = tine.GetPrestatie(0);
                        d.GebruiksDatum = d.Prestatie.DatumPrestatie;
                    }
                    tine.Portefeuille.Add(d);
                }
                _context.Gebruikers.Add(tine);
                ApplicationUser user3 = new ApplicationUser { UserName = tine.Email, Email = tine.Email };
                await _userManager.CreateAsync(user3, "P@ssword1");
                await _userManager.AddClaimAsync(user3, new Claim(ClaimTypes.Role, "customer"));

                _context.SaveChanges();
  
            }

           
        }

   
    }
}
