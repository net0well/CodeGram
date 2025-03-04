using CodeGram.Data.Models;

namespace CodeGram.ViewModel.Users
{
    public class GetUserProfileVM
    {
        public User User { get; set; }
        public List<Post> Posts { get; set; }
    }
}
