namespace InfoScreenServer.ViewModels
{
    public class RoomDetailsVm
    {
        public string Name { get; set; }
        
        public string Image => $"images/{Name}.png";

        public int Capacity { get; set; }
    }
}