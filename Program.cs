using System;
using System.Collections.Generic;
using System.IO;
using System.Text.Json;
using System.Linq;
using Geolocation;



namespace IoT_Academy
{
    
    public class GPS_Data
    {
        public float Latitude { get; set; }
        public float Longitude { get; set; }
        public DateTimeOffset GpsTime { get; set; }
        public int Speed { get; set; }
        public int Angle { get; set; }
        public int Altitude { get; set; }
        public int Satellites { get; set; }
    }

    public class Fastest_Record
    {
        public int Start_Index { get; set; }
        public int End_Index { get; set; }
        public int Time { get; set; }
        public float Length { get; set; }
        //public float Avg_Speed { get; set; }
        public Fastest_Record(int start_index, int end_index, int time, float length)
        {
            Start_Index = start_index;
            End_Index = end_index;
            Time = time;
            Length = length;
        }
        
    }
    
    internal class Program
    {   
        public static List<GPS_Data>ReadJsonData()
        {
            string jsonFileName = "2019-07.json";
            string jsonString = File.ReadAllText(jsonFileName);
            List<GPS_Data> json_gps_data = JsonSerializer.Deserialize<List<GPS_Data>>(jsonString);

            return json_gps_data;
        }

        public static List<GPS_Data> ReadCsvData()
        {
            List<GPS_Data> csv_gps_data = new List<GPS_Data>();
            string csvFileName = "2019-08.csv";
            foreach (string line in File.ReadLines(csvFileName))
            {
                string[] csv_data = line.Split(',');
                GPS_Data csv_element = new GPS_Data();
                csv_element.Latitude = float.Parse(csv_data[0]);
                csv_element.Longitude = float.Parse(csv_data[1]);
                csv_element.GpsTime = DateTimeOffset.Parse(csv_data[2]);
                csv_element.Speed = int.Parse(csv_data[3]);
                csv_element.Angle = int.Parse(csv_data[4]);
                csv_element.Altitude = int.Parse(csv_data[5]);
                csv_element.Satellites = int.Parse(csv_data[6]);
                csv_gps_data.Add(csv_element);
            }

            return csv_gps_data;
        }

        public static byte[] Read_Bytes(byte[] Storage, BinaryReader ByteReader)
        {
            ByteReader.Read(Storage, 0, Storage.Length);
            Array.Reverse(Storage);
            return Storage;
        }

        public static List<GPS_Data> ReadBinData()
        {
            List<GPS_Data> bin_gps_data = new List<GPS_Data>();
            string binFileName = "2019-09.bin";
            using (BinaryReader ByteReader = new BinaryReader(File.Open(binFileName, FileMode.Open)))
            {
                byte[] Latitude = new byte[4];
                byte[] Longitude = new byte[4];
                byte[] Date = new byte[8];
                byte[] Speed = new byte[2];
                byte[] Angle = new byte[2];
                byte[] Altitude = new byte[2];
                byte[] Satellites = new byte[1];
                while (ByteReader.PeekChar() != -1)
                {
                    GPS_Data bin_element = new GPS_Data();
                    bin_element.Latitude = (float)BitConverter.ToInt32(Read_Bytes(Latitude, ByteReader), 0) / 10000000;
                    bin_element.Longitude = (float)BitConverter.ToInt32(Read_Bytes(Longitude, ByteReader), 0) / 10000000;
                    bin_element.GpsTime = DateTimeOffset.FromUnixTimeMilliseconds(BitConverter.ToInt64(Read_Bytes(Date, ByteReader), 0));
                    bin_element.Speed = (int)BitConverter.ToInt16(Read_Bytes(Speed, ByteReader), 0);
                    bin_element.Angle = (int)BitConverter.ToInt16(Read_Bytes(Angle, ByteReader), 0);
                    bin_element.Altitude = (int)BitConverter.ToInt16(Read_Bytes(Altitude, ByteReader), 0);
                    bin_element.Satellites = (int)Read_Bytes(Satellites, ByteReader)[0];

                    bin_gps_data.Add(bin_element);
                }
            }
            return bin_gps_data;
        }
        public static void Add_To_Histogram(int data, List<int> Histogram) 
        {
            while(Histogram.Count<=data)
            {
                Histogram.Add(0);
            }
            Histogram[data]++;
        }

