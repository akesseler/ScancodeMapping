/*
 * MIT License
 * 
 * Copyright (c) 2022 plexdata.de
 * 
 * Permission is hereby granted, free of charge, to any person obtaining a copy
 * of this software and associated documentation files (the "Software"), to deal
 * in the Software without restriction, including without limitation the rights
 * to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
 * copies of the Software, and to permit persons to whom the Software is
 * furnished to do so, subject to the following conditions:
 * 
 * The above copyright notice and this permission notice shall be included in all
 * copies or substantial portions of the Software.
 * 
 * THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
 * IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
 * FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
 * AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
 * LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
 * OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE
 * SOFTWARE.
 */

using Microsoft.Win32;
using System;

namespace ScancodeMapping
{
    public class ScancodeMap
    {
        // Binary data consists at least of version, flags, elements and termination!
        private const Int32 minimum = 4 * sizeof(Int32);

        private static readonly RegistryKey regRoot = Registry.LocalMachine;
        private static readonly String regSubkey = "SYSTEM\\CurrentControlSet\\Control\\Keyboard Layout";
        private static readonly String regValue = "Scancode Map";

        private Byte[] binary = null;
        private Int32[] mappings = null;
        private Int32 version = 0;
        private Int32 flags = 0;
        private Int32 termination = 0;
        private Boolean dirty = true; // Binary not yet created or content has changed!

        public ScancodeMap()
        {
            this.version = 0;
            this.flags = 0;
            this.mappings = null;
            this.termination = 0;
            this.binary = null;
            this.dirty = true;
        }

        public ScancodeMap(Int32 version, Int32 flags)
            : this()
        {
            this.version = version;
            this.flags = flags;
        }

        public ScancodeMap(Int32 version, Int32 flags, Int32 termination)
            : this(version, flags)
        {
            this.termination = termination;
        }

        public ScancodeMap(Byte[] binary)
        {
            this.Load(binary);
        }

        public Int32 Version
        {
            get { return this.version; }
            set { this.version = value; this.dirty = true; }
        }

        public Int32 Flags
        {
            get { return this.flags; }
            set { this.flags = value; this.dirty = true; }
        }

        public Int32 Termination
        {
            get { return this.termination; }
            set { this.termination = value; this.dirty = true; }
        }

        public Int32[] Mappings
        {
            get { return this.mappings; }
            set { this.mappings = (Int32[])value.Clone(); this.dirty = true; }
        }

        public Byte[] Binary
        {
            get { return this.Make(); }
            set { this.Load(value); }
        }

        public static ScancodeMap RegistryLoad()
        {
            ScancodeMap result = null;
            RegistryKey subkey = null;

            // This key must always exist because its a system depending registry key!
            subkey = regRoot.OpenSubKey(regSubkey, false); // Open for read acccess only!

            if (subkey != null)
            {
                Object value = subkey.GetValue(regValue, null);

                if (value != null && value.GetType() == typeof(Byte[]))
                {
                    result = new ScancodeMap((Byte[])value);
                }

                subkey.Close();
            }

            return result;
        }

        public static Boolean RegistrySave(ScancodeMap value)
        {
            // TODO: Optimization by respecting IDisposable and using.

            Boolean success = false;
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
                subkey.Dispose();
            }

            return success;
        }

