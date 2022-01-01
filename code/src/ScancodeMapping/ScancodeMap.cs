using System;
using Microsoft.Win32;

namespace ScancodeMapping
{
    public class ScancodeMap
    {
        private const int minimum = 4 * sizeof(int); // Binary data consists at least of version, flags, elements and termination!

        private static RegistryKey regRoot = Registry.LocalMachine;
        private static string regSubkey = "SYSTEM\\CurrentControlSet\\Control\\Keyboard Layout";
        private static string regValue = "Scancode Map";

        private byte[] binary = null;
        private int[] mappings = null;
        private int version = 0;
        private int flags = 0;
        private int termination = 0;
        private bool dirty = true; // Binary nor yet created or content has changed!

        //
        // Constrution
        //

        //
        // Summary:
        //      Default construtor. Everything is initialized with zeros 
        //      and the dirty flag is set too.
        //
        public ScancodeMap()
        {
            this.version = 0;
            this.flags = 0;
            this.mappings = null;
            this.termination = 0;
            this.binary = null;
            this.dirty = true;
        }

        //
        // Summary: 
        //      A constructor to provide version and flag settings.
        //
        // Parameters:
        //      version: 
        //          Should be zero.
        //      flags:
        //          Should be zero.
        //
        public ScancodeMap(int version, int flags)
            : this()
        {
            this.version = version;
            this.flags = flags;
        }

        //
        // Summary: 
        //      A constructor to provide version, flag and termination settings.
        //      
        // Parameters:
        //      version: 
        //          Should be zero.
        //      flags: 
        //          Should be zero.
        //      termination: 
        //          Should be zero.
        //
        public ScancodeMap(int version, int flags, int termination)
            : this(version, flags)
        {
            this.termination = termination;
        }

        //
        // Summary:
        //      A constructor to provide binary data from registry.
        //      
        // Parameters:
        //      binary: 
        //          Initial binary data.
        //
        public ScancodeMap(byte[] binary)
        {
            this.Load(binary);
        }

        //
        // Properties
        //

        //
        // Summary:
        //      Sets and gets currently used version. Be aware of little endian usage!
        //
        public int Version
        {
            get { return this.version; }
            set { this.version = value; this.dirty = true; }
        }

        //
        // Summary:
        //      Sets and gets currently used flags. Be aware of little endian usage!
        //
        public int Flags
        {
            get { return this.flags; }
            set { this.flags = value; this.dirty = true; }
        }

        //
        // Summary:
        //      Sets and gets currently used terminiation. Be aware of little endian usage!
        //
        public int Termination
        {
            get { return this.termination; }
            set { this.termination = value; this.dirty = true; }
        }

        //
        // Summary:
        //      Sets and gets currently used mappings as an array. Be aware of little endian usage!
        //
        public int[] Mappings
        {
            get { return this.mappings; }
            set { this.mappings = (int[])value.Clone(); this.dirty = true; }
        }

        //
        // Summary:
        //      Sets and gets currently used binary data representation of class content.
        //
        public byte[] Binary
        {
            get { return this.Make(); }
            set { this.Load(value); }
        }

        //
        // Registry access
        //

        //
        // Summary:
        //      Reads the value of "Scancode Map" from the registry and returns an 
        //      initialized instance of class "ScancodeMap".
        //
        // Returns:
        //      Instance of class "ScancodeMap" or NULL if not found.
        //
        public static ScancodeMap RegistryLoad()
        {
            ScancodeMap result = null;
            RegistryKey subkey = null;

            // This key must always exist because its a system depending registry key!
            subkey = regRoot.OpenSubKey(regSubkey, false); // Open for read acccess only!

            if (subkey != null)
            {
                object value = subkey.GetValue(regValue, null);

                if (value != null && value.GetType() == typeof(byte[]))
                {
                    result = new ScancodeMap((byte[])value);
                }

                subkey.Close();
            }

            return result;
        }

