using System;
using System.Collections.Generic;
using System.Text;

namespace MDValidator.Domain.Configuration.LaneMaster
{
    public class LaneMasterRecord
    {
        /// <summary>
        /// Lane ID
        /// </summary>
        public string Lane { get; set; }

        /// <summary>
        /// Origin
        /// </summary>
        public string Origin { get; set; }

        /// <summary>
        /// Destination
        /// </summary>
        public string Destination { get; set; }

        /// <summary>
        /// Material type
        /// </summary>
        public string MTART { get; set; }

        /// <summary>
        /// Leg
        /// </summary>
        public int Leg { get; set; }

        /// <summary>
        /// Sending/Receiving [S/R]
        /// </summary>
        public string Site { get; set; }

        /// <summary>
        /// System
        /// </summary>
        public string SYSID { get; set; }

        /// <summary>
        /// Import/Export
        /// </summary>
        public string Scenario { get; set; }

        /// <summary>
        /// Country of Origin
        /// </summary>
        public string HERKL { get; set; }

        /// <summary>
        /// Company Code
        /// </summary>
        public string BUKRS { get; set; }

        /// <summary>
        /// Plant Code
        /// </summary>
        public string WERKS { get; set; }

        /// <summary>
        /// Sales Organization
        /// </summary>
        public string VKORG { get; set; }

        /// <summary>
        /// Distribution Channel
        /// </summary>
        public string VTWEG { get; set; }

        /// <summary>
        /// Division
        /// </summary>
        public string SPART { get; set; }

        /// <summary>
        /// Storage location
        /// </summary>
        public string SLOC { get; set; }

        /// <summary>
        /// Purchasing organization
        /// </summary>
        public string EKORG { get; set; }

        /// <summary>
        /// PO Group
        /// </summary>
        public string EKGRP { get; set; }

        /// <summary>
        /// Inforecord/Contract
        /// </summary>
        public string INFNR { get; set; }

        /// <summary>
        /// Price condition
        /// </summary>
        public string KSCHL { get; set; }

        /// <summary>
        /// Currency
        /// </summary>
        public string WAERS { get; set; }

        /// <summary>
        /// Master condition records for PO's
        /// </summary>
        public string MasterConditionRecordForPO { get; set; }

        /// <summary>
        /// Confirmation control key 
        /// </summary>
        public string BSTAE { get; set; }

        /// <summary>
        /// Document type (SO/PO, ISTO)
        /// </summary>
        public string DOCTYPE { get; set; }

        /// <summary>
        /// Ship-to
        /// </summary>
        public string KUNWE { get; set; }

        /// <summary>
        /// Bill-to
        /// </summary>
        public string KUNRE { get; set; }

        /// <summary>
        /// Incoterm
        /// </summary>
        public string INCO1 { get; set; }

        /// <summary>
        /// Sold-to 
        /// </summary>
        public string KUNAG { get; set; }

        /// <summary>
        /// Payer
        /// </summary>
        public string KUNRG { get; set; }

        /// <summary>
        /// Regi Affi Vendor
        /// </summary>
        public string LIFNR { get; set; }

        /// <summary>
        /// Invoice party
        /// </summary>
        public string LIFN2 { get; set; }

        /// <summary>
        /// Customer account assignment group
        /// </summary>
        public string KTGRD { get; set; }

        /// <summary>
        /// Transportation zone
        /// </summary>
        public string LZONE { get; set; }

        /// <summary>
        /// Movement type
        /// </summary>
        public string BWART { get; set; }

        /// <summary>
        /// Payment term
        /// </summary>
        public string ZTERM { get; set; }

        /// <summary>
        /// Date and Time of creation of this record
        /// </summary>
        public DateTime DateTime { get; set; }

        /// <summary>
        /// User who created this record
        /// </summary>
        public string User { get; set; }

    }
}
