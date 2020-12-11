namespace ServiceManager.Client.Services
{
    public class Style
    {
        //public string Height { get; set; }

        public string GetHeight(int count) {
            if(count < 3) {
                return "100px";
            }
            else if(count < 5) {
                return "200px";
            }
            else {
                return "";
            }
        }
    }
}