        //
        // Summary:
        //      Writes the value of "Scancode Map" into the registry.
        //
        // Parameters:
        //      value:
        //          An initialized instance of class "ScancodeMap".
        //
        // Returns:
        //      TRUE if successful and FALSE otherwise.
        //
        public static bool RegistrySave(ScancodeMap value)
        {
            bool success = false;
            RegistryKey subkey = null;

            // This key must always exist because its a system depending registry key!
            subkey = regRoot.OpenSubKey(regSubkey, true); // Open for write acccess!

            if (subkey != null)
            {
                if (value != null)
                {
                    try
                    {
                        subkey.SetValue(regValue, value.Binary, RegistryValueKind.Binary);
                        success = true;
                    }
                    catch (Exception)
                    {
                        success = false;
                    }
                }
                subkey.Close();
            }
            return success;
        }

        //
        // Summary:
        //      Removes the value of "Scancode Map" from the registry.
        //
        // Returns:
        //      TRUE if successful and FALSE otherwise.
        //
        public static void RegistryRemove()
        {
            RegistryKey subkey = null;

            // This key must always exist because its a system depending registry key!
            subkey = regRoot.OpenSubKey(regSubkey, true); // Open for write acccess!

            if (subkey != null)
            {
                try
                {
                    subkey.DeleteValue(regValue, false); // Don't throw an exception on missing value!
                }
                catch (Exception)
                {
                }
                subkey.Close();
            }
        }

        //
        // Member functions
        //

        //
        // Summary:
        //      Appends a new mapping entry at once.
        //
        // Parameters:
        //      mapping:
        //          Mapping to be added. Be aware of right data format!
        //
        public void Append(int mapping)
        {
            int count = 1;
            int[] temp = null;

            // Copy if exists.
            if (this.mappings != null)
            {
                count += this.mappings.Length;
                temp = new int[count];
                mappings.CopyTo(temp, 0);
            }
            // Just create with only one entry.
            else
            {
                temp = new int[count];
            }

            // Add new value and re-assign currently used mappings.
            temp.SetValue(mapping, count - 1);
            mappings = temp;
            this.dirty = true;
        }

        //
        // Summary:
        //      Appends a new mapping entry which consists of two parts. 
        //      A keycode consists of the scancode and the extended code 
        //      in the order of [scancode][extended]!
        //
        // Parameters:
        //      oriKeycode:
        //          Scancode and extended code of original key code.
        //          Be aware of right data format!
        //      mapKeycode:
        //          Scancode and extended code of key code to be used. 
        //          Be aware of right data format!
        //
        public void Append(int oriKeycode, int mapKeycode)
        {
            // Still use big endian!
            this.Append(
                this.Combine((ushort)oriKeycode, (ushort)mapKeycode)
            );
        }

        //
        // Summary:
        //      Appends a new mapping entry which consists of four parts.
        //
        // Parameters:
        //      oriScancode:
        //          Scancode of original key code.
        //      oriExtended:
        //          Extended code of original key code.
        //      mapScancode:
        //          Scancode of key code to be used.
        //      mapExtended:
        //          Extended code of key code to be used.
        //
        public void Append(int oriScancode, int oriExtended, int mapScancode, int mapExtended)
        {
            // Still use big endian!
            this.Append(
                this.Combine((byte)oriScancode, (byte)oriExtended),
                this.Combine((byte)mapScancode, (byte)mapExtended)
            );
        }

        //
        // Summary:
        //      Gets the list index of given mapping value. Keep in mind the 
        //      original keycode stays untouched every time!
        //
        // Parameters:
        //      mapping:
        //          Mapping to be added. Be aware of right data format!
        //
        // Returns:
        //      The list index if found and -1 otherwise.
        //
        public int IndexOf(int mapping)
        {
            int result = -1;
            if (this.mappings != null)
            {
                ushort oriKeycode = 0;
                ushort mapKeycode = 0;
                this.Split(mapping, out oriKeycode, out mapKeycode);

                for (int index = 0; index < this.mappings.Length; index++)
                {
                    ushort hiWord = 0;
                    ushort loWord = 0;
                    this.Split(this.mappings[index], out hiWord, out loWord);

                    if (oriKeycode == hiWord)
                    {
                        result = index;
                        break;
                    }
                }
            }
            return result;
        }

