using System;
using System.Collections.Generic;
using System.IO;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.PixelFormats;
using SixLabors.ImageSharp.Processing;
using System.Drawing;
using SixLabors.Shapes;
using Nito.AsyncEx;
using System.Text.Json;

namespace CognitiveCoreUCU
{
    public class CognitiveFace
    {
        private bool initialized = false;

        private static string subscriptionKey;

        // NOTE: You must use the same region in your REST call as you used to
        // obtain your subscription keys. For example, if you obtained your
        // subscription keys from westus, replace "westcentralus" in the URL
        // below with "westus".
        //
        // Free trial subscription keys are generated in the westcentralus region.
        // If you use a free trial subscription key, you shouldn't need to change
        // this region.
        const string uriBase = "https://eastus.api.cognitive.microsoft.com/face/v1.0/detect"; //"https://cognitiveapiucu.cognitiveservices.azure.com/face/v1.0/detect";

        /// <summary>
        /// Indica si se debe dibujar un recuadro en cada cara encontrada
        /// </summary>
        /// <value></value>
        public bool MarkFaces {get;set;}
        Rgba32 boxColor;
        /// <summary>
        /// Indica si la ultima llamada encontró o no una cara en la imagen
        /// </summary>
        /// <value></value>
        public bool FaceFound { get; private set; }
        /// <summary>
        /// Indica si la ultima llamada encontró una sonrisa o no
        /// </summary>
        /// <value></value>
        public bool SmileFound {get; private set;}
        /// <summary>
        /// Esta clase se encarga de consultar el servicio cloud para encontrar caras en las fotos
        /// </summary>
        /// <param name="subscriptionKey">api key para invocar el servicio</param>
        /// <param name="boxColor">color a utilizar para dibujar un recuadro si se encuentra una cara</param>
        public CognitiveFace(bool markFaces = false, System.Drawing.Color? boxColor = null)
        {
            if (!initialized)
            {
                AsyncContext.Run(InitializeAsync);
				initialized = true;
            }

            if(boxColor is null)
            {
                this.MarkFaces = false;
            }
            else
            {
                this.boxColor = new Rgba32(boxColor.Value.R,boxColor.Value.G,boxColor.Value.B,boxColor.Value.A);
            }
            this.MarkFaces = markFaces;
        }

		private static async Task InitializeAsync()
		{
			HttpClient client = new HttpClient();
            HttpResponseMessage response = await client.GetAsync(@"https://pii-secretsapiwebapp.azurewebsites.net/CognitiveApiSecrets");
            string result = await response.Content.ReadAsStringAsync();
            CognitiveApiSecrets secrets = System.Text.Json.JsonSerializer.Deserialize<CognitiveApiSecrets>(result,
                new JsonSerializerOptions { PropertyNamingPolicy  = JsonNamingPolicy.CamelCase });

			subscriptionKey = secrets.SubscriptionKey;

			//sigHasher = new HMACSHA1(new ASCIIEncoding().GetBytes(string.Format("{0}&{1}", consumerKeySecret, accessTokenSecret)));
		}

        public void Recognize(string path)
        {
            FaceFound = false;
            this.MakeAnalysisRequest(path).Wait();
        }
        /// <summary>
        /// Gets the analysis of the specified image by using the Face REST API.
        /// </summary>
        /// <param name="imageFilePath">The image file.</param>
        private async Task MakeAnalysisRequest(string imageFilePath)
        {
            HttpClient client = new HttpClient();

            // Request headers.
            client.DefaultRequestHeaders.Add("Ocp-Apim-Subscription-Key", subscriptionKey);

            // Request parameters. A third optional parameter is "details".
            string requestParameters = "returnFaceId=true&returnFaceLandmarks=false" +
                "&returnFaceAttributes=age,gender,headPose,smile,facialHair,glasses," +
                "emotion,hair,makeup,occlusion,accessories,blur,exposure,noise";

            //returnFaceId=true&returnFaceLandmarks=false&recognitionModel=recognition_04&returnRecognitionModel=false&detectionModel=detection_03&faceIdTimeToLive=86400 HTTP/1.1

            // Assemble the URI for the REST API Call.
            string uri = uriBase + "?" + requestParameters;

            HttpResponseMessage response;

            // Request body. Posts a locally stored JPEG image.
            byte[] byteData = GetImageAsByteArray(imageFilePath);

            using (ByteArrayContent content = new ByteArrayContent(byteData))
            {
                // This example uses content type "application/octet-stream".
                // The other content types you can use are "application/json"
                // and "multipart/form-data".
                content.Headers.ContentType =
                    new MediaTypeHeaderValue("application/octet-stream");

                // Execute the REST API call.
                response = await client.PostAsync(uri, content);

                // Get the JSON response.
                string contentString = await response.Content.ReadAsStringAsync();
List<CognitiveResult> resArray = JsonConvert.DeserializeObject<List<CognitiveResult>>(contentString);
                this.SmileFound = false;
                if (resArray.Count > 0)
                {
                    this.FaceFound = true;
                }
                foreach (CognitiveResult face in resArray)
                {
                    this.SmileFound = face.faceAttributes.smile > 0.6;
                    if(this.MarkFaces)
                    {
                        DrawRectangle(face, imageFilePath);
                    }
                }
            }
        }
        private void DrawRectangle(CognitiveResult face, string imgPath)
        {
            int x0 = face.faceRectangle.left;
            int y0 = face.faceRectangle.top;
            int width = face.faceRectangle.width;
            int height = face.faceRectangle.height;
            SixLabors.Primitives.RectangleF recc = new SixLabors.Primitives.RectangleF(x0,y0,width,height);
            float thick = 4;
            using (Image<Rgba32> image = Image.Load(imgPath))
            {
                image.Mutate(DrawRectangleExtensions=> DrawRectangleExtensions.Draw(
                        color:boxColor,
                        thickness:thick,
                        shape: recc
                    )
                );
                image.Save("tmpFace.jpg"); // Automatic encoder selected based on extension.
            }
        }

        /// <summary>
        /// Returns the contents of the specified file as a byte array.
        /// </summary>
        /// <param name="imageFilePath">The image file to read.</param>
        /// <returns>The byte array of the image data.</returns>
        static byte[] GetImageAsByteArray(string imageFilePath)
        {
            using (FileStream fileStream =
                new FileStream(imageFilePath, FileMode.Open, FileAccess.Read))
            {
                BinaryReader binaryReader = new BinaryReader(fileStream);
                return binaryReader.ReadBytes((int)fileStream.Length);
            }
        }
        private class CognitiveResult
        {
            public string faceId { get; set; }
            public FaceRect faceRectangle { get; set; }

            public FaceAttributes faceAttributes {get; set;}
        }
        private class FaceAttributes
        {
            public double age {get;set;}
            public string gender {get;set;}
            public double smile {get;set;}
        }
        private class FaceRect
        {
            public int top { get; set; }
            public int left { get; set; }
            public int width { get; set; }
            public int height { get; set; }
        }
    }
}