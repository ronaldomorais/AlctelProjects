using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace Alctel.WorkstationDataReader.ConsoleApp
{
    class Program
    {
        private const string KEY_PATH_01 = @"SOFTWARE\WOW6432Node\Interactive Intelligence\EIC\Directory Services\Root\VIRCICSER001\Production\AdminConfig\StationInfo";
        private const string KEY_PATH_02 = @"SOFTWARE\WOW6432Node\Interactive Intelligence\EIC\Directory Services\Root\VIRCICSER001\Production\VIRCICSER001\Workstations";
        private const string KEY_PATH_03 = @"SOFTWARE\WOW6432Node\Interactive Intelligence\EIC\Directory Services\Root\VIRCICSER001\Production\VIRCICSER001\IP Phones";

        private const string KEY_PATH_USERS_01 = @"SOFTWARE\WOW6432Node\Interactive Intelligence\EIC\Directory Services\Root\VIRCICSER001\Production\AdminConfig\UserInfo";
        private const string KEY_PATH_USERS_02 = @"SOFTWARE\WOW6432Node\Interactive Intelligence\EIC\Directory Services\Root\VIRCICSER001\Production\Users";
        

        static void Main(string[] args)
        {
            bool bWorkgroup = false;

            if (bWorkgroup)
                Workgroups();
            else
                Agentes();
   

            Console.WriteLine("Terminado");
            Console.ReadKey();
        }

        static void Workgroups()
        {
#pragma warning disable CA1416 // Validate platform compatibility
            RegistryKey key = Registry.LocalMachine.OpenSubKey(KEY_PATH_01);
            List<StationInfo> stationInfos = new List<StationInfo>();

            if (key != null)
            {
                List<string> subkeysLst = key.GetSubKeyNames().ToList();

                if (subkeysLst.Count() > 0)
                {
                    foreach (string stationName in subkeysLst)
                    {
                        RegistryKey subKey = Registry.LocalMachine.OpenSubKey($"{KEY_PATH_01}\\{stationName}");
                        if (subKey != null)
                        {
                            string active = "No";
                            var activeObj = subKey.GetValue("Active");

                            if (activeObj != null)
                            {
                                var activePair = new KeyValuePair<string, object>(stationName, activeObj);
                                active = ((string[])activePair.Value)[0];
                            }

                            var countedLicenseObj = subKey.GetValue("Counted Licenses");
                            var countedLicensePair = new KeyValuePair<string, object>(stationName, countedLicenseObj);


                            string licenseBasicStation = "";
                            if (countedLicensePair.Value != null)
                            {
                                List<string> countedLicenseList = ((string[])countedLicensePair.Value).ToList();
                                licenseBasicStation = countedLicenseList.Where(l => l == "I3_LICENSE_BASIC_STATION").FirstOrDefault();
                                //licenseBasicStation = countedLicenseList.Where(l => l == "I3_ACCESS_FEEDBACK").FirstOrDefault(); 
                                if (licenseBasicStation == null)
                                    licenseBasicStation = "";
                            }

                            string macAddress = "";
                            try
                            {
                                //Para chave: SOFTWARE\WOW6432Node\Interactive Intelligence\EIC\Directory Services\Root\VIRCICSER001\Production\VIRCICSER001\Workstations
                                RegistryKey key1 = Registry.LocalMachine.OpenSubKey(KEY_PATH_02);

                                if (key1 != null)
                                {
                                    RegistryKey subKey1 = Registry.LocalMachine.OpenSubKey($"{KEY_PATH_02}\\{stationName}");
                                    if (subKey1 != null)
                                    {
                                        var macAddressObj = subKey1.GetValue("MAC Address");

                                        if (macAddressObj != null)
                                        {
                                            var macAddressPair = new KeyValuePair<string, object>(stationName, macAddressObj);
                                            macAddress = ((string[])macAddressPair.Value)[0];
                                        }
                                    }

                                    key1.Close();
                                }
                                //Para chave: SOFTWARE\WOW6432Node\Interactive Intelligence\EIC\Directory Services\Root\VIRCICSER001\Production\VIRCICSER001\Workstations

                            }
                            catch (Exception ex)
                            {
                                Console.WriteLine($"Exception: {ex.Message} - Trace: {ex.StackTrace}");
                            }

                            try
                            {
                                ////SOFTWARE\WOW6432Node\Interactive Intelligence\EIC\Directory Services\Root\VIRCICSER001\Production\VIRCICSER001\IP Phones
                                if (macAddress == "")
                                {
                                    RegistryKey key2 = Registry.LocalMachine.OpenSubKey(KEY_PATH_03);

                                    if (key2 != null)
                                    {
                                        List<string> subkeysLst2 = key2.GetSubKeyNames().ToList();

                                        if (subkeysLst2.Count() > 0)
                                        {
                                            foreach (string subkeyname2 in subkeysLst2)
                                            {
                                                RegistryKey subKey1 = Registry.LocalMachine.OpenSubKey($"{KEY_PATH_03}\\{subkeyname2}");

                                                if (subKey1 != null)
                                                {
                                                    string name = "";
                                                    var nameObj = subKey1.GetValue("Name");

                                                    if (nameObj != null)
                                                    {
                                                        var namePair = new KeyValuePair<string, object>(subkeyname2, nameObj);
                                                        name = ((string[])namePair.Value)[0];

                                                        if (name == stationName)
                                                        {
                                                            var macAddressObj = subKey1.GetValue("MAC Address");

                                                            if (macAddressObj != null)
                                                            {
                                                                var macAddressPair = new KeyValuePair<string, object>(stationName, macAddressObj);
                                                                macAddress = ((string[])macAddressPair.Value)[0];
                                                                break;
                                                            }
                                                        }
                                                    }
                                                }
                                            }
                                        }

                                        key2.Close();
                                    }
                                }
                                ////SOFTWARE\WOW6432Node\Interactive Intelligence\EIC\Directory Services\Root\VIRCICSER001\Production\VIRCICSER001\IP Phones
                            }
                            catch (Exception ex)
                            {
                                Console.WriteLine($"Exception: {ex.Message} - Trace: {ex.StackTrace}");
                            }

                            string workgroup = "";

                            if (macAddress != "") 
                            {
                                if (macAddress.ToUpper().Contains("CONCENTRIX")) workgroup = "CONCENTRIX";
                                else if (macAddress.ToUpper().Contains("GRUPOAEC")) workgroup = "AEC";
                                else if (macAddress.ToUpper().Contains("ML")) workgroup = "OUVIDORIA";
                                else workgroup = "ATENTO";
                            }

                            StationInfo stationInfo = new StationInfo()
                            {
                                StationName = stationName,
                                Active = active,
                                LicenseBasicStation = licenseBasicStation == "" ? false : true,
                                MacAddress = macAddress,
                                Workgroup = workgroup,
                            };

                            //if (stationInfo.LicenseBasicStation)
                            //    stationInfos.Add(stationInfo);
                            stationInfos.Add(stationInfo);
                        }
                    }
                }

                key.Close();
            }

            if (stationInfos.Count() > 0)
            {
                StreamWriter writer = new StreamWriter("StationInfo.csv");

                writer.WriteLine("Nome da Maquina;Ativada;Licenciada;Mac Address;Workgroup");

                foreach (StationInfo station in stationInfos)
                {
                    writer.WriteLine($"{station.StationName};{station.Active};{station.LicenseBasicStation};{station.MacAddress};{station.Workgroup}");
                }
                writer.Flush();
                writer.Close();
            }

#pragma warning restore CA1416 // Validate platform compatibility

        }
        static void Agentes()
        {
#pragma warning disable CA1416 // Validate platform compatibility
            RegistryKey key = Registry.LocalMachine.OpenSubKey(KEY_PATH_USERS_01);
            List<UserInfo> userInfos = new List<UserInfo>();

            if (key != null)
            {
                List<string> subkeysLst = key.GetSubKeyNames().ToList();

                if (subkeysLst.Count() > 0)
                {
                    foreach (string userName in subkeysLst)
                    {
                        RegistryKey subKey = Registry.LocalMachine.OpenSubKey($"{KEY_PATH_USERS_01}\\{userName}");
                        if (subKey != null)
                        {
                            string active = "No";

                            var activeObj = subKey.GetValue("Active");

                            if (activeObj != null)
                            {
                                var activePair = new KeyValuePair<string, object>(userName, activeObj);
                                active = ((string[])activePair.Value)[0];
                            }

                            var countedLicenseObj = subKey.GetValue("Counted Licenses");

                            List<string> licencasAssociadas = new List<string>();
                            if (countedLicenseObj != null)
                            {
                                var countedLicensePair = new KeyValuePair<string, object>(userName, countedLicenseObj);

                                for (int i = 0; i < ((string[])countedLicensePair.Value).Count(); i++)
                                {
                                    licencasAssociadas.Add(((string[])countedLicensePair.Value)[i]);
                                }

                                UserInfo userInfo = new UserInfo();
                                userInfo.Username = userName;
                                userInfo.Active = active;
                                userInfo.CountedLicenses = licencasAssociadas;

                                userInfos.Add(userInfo);
                            }
                        }
                    }
                }

                key.Close();
            }

            key = Registry.LocalMachine.OpenSubKey(KEY_PATH_USERS_02);

            if (key != null)
            {
                List<string> subkeysLst = key.GetSubKeyNames().ToList();

                if (subkeysLst.Count() > 0)
                {
                    foreach (string userName in subkeysLst)
                    {
                        RegistryKey subKey = Registry.LocalMachine.OpenSubKey($"{KEY_PATH_USERS_02}\\{userName}");
                        if (subKey != null)
                        {
                            string displayName = "";

                            var displayNameObj = subKey.GetValue("displayName");

                            if (displayNameObj != null)
                            {
                                var displayNamePair = new KeyValuePair<string, object>(userName, displayNameObj);
                                displayName = ((string[])displayNamePair.Value)[0];
                            }

                            string roles = "";

                            var rolesObj = subKey.GetValue("Role");

                            if (rolesObj != null)
                            {
                                var rolesPair = new KeyValuePair<string, object>(userName, rolesObj);
                                roles = String.Join(" - ", ((string[])rolesPair.Value));
                            }

                            string workgroups = "";

                            var workgroupsObj = subKey.GetValue("Workgroups");

                            if (workgroupsObj != null)
                            {
                                var workgroupPair = new KeyValuePair<string, object>(userName, workgroupsObj);
                                workgroups = String.Join(" - ", ((string[])workgroupPair.Value));
                            }

                            var userFind = userInfos.Find(u => u.Username == userName);

                            if (userFind != null)
                            {
                                userFind.DisplayName = displayName;
                                userFind.Role = roles;
                                userFind.Workgroup = workgroups;
                            }
                        }
                    }
                }

                key.Close();
            }

            if (userInfos.Count() > 0)
            {
                StreamWriter writer = new StreamWriter("UsersInfo.csv");

                writer.WriteLine("Nome Usuário;Ativada;Workgroups;Roles;Licenças");

                foreach (UserInfo user in userInfos)
                {
                    writer.WriteLine($"{user.Username};{user.Active};{user.Workgroup};{user.Role};{String.Join(" - ", user.CountedLicenses)}");
                }
                writer.Flush();
                writer.Close();
            }

#pragma warning restore CA1416 // Validate platform compatibility

        }
    }
}
