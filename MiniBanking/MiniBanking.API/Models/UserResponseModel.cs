using MiniBanking.Core.Models;

namespace MiniBanking.API.Models;

public class UserResponseModel:BaseModel
{
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public string Email { get; set; }
    public string PhoneNumber { get; set; }
    public string Image { get; set; }
    public string IsActive { get; set; }
}