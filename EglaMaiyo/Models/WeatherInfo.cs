namespace EglaMaiyo.Models
{
    public class WeatherInfo
    {
        public string City { get; set; }
        public string Description { get; set; }  
        public float TemperatureCelsius { get; set; }
        public float TemperatureFahrenheit => 32 + (TemperatureCelsius * 9 / 5);
    }
}
