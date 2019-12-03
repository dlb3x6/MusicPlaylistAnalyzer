using System;
using System.Collections.Generic;
using System.Collections;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace MusicPlaylistAnalyzer
{
    class Program
    {
        static void Main(string[] args)
        {
            if (args.Length != 2) //command line stuff
            {
                Console.WriteLine("Error: Please enter the correct amount of arguments. Structure: MusicPlaylistAnalyzer.exe <data text file> <report to be written to>");
                Environment.Exit(1);
            }

            string playlist = args[0];
            string report = args[1];
           
            if (File.Exists(playlist) == false)
            {
                Console.WriteLine("Error: Playlist file not found");
                Environment.Exit(1);
            }
            if (File.Exists(report) == false)
            {
                Console.WriteLine("Error: Report file not found.");
                Environment.Exit(1);
            }

            List<MusicData> list = null; //collect all information
            StreamReader reader = null;
            int row = 0;

            try
            {
                reader = new StreamReader(playlist);
                list = new List<MusicData>();

                while (!reader.EndOfStream)
                {
                    var line = reader.ReadLine();
                    row++;
                    if (row == 1) //skips headers
                        continue;
                    var values = line.Split('\t');
                    if (values.Length != 8)
                    {
                        Console.WriteLine($"Error: Row {row} doesn't have the correct amount of data.");
                        Environment.Exit(2);
                    }
                    try
                    {
                        string name = values[0];
                        string artist = values[1];
                        string album = values[2];
                        string genre = values[3];
                        int size = int.Parse(values[4]);
                        int time = int.Parse(values[5]);
                        int year = int.Parse(values[6]);
                        int plays = int.Parse(values[7]);

                        MusicData obj = new MusicData(name, artist, album, genre, size, time, year, plays);
                        list.Add(obj);
                    }
                    catch
                    {
                        Console.WriteLine($"Error: Data is invalid in row {row}");
                        Environment.Exit(2);
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error: File could not be read: {0}" , ex.Message);
                Environment.Exit(2);
            }

            string content = "Music Playlist Report\n\n";
            content += "Songs that received 200 or more plays:\n";
            var songPlays = from data in list where data.Plays >= 200 select new { data.Name, data.Artist, data.Plays };
            foreach (var song in songPlays)
            {
                content += song + ", ";
            }
            content += "\nNumber of Alternative songs: ";
            var altSongs = from data in list where data.Genre == "Alternative" select new { data.Name, data.Artist };
            int counterAlt = 0;
            foreach (var song in altSongs)
            {
                counterAlt++;
            }
            content += $"{counterAlt}\n";
            content += "Number of Hip Hop/Rap songs: ";
            var rapSongs = from data in list where data.Genre == "Hip Hop/Rap" select new { data.Name, data.Artist };
            int counterRap = 0;
            foreach (var song in rapSongs)
            {
                counterRap++;
            }
            content += $"{counterRap}\n";
            content += "Songs from the album Welcome to the Fishbowl:\n";
            var fishbowl = from data in list where data.Album == "Welcome to the Fishbowl" select new { data.Name, data.Artist };
            foreach (var song in fishbowl)
            {
                content += song + ", ";
            }
            content += "\nSongs from before 1970:\n";
            var oldSongs = from data in list where data.Year < 1970 select new { data.Name, data.Artist };
            foreach (var song in oldSongs)
            {
                content += song + ", ";
            }
            content += "\nSong names longer than 85 characters:\n";
            var swolSongs = from data in list where data.Name.Length > 85 select new { data.Name, data.Artist };
            foreach (var song in swolSongs)
            {
                content += song + ", ";
            }
            content += "\nLongest song: ";
            var longSong = from data in list select data.Time;
            int maxArr = 0;
            foreach (var song in longSong)
            {
               if (song > maxArr)
                {
                    maxArr = song;
                } 
            }
            var longSong2 = from data in list where data.Time == maxArr select new { data.Name, data.Artist };
            foreach (var song in longSong2)
            {
                content += song;
            }

            try
            {
                File.WriteAllText(report, content);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error: Report file failed to be written to: {ex.Message}");
            }
            finally
            {
                if (reader != null)
                {
                    reader.Close();
                }
            }
            Console.WriteLine("Report successfully saved :O");
            Environment.Exit(3);
        }
    }
}