        //
        // Summary:
        //      Gets the list index of given mapping value. A keycode 
        //      consists of the scancode and the extended code in the 
        //      order of [scancode][extended]! Keep in mind original 
        //      keycode stays untouched all the time! 
        //
        // Parameters:
        //      xxx:
        //          xxx
        //      oriKeycode:
        //          Scancode and extended code of original key code.
        //          Be aware of right data format!
        //      mapKeycode:
        //          Scancode and extended code of key code to be used. 
        //          Be aware of right data format!
        //
        // Returns:
        //      The list index if found and -1 otherwise.
        //
        public int IndexOf(int oriKeycode, int mapKeycode)
        {
            return this.IndexOf(
                this.Combine((ushort)oriKeycode, (ushort)mapKeycode)
            );
        }

        //
        // Summary:
        //      Gets the list index of given mapping value. Keep in mind 
        //      original keycode stays untouched all the time!
        //
        // Parameters:
        //      oriScancode:
        //          Scancode of original key code.
        //      oriExtended:
        //          Extended code of original key code.
        //      mapScancode:
        //          Scancode of key code to be used.
        //      mapExtended:
        //          Extended code of key code to be used.
        //
        // Returns:
        //      The list index if found and -1 otherwise.
        //
        public int IndexOf(int oriScancode, int oriExtended, int mapScancode, int mapExtended)
        {
            return this.IndexOf(
                this.Combine((byte)oriScancode, (byte)oriExtended),
                this.Combine((byte)mapScancode, (byte)mapExtended)
            );
        }

        //
        // Summary:
        //      Returns the mapping code at given index.
        //
        // Parameters:
        //      index:
        //          The list index to read from.
        //      mapping:
        //          The requested mapping value. This value is set 
        //          to zero if given index contains improper data.
        //
        // Returns:
        //      TRUE if found and FALSE if not.
        //
        public bool GetAt(int index, out int mapping)
        {
            bool success = false;
            mapping = 0;

            if (this.mappings != null)
            {
                if (0 <= index && index < this.mappings.Length)
                {
                    mapping = this.mappings[index];
                    success = true;
                }

            }
            return success;
        }

        //
        // Summary:
        //      Returns the original keycode and mapping keycode at given 
        //      index. A keycode consists of the scancode and the extended 
        //      code in the order of [scancode][extended]!
        //
        // Parameters:
        //      index:
        //          The list index to read from.
        //      oriKeycode:
        //          The requested original keycode value. This value is set 
        //          to zero if given index contains improper data.
        //      mapKeycode:
        //          The requested mapping keycode value. This value is set 
        //          to zero if given index contains improper data.
        //
        // Returns:
        //      TRUE if found and FALSE if not.
        //
        public bool GetAt(int index, out int oriKeycode, out int mapKeycode)
        {
            bool success = false;
            int mapping = 0;

            oriKeycode = 0;
            mapKeycode = 0;

            if (this.GetAt(index, out mapping))
            {
                ushort hiWord = 0;
                ushort loWord = 0;
                this.Split(mapping, out hiWord, out loWord);

                oriKeycode = hiWord;
                mapKeycode = loWord;

                success = true;
            }
            return success;
        }

