using Newtonsoft.Json.Linq;
using System.Text;
using System.Web;
using System;
using System.Linq;
using System.IO;
using WeatherAPI;
using Microsoft.Extensions.Configuration;

var sr = new StreamReader("appsettings.json");
var json = sr.ReadToEnd().Split(",");
var OCKey = json[0].Split(":")[1].Replace("\"", " ").Trim().ToString();
var weatherKey = json[1].Split(":")[1].Replace("\"", " ").Replace("}", " ").Trim().ToString();


var client = new HttpClient();
var encode = new ASCIIEncoding();


Console.WriteLine("Weather for?:");
var locationAddressDecode = Console.ReadLine();
var locationAddressEncode = HttpUtility.UrlEncode(locationAddressDecode, encode);

var geoCodingURL = $"https://api.opencagedata.com/geocode/v1/json?q={locationAddressEncode}&key={OCKey}";
var geoCodingResponse = client.GetStringAsync(geoCodingURL).Result;
var geoCodeResult = JObject.Parse(geoCodingResponse).SelectToken("results");

var latitudeDMS = geoCodeResult[0]["annotations"]["DMS"]["lat"].ToString();
var longitudeDMS = geoCodeResult[0]["annotations"]["DMS"]["lng"].ToString();

var latDegrees = Utility.ConvertDegreeAngleToDouble(latitudeDMS);
var lngDegrees = Utility.ConvertDegreeAngleToDouble(longitudeDMS);

var weatherURL = $"https://api.openweathermap.org/data/2.5/weather?lat={latDegrees}&lon={lngDegrees}&appid={weatherKey}&units=imperial";
var weatherResponse = client.GetStringAsync(weatherURL).Result;
var weatherResult = JObject.Parse(weatherResponse).SelectToken("main").SelectToken("temp").ToString();

Console.WriteLine($"{weatherResult} degrees F");