        public static string Make_Histogram_Bar(int Quantity, int Bar_Spaces)
        {
            string bar = "";
            for(int i=0; i<Bar_Spaces; i++)
            {
                if (i < Quantity)
                    bar += "X";
                else
                    bar += " ";
            }
            return bar;
        }

        public static void Display_Small_Histogram(List<int> Histogram, String name)
        {
            int Largest_Value = Histogram.Max();
            name += " histogram";
            Console.WriteLine("+--------------------+--------+");
            Console.WriteLine($"|{name,-20}| Hits   |");
            Console.WriteLine("+--------------------+--------+");
            for (int i = 0; i < Histogram.Count; i++)
            {
                string Bar = Make_Histogram_Bar((int)Math.Ceiling((Histogram[i] * 1.0 / (Largest_Value * 1.0 / 15))), 15);
                Console.WriteLine($"|{i,-4}|" + Bar + $"| {Histogram[i],-7}|");
            }
            Console.WriteLine("+----+---------------+--------+");

        }
        public static List<int> Group_Histogram_Data(List<int> Histogram)
        {
            int sum = 0;
            List<int> Group = new List<int>();
            for(int i=0; i<19; i++)
            {
                sum = 0;
                for(int j=0; j<Histogram.Count/20; j++)
                {
                    sum+=Histogram[j + i * Histogram.Count / 20];
                }
                Group.Add(sum);
            }
            sum = 0;
            for (int i=19*Histogram.Count/20; i<Histogram.Count; i++)
            {
                sum += Histogram[i];
            }
            Group.Add(sum);
            return Group;
        }
        public static void Display_Large_Histogram(List<int> Histogram, String name)
        {
            List<int> Group = Group_Histogram_Data(Histogram);
            int Largest_Value = Group.Max();
            int scale = Histogram.Count / 20;
            name += " histogram";
            Console.WriteLine("+-------------------------+--------+");
            Console.WriteLine($"|{name,-25}| Hits   |");
            Console.WriteLine("+-------------------------+--------+");
            for (int i = 0; i < Group.Count-1; i++)
            {
                string Bar = Make_Histogram_Bar((int)Math.Ceiling((Group[i] * 1.0 / (Largest_Value * 1.0 / 15))), 15);
                Console.WriteLine($"|{i*scale,3} - {(i+1) * scale -1,3}|" + Bar + $"| {Group[i],-7}" + "|");
            }
            Console.WriteLine($"|{(Group.Count-1) * scale,3} - {Histogram.Count-1,3}|" + Make_Histogram_Bar((int)Math.Ceiling((Group[Group.Count-1] * 1.0 / (Largest_Value * 1.0 / 15))), 15) + $"| {Group[Group.Count - 1],-7}|");
            Console.WriteLine("+---------+---------------+--------+");

        }

        public static float Calculate_Distance(GPS_Data Point1, GPS_Data Point2)
        {
            return (float)GeoCalculator.GetDistance(Point1.Latitude, Point1.Longitude, Point2.Latitude, Point2.Longitude, 3, DistanceUnit.Kilometers);
        }

        public static int Calculate_Time(GPS_Data Point1, GPS_Data Point2)
        {
            return (int)Math.Abs(Point1.GpsTime.ToUnixTimeSeconds() - Point2.GpsTime.ToUnixTimeSeconds());
        }

