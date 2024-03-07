using DENKApi.NET.Core;
using DENKApi.NET.CPU;
using DENKApi.NET.WindowsHelper;
using System;
using System.IO;
using System.Text;

namespace DENKApi.NET.Sample.WindowsHelperCPU
{
    internal class Program
    {
        static void Main()
        {
            Console.WriteLine("DENKApi.NET.CPU example!");

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

            response = denkApi.ReadAllModels(networkPathBytes, readModelBuffer, ref readModelBufferSize, DenkWeitDevices.CPU);
            if (response != DenkWeitErrorCode.NoError)
            {
                Console.WriteLine($"Error while loading model(s): '{response}'!");
                return;
            }
            Console.WriteLine("Successfully loaded model!");

            // 3. LOAD IMAGE
            Int32 datasetIndex = 0;
            byte[] imageAsBytes = ImageConverter.GetImageAsByteArrayFromFile(@"image/crack_example.png", System.Drawing.Imaging.ImageFormat.Png);

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

            //     convert results from protobuf based byte array to Results object
            Results anaylsisResults = ResultConverter.ConvertByteArrayToResultsObject(resultBuffer, resultBufferSize);

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

            Console.ReadLine();
        }
    }
}
