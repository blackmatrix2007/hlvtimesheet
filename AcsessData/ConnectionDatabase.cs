using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web;

namespace HLVTimeSheet.AcsessData
{
    public class ConnectionDatabase
    {
        public static string ConStringHLVTimeSheet;

        //private string server = @"DESKTOP-B4762M4\SQL2022DEV";
        //private string databasename = "GDC_HLV_Jisseki";
        //private string username = "giangdc";
        //private string password = "giangdc@";

        private string server = @"103.159.50.204, 89";
        private string databasename = "GDC_HLV_WorkSchedule";
        private string username = "giangdc";
        private string password = "giangdc@";

        private string databasenamePC = "GDC_HLV_ParkingCar";
        private string databasenameOC = "GDC_HLV_OperationControl";        
        private string databasenameWS = "GDC_HLV_WorkSchedule";
        private string databasenameWHVP = "GDC_HLV_Warehouse";
        private string databasenameNWHP = "GDC_HLV_WarehouseNormalHP";
        private string databasenameBWHP = "GDC_HLV_WarehouseHaiPhong";

        public string ReturnConnectionDatabase()
        {
            return @"Server= " + server + "; Database=" + databasename + "; User ID=" + username + "; pwd=" + password + " ";
        }
        public string ReturnConnectionDatabasePC()
        {
            return @"Server= " + server + "; Database=" + databasenamePC + "; User ID=" + username + "; pwd=" + password + " ";
        }
        public string ReturnConnectionDatabaseOC()
        {
            return @"Server= " + server + "; Database=" + databasenameOC + "; User ID=" + username + "; pwd=" + password + " ";
        }
        public string ReturnConnectionDatabaseWS()
        {
            return @"Server= " + server + "; Database=" + databasenameWS + "; User ID=" + username + "; pwd=" + password + " ";
        }
        public string ReturnConnectionDatabaseWHVP()
        {
            return @"Server= " + server + "; Database=" + databasenameWHVP + "; User ID=" + username + "; pwd=" + password + " ";
        }

        public string ReturnConnectionDatabaseNWHP()
        {
            return @"Server= " + server + "; Database=" + databasenameNWHP + "; User ID=" + username + "; pwd=" + password + " ";
        }

        public string ReturnConnectionDatabaseBWHP()
        {
            return @"Server= " + server + "; Database=" + databasenameBWHP + "; User ID=" + username + "; pwd=" + password + " ";
        }

        public string ReturnConnectionDatabase(string server, string databasename, string username, string password)
        {
            if (server.Length < 3)
                server = this.server;
            if (databasename.Length < 3)
                databasename = this.databasename;

            return @"Server= " + server + "; Database=" + databasename + "; User ID=" + this.username + "; pwd=" + this.password + " ";
        }
        public string ReturnConnectionDatabasePC(string server, string databasename, string username, string password)
        {
            if (server.Length < 3)
                server = this.server;
            if (databasename.Length < 3)
                databasename = this.databasename;

            return @"Server= " + server + "; Database=" + databasenamePC + "; User ID=" + this.username + "; pwd=" + this.password + " ";
        }
        public string ReturnConnectionDatabaseOC(string server, string databasename, string username, string password)
        {
            if (server.Length < 3)
                server = this.server;
            if (databasename.Length < 3)
                databasename = this.databasename;

            return @"Server= " + server + "; Database=" + databasenameOC + "; User ID=" + this.username + "; pwd=" + this.password + " ";
        }
        public string ReturnConnectionDatabaseWS(string server, string databasename, string username, string password)
        {
            if (server.Length < 3)
                server = this.server;
            if (databasename.Length < 3)
                databasename = this.databasename;

            return @"Server= " + server + "; Database=" + databasenameWS + "; User ID=" + this.username + "; pwd=" + this.password + " ";
        }
        public string ReturnConnectionDatabaseWHVP(string server, string databasename, string username, string password)
        {
            if (server.Length < 3)
                server = this.server;
            if (databasename.Length < 3)
                databasename = this.databasename;

            return @"Server= " + server + "; Database=" + databasenameWHVP + "; User ID=" + this.username + "; pwd=" + this.password + " ";
        }

        public string ReturnConnectionDatabaseNWHP(string server, string databasename, string username, string password)
        {
            if (server.Length < 3)
                server = this.server;
            if (databasename.Length < 3)
                databasename = this.databasename;

            return @"Server= " + server + "; Database=" + databasenameNWHP + "; User ID=" + this.username + "; pwd=" + this.password + " ";
        }

        public string ReturnConnectionDatabaseBWHP(string server, string databasename, string username, string password)
        {
            if (server.Length < 3)
                server = this.server;
            if (databasename.Length < 3)
                databasename = this.databasename;

            return @"Server= " + server + "; Database=" + databasenameBWHP + "; User ID=" + this.username + "; pwd=" + this.password + " ";
        }
    }
}