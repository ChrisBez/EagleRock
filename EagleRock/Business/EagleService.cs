using EagleRock.Models;

namespace EagleRock.Business
{
    public class EagleService
    {
        public bool StoreData(TrafficData data)
        {
            if (data is not null)
            { 
                return true; 
            }


            return false;
        }


    }
}