        //
        // Summary:
        //      Returns the scancode and extended code of original keycode 
        //      and scancode and extended code of mapping keycode at given 
        //      index.
        //
        // Parameters:
        //      index:
        //          The list index to read from.
        //      oriScancode:
        //          The requested scancode value of original keycode. This value 
        //          is set to zero if given index contains improper data.
        //      oriExtended:
        //          The requested extended code value of original keycode. This 
        //          value is set to zero if given index contains improper data.
        //      mapScancode:
        //          The requested scancode value of mapping keycode. This value 
        //          is set to zero if given index contains improper data.
        //      mapExtended:
        //          The requested extended code value of mapping keycode. This 
        //          value is set to zero if given index contains improper data.
        //
        // Returns:
        //      TRUE if found and FALSE if not.
        //
        public bool GetAt(int index, out int oriScancode, out int oriExtended, out int mapScancode, out int mapExtended)
        {
            bool success = false;
            int hiWord = 0;
            int loWord = 0;

            oriScancode = 0;
            oriExtended = 0;
            mapScancode = 0;
            mapExtended = 0;

            if (this.GetAt(index, out hiWord, out loWord))
            {
                byte hiByte = 0;
                byte loByte = 0;

                this.Split(hiWord, out hiByte, out loByte);

                oriScancode = hiByte;
                oriExtended = loByte;

                this.Split(loWord, out hiByte, out loByte);

                mapScancode = hiByte;
                mapExtended = loByte;

                success = true;
            }
            return success;
        }

        //
        // Summary:
        //      Sets the mapping code at given index.
        //
        // Parameters:
        //      index:
        //          The list index to write to.
        //      mapping:
        //          The new mapping value.
        //
        // Returns:
        //      TRUE if found and FALSE if not.
        //
        public bool SetAt(int index, int mapping)
        {
            bool success = false;

            if (this.mappings != null)
            {
                if (0 <= index && index < this.mappings.Length)
                {
                    this.mappings[index] = mapping;
                    this.dirty = true;
                    success = true;
                }
            }
            return success;
        }

        //
        // Summary:
        //      Sets the original keycode and mapping keycode at given index. 
        //      A keycode consists of the scancode and the extended code in 
        //      the order of [scancode][extended]!
        //
        // Parameters:
        //      index:
        //          The list index to write to.
        //      oriKeycode:
        //          The new original keycode value.
        //      mapKeycode:
        //          The new mapping keycode value.
        //
        // Returns:
        //      TRUE if found and FALSE if not.
        //
        public bool SetAt(int index, int oriKeycode, int mapKeycode)
        {
            return this.SetAt(
                index,
                this.Combine((ushort)oriKeycode, (ushort)mapKeycode)
            );
        }

        //
        // Summary:
        //      Sets the scancode and extended code of original keycode and 
        //      scancode and extended code of mapping keycode at given index.
        //
        // Parameters:
        //      index:
        //          The list index to write to.
        //      oriScancode:
        //          The original scancode value of original keycode.
        //      oriExtended:
        //          The original extended code value of original keycode.
        //      mapScancode:
        //          The new scancode value of mapping keycode.
        //      mapExtended:
        //          The new extended code value of mapping keycode.
        //
        // Returns:
        //      TRUE if found and FALSE if not.
        //
        public bool SetAt(int index, int oriScancode, int oriExtended, int mapScancode, int mapExtended)
        {
            return this.SetAt(
                index,
                this.Combine((byte)oriScancode, (byte)oriExtended),
                this.Combine((byte)mapScancode, (byte)mapExtended)
            );
        }

        //
        // Summary:
        //      Removes given mapping value from mappings list. For this operation 
        //      the high word of given mapping is the only relevant part to ensure 
        //      removement!
        //
        // Parameters:
        //      mapping:
        //          Mapping to be removed.
        //
        public void Remove(int mapping)
        {
            int remove = this.IndexOf(mapping);
            if (remove != -1)
            {
                int[] temp = new int[this.mappings.Length - 1];
                int position = 0;

                for (int index = 0; index < this.mappings.Length; index++)
                {
                    // Skip entry to be removed only!
                    if (index != remove)
                    {
                        temp[position++] = this.mappings[index];
                    }
                }
                this.mappings = temp;
                this.dirty = true;
            }
        }

