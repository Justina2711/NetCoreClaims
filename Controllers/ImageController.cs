using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;
using System.Threading.Tasks;
using ClaimsDemo.Providers;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Distributed;

namespace ClaimsDemo.Controllers
{
    public class ImageController : Controller
    {
        private readonly IDistributedCache _cache;

        public ImageController(IDistributedCache cache)
        {
            _cache = cache;
        }

        [Route("image")]
        public async Task<IActionResult> Index(string url, int destinationWidth, int destinationHeight)
        {
            Image sourceImage = await this.LoadImageFromUrl(url);

            if (sourceImage != null)
            {
                try
                {
                    var key = $"/{destinationWidth}/{destinationHeight}/{url}";
                   var data= await _cache.GetAsync(key);
                    //Stream outputStream = await _cache.GetAsyncModel<Stream>(key);
                    if (data == null)
                    {
                        var memoryStream = new MemoryStream();
                        Image destinationImage = this.ResizeImageKeepAspectRatio(sourceImage, destinationWidth, destinationHeight);
                        destinationImage.Save(memoryStream, ImageFormat.Jpeg);

                        memoryStream.Seek(0, SeekOrigin.Begin);
                       
                        data = memoryStream.ToArray();
                        await _cache.SetAsync(key, data, new DistributedCacheEntryOptions() { AbsoluteExpirationRelativeToNow = TimeSpan.FromHours(24) });
                    }
  
                    return this.File(data, "image/jpg");
                }
                catch (Exception e)
                {

                    throw new Exception(e.Message);
                }


            }

            return this.NotFound();
        }

        private async Task<Image> LoadImageFromUrl(string url)
        {
            Image image = null;

            try
            {

                HttpClient httpClient = new HttpClient();
                HttpResponseMessage response = await httpClient.GetAsync(url);
                Stream inputStream = await response.Content.ReadAsStreamAsync();

                using (Bitmap temp = new Bitmap(inputStream))
                {
                    image = new Bitmap(temp);
                }
            }

            catch
            {
                // Add error logging here
            }

            return image;
        }

       


        public Image ResizeImageKeepAspectRatio(Image source, int width, int height)
        {
            Image result = null;

            try
            {
                if (source.Width != width || source.Height != height)
                {
                    // Resize image
                    float sourceRatio = (float)source.Width / source.Height;

                    using (var target = new Bitmap(width, height))
                    {
                        using (var g = System.Drawing.Graphics.FromImage(target))
                        {
                            g.CompositingQuality = CompositingQuality.HighQuality;
                            g.InterpolationMode = InterpolationMode.HighQualityBicubic;
                            g.SmoothingMode = SmoothingMode.HighQuality;

                            // Scaling
                            float scaling;
                            float scalingY = (float)source.Height / height;
                            float scalingX = (float)source.Width / width;
                            if (scalingX < scalingY) scaling = scalingX; else scaling = scalingY;

                            int newWidth = (int)(source.Width / scaling);
                            int newHeight = (int)(source.Height / scaling);

                            // Correct float to int rounding
                            if (newWidth < width) newWidth = width;
                            if (newHeight < height) newHeight = height;

                            // See if image needs to be cropped
                            int shiftX = 0;
                            int shiftY = 0;

                            if (newWidth > width)
                            {
                                shiftX = (newWidth - width) / 2;
                            }

                            if (newHeight > height)
                            {
                                shiftY = (newHeight - height) / 2;
                            }

                            // Draw image
                            g.DrawImage(source, -shiftX, -shiftY, newWidth, newHeight);
                        }

                        result = (Image)target.Clone();
                    }
                }
                else
                {
                    // Image size matched the given size
                    result = (Image)source.Clone();
                }
            }
            catch (Exception)
            {
                result = null;
            }

            return result;
        }

        public ActionResult ImageView()
        {
            return View();
        }
    }
}