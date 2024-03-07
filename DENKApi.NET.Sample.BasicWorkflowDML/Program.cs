// See https://aka.ms/new-console-template for more information
using DENKApi.NET.Core;
using DENKApi.NET.DML;
using ProtoBuf;
using System.Drawing;
using System.Text;

Console.WriteLine("DENKApi.NET.GPU example!");

IDENKApi denkApi = DENKApiFactory.GetApi();

// 1. AUTHENTICATE
string token = File.ReadAllText("Token.txt"); // use your Token for your model instead!
DenkWeitErrorCode response = denkApi.TokenLogin(token, "");
if (response != DenkWeitErrorCode.NoError)
{
    Console.WriteLine($"Error while authenticating via token: '{response}'!");
    return;
}
Console.WriteLine("Successfully authenticated with Token!");

// 2. LOAD MODEL
byte[] networkPathBytes = Encoding.UTF8.GetBytes(@"model/");
int readModelBufferSize = 10000; // this is only a default. ReadAllModels will provide the correct buffer size
byte[] readModelBuffer = new byte[readModelBufferSize];

response = denkApi.ReadAllModels(networkPathBytes, readModelBuffer, ref readModelBufferSize, DenkWeitDevices.GPU1);
if (response != DenkWeitErrorCode.NoError)
{
    Console.WriteLine($"Error while loading model(s): '{response}'!");
    return;
}
Console.WriteLine("Successfully loaded model!");

// 3. LOAD IMAGE
Int32 datasetIndex = 0;
byte[] imageAsBytes;

//     load image into byte array
using (Bitmap sourceImage = new(@"image/crack_example.png"))
{
    using MemoryStream ms = new();
    sourceImage.Save(ms, System.Drawing.Imaging.ImageFormat.Png);
    imageAsBytes = ms.ToArray();
}

//     load byte array into network
response = denkApi.LoadImageData(ref datasetIndex, imageAsBytes, imageAsBytes.Length);
if (response != DenkWeitErrorCode.NoError)
{
    Console.WriteLine($"Error while loading image: '{response}'!");
    return;
}
Console.WriteLine("Successfully loaded image!");

// 4. EVALUATE IMAGE
response = denkApi.EvaluateImage(datasetIndex);
if (response != DenkWeitErrorCode.NoError)
{
    Console.WriteLine($"Error while evaluating image: '{response}'!");
    return;
}
Console.WriteLine("Successfully evaluated image!");

// 5. GET RESULTS
int resultBufferSize = 10000;
byte[] resultBuffer = new byte[resultBufferSize];

response = denkApi.GetResults(datasetIndex, resultBuffer, ref resultBufferSize);
if (response != DenkWeitErrorCode.NoError)
{
    Console.WriteLine($"Error while getting results from network: '{response}'!");
    return;
}
Array.Resize(ref resultBuffer, resultBufferSize); // this is important, because native API sets the size!

//     convert results from protobuf based byte array to Results object
Results anaylsisResults;
using (MemoryStream resultAsStream = new MemoryStream(resultBuffer))
{
    anaylsisResults = Serializer.Deserialize<Results>(resultAsStream);
}

//     output some of the results fields
foreach (var outputField in anaylsisResults.Outputs)
{
    Console.WriteLine($"Field-FeatureUid: '{outputField.FeatureUid}'");
    foreach (var feature in outputField.Features)
    {
        Console.WriteLine($"   Feature-Label: '{feature.Label}'");
        Console.WriteLine($"   Feature-Length: '{feature.Length}'");
        Console.WriteLine($"   Feature-Width: '{feature.Width}'");
        Console.WriteLine($"   Feature-Probability: '{feature.Probability}'");
        Console.WriteLine($"   Feature-Rectangle: 'x:{feature.RectX}, y:{feature.RectY}, w:{feature.RectW}, h: {feature.RectH}'");
    }
}
Console.WriteLine("Successfully showed results!");

// 6. DEALLOCATE
response = denkApi.EndSession();
if (response != DenkWeitErrorCode.NoError)
{
    Console.WriteLine($"Error while ending session: '{response}'!");
    return;
}
Console.WriteLine("Successfully ended session!");
