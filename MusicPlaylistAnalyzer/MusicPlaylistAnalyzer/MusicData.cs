namespace MusicPlaylistAnalyzer
{
    public class MusicData
    {
        public string Name, Artist, Album, Genre;
        public int Size, Time, Year, Plays;

        public MusicData (string name, string artist, string album, string genre, int size, int time, int year, int plays)
        {
            Name = name;
            Artist = artist;
            Album = album;
            Genre = genre;
            Size = size;
            Time = time;
            Year = year;
            Plays = plays;
        }
    }
}
