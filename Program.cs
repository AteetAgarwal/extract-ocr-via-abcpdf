// See https://aka.ms/new-console-template for more information
using System.Drawing;
using System.Drawing.Imaging;
using System.Text;
using System.Text.RegularExpressions;
using System.Xml.Linq;
using WebSupergoo.ABCpdf13;
using WebSupergoo.ABCpdf13.Objects;
using static WebSupergoo.ABCpdf13.Objects.ObjectSoup;

internal class Program
{
    private static async Task Main(string[] args)
    {
        // Ensure you have the ABCpdf license key set up correctly
        // You can obtain a license key from the ABCpdf website
        string ABCPDFLicenseKey = "";
        XSettings.InstallLicense(ABCPDFLicenseKey);

        //Provide online PDF URL
        // For example, you can use a URL to a PDF file hosted online
        string pdfUrl = ""; // Replace with your URL
        if (!Directory.Exists("images"))
            Directory.CreateDirectory("images");

        using (HttpClient client = new HttpClient())
        {
            // Download the PDF as a stream
            using (Stream pdfStream = await client.GetStreamAsync(pdfUrl))
            {
                // Convert stream to byte array
                using (MemoryStream memoryStream = new MemoryStream())
                {
                    await pdfStream.CopyToAsync(memoryStream);
                    byte[] pdfData = memoryStream.ToArray();

                    // Load PDF into ABCpdf
                    using (Doc doc = new Doc())
                    {
                        doc.Read(pdfData); // Read from byte array

                        StringBuilder extractedText = new StringBuilder();
                        int totalPages = doc.PageCount;

                        Console.WriteLine($"PDF Loaded Successfully! Total Pages: {totalPages}");

                        int imageIndex = 1;
                        doc.PageNumber = 1; // Set current page

                        // Extract images from PixMap (more reliable)
                        foreach (var objId in doc.ObjectSoup)
                        {
                            if (objId != null)
                            {
                                if (doc.ObjectSoup[objId.ID] is PixMap pixMap)
                                {
                                    string outputPath = Path.Combine("images", $"Image{imageIndex}.png");
                                    using (Bitmap bm = pixMap.GetBitmap())
                                    {
                                        using (MemoryStream imagems = new MemoryStream())
                                        {
                                            bm.Save(imagems, ImageFormat.Png); // Save as PNG format
                                            imagems.Position = 0; // Reset stream position for further use
                                        }
                                        bm.Save(outputPath);
                                    }
                                    Console.WriteLine($"Saved Image: {outputPath}");
                                    imageIndex++;
                                }
                            }
                        }

                        // Loop through all pages and extract text
                        for (int i = 1; i <= totalPages; i++)
                        {
                            doc.PageNumber = i; // Set page number
                            extractedText.AppendLine(doc.GetText(Page.TextType.Text, true));
                            extractedText.AppendLine($"--- Page {i} ---");
                        }

                            /*
                            // Loop through all pages and extract text
                            for (int i = 1; i <= totalPages; i++)
                            {
                                doc.PageNumber = i; // Set page number
                                extractedText.AppendLine($"--- Page {i} ---");

                                HashSet<string> hrefs = new HashSet<string>();
                                string pattern = "<image xlink:href\\s*=\\s*(?:[\"'](?<1>[^\"']*)[\"']|(?<1>\\S+))";
                                string image = doc.GetInfo(doc.PageNumber, Page.TextType.Svg.ToString());
                                if (string.IsNullOrEmpty(image))
                                {
                                    image = doc.GetInfo(doc.PageNumber, Page.TextType.SvgPlus.ToString());
                                }
                                if (string.IsNullOrEmpty(image))
                                {
                                    image = doc.GetInfo(doc.PageNumber, Page.TextType.SvgPlus2.ToString());
                                }
                                if (!string.IsNullOrEmpty(image))
                                {
                                    MatchCollection matches = Regex.Matches(image, pattern);
                                    foreach (Match match in matches)
                                        hrefs.Add(match.Groups[1].Value);
                                    foreach (string href in hrefs)
                                    {
                                        string imageName = Path.Combine("images", href);
                                        if (!File.Exists(image))
                                        {
                                            // href is of form "imageXX.png" where XX is the PixMap ID
                                            int id = int.Parse(href.Substring(5, href.Length - 9));
                                            PixMap? pm = doc.ObjectSoup[id] as PixMap;
                                            using (Bitmap bm = pm.GetBitmap())
                                                bm.Save(imageName);
                                        }
                                    }
                                }
                                else
                                {
                                    extractedText.AppendLine(doc.GetText(Page.TextType.Text, true));
                                }

                                extractedText.AppendLine(); // Add a new line for readability
                            }
                            */
                            // Print extracted text
                            Console.WriteLine("Extracted Text from All Pages:");
                        Console.WriteLine(extractedText.ToString());

                        // Optionally, save the text to a file
                        File.WriteAllText("ExtractedText.txt", extractedText.ToString());
                        Console.WriteLine("Extracted text saved to ExtractedText.txt");
                    }
                }
            }
        }

    }
}