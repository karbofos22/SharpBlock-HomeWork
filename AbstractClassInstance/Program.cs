namespace AbstractClassInstance
{
    public abstract class LandTransport
    {
    }

    public class Car : LandTransport
    {
    }

    internal class Program
    {
        static void Main(string[] args)
        {
            Type landTransport = typeof(LandTransport);
            Type car = typeof(Car);

            if (landTransport.IsAssignableFrom(car))
            {
                object carInstance = Activator.CreateInstance(car);

                LandTransport landTransportCar = carInstance as LandTransport;

                if (landTransportCar is LandTransport)
                {
                    Console.WriteLine("landTransportCar is an instance of LandTransport");
                }
                else
                {
                    Console.WriteLine("landTransportCar is not an instance of LandTransport");
                }
            }
        }
    }
}