using SCADA_CORE.Database;
using SCADA_CORE.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Security.Cryptography;
using System.ServiceModel;
using System.Text;

namespace SCADA_CORE.DatabaseManagerService
{
    // NOTE: You can use the "Rename" command on the "Refactor" menu to change the class name "DatabaseManagerService" in code, svc and config file together.
    // NOTE: In order to launch WCF Test Client for testing this service, please select DatabaseManagerService.svc or DatabaseManagerService.svc.cs at the Solution Explorer and start debugging.
    public class DatabaseManagerService : IDatabaseManagerService
    {

        public static UserContext db = new UserContext();
        public bool Login(string username, string password)
        {
            List<User> allUsers = db.Users.ToList();

            foreach (User currentUser in allUsers)
            {
                if (currentUser.username.ToLower().Equals(username.ToLower()))
                {
                    string hashedPassword = Hash(password);

                    if (hashedPassword.Equals(currentUser.password))
                    {
                        return true;
                    }
                }
            }

            return false;
        }

        public bool Register(string username, string password)
        {


            List<User> allUsers = new List<User>();

            allUsers = db.Users.ToList();
            if (allUsers.Count != 0)
            {
                foreach (User currentUser in allUsers)
                {
                    if (currentUser.username.ToLower().Equals(username.ToLower()))
                    {
                        return false;
                    }
                }

            }
            string hashedPassword = Hash(password);
            User user = new User(username, hashedPassword);
            db.Users.Add(user);
            db.SaveChanges();
            return true;
        }

        public string Hash(string raw)
        {
            string hash = string.Empty;

            using (SHA256Managed crypt = new SHA256Managed())
            {
                byte[] crypto = crypt.ComputeHash(Encoding.ASCII.GetBytes(raw));

                foreach (byte theByte in crypto)
                    hash += theByte.ToString("x2");
            }

            return hash;

        }

        public string AddAnalogOutputTag(string tagId, string description, string ioAddress, int scanTime, int lowLimit, int highLimit)
        {
            return TagProcessing.AddAnalogOutputTag(tagId, description, ioAddress, scanTime, lowLimit, highLimit);

        }

        public string AddAnalogInputTag(string tagId, string description, string driver, string ioAddress, int ScanTime, bool onScan, int lowLimit, int highLimit, string units)
        {
            return TagProcessing.AddAnalogInputTag(tagId, description, driver, ioAddress, ScanTime, onScan,lowLimit, highLimit, units);

        }

        public string AddDigitalOutputTag(string tagId, string description, string ioAddress, int initValue)
        {
            return TagProcessing.AddDigitalOutputTag(tagId, description, ioAddress, initValue);

        }

        public string AddDigitalInputTag(string tagId, string description, string driver, string ioAddress, int ScanTime, bool onScan)
        {
            return TagProcessing.AddDigitalInputTag(tagId, description, driver, ioAddress, ScanTime, onScan);

        }

        public string RemoveTag(string tagId)
        {
            return TagProcessing.RemoveTag(tagId);
        }

        public string SwitchScanMode(string tagId)
        {
            return TagProcessing.SwitchScanMode(tagId);
        }

     

        public string ChangeOutputValue(string tagId, double newValue)
        {
            return TagProcessing.ChangeOutputValue(tagId, newValue);
        }

        public void LoadXml()
        {
            TagProcessing.Init();
        }

        public Dictionary<string, double> GetOutputValues()
        {
            return TagProcessing.TagOutputValues;
        }

        public string AddAlarmForAnalog(string tagId, string type, int priority)
        {
            return TagProcessing.AddAlarm(tagId, type, priority);
        }

        public string RemoveAlarm(string tagId)
        {
            return TagProcessing.RemoveAlarm(tagId);
        }
    }

}
