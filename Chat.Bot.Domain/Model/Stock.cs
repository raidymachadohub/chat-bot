namespace Chat.Bot.Domain.Model
{
    public class Stock
    {
        public string Symbol { get; set; }
        public DateTime Date { get; set; }
        public string Time { get; set; }
        public decimal Open { get; set; }
        public decimal High { get; set; }
        public decimal Low { get; set; }
        public decimal Close { get; set; }
        public long Volume { get; set; }

        public Stock(){ }
        

        public Stock(string symbol, DateTime date, string time, decimal open, decimal high, decimal low, decimal close,
            long volume)
        {
            Symbol = symbol;
            Date = date;
            Time = time;
            Open = open;
            High = high;
            Low = low;
            Close = close;
            Volume = volume;
        }
    }
}