        public static Fastest_Record Find_Fastest_Record(List<GPS_Data> gps_data)
        {
            Fastest_Record fastest = new Fastest_Record(0, 0, 0, 0);
            Fastest_Record temporary= new Fastest_Record(0, 0, 0, 0);
            bool First_Found = false;
            int i = 1;
            while(i!=gps_data.Count)
            {
                if (temporary.Length<100)
                {
                    temporary.End_Index = i;
                    temporary.Time = Calculate_Time(gps_data[temporary.End_Index], gps_data[temporary.Start_Index]);
                    temporary.Length += Calculate_Distance(gps_data[temporary.End_Index], gps_data[temporary.End_Index - 1]);
                    i++;
                }
                if(temporary.Length>=100)
                {
                    
                    if(temporary.Time<fastest.Time || !First_Found)
                    {
                        fastest.Start_Index = temporary.Start_Index;
                        fastest.End_Index = temporary.End_Index;
                        fastest.Time = temporary.Time;
                        fastest.Length = temporary.Length;
                        First_Found = true;
                    }
                    temporary.Start_Index += 1;

                    temporary.Time = Calculate_Time(gps_data[temporary.End_Index], gps_data[temporary.Start_Index]);
                    temporary.Length -= Calculate_Distance(gps_data[temporary.Start_Index], gps_data[temporary.Start_Index - 1]);
                }
            }
            return fastest;
        }

        public static void Display_Fastest_Record(List<GPS_Data> gps_data)
        {
            Fastest_Record route = Find_Fastest_Record(gps_data);
            Console.WriteLine($"Fastest road section of at least 100km was driven over {route.Time}s and was {route.Length}km long");
            Console.WriteLine($"Start position {gps_data[route.Start_Index].Latitude}; {gps_data[route.Start_Index].Longitude}");
            Console.WriteLine($"Start gps time {gps_data[route.Start_Index].GpsTime}");
            Console.WriteLine($"End position {gps_data[route.End_Index].Latitude}; {gps_data[route.End_Index].Longitude}");
            Console.WriteLine($"End gps time {gps_data[route.End_Index].GpsTime}");
            Console.WriteLine($"Average speed: {route.Length/((float)route.Time/3600):0.0}km/h");
        }

        public static void Main()
        {
            bool READ_JSON_DATA = false;
            bool READ_CSV_DATA = false;
            bool READ_BIN_DATA = true;
            bool CALCULATE_SATELITE_HISTOGRAM = true;
            bool CALCULATE_SPEED_HISTOGRAM = true;
            bool DISPLAY_FASTEST_RECORD = true;

            List<GPS_Data> gps_data = new List<GPS_Data>();

            if (READ_JSON_DATA)
            {
                gps_data.AddRange(ReadJsonData());
            }

            if(READ_CSV_DATA)
            {
                gps_data.AddRange(ReadCsvData());
            }

            if (READ_BIN_DATA)
            {
                gps_data.AddRange(ReadBinData());
            }

            if (CALCULATE_SATELITE_HISTOGRAM)
            {
                List<int> Histogram = new List<int>();
                foreach(GPS_Data data_element in gps_data)
                {
                    Add_To_Histogram(data_element.Satellites, Histogram);
                }
                if (Histogram.Count > 25)
                {
                    Display_Large_Histogram(Histogram, "Satellite");
                }
                else
                {
                    Display_Small_Histogram(Histogram, "Satellite");
                }
            }

            if (CALCULATE_SPEED_HISTOGRAM)
            {
                List<int> Histogram = new List<int>();
                foreach (GPS_Data data_element in gps_data)
                {
                    Add_To_Histogram(data_element.Speed, Histogram);
                }
                if(Histogram.Count>25)
                {
                    Display_Large_Histogram(Histogram, "Speed");
                }
                else
                {
                    Display_Small_Histogram(Histogram, "Speed");
                }
            }
            if (DISPLAY_FASTEST_RECORD)
            {
                Display_Fastest_Record(gps_data);
            }
        }
    }
}
