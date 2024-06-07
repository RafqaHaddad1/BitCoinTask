namespace BiTCoin2.NewFolder
{
    public class BCT2
    {
        public int id { get; set; }
        public double last_price { get; set; }
        public double timestamp { get; set; }
        public DateTime ConvertTimestampToDateTime()
        {
            DateTime dateTime = new DateTime(1970, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc);
            dateTime = dateTime.AddSeconds(timestamp).ToLocalTime();
            return dateTime;
        }
    }
}
