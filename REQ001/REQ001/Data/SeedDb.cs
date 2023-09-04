using REQ001.Data.Entities;
using REQ001.Helpers;

namespace REQ001.Data
{
    public class SeedDb
    {
        private readonly DataContext _context;
        private readonly IUserHelper _userHelper;
        public SeedDb(DataContext context, IUserHelper userHelper)
        {
            _context = context;
            _userHelper = userHelper;
        }

        public async Task SeedAsync()
        {
            await _context.Database.EnsureCreatedAsync();            
            await CheckUserAsync("0801198713256", "Gerardo", "Lanza", "glanza007@gmail.com", "3307 7964", "Calle Luna Calle Sol");

        }

        

        private async Task<User> CheckUserAsync(
        string document,
        string firstName,
        string lastName,
        string email,
        string phone,
        string address
        )
        {
            User user = await _userHelper.GetUserAsync(email);
            if (user == null)
            {
                user = new User
                {
                    FirstName = firstName,
                    LastName = lastName,
                    Email = email,
                    UserName = email,
                    PhoneNumber = phone,
                    Address = address,
                    Document = document,                    
                };

                await _userHelper.AddUserAsync(user, "123456");
                
            }

            return user;
        }

    }
}