        //
        // Summary:
        //      Removes given original keycode and mapping keycode from mappings 
        //      list. A keycode consists of the scancode and the extended code 
        //      in the order of [scancode][extended]!
        //
        // Parameters:
        //      oriKeycode:
        //          The new original keycode value.
        //      mapKeycode:
        //          The new mapping keycode value (can be zero).
        //
        public void Remove(int oriKeycode, int mapKeycode)
        {
            this.Remove(
                this.Combine((ushort)oriKeycode, (ushort)mapKeycode)
            );
        }

        //
        // Summary:
        //      Removes given scancode and extended code of original keycode 
        //      and scancode and extended code from mappings list.
        //
        // Parameters:
        //      oriScancode:
        //          The original scancode value of original keycode.
        //      oriExtended:
        //          The original extended code value of original keycode.
        //      mapScancode:
        //          The new scancode value of mapping keycode. Can be zero
        //      mapExtended:
        //          The new extended code value of mapping keycode. Can be zero
        //
        public void Remove(int oriScancode, int oriExtended, int mapScancode, int mapExtended)
        {
            this.Remove(
                this.Combine((byte)oriScancode, (byte)oriExtended),
                this.Combine((byte)mapScancode, (byte)mapExtended)
            );
        }

        //
        // Summary:
        //      Checks the existance of mapping entries.
        //
        // Returns:
        //      TRUE if the mappings list contains at least one entry 
        //      and FALSE otherwise.
        //
        public bool HasEntries()
        {
            if (this.mappings != null && this.mappings.Length > 0)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        //
        // Summary:
        //      Returns a string representing data to be written to the registry 
        //      depending on given parameter. This string uses a format like: 
        //      ["Scancode Map"=hex:00,00,00,00,00,00,00,00,02,00,00,00,2a,00,3a,00,00,00,00,00] 
        //      or if parameter 'includeName' is FALSE a format like:
        //      [00,00,00,00,00,00,00,00,02,00,00,00,2a,00,3a,00,00,00,00,00].
        //
        // Parameters:
        //      includeName:
        //          If TRUE the result contains the registry value name. 
        //          If FALSE only hexadecimal data will be used.
        //
        // Returns:
        //      A string representing the data of currently used mappings.
        //
        public string GetRawData(bool includeName)
        {
            string result = "";

            if (includeName)
            {
                result += "\"" + ScancodeMap.regValue + "\"=hex:";
            }

            byte[] data = this.Binary;

            for (int index = 0; index < data.Length; index++)
            {
                result += data[index].ToString("X2");

                if (index < data.Length - 1)
                {
                    result += ",";
                }
            }
            return result;
        }

        //
        // Summary:
        //      Returns a string representing data to be written 
        //      to the registry. This string uses a format like:
        //      ["Scancode Map"=hex:00,00,00,00,00,00,00,00,02,00,00,00,2a,00,3a,00,00,00,00,00].
        //
        // Returns:
        //      A string representing the data of currently used mappings.
        //
        public string GetRawData()
        {
            return GetRawData(true);
        }

        //
        // Summary:
        //
        public string[] GetRawDataList()
        {
            const string format = "{0} {1} {2} {3}";

            string[] result = null;
            
            int position = 0;
            int entries = 4;  // At least four pieces (version, flags, count, termination)
            int elements = 1;
            byte byte1, byte2, byte3, byte4;
            ushort word1, word2;

            // Calculate needed array size.
            if (this.mappings != null)
            {
                entries += this.mappings.Length;
                elements += this.mappings.Length;
            }

            // Create result array.
            result = new string[entries];

            // Add current version (header)
            this.Split(this.Version, out word1, out word2);
            this.Split(word1, out byte1, out byte2);
            this.Split(word2, out byte3, out byte4);

            result[position++] = string.Format(
                format,
                byte4.ToString("X2"),
                byte3.ToString("X2"),
                byte2.ToString("X2"),
                byte1.ToString("X2")
            );

            // Add current flags (header)
            this.Split(this.Flags, out word1, out word2);
            this.Split(word1, out byte1, out byte2);
            this.Split(word2, out byte3, out byte4);

            result[position++] = string.Format(
                format,
                byte4.ToString("X2"),
                byte3.ToString("X2"),
                byte2.ToString("X2"),
                byte1.ToString("X2")
            );

            // Add total number of elements (header)
            this.Split(elements, out word1, out word2);
            this.Split(word1, out byte1, out byte2);
            this.Split(word2, out byte3, out byte4);

            result[position++] = string.Format(
                format,
                byte4.ToString("X2"),
                byte3.ToString("X2"),
                byte2.ToString("X2"),
                byte1.ToString("X2")
            );

            // Add mappings if needed.
            if (this.mappings != null)
            {
                for (int index = 0; index < this.mappings.Length; index++)
                {
                    this.Split(this.mappings[index], out word1, out word2);
                    this.Split(word1, out byte1, out byte2);
                    this.Split(word2, out byte3, out byte4);

                    result[position++] = string.Format(
                        format,
                        byte3.ToString("X2"), // mapped scancode
                        byte4.ToString("X2"), // mapped extended
                        byte1.ToString("X2"), // original scancode
                        byte2.ToString("X2")  // original extended
                    );
                }
            }

            // Last but not least add termination.
            this.Split(this.Termination, out word1, out word2);
            this.Split(word1, out byte1, out byte2);
            this.Split(word2, out byte3, out byte4);

            result[position++] = string.Format(
                format,
                byte4.ToString("X2"),
                byte3.ToString("X2"),
                byte2.ToString("X2"),
                byte1.ToString("X2")
            );

            return result;
        }

        //
        // Summary:
        //      Writes current content of the instance into given file. If the file 
        //      doesn't exist it will be created. Otherwise an already existing file 
        //      will be overwritten!
        //
        // Parameters:
        //      regFile:
        //          Name of the file to be created.
        //
        public void ExportAsFile(string regFile)
        {
            System.Reflection.AssemblyName application = System.Reflection.Assembly.GetAssembly(this.GetType()).GetName();
            string date = DateTime.Now.Date.Year.ToString("D4") + "-" + DateTime.Now.Date.Month.ToString("D2") + "-" + DateTime.Now.Date.Day.ToString("D2");
            System.IO.TextWriter writer = new System.IO.StreamWriter(regFile);

            writer.WriteLine("Windows Registry Editor Version 5.00"); // Must be the first line and should 
            writer.WriteLine("");                                     // be followed by an empty line!
            writer.WriteLine("; -----------------------------------------------------------------------------");
            writer.WriteLine("; File created on " + date + " by " + application.Name + " (" + application.Version + ")");
            writer.WriteLine("; -----------------------------------------------------------------------------");
            writer.WriteLine(";");
            writer.WriteLine("; Remark");
            writer.WriteLine("; ------");
            writer.WriteLine(";");
            writer.WriteLine("; Scancode mappings stored in the Windows registry are always formatted");
            writer.WriteLine("; as little endian values!");
            writer.WriteLine(";");
            writer.WriteLine("; Structure");
            writer.WriteLine("; ---------");
            writer.WriteLine(";");
            writer.WriteLine("; +--------------+-----------+-------------------------------+");
            writer.WriteLine("; | Offset       | Size      | Data                          |");
            writer.WriteLine("; +--------------+-----------+-------------------------------+");
            writer.WriteLine("; | byte 0       | 4 bytes   | Version Information (Header)  |");
            writer.WriteLine("; | byte 4       | 4 bytes   | Flags (Header)                |");
            writer.WriteLine("; | byte 8       | 4 bytes   | Number of Mappings (Header)   |");
            writer.WriteLine("; | byte 12...n  | n*4 bytes | Individual Mappings           |");
            writer.WriteLine("; | last 4 bytes | 4 bytes   | Null Terminator (0x00000000)  |");
            writer.WriteLine("; +--------------+-----------+-------------------------------+");
            writer.WriteLine(";");
            writer.WriteLine("; Example");
            writer.WriteLine("; -------");
            writer.WriteLine(";");
            writer.WriteLine("; This code snippet shows the registry data in hexadecimal encoding needed");
            writer.WriteLine("; to map the CAPS LOCK keyboard key onto the SHIFT keyboard key.");
            writer.WriteLine(";");
            writer.WriteLine("; Version:    0x00000000");
            writer.WriteLine("; Flags:      0x00000000");
            writer.WriteLine("; Elements:   0x00000002");
            writer.WriteLine("; Mapping:    0x003A002A");
            writer.WriteLine("; Terminator: 0x00000000");
            writer.WriteLine("; Binary:     00,00,00,00,00,00,00,00,02,00,00,00,2A,00,3A,00,00,00,00,00");
            writer.WriteLine(";");
            writer.WriteLine("; -----------------------------------------------------------------------------");
            writer.WriteLine("");
            writer.WriteLine("[" + regRoot.ToString() + "\\" + regSubkey + "]");
            writer.WriteLine(GetRawData(true));
            writer.WriteLine("");
            writer.Write("; EOF");

            writer.Close();
        }

        //
        // Private section.
        //

        //
        // Summary:
        //      Combines given parameters to a short value.
        //
        // Parameters:
        //      high:
        //          The high byte value.
        //      low:
        //          The low byte value.
        //
        // Returns:
        //      The combined integer expression.
        //
        private int Combine(byte high, byte low)
        {
            return (int)((high & 0x000000FF) << 8 | (low & 0x000000FF)); // HIBYTE | LOBYTE
        }

        //
        // Summary:
        //      Combines given parameters to a integer value.
        //
        // Parameters:
        //      high:
        //          The high word value.
        //      low:
        //          The low word value.
        //
        // Returns:
        //      The combined integer expression.</returns>
        //
        private int Combine(ushort high, ushort low)
        {
            return (int)((high & 0x0000FFFF) << 16 | (low & 0x0000FFFF)); // HIBYTE | LOBYTE
        }

        //
        // Summary:
        //      Splits given value into its byte parts.
        //
        // Parameters:
        //      value:
        //          The value to be split.
        //      high:
        //          The high byte of given value.
        //      low:
        //          The low byte of given value.
        //
        private void Split(int value, out byte high, out byte low)
        {
            high = (byte)(value >> 8); // HIBYTE
            low = (byte)(value);       // LOWORD
        }

        //
        // Summary:
        //      Splits given value into its short parts.
        //
        // Parameters:
        //      value:
        //          The value to be split.
        //      high:
        //          The high word of given value.
        //      low:
        //          The low word of given value.
        //
        private void Split(int value, out ushort high, out ushort low)
        {
            high = (ushort)(value >> 16); // HIBYTE
            low = (ushort)(value);        // LOWORD
        }

        //
        // Summary:
        //      Loads given binary data into this instance.
        //
        // Parameters:
        //      binary:
        //          The input data buffer.
        //
        private void Load(byte[] binary)
        {
            // The whole binary data buffer consists at least of:
            //      4 bytes for version
            //      4 bytes for flags
            //      4 bytes for elements
            // The mapping data part consists at least of:
            //      4 bytes for termination
            // This means the buffer has at least 16 bytes and looks like: 
            // 00,00,00,00, 00,00,00,00, 01,00,00,00, 00,00,00,00 (little endian!)

            if (binary != null && binary.Length >= minimum)
            {
                int position = 0;
                int elements = 0;

                // Restore version from binary buffer using big endian.
                this.version = binary[position + 3] << 24 |
                               binary[position + 2] << 16 |
                               binary[position + 1] << 8 |
                               binary[position + 0];
                // Move next...
                position += sizeof(int);

                // Restore flags from binary buffer using big endian.
                this.flags = binary[position + 3] << 24 |
                             binary[position + 2] << 16 |
                             binary[position + 1] << 8 |
                             binary[position + 0];

                // Move next...
                position += sizeof(int);

                // Restore elements from binary buffer using big endian.
                elements = binary[position + 3] << 24 |
                           binary[position + 2] << 16 |
                           binary[position + 1] << 8 |
                           binary[position + 0];

                // Move next...
                position += sizeof(int);

                // Put mapping data if exist into temp buffer using little endian.
                this.mappings = null; // Must be new!

                // Calculate last element excluding termination.
                int last = (elements - 1) * sizeof(int) + position;

                while (position < last)
                {
                    // Registry contains little endian data format which must be shifted.
                    this.Append(
                        binary[position + 2], // original scancode
                        binary[position + 3], // original extended
                        binary[position + 0], // mapped scancode
                        binary[position + 1]  // mapped extended
                    );

                    // Move next...
                    position += sizeof(int);
                }

                // Restore termination from binary buffer using big endian.
                this.termination = binary[position + 3] << 24 |
                                   binary[position + 2] << 16 |
                                   binary[position + 1] << 8 |
                                   binary[position++]; // move next

                // Don't forget to set this content dirty!
                this.dirty = true;
            }
        }

        //
        // Summary:
        //      Creates a binary representation of current instance's content.
        //
        // Returns:
        //      A buffer with the binary output data.
        //
        private byte[] Make()
        {
            // The whole binary data buffer consists at least of:
            //      4 bytes for version
            //      4 bytes for flags
            //      4 bytes for elements
            // The mapping data part consists at least of:
            //      4 bytes for termination
            // This means the buffer has at least 16 bytes and looks like: 
            // 00,00,00,00, 00,00,00,00, 01,00,00,00, 00,00,00,00 (little endian!)

            if (this.dirty || this.binary == null)
            {
                int count = minimum;
                int elements = 1; // Mapping data elements consists at least of the termination!
                int position = 0;
                byte[] temp = null;

                if (this.mappings != null)
                {
                    elements += this.mappings.Length;
                    count += this.mappings.Length * sizeof(int);
                }

                // Create temp buffer to store data.
                temp = new byte[count];

                // Put version into temp buffer using little endian.
                temp[position++] = (byte)this.version;
                temp[position++] = (byte)(this.version >> 8);
                temp[position++] = (byte)(this.version >> 16);
                temp[position++] = (byte)(this.version >> 24);

                // Put flags into temp buffer using little endian.
                temp[position++] = (byte)this.flags;
                temp[position++] = (byte)(this.flags >> 8);
                temp[position++] = (byte)(this.flags >> 16);
                temp[position++] = (byte)(this.flags >> 24);

                // Put elements count into temp buffer using little endian.
                temp[position++] = (byte)elements;
                temp[position++] = (byte)(elements >> 8);
                temp[position++] = (byte)(elements >> 16);
                temp[position++] = (byte)(elements >> 24);

                // Put mapping data if exist into temp buffer using little endian.
                if (this.mappings != null)
                {
                    for (int index = 0; index < this.mappings.Length; index++)
                    {
                        temp[position++] = (byte)(this.mappings[index] >> 8);  // mapped scancode
                        temp[position++] = (byte)(this.mappings[index]);       // mapped extended
                        temp[position++] = (byte)(this.mappings[index] >> 24); // original scancode
                        temp[position++] = (byte)(this.mappings[index] >> 16); // original extended
                    }
                }

                // Put termination into temp buffer using little endian.
                temp[position++] = (byte)this.termination;
                temp[position++] = (byte)(this.termination >> 8);
                temp[position++] = (byte)(this.termination >> 16);
                temp[position++] = (byte)(this.termination >> 24);

                // Last bur not least assigne temp buffer to member variable.
                this.binary = temp;
            }

            return this.binary;
        }
    }
}
