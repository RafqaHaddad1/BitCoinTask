namespace BiTCoin2.NewFolder
{
    public class BTC1

    {
        public int id { get; set; }
        public double last { get; set; }
        public float percent_change_24 { get; set; }


        public double timestamp { get; set; }
        public DateTime ConvertTimestampToDateTime()
        {
            DateTime dateTime = new DateTime(1970, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc);
            dateTime = dateTime.AddSeconds(timestamp).ToLocalTime();
            return dateTime;
        }
    }


}