        public static void RegistryRemove()
        {
            // TODO: Optimization by respecting IDisposable and using.

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
                subkey.Dispose();
            }
        }

        public void Append(Int32 mapping)
        {
            Int32 count = 1;
            Int32[] temp = null;

            // Copy if exists.
            if (this.mappings != null)
            {
                count += this.mappings.Length;
                temp = new Int32[count];
                this.mappings.CopyTo(temp, 0);
            }
            // Just create with only one entry.
            else
            {
                temp = new Int32[count];
            }

            // Add new value and re-assign currently used mappings.
            temp.SetValue(mapping, count - 1);
            this.mappings = temp;
            this.dirty = true;
        }

        public void Append(Int32 oriKeycode, Int32 mapKeycode)
        {
            // Still use big endian!
            this.Append(this.Combine((UInt16)oriKeycode, (UInt16)mapKeycode));
        }

        public void Append(Int32 oriScancode, Int32 oriExtended, Int32 mapScancode, Int32 mapExtended)
        {
            // Still use big endian!
            this.Append(this.Combine((Byte)oriScancode, (Byte)oriExtended), this.Combine((Byte)mapScancode, (Byte)mapExtended));
        }

        public Int32 IndexOf(Int32 mapping)
        {
            Int32 result = -1;
            if (this.mappings != null)
            {
                this.Split(mapping, out UInt16 oriKeycode, out UInt16 mapKeycode);

                for (Int32 index = 0; index < this.mappings.Length; index++)
                {
                    this.Split(this.mappings[index], out UInt16 hiWord, out UInt16 loWord);

                    if (oriKeycode == hiWord)
                    {
                        result = index;
                        break;
                    }
                }
            }
            return result;
        }

        public Int32 IndexOf(Int32 oriKeycode, Int32 mapKeycode)
        {
            return this.IndexOf(this.Combine((UInt16)oriKeycode, (UInt16)mapKeycode));
        }

        public Int32 IndexOf(Int32 oriScancode, Int32 oriExtended, Int32 mapScancode, Int32 mapExtended)
        {
            return this.IndexOf(this.Combine((Byte)oriScancode, (Byte)oriExtended), this.Combine((Byte)mapScancode, (Byte)mapExtended));
        }

        public Boolean GetAt(Int32 index, out Int32 mapping)
        {
            Boolean success = false;
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

        public Boolean GetAt(Int32 index, out Int32 oriKeycode, out Int32 mapKeycode)
        {
            Boolean success = false;

            oriKeycode = 0;
            mapKeycode = 0;

            if (this.GetAt(index, out Int32 mapping))
            {
                this.Split(mapping, out UInt16 hiWord, out UInt16 loWord);

                oriKeycode = hiWord;
                mapKeycode = loWord;

                success = true;
            }
            return success;
        }

        public Boolean GetAt(Int32 index, out Int32 oriScancode, out Int32 oriExtended, out Int32 mapScancode, out Int32 mapExtended)
        {
            Boolean success = false;

            oriScancode = 0;
            oriExtended = 0;
            mapScancode = 0;
            mapExtended = 0;

            if (this.GetAt(index, out Int32 hiWord, out Int32 loWord))
            {

                this.Split(hiWord, out Byte hiByte, out Byte loByte);

                oriScancode = hiByte;
                oriExtended = loByte;

                this.Split(loWord, out hiByte, out loByte);

                mapScancode = hiByte;
                mapExtended = loByte;

                success = true;
            }
            return success;
        }

        public Boolean SetAt(Int32 index, Int32 mapping)
        {
            Boolean success = false;

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

        public Boolean SetAt(Int32 index, Int32 oriKeycode, Int32 mapKeycode)
        {
            return this.SetAt(index, this.Combine((UInt16)oriKeycode, (UInt16)mapKeycode));
        }

        public Boolean SetAt(Int32 index, Int32 oriScancode, Int32 oriExtended, Int32 mapScancode, Int32 mapExtended)
        {
            return this.SetAt(index, this.Combine((Byte)oriScancode, (Byte)oriExtended), this.Combine((Byte)mapScancode, (Byte)mapExtended));
        }

        public void Remove(Int32 mapping)
        {
            Int32 remove = this.IndexOf(mapping);
            if (remove != -1)
            {
                Int32[] temp = new Int32[this.mappings.Length - 1];
                Int32 position = 0;

                for (Int32 index = 0; index < this.mappings.Length; index++)
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

        public void Remove(Int32 oriKeycode, Int32 mapKeycode)
        {
            this.Remove(this.Combine((UInt16)oriKeycode, (UInt16)mapKeycode));
        }

        public void Remove(Int32 oriScancode, Int32 oriExtended, Int32 mapScancode, Int32 mapExtended)
        {
            this.Remove(this.Combine((Byte)oriScancode, (Byte)oriExtended), this.Combine((Byte)mapScancode, (Byte)mapExtended));
        }

        public Boolean HasEntries()
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

        public String GetRawData(Boolean includeName)
        {
            String result = String.Empty;

            if (includeName)
            {
                result += "\"" + ScancodeMap.regValue + "\"=hex:";
            }

            Byte[] data = this.Binary;

            for (Int32 index = 0; index < data.Length; index++)
            {
                result += data[index].ToString("X2");

                if (index < data.Length - 1)
                {
                    result += ",";
                }
            }
            return result;
        }

        public String GetRawData()
        {
            return this.GetRawData(true);
        }

        public String[] GetRawDataList()
        {
            const String format = "{0} {1} {2} {3}";

            String[] result = null;

            Int32 position = 0;
            Int32 entries = 4;  // At least four pieces (version, flags, count, termination)
            Int32 elements = 1;

            // Calculate needed array size.
            if (this.mappings != null)
            {
                entries += this.mappings.Length;
                elements += this.mappings.Length;
            }

            // Create result array.
            result = new String[entries];

            // Add current version (header)
            this.Split(this.Version, out UInt16 word1, out UInt16 word2);
            this.Split(word1, out Byte byte1, out Byte byte2);
            this.Split(word2, out Byte byte3, out Byte byte4);

            result[position++] = String.Format(
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

            result[position++] = String.Format(
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

            result[position++] = String.Format(
                format,
                byte4.ToString("X2"),
                byte3.ToString("X2"),
                byte2.ToString("X2"),
                byte1.ToString("X2")
            );

            // Add mappings if needed.
            if (this.mappings != null)
            {
                for (Int32 index = 0; index < this.mappings.Length; index++)
                {
                    this.Split(this.mappings[index], out word1, out word2);
                    this.Split(word1, out byte1, out byte2);
                    this.Split(word2, out byte3, out byte4);

                    result[position++] = String.Format(
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

            result[position++] = String.Format(
                format,
                byte4.ToString("X2"),
                byte3.ToString("X2"),
                byte2.ToString("X2"),
                byte1.ToString("X2")
            );

            return result;
        }

        public void ExportAsFile(String regFile)
        {
            // TODO: Optimization by respecting IDisposable and using.

            System.Reflection.AssemblyName application = System.Reflection.Assembly.GetAssembly(this.GetType()).GetName();
            String date = DateTime.Now.Date.Year.ToString("D4") + "-" + DateTime.Now.Date.Month.ToString("D2") + "-" + DateTime.Now.Date.Day.ToString("D2");
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
            writer.WriteLine(this.GetRawData(true));
            writer.WriteLine("");
            writer.Write("; EOF");

            writer.Close();
            writer.Dispose();
        }

        private Int32 Combine(Byte high, Byte low)
        {
            return (Int32)((high & 0x000000FF) << 8 | (low & 0x000000FF));
        }

        private Int32 Combine(UInt16 high, UInt16 low)
        {
            return (Int32)((high & 0x0000FFFF) << 16 | (low & 0x0000FFFF));
        }

        private void Split(Int32 value, out Byte high, out Byte low)
        {
            high = (Byte)(value >> 8);
            low = (Byte)value;
        }

        private void Split(Int32 value, out UInt16 high, out UInt16 low)
        {
            high = (UInt16)(value >> 16);
            low = (UInt16)value;
        }

        private void Load(Byte[] binary)
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
                Int32 position = 0;
                Int32 elements = 0;

                // Restore version from binary buffer using big endian.
                this.version = binary[position + 3] << 24 |
                               binary[position + 2] << 16 |
                               binary[position + 1] << 8 |
                               binary[position + 0];
                // Move next...
                position += sizeof(Int32);

                // Restore flags from binary buffer using big endian.
                this.flags = binary[position + 3] << 24 |
                             binary[position + 2] << 16 |
                             binary[position + 1] << 8 |
                             binary[position + 0];

                // Move next...
                position += sizeof(Int32);

                // Restore elements from binary buffer using big endian.
                elements = binary[position + 3] << 24 |
                           binary[position + 2] << 16 |
                           binary[position + 1] << 8 |
                           binary[position + 0];

                // Move next...
                position += sizeof(Int32);

                // Put mapping data if exist into temp buffer using little endian.
                this.mappings = null; // Must be new!

                // Calculate last element excluding termination.
                Int32 last = (elements - 1) * sizeof(Int32) + position;

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
                    position += sizeof(Int32);
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

        private Byte[] Make()
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
                Int32 count = minimum;
                Int32 elements = 1; // Mapping data elements consists at least of the termination!
                Int32 position = 0;
                Byte[] temp = null;

                if (this.mappings != null)
                {
                    elements += this.mappings.Length;
                    count += this.mappings.Length * sizeof(Int32);
                }

                // Create temp buffer to store data.
                temp = new Byte[count];

                // Put version into temp buffer using little endian.
                temp[position++] = (Byte)this.version;
                temp[position++] = (Byte)(this.version >> 8);
                temp[position++] = (Byte)(this.version >> 16);
                temp[position++] = (Byte)(this.version >> 24);

                // Put flags into temp buffer using little endian.
                temp[position++] = (Byte)this.flags;
                temp[position++] = (Byte)(this.flags >> 8);
                temp[position++] = (Byte)(this.flags >> 16);
                temp[position++] = (Byte)(this.flags >> 24);

                // Put elements count into temp buffer using little endian.
                temp[position++] = (Byte)elements;
                temp[position++] = (Byte)(elements >> 8);
                temp[position++] = (Byte)(elements >> 16);
                temp[position++] = (Byte)(elements >> 24);

                // Put mapping data if exist into temp buffer using little endian.
                if (this.mappings != null)
                {
                    for (Int32 index = 0; index < this.mappings.Length; index++)
                    {
                        temp[position++] = (Byte)(this.mappings[index] >> 8);  // mapped scancode
                        temp[position++] = (Byte)(this.mappings[index]);       // mapped extended
                        temp[position++] = (Byte)(this.mappings[index] >> 24); // original scancode
                        temp[position++] = (Byte)(this.mappings[index] >> 16); // original extended
                    }
                }

                // Put termination into temp buffer using little endian.
                temp[position++] = (Byte)this.termination;
                temp[position++] = (Byte)(this.termination >> 8);
                temp[position++] = (Byte)(this.termination >> 16);
                temp[position++] = (Byte)(this.termination >> 24);

                // Last bur not least assigne temp buffer to member variable.
                this.binary = temp;
            }

            return this.binary;
        }
    }
}
