namespace SPASv2.Models
{
    public class UtilityViewModel
    {
        public AlertProperties Alert { get; set; }
    }

    public class AlertProperties {

        public AlertProperties() 
        {
            title = "";
            text = "";
            icon = "";
            successmethod = "";

            confirmButtonText = "Ok";
            cancelButtonText = "Cancel";

            showCancelButton = false;
        }

        public string title { get; set; }
        public string text { get; set; }
        public string icon { get; set; }
        public string successmethod { get; set; }

        public string confirmButtonText { get; set; }
        public string cancelButtonText { get; set; }

        public bool showCancelButton { get; set; }

        public string cancelmethod { get; set; }
    }
}
