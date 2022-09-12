using Newtonsoft.Json.Linq;
using System.Text;
using System.Web;
using System;
using System.Linq;
using System.IO;
using WeatherAPI;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;


var sr = new StreamReader("appsettings.json");
var json = sr.ReadToEnd();


List<Key> keys = JsonConvert.DeserializeObject<List<Key>>(json);


var client = new HttpClient();
var encode = new ASCIIEncoding();


Console.WriteLine("Weather for?:");
var locationAddressDecode = Console.ReadLine();
var locationAddressEncode = HttpUtility.UrlEncode(locationAddressDecode, encode);


try
{
    var geoCodingURL = $"https://api.opencagedata.com/geocode/v1/json?q={locationAddressEncode}&key={keys[0].ApiKey}";
    var geoCodingResponse = client.GetStringAsync(geoCodingURL).Result;
    var geoCodeResult = JObject.Parse(geoCodingResponse);

    var latitudeDMS = geoCodeResult["results"][0]["annotations"]["DMS"]["lat"].ToString();
    var longitudeDMS = geoCodeResult["results"][0]["annotations"]["DMS"]["lng"].ToString();

    var latDegrees = Utility.ConvertDegreeAngleToDouble(latitudeDMS);
    var lngDegrees = Utility.ConvertDegreeAngleToDouble(longitudeDMS);

    var weatherURL = $"https://api.openweathermap.org/data/2.5/weather?lat={latDegrees}&lon={lngDegrees}&appid={keys[1].ApiKey}&units=imperial";
    var weatherResponse = client.GetStringAsync(weatherURL).Result;
    var weatherResult = JObject.Parse(weatherResponse);

    var forecast = weatherResult["weather"][0]["description"].ToString();
    var currentTemp = weatherResult["main"]["temp"].ToString();
    var minTemp = weatherResult["main"]["temp_min"].ToString();
    var maxTemp = weatherResult["main"]["temp_max"].ToString();
    var country = weatherResult["sys"]["country"].ToString();
    var city = weatherResult["name"].ToString();

    Console.WriteLine($"{city}, {country}");
    Console.WriteLine(forecast);
    Console.WriteLine($"Right now: {currentTemp} degrees F");
    Console.WriteLine($"Daily high: {maxTemp}");
    Console.WriteLine($"Daily low: {minTemp}");
}
catch (ArgumentOutOfRangeException)
{
    Console.WriteLine("error: invalid search");
}
catch (HttpRequestException)
{
    Console.WriteLine("error: invalid request");
}
catch (Exception)
{
    Console.WriteLine("an unexpected error occured");
}