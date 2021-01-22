using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CandidateCodeTest.Models
{
    public class Customer
    {

        ///<summary>
        /// Gets or sets Customer Id.
        ///</summary>
        public int CustomerId { get; set; }

        ///<summary>
        /// Gets or sets Customer Title
        ///</summary>
        public string Title { get; set; }

        ///<summary>
        /// Gets or sets Customer First Name.
        ///</summary>
        public string FirstName { get; set; }

        ///<summary>
        /// Gets or sets Customer Surname.
        ///</summary>
        public string Surname { get; set; }

        ///<summary>
        /// Gets or sets Customer Product Name.
        ///</summary>
        public string ProductName { get; set; }

        ///<summary>
        /// Gets or sets Customer Payout Amount.
        ///</summary>
        public double PayoutAmount { get; set; }

        ///<summary>
        /// Gets or sets Customer Annual Premium Amount 
        ///</summary>
        public double AnnualPremium { get; set; }
    }
}