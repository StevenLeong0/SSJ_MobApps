using BCryptHash = BCrypt.Net.BCrypt;
public class Hiel {
        public void Hi(string plainPassword)
        {
var hash = BCryptHash.HashPassword(plainPassword);
        
}
}
