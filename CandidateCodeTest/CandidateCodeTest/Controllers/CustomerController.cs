
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Mvc;
using CandidateCodeTest.Models;

namespace CandidateCodeTest.Controllers
{
    public class CustomerController : Controller
    {
        // GET: Customer
        public ActionResult Index()
        {
            return View();
        }

        // <summary>
        /// Post method for importing users 
        /// </summary>
        /// <param name="postedFile"></param>
        /// <returns></returns>     
        [HttpPost]
        public ActionResult Index(HttpPostedFileBase postedFile)
        {
            if (postedFile != null)
            {
                string fileExtension = Path.GetExtension(postedFile.FileName);

                //Validate uploaded file and return error.
                if (fileExtension != ".csv")
                {
                    ViewBag.Message = "Please select the csv file with .csv extension";
                    return View();
                }

                try
                {
                    var customers = new List<Customer>();
                    using (var sreader = new StreamReader(postedFile.InputStream))
                    {
                        //First line is header. If header is not passed in csv then we can neglect the below line.
                        string[] headers = sreader.ReadLine().Split(',');
                        //Loop through the records
                        while (!sreader.EndOfStream)
                        {
                            string[] rows = sreader.ReadLine().Split(',');

                            //amount validation
                            if (!isNumeric(rows[5].ToString()))
                            {
                                throw new InvalidAmountException("Amount should be numeric");
                            }

                            customers.Add(new Customer
                            {
                                CustomerId = int.Parse(rows[0].ToString()),
                                Title = rows[1].ToString(),
                                FirstName = rows[2].ToString(),
                                Surname = rows[3].ToString(),
                                ProductName = rows[4].ToString(),
                                PayoutAmount = double.Parse(rows[5].ToString()),
                                AnnualPremium = double.Parse(rows[6].ToString())
                            });
                        }
                    }

                    string txtFilePath = System.Web.Configuration.WebConfigurationManager.AppSettings["FilePath"].ToString();
                    Directory.CreateDirectory(txtFilePath);

                    foreach (var cust in customers)
                    {
                        string txtFileName = string.Concat(cust.CustomerId, cust.FirstName) + ".txt";
                        string txtFullPath = Path.Combine(txtFilePath, txtFileName);

                        if (System.IO.File.Exists(txtFullPath))
                        {
                            System.IO.File.Delete(txtFullPath);

                        }

                        using (StreamWriter writer = System.IO.File.CreateText(txtFullPath))
                        {
                            writer.WriteLine(DateTime.Now.ToString("dd/MM/yyyy") + Environment.NewLine, CultureInfo.InvariantCulture);
                            writer.WriteLine("FAO: " + cust.Title + " " + cust.FirstName + " " + cust.Surname + Environment.NewLine);
                            writer.WriteLine("RE: Your Renewal" + Environment.NewLine);
                            writer.WriteLine("Dear " + cust.Title + " " + cust.Surname + Environment.NewLine);
                            writer.WriteLine("We hereby invite you to renew your Insurance Policy, subject to the following terms" + Environment.NewLine);
                            writer.WriteLine("Your chosen insurance product is " + cust.ProductName + Environment.NewLine);
                            writer.WriteLine("Your annual premium will be " + "£" + cust.PayoutAmount + Environment.NewLine);
                            writer.WriteLine("The amount payable to you in the event of a valid claim will be " + "£" + cust.AnnualPremium + Environment.NewLine);

                            //Amount calculations
                            double creditCharge = 0.05 * cust.AnnualPremium;
                            double totalPremium = cust.AnnualPremium + creditCharge;
                            double avgMonthlyPremium = totalPremium / 12;
                            double shortageAmount = Math.Abs((avgMonthlyPremium - Math.Round(avgMonthlyPremium, 2)));
                            double initialMonthlyPayment = (shortageAmount * 12) + avgMonthlyPremium;
                            double otherMonthlyPayment = Math.Round(avgMonthlyPremium, 2);

                            writer.WriteLine("If you choose to pay by Direct Debit, we will add a credit charge of £" + creditCharge +
                                   ", bringing the total to £" + totalPremium + ".This is payable by an initial payment of £" + initialMonthlyPayment +
                                   ", followed by 11 payments of £" + otherMonthlyPayment + Environment.NewLine);
                            writer.WriteLine("Please get in touch with us to arrange your renewal by visiting https://www.regallutoncodingtest.co.uk/renew or calling us on 01625 123456." + Environment.NewLine);
                            writer.WriteLine("Kind Regards");
                            writer.WriteLine("Regal Luton");
                        }
                    }

                    ViewBag.Message = "Customer Letter created successfuly @path: " + txtFilePath;
                }
                catch (InvalidAmountException ex)
                {
                    ViewBag.Message = ex.Message;
                }
            }
            else
            {
                ViewBag.Message = "Missing Input File.Please select file to upload !!!";

            }

            return View();
        }

        private static bool isNumeric(string s)
        {
            return double.TryParse(s, out double n);
        }

        private class InvalidAmountException : Exception
        {
            public InvalidAmountException(String message) : base(message)
            {

            }
        }

    }
}
