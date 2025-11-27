namespace SPASv2.Models
{
    public class AccountViewModel
    {
        public AuthentiCate AuthentiCate { get; set; } = new AuthentiCate();
    }

    public class AuthentiCate
    {

        public AuthentiCate()
        {
            title = "Authentication";
            text = "Please enter your username and password to complete the authentication process.";
            successmethod = "";

            confirmButtonText = "Ok";
            cancelButtonText = "Cancel";

            showCancelButton = true;
        }

        public string title { get; set; }
        public string text { get; set; }
        public string successmethod { get; set; }

        public string confirmButtonText { get; set; }
        public string cancelButtonText { get; set; }

        public bool showCancelButton { get; set; }

        public string cancelmethod { get; set; }
    }
}
