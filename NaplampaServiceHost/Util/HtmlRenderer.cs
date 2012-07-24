using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Drawing.Imaging;
using System.Drawing;
using System.Windows.Forms;
using System.Threading;

namespace NaplampaWcfHost.Util
{
    public class HtmlRenderer
    {
        private static WebBrowser webBrowser = null;
        private static string outputFilename = "";
        public static bool CompletedSuccessfully = false;

        static HtmlRenderer()
        {
            CompletedSuccessfully = false;
            webBrowser = new WebBrowser();
            webBrowser.DocumentCompleted += new WebBrowserDocumentCompletedEventHandler(webBrowser_DocumentCompleted);
        }

        public static void RenderHtml(string url, string outputFilename)
        {
            HtmlRenderer.outputFilename = outputFilename;
            webBrowser.Navigate(url);
        }

        private static void webBrowser_DocumentCompleted(object sender, WebBrowserDocumentCompletedEventArgs e)
        {
            if (GetImageFromHtml(outputFilename, webBrowser))
            {
                CompletedSuccessfully = true;
            }
            else
            {
                CompletedSuccessfully = false;
            }
        }

        /// <summary>
        /// Method for saving Html content into image
        /// </summary>
        /// <param name="imageName">File name of image</param>
        /// <param name="webBrowser">Target WebBrowser control to query for content</param>
        /// <returns>True if save was succesfull</returns>
        /// <seealso cref="System.Windows.Forms.WebBrowser" />

        private static bool GetImageFromHtml(string imageName, WebBrowser webBrowser)
        {
            if (webBrowser.Document == null)
            {
                return false;
            }

            // Give time to WebBrowser control to finish rendering of document
            Thread.Sleep(200);

            try
            {

                // save old width / height
                Size originalSize = new Size(webBrowser.Width, webBrowser.Height);

                // Change to full scroll size
                int scrollHeight = webBrowser.Document.Body.ScrollRectangle.Height;
                int scrollWidth = webBrowser.Document.Body.ScrollRectangle.Width;

                Bitmap image = new Bitmap(scrollWidth, scrollHeight);
                webBrowser.Size = new Size(scrollWidth, scrollHeight);

                // Draw to image
                webBrowser.DrawToBitmap(image, webBrowser.ClientRectangle);
                webBrowser.Size = originalSize;

                // Old one with bad quality:
                // image.Save(imageName, ImageFormat.Jpeg);

                // Save in full quality
                SaveJPG(image, imageName, 100);

                return true;
            }

            catch { return false; }
        }

        // <summary>
        /// Gets codec info by given mimeType
        /// </summary>
        /// <param name="mimeType">mimeType to lookup for</param>
        /// <returns>ImageCodecInfo if all ok or null</returns>

        private static ImageCodecInfo GetCodecInfo(String mimeType)
        {
            ImageCodecInfo[] encoders;
            encoders = ImageCodecInfo.GetImageEncoders();
            for (int iterator = 0; iterator < encoders.Length; ++iterator)
            {
                if (encoders[iterator].MimeType == mimeType)
                    return encoders[iterator];
            }
            return null;
        }

        /// <summary>
        /// Save an Image to JPEG with given compression quality
        /// </summary>
        /// <param name="image">Image to save</param>
        /// <param name="imageName">File name to store image</param>
        /// <param name="quality">Quality parameter: 0 - lowest quality, smallest size,
        /// 100 - max quality and size</param>
        /// <returns>True if save was succesfull</returns>

        private static bool SaveJPG(Image image, string imageName, long qual)
        {
            EncoderParameters eps = new EncoderParameters(1);
            eps.Param[0] = new EncoderParameter(System.Drawing.Imaging.Encoder.Quality, qual);
            ImageCodecInfo ici = GetCodecInfo("image/jpeg");

            if (ici == null) { return false; }

            image.Save(imageName, ici, eps);
            return true;
        }

    }
}
